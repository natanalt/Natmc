using Natmc.Json;
using Natmc.Logging;
using Natmc.Resources.Languages;
using Natmc.Resources.Readers;
using Natmc.Ui.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Natmc.Resources
{
    public class ResourcePack
    {
        private static readonly LogScope Log = new LogScope("ResourcePackLoader");

        // 6 => 1.16.2 - 1.16.3
        public const int CurrentPackFormat = 6;

        public string Id;
        public int PackFormat;
        public TextComponent Description;
        public List<Language> AdditionalLanguages;
        public IPackReader PackReader;

        public static IPackReader CreatePackReader(string archivePath)
        {
            if (Filesystem.DirectoryExists(archivePath))
            {
                return new DirectoryPackReader(archivePath);
            }
            else if (archivePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) && Filesystem.FileExists(archivePath))
            {
                return new ZipPackReader(archivePath);
            }
            else
            {
                throw new FormatException("Unknown resource pack type");
            }
        }

        public ResourcePack(string path)
        {
            Id = path;
            PackReader = CreatePackReader(path);
            Log.Info($"Opening resource pack `{path}`, pack reader type {PackReader.GetType().Name}");

            if (!PackReader.FileExists("pack.mcmeta"))
                throw new FormatException("pack.mcmeta doesn't exist");

            var packMetaRoot = JObject.Parse(PackReader.ReadTextFile("pack.mcmeta"));
            var packMetaScheme = new ObjectSchema
            {
                ["pack"] = new ObjectSchema
                {
                    ["description"] = "string|object",
                    ["pack_format"] = "number",
                },
                ["language"] = new ObjectSchema()
            };
            JsonValidator.ValidateWithException(packMetaRoot, packMetaScheme, "Invalid pack.mcmeta");

            var packMeta = packMetaRoot.Value<JObject>("pack");
            PackFormat = packMeta.Value<int>("pack_format");

            var descriptionToken = packMeta["description"];
            if (descriptionToken.Type == JTokenType.String)
            {
                Description = new StringComponent(descriptionToken.Value<string>());
            }
            else
            {
                Description = TextComponent.FromJsonObject(
                    packMeta["description"].Value<JObject>(),
                    out string parseError);
                if (Description == null)
                    throw new FormatException($"Invalid pack.mcmeta - invalid description - {parseError}");
            }

            AdditionalLanguages = new List<Language>();
            if (packMetaRoot.ContainsKey("language"))
            {
                var languagesTag = packMetaRoot["language"];
                if (languagesTag.Type != JTokenType.Object)
                    throw new FormatException("Invalid pack.mcmeta - invalid type of language");

                foreach (var kv in languagesTag.Value<JObject>())
                {
                    var value = kv.Value.Value<JObject>();

                    var languageSchema = new ObjectSchema
                    {
                        ["name"] = "string",
                        ["region"] = "string"
                    };
                    JsonValidator.ValidateWithException(value, languageSchema, $"Invalid language {kv.Key}");

                    var language = new Language
                    {
                        Code = kv.Key,
                        Name = value["name"].ToString(),
                        Region = value["region"].ToString(),
                        IsBidirectional = value.ContainsKey("bidirectional") && value.Value<bool>("bidirectional"),
                    };

                    AdditionalLanguages.Add(language);
                }
            }
        }
    }
}
