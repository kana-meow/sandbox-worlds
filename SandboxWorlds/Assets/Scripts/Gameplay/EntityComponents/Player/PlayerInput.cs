using Unity.Cinemachine;
using UnityEngine;

namespace Base.Component {

    public class PlayerInput : EntityComponent {
        private Movement movement;
        private Look look;

        public override void OnInitialize() {
            movement = GetComponent<Movement>();
            look = GetComponent<Look>();

            // temporary camera logic

            CinemachineBrain cineBrain = Camera.main.GetComponent<CinemachineBrain>();
            CinemachineCamera cam = (CinemachineCamera)cineBrain.ActiveVirtualCamera;
            cam.Follow = transform;

            cam.GetComponent<CinemachineFollow>().FollowOffset = Vector3.up * 1.8f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            // new input system later

            Vector3 normalizedMoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            Vector3 forward = transform.forward;
            forward.y = 0;  // Make sure the Y axis is ignored, as we don't want vertical movement

            // Get the camera's right direction (ignoring the Y component)
            Vector3 right = transform.right;
            right.y = 0;  // Again, ignore the Y axis

            // Calculate the movement direction based on the camera's facing
            Vector3 moveDirection = forward * normalizedMoveDir.z + right * normalizedMoveDir.x;

            movement.Move(moveDirection);
            look.LookTowardsInstant(Camera.main.transform.forward);
        }
    }
}