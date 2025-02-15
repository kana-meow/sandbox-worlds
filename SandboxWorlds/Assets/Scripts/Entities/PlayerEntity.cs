using UnityEngine;
using UnityEngine.InputSystem;

namespace Base {

    public class PlayerEntity : Entity {

        [Header("Interaction Settings")]
        public float interactRange = 2;
        [SerializeField] private LayerMask interactionMask;

        [Header("Ground Check Settings")]
        [SerializeField] private bool isGrounded = false;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckLength = 0.4f;

        [HideInInspector] public Vector3 cameraForward;

        private PlayerInput playerInput;
        private bool hasObjectInRange = false;
        private GameObject targetObject;

        public Vector3 PlayerPos {
            get {
                return transform.position + Vector3.up * 1.8f; // player height
            }
        }

        private void Start() {
            playerInput = GetComponent<PlayerInput>();
        }

        public void Interact() {
            if (hasObjectInRange) {
                if (targetObject.TryGetComponent<Component.Interact>(out Component.Interact interact)) {
                    interact.OnInteract?.Invoke(this);
                } else if (targetObject.TryGetComponent<Component.Health>(out Component.Health health)) {
                    // else if to avoid attacking interactables. This will be replaced when items and weapons get added
                    GetComponent<Component.Attack>().TryAttacking(health);
                }
            }
        }

        public void InteractAlt() {
            if (hasObjectInRange) {
                if (Input.GetMouseButtonDown(1)) {
                    if (targetObject.TryGetComponent<Component.Interact>(out Component.Interact interact))
                        interact.OnInteractAlt?.Invoke(this);
                }
            }
        }

        private void Update() {
            cameraForward = Camera.main.transform.forward;

            // check if raycast hits something on the interaction mask
            if (Physics.Raycast(PlayerPos, cameraForward, out RaycastHit hit, interactRange, interactionMask)) {
                hasObjectInRange = true;
                targetObject = hit.transform.gameObject;
            } else {
                hasObjectInRange = false;
                targetObject = null;
            }

            isGrounded = Physics.Raycast(transform.position, Vector3.down * groundCheckLength, out RaycastHit groundHit, groundLayer);

            if (isGrounded) {
                float slopeAngle = Vector3.Angle(groundHit.normal, Vector3.up);
                Debug.Log(slopeAngle);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(PlayerPos, transform.position + transform.up * 1.8f + cameraForward * interactRange);

            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckLength);
        }
    }
}