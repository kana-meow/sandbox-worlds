using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using System;

namespace Base {

    public class Utils : MonoBehaviour {

        public static Type GetTypeFromString(string input) {
            if (string.IsNullOrEmpty(input)) {
                return null;
            }

            string lowercase = input.ToLower();
            string typeString = ToTypeName(lowercase);

            return Type.GetType(typeString);
        }

        // makes first letter and each letter after '.' capitalized to match type names e.g.: (base.entity => Base.Entity)
        private static string ToTypeName(string input) {
            string[] parts = input.Split('.');
            for (int i = 0; i < parts.Length; i++) {
                if (!string.IsNullOrWhiteSpace(parts[i])) {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1);
                }
            }

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
    }

    public class JsonObjectRoot {

        [JsonProperty("format_version")]
        public string FormatVersion { get; set; }

        [JsonProperty("value")]
        public JObject Value { get; set; }
    }
}