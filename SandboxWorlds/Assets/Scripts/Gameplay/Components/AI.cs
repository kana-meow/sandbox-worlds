using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Component {

    public class AI : _BaseComponent {

        [JsonProperty("behavior")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BehaviorType behavior;

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