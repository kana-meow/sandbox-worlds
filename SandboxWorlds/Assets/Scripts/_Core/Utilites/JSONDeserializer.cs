using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class JSONDeserializer {

    // deserializes object of type 'T' from .json file path
    public static bool TryGetFromJson<T>(string filePath, out T value) {
        if (File.Exists(filePath)) {
            if (Path.GetExtension(filePath).ToLower() == ".json") {
                try {
                    string json = File.ReadAllText(filePath);
                    value = JsonConvert.DeserializeObject<T>(json);
                    return true;
                }
                catch (JsonException e) {
                    Debug.LogError($"[JSONDeserializer] Error deserializing file '{filePath}': {e.Message}.");
                }
                catch (System.Exception e) {
                    Debug.LogError($"[JSONDeserializer] Unexpected error when trying to deserialize file '{filePath}': {e.Message}.");
                }
            } else {
                Debug.LogError($"[JSONDeserializer] File '{filePath}' is not a '.json' file.");
            }
        } else {
            Debug.LogError($"[JSONDeserializer] File '{filePath}' could not be found.");
        }

        value = default;
        return false;
    }
}