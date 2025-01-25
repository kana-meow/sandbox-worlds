using UnityEngine;

namespace Base.Component {

    public class Health : _BaseComponent {
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

        private class HealthValue {
            public int value = -1;
            public int min_value = -1;
            public int max_value = -1;

            public static implicit operator HealthValue(int value) {
                return new HealthValue {
                    value = value,
                };
            }
        }
    }
}