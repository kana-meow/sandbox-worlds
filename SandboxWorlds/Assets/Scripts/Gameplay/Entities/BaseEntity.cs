using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using Unity.VisualScripting;

namespace Base {

    public class BaseEntity : MonoBehaviour {
        public EntityData data;
        public Guid guid;

        public event Action OnInitializeComponent;

        private void Start() {
            Initialize("passive_animal");
            Debug.Log(data.Identifier);
        }

        public void Initialize(string entityID) {
            // temporary, will search inside default behavior pack based on "base" namespace later
            if (!JSONDeserializer.TryGetFromJson($"{Application.streamingAssetsPath}/behavior_packs/default/entities/{entityID}.json", out data)) {
                Destroy(gameObject);
                return;
            }
            guid = Guid.NewGuid();

            //EntityComponentFactory.AddEntityComponents(this, data.Components);
        }

        public void InitializeComponents() {
            OnInitializeComponent?.Invoke();
        }
    }
}

[System.Serializable]
public class EntityJson {

    [JsonProperty("format_version")]
    public string FormatVersion { get; set; }

    [JsonProperty("base.entity")]
    public EntityData Entity { get; set; }
}

public class EntityData {

    [JsonProperty("identifier")]
    public string Identifier { get; set; }

    [JsonProperty("description")]
    public Description _Description { get; set; }

    public class Description {

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("category")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityCategory Category { get; set; }

        public enum EntityCategory {
            None,
            Animal,
            Monster,
            Boss,
            Player
        }
    }

    [JsonProperty("components")]
    public Dictionary<string, Dictionary<string, object>> Components { get; set; }
}