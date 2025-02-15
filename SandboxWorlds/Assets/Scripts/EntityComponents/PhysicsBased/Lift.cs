using UnityEngine;

namespace Base.Component {

    [RequireComponent(typeof(Rigidbody))]
    public class Lift : EntityComponent, IInteractable {
        private PlayerEntity player;
        private Rigidbody rb;

        [Header("Lift Settings")]
        [SerializeField] private float liftDistance = 2;
        [SerializeField] private float liftSpeed = 20;

        private void Start() {
            enabled = false;
            rb = GetComponent<Rigidbody>();
        }

        public void Interact(PlayerEntity player) {
            if (enabled == true) {
                StopLifting();
            } else {
                StartLifting(player);
            }
        }

        public void InteractAlt(PlayerEntity player) {
            StopLifting();
        }

        private void StartLifting(PlayerEntity player) {
            enabled = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            this.player = player;
        }

        private void StopLifting() {
            rb.useGravity = true;
            player = null;
            enabled = false;
        }

        private void Update() {
            if (player != null) {
                rb.MovePosition(Vector3.Lerp(transform.position, player.PlayerPos + Camera.main.transform.forward * liftDistance, liftSpeed * Time.deltaTime));
            } else {
                StopLifting();
            }
        }
    }
}