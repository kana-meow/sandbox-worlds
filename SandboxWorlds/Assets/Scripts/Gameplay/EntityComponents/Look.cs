using UnityEngine;

namespace Base.Component {

    public class Look : EntityComponent {

        public override void OnInitialize() {
        }

        public void LookTowardsInstant(Vector3 dir) {
            float currentX = transform.eulerAngles.x;
            float currentZ = transform.eulerAngles.z;

            float targetY = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(currentX, targetY, currentZ);
        }
    }
}