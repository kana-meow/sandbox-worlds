using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Base.Component {

    public class Health : _BaseComponent {

        [JsonProperty("value")]
        [JsonConverter(typeof(HealthValueConverter))]
        public int value = 20;

        public int max = -1;

        public override void OnInitialize() {
            if (max == -1) {
                max = value;
            }
        }

        protected class HealthValueConverter : JsonConverter {

            public override bool CanConvert(Type objectType) {
                return objectType == typeof(int);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                var token = JToken.Load(reader);

                if (token.Type == JTokenType.Object) {
                    // if it's an object, read min_value and max_value
                    var min = token["min_value"]?.Value<int>() ?? -1;
                    var max = token["max_value"]?.Value<int>() ?? -1;

                    if (min != -1 && max != -1) {
                        return UnityEngine.Random.Range(min, max);
                    }
                } else if (token.Type == JTokenType.Integer) {
                    // if it's just an int, return it
                    return token.Value<int>();
                }

                throw new JsonSerializationException("Invalid format for 'value' in Health Component.");
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                writer.WriteValue(value);
            }
        }
    }
}