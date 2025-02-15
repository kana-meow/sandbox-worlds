using Unity.Cinemachine;
using UnityEngine;

namespace Base.Component {

    [RequireComponent(typeof(Movement), typeof(UnityEngine.InputSystem.PlayerInput))]
    public class PlayerInput : EntityComponent {
        private Movement movement;

        private void Start() {
            movement = GetComponent<Movement>();

            // get cinemachine camera
            CinemachineBrain cineBrain = Camera.main.GetComponent<CinemachineBrain>();
            CinemachineCamera cam = (CinemachineCamera)cineBrain.ActiveVirtualCamera;

            // set follow
            cam.Follow = transform;
            cam.GetComponent<CinemachineFollow>().FollowOffset = Vector3.up * 1.8f;

            // hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update() {
            // rotate towards camera view
            float currentX = transform.eulerAngles.x;
            float currentZ = transform.eulerAngles.z;

            Vector3 dir = Camera.main.transform.forward;

            float targetY = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(currentX, targetY, currentZ);

            // new input system will be added later on
            Vector3 normalizedMoveDirection = new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            // get forward and right direction while ignoring the y
            Vector3 forward = transform.forward;
            forward.y = 0;

            Vector3 right = transform.right;
            right.y = 0;

            // calculate move direction based on where the transform is looking towards
            Vector3 moveDirection = right * normalizedMoveDirection.x + forward * normalizedMoveDirection.z;

            movement.MoveInDirection(moveDirection);
        }
    }
}