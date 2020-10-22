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
        // 6 => 1.16.2 - 1.16.3
        public const int CurrentPackFormat = 6;

        public int PackFormat;
        public TextComponent Description;
        public List<Language> AdditionalLanguages;
        public IPackReader PackReader;

        public ResourcePack(string path)
        {
            if (Directory.Exists(path))
                PackReader = new DirectoryPackReader(path);
            else
                PackReader = new ZipPackReader(path);

            if (!PackReader.FileExists("pack.mcmeta"))
                throw new FileNotFoundException("pack.mcmeta does not exist");

            string packMetaData;
            using (var stream = PackReader.OpenFile("pack.mcmeta"))
            {
                using var reader = new StreamReader(stream);
                packMetaData = reader.ReadToEnd();
            }

            var packMetaRoot = new JObject(packMetaData);
            
            var packMeta = packMetaRoot["pack"];

            if (packMeta["pack_format"].Type != JTokenType.Integer)
                throw new FormatException("pack_format must be an integer");
            PackFormat = packMeta["pack_format"].Value<int>();

            var descriptionToken = packMeta["description"];
            if (descriptionToken.Type == JTokenType.String)
                Description = new StringComponent(descriptionToken.Value<string>());
            else if (descriptionToken.Type == JTokenType.Object)
            {
                Description = TextComponent.FromJsonObject(descriptionToken.Value<JObject>(), out string error);
                if (Description == null)
                    throw new FormatException(error);
            }
            else
                throw new FormatException("Invalid description type");

            AdditionalLanguages = new List<Language>();
            if (packMetaRoot.ContainsKey("languages"))
            {
                if (packMetaRoot["language"] is JObject languages)
                {
                    foreach (var kv in languages)
                    {
                        AdditionalLanguages.Add(new Language
                        {
                            Code = kv.Key,
                            Name = kv.Value["name"].ToString(),
                            Region = kv.Value["region"].ToString(),
                            IsBidirectional = kv.Value["bidirectional"].Value<bool>()
                        });
                    }
                }
                else
                {
                    throw new FileLoadException("languages should be a JObject");
                }
            }
        }
    }
}
