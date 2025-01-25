using UnityEngine;

public class HealthComponent : _BaseComponent {
    [SerializeField] private float _health;

    public float Health {
        get {
            return _health;
        }
        set {
            float previousHealth = _health;
            float clampedValue = Mathf.Clamp(value, 0, maxHealth);

            if (clampedValue != previousHealth) {
                _health = clampedValue;

                /*
                if (clampedValue <= 0) {
                    OnDeath?.Invoke();
                } else if (clampedValue > previousHealth) {
                    OnHealthIncreased?.Invoke();
                } else {
                    OnHealthDecreased?.Invoke();
                }*/
            }
        }
    }

    public float maxHealth;
    public float regenPerSecond;

    public override void OnInitialize() {
    }
}