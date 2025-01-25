using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using Unity.VisualScripting;

namespace Base {

    public class BaseEntity : MonoBehaviour {
        public Entity data;
        public Guid guid;

        public event Action OnInitializeComponent;

        private void Start() {
            Initialize("passive_animal");
        }

        public void Initialize(string entityID) {
            // temp for testing
            string path = $"{Application.streamingAssetsPath}/behavior_packs/default/entities/{entityID}.json";
            if (!Utils.JsonDeserializer.TryParseJson<Entity>(path, out data)) {
                Destroy(gameObject);
            }

            guid = Guid.NewGuid();

            EntityComponentFactory.AddEntityComponents(this, data.Components);
        }

        public void InitializeComponents() {
            OnInitializeComponent?.Invoke();
        }
    }

    public class Entity {

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("description")]
        public EntityDescription Description { get; set; }

        public class EntityDescription {

            [JsonProperty("display_name")]
            public string DisplayName { get; set; }

            [JsonProperty("category")]
            [JsonConverter(typeof(StringEnumConverter))]
            public EntityCategory Category { get; set; }

            [JsonProperty("is_spawnable")]
            public bool IsSpawnable { get; set; }

            [JsonProperty("is_summonable")]
            public bool IsSummonable { get; set; }

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
}