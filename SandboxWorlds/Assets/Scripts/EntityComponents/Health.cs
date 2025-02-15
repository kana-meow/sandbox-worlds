using UnityEngine;

namespace Base.Component {

    public class Health : MonoBehaviour {
        [SerializeField] private float _currentHealth;
        [SerializeField] private float maxHealth;

        public float CurrentHealth {
            get {
                return _currentHealth;
            }
            set {
                _currentHealth = Mathf.Clamp(value, 0, maxHealth);
            }
        }

        public void Damage(float amount) {
            CurrentHealth -= amount;

            if (CurrentHealth == 0) {
                /*if (TryGetComponent<DeathOverride>(out DeathOverride death)) {
                    death.PerformDeath();
                }*/

                Destroy(gameObject);
            }
        }
    }
}