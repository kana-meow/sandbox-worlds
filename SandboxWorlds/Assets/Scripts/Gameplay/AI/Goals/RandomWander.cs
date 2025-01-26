using UnityEngine;

namespace Base.AI.Goal {

    public class RandomWander : BaseGoal {

        public RandomWander(BaseEntity entity) : base(entity) {
        }

        public override int Priority => throw new System.NotImplementedException();

        public override bool IsReplacable => throw new System.NotImplementedException();

        public override EntityControls[] Controls => throw new System.NotImplementedException();

        public override void Activate() {
            throw new System.NotImplementedException();
        }

        public override bool CanActivate() {
            throw new System.NotImplementedException();
        }

        public override void Deactivate() {
            throw new System.NotImplementedException();
        }

        public override void UpdateGoal() {
            throw new System.NotImplementedException();
        }
    }
}