using Newtonsoft.Json;
using UnityEngine;

namespace Base {

    public class GameManager : MonoBehaviour {

        private void Awake() {
            // initialize converters for Vector3 and Vector2
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                Converters = { new Utils.Vector3Converter(), new Utils.Vector2Converter() }
            };
        }
    }
}