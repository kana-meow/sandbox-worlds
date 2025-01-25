using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveComponent : _BaseComponent {
    public float moveSpeed = 3;

    private NavMeshAgent _agent;

    private float forwardTimer = 0;
    private float walkTime = 0;

    private NavMeshAgent Agent {
        get {
            if (_agent == null) {
                _agent = GetComponent<NavMeshAgent>();
                _agent.speed = moveSpeed;
            }
            return _agent;
        }
    }

    public override void OnInitialize() {
    }

    public void MoveTo(Vector3 pos) {
        Agent.SetDestination(pos);

        if (!Agent.pathPending && Agent.pathStatus != NavMeshPathStatus.PathComplete) {
            Debug.LogWarning("Invalid or incomplete path to target position!");
            Agent.ResetPath();
        }
    }

    public void MoveForwardForSeconds(float time) {
        forwardTimer = 0;
        walkTime = time;

        StartCoroutine(UpdateWalkForwards());
    }

    private IEnumerator UpdateWalkForwards() {
        while (forwardTimer < walkTime) {
            forwardTimer += Time.deltaTime;
            MoveTo(transform.position + transform.forward * 2);
            yield return null;
        }
        ResetPath();
    }

    public void MoveByInput(Vector2 input, Rigidbody rb) {
        Vector3 inputDirection = new Vector3(input.x, 0, input.y);

        Vector3 moveDirection = rb.transform.TransformDirection(inputDirection);

        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);
    }

    public void ResetPath() {
        Agent.ResetPath();
    }

    public bool IsMoving() {
        return Agent.velocity.magnitude! > 0.1;
    }
}