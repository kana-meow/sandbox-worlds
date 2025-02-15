using UnityEngine;

namespace Base.Component {

    [RequireComponent(typeof(Rigidbody))]
    public class Movement : EntityComponent {
        private Rigidbody rb;

        public float moveSpeed = 5f;

        private void OnEnable() {
            // get and adjust rigidbody
            rb = GetComponent<Rigidbody>();
        }

        public void MoveInDirection(Vector3 direction) {
            Vector3 targetPosition = rb.position + Time.deltaTime * moveSpeed * direction.normalized;
            rb.MovePosition(targetPosition);
        }
    }
}