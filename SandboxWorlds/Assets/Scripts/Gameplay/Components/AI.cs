using Base.AI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Base.Component {

    public class AI : BaseComponent {

        [JsonProperty("behavior")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BehaviorType behavior;

        [JsonProperty("goals")]
        public List<JObject> goals;
        private List<BaseGoal> inactiveGoals = new();
        private List<BaseGoal> pendingGoals = new();
        private List<BaseGoal> activeGoals = new();

        //private Dictionary<>

        public override void OnInitialize() {
            inactiveGoals = Factories.AIGoalFactory.GetGoalsFromJson(Entity, goals);

            inactiveGoals[0].Activate();
        }

        private void Update() {
        }

        public enum BehaviorType {
            None,
            Passive,
            Neutral,
            Agressive
        }
    }
}