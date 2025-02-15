using UnityEngine;

namespace Base.Component {

    public class Attack : MonoBehaviour {
        public float attackCooldown = 1f; // cooldown in seconds
        public int damage = 2;
        private float lastAttack = -Mathf.Infinity; // time since last attack

        public void TryAttacking(Health health) {
            float currentTime = Time.time;

            if (currentTime - lastAttack >= attackCooldown) {
                PerformAttack(health);
                lastAttack = currentTime;
            }
        }

        private void PerformAttack(Health health) {
            health.Damage(damage);
        }
    }
}