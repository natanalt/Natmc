using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Json
{
    public static class JsonValidator
    {
        public static void ValidateWithException(JToken t, object schema, string messagePrefix = "")
        {
            if (!Validate(t, schema, out string errorMessage))
                throw new ValidationException(messagePrefix + " " + errorMessage);
        }

        public static bool Validate(JToken t, object schema, out string errorMessage)
            => ValidateInner(t, schema, t.Path, out errorMessage);

        private static bool ValidateInner(
            JToken t,
            object schema,
            string path,
            out string errorMessage)
        {
            if (!ValidateType(t, schema, path))
            {
                errorMessage = $"Expected {path} to be {schema}";
                return false;
            }

            if (schema is ObjectSchema)
            {
                var objectSchema = schema as ObjectSchema;
                var obj = t.Value<JObject>();

                foreach (var kv in objectSchema)
                {
                    if (!ValidateInner(
                        obj[kv.Key],
                        kv.Value,
                        $"{path}{(path.Length == 0 ? "" : ".")}{kv.Key}",
                        out errorMessage))
                        return false;
                }
            }
            else if (schema is ArraySchema)
            {
                var arraySchema = schema as ArraySchema;
                var array = t.Value<JArray>();
                for (var i = 0; i < array.Count; i += 1)
                {
                    var element = array[i];
                    if (!ValidateInner(
                        element,
                        arraySchema.Schema,
                        $"{path}{(path.Length == 0 ? "" : ".")}{i}",
                        out errorMessage))
                        return false;
                }
            }

            errorMessage = null;
            return true;
        }

        private static bool ValidateType(JToken t, object schema, string path)
        {
            if (schema is ObjectSchema)
            {
                return t.Type == JTokenType.Object;
            }
            else if (schema is ArraySchema)
            {
                return t.Type == JTokenType.Array;
            }
            else if (schema is string)
            {
                var schemaString = schema as string;
                var possibleTypes = schemaString.Split('|');
                var typeMatched = false;
                foreach (var type in possibleTypes)
                {
                    if (typeMatched)
                        break;
                    switch (type)
                    {
                        case "string":
                            typeMatched = t.Type == JTokenType.String;
                            break;
                        case "number":
                            typeMatched = t.Type == JTokenType.Integer || t.Type == JTokenType.Float;
                            break;
                        case "bool":
                            typeMatched = t.Type == JTokenType.Boolean;
                            break;
                        case "null":
                            typeMatched = t.Type == JTokenType.Null;
                            break;
                    }
                }
                return typeMatched;
            }
            throw new InvalidOperationException($"Invalid schema for {path}");
        }
    }
}
