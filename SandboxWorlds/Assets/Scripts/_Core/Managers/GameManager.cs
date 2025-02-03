using Newtonsoft.Json;
using UnityEngine;

namespace Base {

    public class GameManager : MonoBehaviour {

        private void Awake() {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                Converters = { new Utils.Vector3Converter(), new Utils.Vector2Converter() }
            };
        }
    }
}