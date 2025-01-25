using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public class BaseEntity : MonoBehaviour {
    public EntityData data;
    public Guid guid;

    public event Action OnInitializeComponent;

    private void Start() {
        Initialize("passive_animal");
    }

    public void Initialize(string entityID) {
        // temporary, will search inside default behavior pack based on "base" namespace later
        if (!JSONDeserializer.TryGetFromJson<EntityData>(Application.dataPath + "/behavior_packs/default/" + entityID + ".json", out data)) {
            Destroy(gameObject);
            return;
        }
        guid = Guid.NewGuid();

        EntityComponentFactory.AddEntityComponents(this, data.Components);
    }

    public void InitializeComponents() {
        OnInitializeComponent?.Invoke();
    }
}

[System.Serializable]
public class EntityData {

    [JsonProperty("entityID")]
    public string EntityID { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("category")]
    [JsonConverter(typeof(StringEnumConverter))]
    public EntityCategory Category { get; set; }

    [JsonProperty("components")]
    public Dictionary<string, Dictionary<string, object>> Components { get; set; }

    public enum EntityCategory {
        Animal,
        Monster,
        Boss,
        Player,
    }
}