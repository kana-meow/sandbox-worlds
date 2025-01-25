using UnityEngine;
using UnityEngine.InputSystem;

public class JumpComponent : _BaseComponent {
    public float jumpForce = 2;

    public override void OnInitialize() {
    }

    public void PerformJump(InputAction.CallbackContext context, Rigidbody rb) {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}