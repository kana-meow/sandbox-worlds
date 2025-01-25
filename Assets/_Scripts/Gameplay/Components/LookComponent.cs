using System.Collections;
using UnityEngine;

public class LookComponent : _BaseComponent {

    public override void OnInitialize() {
    }

    public float lookSpeed = 1.8f; // Base speed for all rotations

    private Quaternion nextRandomRotation;

    public void LookAt(Vector3 pos) {
        Vector3 dir = pos - transform.position;
        dir.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(dir);

        // Use Slerp for smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
    }

    public void RandomLook(System.Action onComplete = null) {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        nextRandomRotation = Quaternion.LookRotation(randomDirection);

        StartCoroutine(UpdateRandomLook(onComplete));
    }

    private IEnumerator UpdateRandomLook(System.Action onComplete) {
        // calculate the total rotation angle
        float angleToRotate = Quaternion.Angle(transform.rotation, nextRandomRotation);

        // adjust RotateTowards speed to match Slerp's perceived speed
        float adjustedSpeed = angleToRotate / (1f / lookSpeed);

        while (Quaternion.Angle(transform.rotation, nextRandomRotation) > 0.1f) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, nextRandomRotation, Time.deltaTime * adjustedSpeed);
            yield return null;
        }

        transform.rotation = nextRandomRotation; // Ensure final rotation matches target
        onComplete?.Invoke(); // Notify when rotation is complete
    }

    public void RotateTowards(Vector3 forward) {
        transform.rotation = Quaternion.LookRotation(forward);
    }
}