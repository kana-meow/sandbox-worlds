using UnityEngine;

namespace Base.Component {

    [RequireComponent(typeof(Rigidbody))]
    public class Push : EntityComponent, IInteractable {
        private Rigidbody rb;

        [Header("Push Settings")]
        [SerializeField] private float pushForce = 5;

        private void Start() {
            rb = GetComponent<Rigidbody>();
        }

        public void Interact(PlayerEntity player) {
        }

        public void InteractAlt(PlayerEntity player) {
            PerformPush();
        }

        private void PerformPush() {
            rb.AddForce(Vector3.up * 1.8f + Camera.main.transform.forward * pushForce, ForceMode.Impulse);
        }
    }
}