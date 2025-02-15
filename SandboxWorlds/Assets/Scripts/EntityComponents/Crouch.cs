using UnityEngine;

namespace Base.Component {

    [RequireComponent(typeof(CapsuleCollider))]
    public class Crouch : EntityComponent {
        public float defaultHeight = 2;
        public float crouchHeight = 1.75f;

        public bool isCrouching = false;

        public void UpdateCrouch() {
            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            collider.height = isCrouching ? defaultHeight : crouchHeight;
            isCrouching = !isCrouching;
        }
    }
}