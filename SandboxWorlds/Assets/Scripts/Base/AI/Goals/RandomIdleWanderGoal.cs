using UnityEngine;

namespace Base {

    public class RandomIdleWanderGoal : _BaseGoal {

        public RandomIdleWanderGoal(BaseEntity entity) : base(entity) {
        }

        public override int Priority => 5;

        public override bool IsReplacable => true;

        public override EntityControls[] Controls => new EntityControls[] {
        EntityControls.Move,
        EntityControls.Look,
    };

        // timer for next wander
        private float wanderTimer;

        // in what time range the next wander is (in seconds)
        private readonly float minWanderDelay = 5f;
        private readonly float maxWanderDelay = 12f;
        private float nextWanderTime;

        // timer for how long to walk
        private float walkTimer;

        // time range for how long to walk (in seconds)
        private readonly float minWalkTime = 1f;
        private readonly float maxWalkTime = 5f;
        private float nextWalkTime;

        public override bool CanActivate() {
            return true; // set to true because goal is low priority anyways and will be replaced if needed
        }

        public override void Activate() {
            nextWanderTime = Random.Range(minWanderDelay, maxWanderDelay);

            nextWalkTime = Random.Range(minWalkTime, maxWalkTime);
        }

        public override void UpdateGoal() {
            // if walk timer is bigger or equal to next walk time
            if (walkTimer >= nextWalkTime) {
                // increment the wander timer
                //Debug.Log($"Wander: {wanderTimer} ({nextWanderTime})");
                wanderTimer += Time.deltaTime;

                // if wander timer is bigger or equal to the next wander time
                if (wanderTimer >= nextWanderTime) {
                    // reset timer
                    wanderTimer = 0;
                    // randomize next wander time
                    nextWanderTime = Random.Range(minWanderDelay, maxWanderDelay);

                    /*
                    // perform random look in the entity's Look
                    entity.Look.RandomLook(() => {
                        // reset timer
                        walkTimer = 0;
                        // randomize next walk time
                        nextWalkTime = Random.Range(minWalkTime, maxWalkTime);
                        // make entity move forward for the walk time
                        entity.Move.MoveForwardForSeconds(nextWalkTime);
                    });*/
                }
            } else {
                // count up until the next walk time
                //Debug.Log($"Walk: {walkTimer} ({nextWalkTime})");
                walkTimer += Time.deltaTime;
            }
        }

        public override void Deactivate() {
            throw new System.NotImplementedException();
        }
    }
}