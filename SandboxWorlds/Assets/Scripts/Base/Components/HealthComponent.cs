using UnityEngine;

namespace Base {

    public class HealthComponent : _BaseComponent {
        [SerializeField] private int value = 20;
        private int min_value = -1;
        private int max_value = -1;

        private int max = -1;

        public override void OnInitialize() {
            if (min_value != -1 && max_value != -1) {
                value = Random.Range(min_value, max_value);
            }

            if (max == -1) {
                max = value;
            }
        }
    }
}