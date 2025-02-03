using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Base {

    public class Utils {

        public static object ConvertToType(object value, Type targetType) {
            if (value == null) return null;

            // Handle List<T>
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)) {
                Type elementType = targetType.GetGenericArguments()[0]; // Get the type of T in List<T>
                var list = Activator.CreateInstance(targetType) as System.Collections.IList;

                if (value is IEnumerable<object> enumerable) {
                    foreach (var item in enumerable) {
                        list.Add(Convert.ChangeType(item, elementType));
                    }
                }

                return list;
            }

            // Handle other types (primitive and compatible types)
            return Convert.ChangeType(value, targetType);
        }

        public static Type GetTypeFromString(string input) {
            if (string.IsNullOrEmpty(input)) {
                return null;
            }

            string lowercase = input.ToLower();
            string typeString = ToTypeName(lowercase);

            return Type.GetType(typeString);
        }

        public static string ToTypeName(string input) {
            // split input by periods
            string[] parts = input.Split('.');
            // go through each part
            for (int i = 0; i < parts.Length; i++) {
                if (!string.IsNullOrWhiteSpace(parts[i])) {
                    // split each part by underscores
                    string[] subParts = parts[i].Split('_');
                    // go through each subPart
                    for (int j = 0; j < subParts.Length; j++) {
                        if (!string.IsNullOrWhiteSpace(subParts[j])) {
                            // capitalize the first letter of each subpart
                            subParts[j] = char.ToUpper(subParts[j][0]) + subParts[j].Substring(1);
                        }
                    }

                    // join the subparts back together without underscores
                    parts[i] = string.Join("", subParts);

                    // handle special cases (e.g., "AI")
                    if (parts[i].ToUpper() == "AI") {
                        parts[i] = parts[i].ToUpper();
                    }
                }
            }

            // join parts back together with periods for namespaces
            return string.Join(".", parts);
        }

        public static class JsonDeserializer {

            public static bool TryParseJson(string filePath, out object value) {
                // ensure file exists
                if (!File.Exists(filePath)) {
                    Debug.LogError($"[JsonDeserializer] File '{filePath}' doesn't exist or couldn't be found. Make sure you're using a valid file path.");
                    value = default;
                    return false;
                }

                // ensure file is a .json file
                if (Path.GetExtension(filePath).ToLower() != ".json") {
                    Debug.LogError($"[JsonDeserializer] Invalid file type. Please make sure '{filePath}' is a .json file.");
                    value = default;
                    return false;
                }

                // convert .json file to string and parse it to JObject
                string json = File.ReadAllText(filePath);
                JObject root = JObject.Parse(json);

                // will be used later to check version compatabillity
                string formatVersion = root["format_version"]?.ToString();

                // go through each JProperty inside JsonObjectRoot
                foreach (JProperty property in root.Properties()) {
                    // ignore format version
                    if (property.Name != "format_version") {
                        // get type from property name
                        string key = property.Name;
                        Type targetType = GetTypeFromString(key);

                        if (targetType != null) {
                            // return the JProperty as object
                            var deserializedObject = property.Value.ToObject(targetType);
                            value = deserializedObject;
                            return true;
                        } else {
                            Debug.LogError($"[JsonDeserializer] '{property.Name}' could not be parsed to '{targetType.Name}'.");
                        }
                    }
                }

                value = default;
                return false;
            }

            // type specific TryParseJson
            public static bool TryParseJson<T>(string filePath, out T value) {
                if (TryParseJson(filePath, out object newValue)) {
                    value = (T)newValue;
                    return true;
                } else {
                    value = default;
                    return false;
                }
            }
        }

        public class JsonObjectRoot {

            [JsonProperty("format_version")]
            public string FormatVersion { get; set; }

            [JsonProperty("value")]
            public JObject Value { get; set; }
        }

        public class Vector3Converter : JsonConverter {

            public override bool CanConvert(Type objectType) {
                return objectType == typeof(Vector3);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                Vector3 vector = (Vector3)value;
                writer.WriteStartArray();
                writer.WriteValue(vector.x);
                writer.WriteValue(vector.y);
                writer.WriteValue(vector.z);
                writer.WriteEndArray();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                JArray array = JArray.Load(reader);
                return new Vector3(array[0].ToObject<float>(), array[1].ToObject<float>(), array[2].ToObject<float>());
            }
        }

        public class Vector2Converter : JsonConverter {

            public override bool CanConvert(Type objectType) {
                return objectType == typeof(Vector2);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                Vector2 vector = (Vector2)value;
                writer.WriteStartArray();
                writer.WriteValue(vector.x);
                writer.WriteValue(vector.y);
                writer.WriteEndArray();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                JArray array = JArray.Load(reader);
                return new Vector2(array[0].ToObject<float>(), array[1].ToObject<float>());
            }
        }
    }
}