using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerComponent : _BaseComponent {
    private PlayerInput input;
    private InputActionAsset inputActions;

    private Rigidbody rb;

    private Vector2 moveInput;

    private Transform cameraTransform;

    public override void OnInitialize() {
        cameraTransform = Camera.main.transform;

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;

        input = gameObject.AddComponent<PlayerInput>();
        inputActions = Resources.Load<InputActionAsset>("InputSystem_Actions");

        input.actions = inputActions;
        input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;

        input.actions["Jump"].performed += HandleJump;

        UnityEngine.AI.NavMeshObstacle obstacle = gameObject.AddComponent<UnityEngine.AI.NavMeshObstacle>();
        obstacle.size = new Vector3(Entity.Body.width, Entity.Body.height, Entity.Body.width);

        ICinemachineCamera activeCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        if (activeCam is CinemachineCamera vCam) {
            GameObject cameraView = new("Camera View");
            cameraView.transform.parent = gameObject.transform;
            cameraView.transform.localPosition = Vector3.up * (Entity.Body.height * 0.9f); // put camera view 90% of height of player
            vCam.Follow = cameraView.transform;
        }

        // temp
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start() {
        Entity.Body.ChangeMeshColor(Color.red);
    }

    private void HandleJump(InputAction.CallbackContext context) {
        Entity.Jump.PerformJump(context, rb);
    }

    private void Update() {
        moveInput = input.actions["Move"].ReadValue<Vector2>();
        Entity.Move.MoveByInput(moveInput, rb);
        RotatePlayerTowardsCamera();
    }

    private void RotatePlayerTowardsCamera() {
        if (cameraTransform == null) return;

        // Get the camera's forward direction, projected on the horizontal plane
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Ignore vertical direction
        cameraForward.Normalize();

        // Directly set the player's rotation to face the camera's forward direction
        Entity.Look.RotateTowards(cameraForward);
    }
}