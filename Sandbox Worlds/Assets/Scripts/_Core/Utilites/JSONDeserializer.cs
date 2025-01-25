using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class JSONDeserializer {

    // deserializes object of type 'T' from .json file path
    public static bool TryGetFromJson<T>(string filePath, out T value) {
        if (File.Exists(filePath)) {
            if (Path.GetExtension(filePath).ToLower() == ".json") {
                string json = File.ReadAllText(filePath);
                value = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
        }

        Debug.LogError($"[JSONDeserializer] File '{filePath}' could not be deserialized.");
        value = default;
        return false;
    }
}