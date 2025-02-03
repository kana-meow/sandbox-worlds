using UnityEngine;

namespace Base.AI {

    public abstract class Goal {
        public abstract int Priority { get; }
        public abstract bool IsReplacable { get; }

        public GoalState State { get; set; }
        public abstract EntityControls[] Controls { get; }

        protected EntityObject entity;

        public Goal(EntityObject entity) {
            this.entity = entity;
        }

        public abstract bool CanActivate();

        public abstract void Activate();

        public abstract void UpdateGoal();

        public abstract void Deactivate();
    }

    public enum GoalState {
        Inactive,
        Pending,
        Active
    }

    public enum EntityControls {
        Jump,
        Move,
        Look,
        Body,
        Target
    }
}