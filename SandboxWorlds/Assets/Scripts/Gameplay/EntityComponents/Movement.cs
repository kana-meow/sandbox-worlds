using UnityEngine;

namespace Base.Component {

    [RequireComponent(typeof(Rigidbody))]
    public class Movement : EntityComponent {
        private Rigidbody rb;

        // move speed
        public float value = 5;

        public override void OnInitialize() {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void Move(Vector3 direction) {
            Vector3 targetPosition = rb.position + Time.deltaTime * value * direction.normalized;
            rb.MovePosition(targetPosition);
        }
    }
}