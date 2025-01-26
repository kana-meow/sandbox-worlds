using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Component {

    public class AI : _BaseComponent {
        // both of these will be temprorarily stored as strings

        [JsonProperty("behavior")]
        public string _behavior;

        public BehaviorType Behavior {
            get {
                return (BehaviorType)System.Enum.Parse(typeof(BehaviorType), _behavior);
            }
        }

        [JsonProperty("goals")]
        public List<string> goals;

        public override void OnInitialize() {
        }

        public enum BehaviorType {
            None,
            Passive,
            Neutral,
            Agressive
        }
    }
}