using Newtonsoft.Json;
using UnityEngine;

namespace Base.AI.Goal {

    public class RandomWander : BaseGoal {

        public RandomWander(BaseEntity entity) : base(entity) {
        }

        [JsonProperty("value")]
        public int value;

        public override int Priority => 1;

        public override bool IsReplacable => throw new System.NotImplementedException();

        public override EntityControls[] Controls => throw new System.NotImplementedException();

        public override void Activate() {
            Debug.Log(Priority);
            Debug.Log("My value is: " + value);
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