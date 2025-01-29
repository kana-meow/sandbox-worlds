using Base.AI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Base.Factories {

    public static class AIGoalFactory {

        public static List<BaseGoal> GetGoalsFromJson(BaseEntity entity, List<JObject> goalObjects) {
            List<BaseGoal> goals = new();

            foreach (JObject goalObject in goalObjects) {
                string goalKey = goalObject.Properties().FirstOrDefault().Name;
                Type goalType = Utils.GetTypeFromString(goalKey);

                JObject goalData = (JObject)goalObject[goalKey];

                if (typeof(BaseGoal).IsAssignableFrom(goalType)) {
                    BaseGoal goalInstance = (BaseGoal)Activator.CreateInstance(goalType, new object[] { entity });
                    Debug.Log(goalData.ToString());

                    if (goalInstance != null) {
                        JsonSerializer serializer = JsonSerializer.CreateDefault();
                        serializer.Populate(goalData.CreateReader(), goalInstance);
                        goals.Add(goalInstance);
                    }
                }
            }
            return goals;
        }
    }
}