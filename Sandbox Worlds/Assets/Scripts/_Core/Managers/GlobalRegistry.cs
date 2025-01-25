using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GlobalRegistry : Singleton<GlobalRegistry> {
    private Dictionary<string, EntityData> entityEntries = new();

    protected override void Awake() {
        base.Awake();
        string entitesJsonPath = Application.streamingAssetsPath + "/entities.json";
        string json;

        // check if entities.json exists, then save file contents into a string
        if (File.Exists(entitesJsonPath)) {
            json = File.ReadAllText(entitesJsonPath);
        } else {
            Debug.LogError($"[GlobalRegistry] entities.json file could not be found or doesn't exist! (Please make sure file path is corret '{entitesJsonPath}')");
            return;
        }

        // convert entities.json to EntityRoot
        EntityRoot data = JsonConvert.DeserializeObject<EntityRoot>(json);

        // add each entity as a registry entry (<entityID, Entity variable>)
        foreach (EntityData entity in data.Entities) {
            if (!entityEntries.ContainsKey(entity.EntityID)) {
                entityEntries.Add(entity.EntityID, entity);
            } else {
                Debug.LogWarning($"[GlobalRegistry] Registry tried to add duplicate entry for '{entity.EntityID}'!");
            }
        }

        /* Debug Stuff
        foreach (var entity in data.Entities) {
            Debug.Log($"Entity ID: '{entity.EntityID}', Display Name: '{entity.DisplayName}'");

            foreach (var component in entity.Components) {
                Debug.Log($"   Component: '{component.Key}'");

                foreach (var field in component.Value) {
                    Debug.Log($"      {field.Key}: {field.Value}");
                }
            }
        }*/
    }

    public bool TryGetEntity(string entityID, out EntityData data) {
        if (entityEntries.TryGetValue(entityID, out data)) return true;
        else {
            Debug.LogWarning($"[GlobalRegistry] Entity with ID '{entityID}' could not be found! (Did you type the name correctly?)");
            return false;
        }
    }
}

public class EntityRoot {

    [JsonProperty("entities")]
    public List<EntityData> Entities { get; set; }
}