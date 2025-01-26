using Base.AI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Base.Component {

    public class AI : BaseComponent {

        [JsonProperty("behavior")]
        [JsonConverter(typeof(StringEnumConverter))]
        public BehaviorType behavior;

        [JsonProperty("goals")]
        public List<string> goals;
        private List<BaseGoal> _goals;

        public override void OnInitialize() {
            foreach (var goal in goals) {
                Type type = Utils.GetTypeFromString(goal);
                if (type != null) {
                    if (typeof(BaseGoal).IsAssignableFrom(type)) {
                        // get the constructor that takes a BaseEntity as a parameter
                        ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(BaseEntity) });

                        if (constructor != null) {
                            // create the instance and pass the BaseEntity as a parameter
                            BaseGoal goalInstance = (Activator.CreateInstance(type, new object[] { Entity })) as BaseGoal;

                            if (goalInstance != null) {
                                _goals.Add(goalInstance);
                            } else {
                                Debug.LogError($"[Base.Component.AI] Failed to create an instance of goal '{type}'.");
                            }
                        } else {
                            Debug.LogError($"[Base.Component.AI] Goal '{type}' does not have a constructor that accepts a BaseEntity.");
                        }
                    } else {
                        Debug.LogError($"[Base.Component.AI] '{type}' does not inherit 'BaseGoal'!");
                    }
                } else {
                    Debug.LogError($"[Base.Component.AI] Could not find goal script named '{Utils.ToTypeName(goal)}'!");
                }
            }

            _goals[0].Activate();
        }

        public enum BehaviorType {
            None,
            Passive,
            Neutral,
            Agressive
        }
    }
}