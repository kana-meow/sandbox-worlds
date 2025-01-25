using UnityEngine;

namespace Base.AI {

    public abstract class _BaseGoal {
        public abstract int Priority { get; }
        public abstract bool IsReplacable { get; }

        public GoalState State { get; set; }
        public abstract EntityControls[] Controls { get; }

        protected Base.Entity entity;

        public _BaseGoal(Base.Entity entity) {
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