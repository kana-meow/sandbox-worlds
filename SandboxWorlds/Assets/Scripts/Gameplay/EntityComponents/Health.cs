using Newtonsoft.Json;
using UnityEngine;

namespace Base.Component {

    public class Health : EntityComponent {

        [JsonConverter(typeof(Utils.IntRangeConverter))]
        public int value = 20;

        public int max = -1;

        public override void OnInitialize() {
            // if max health isn't set, make it same as value
            if (max == -1) {
                max = value;
            }

            // if max health is set, but value is above it, set value to max
            if (max != -1 && value > max) {
                value = max;
            }
        }
    }
}