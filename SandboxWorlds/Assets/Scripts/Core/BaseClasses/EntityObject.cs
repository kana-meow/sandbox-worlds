using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json.Linq;

namespace Base {

    public class EntityObject : MonoBehaviour {

        [JsonProperty("base.entity")]
        public Entity data;
        public string guid;

        public event Action OnInitializeComponent;

        // for pre-alpha testing only
        public string ID;

        private void Start() {
            // preset identifier for testing purposes
            Initialize(ID);
        }

        public void Initialize(string entityID) {
            // folder location will likely change in future
            string path = $"{Application.streamingAssetsPath}/behavior_packs/default/entities/{entityID}.json";

            // try to get entity data, destroy entity if unsuccessfull to avoid having bugged/glitched entities walking around
            if (!Utils.Json.TryParseJson<Entity>(path, out data)) {
                Destroy(gameObject);
                return;
            }

            // create new global unique identifier
            guid = Guid.NewGuid().ToString();

            // create all components stored inside the data
            Factories.EntityComponents.AddEntityComponents(this, data.Components);
        }

        public void InitializeComponents() {
            OnInitializeComponent?.Invoke();
        }
    }

    public class Entity {

        [JsonProperty("description")]
        public EntityDescription Description { get; set; }

        public class EntityDescription {

            [JsonProperty("identifier")]
            public string Identifier { get; set; }

            [JsonProperty("display_name")]
            public string DisplayName { get; set; }

            [JsonProperty("type")]
            [JsonConverter(typeof(StringEnumConverter))]
            public EntityType Type { get; set; }

            [JsonProperty("category")]
            [JsonConverter(typeof(StringEnumConverter))]
            public EntityCategory Category { get; set; }

            public enum EntityType {
                none,
                passive,
                neutral,
                agressive
            }

            public enum EntityCategory {
                none,
                animal,
                monster,
                boss,
                player
            }
        }

        [JsonProperty("components")]
        public Dictionary<string, JObject> Components { get; set; }
    }
}