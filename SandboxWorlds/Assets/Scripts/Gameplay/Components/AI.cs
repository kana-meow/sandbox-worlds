using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

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