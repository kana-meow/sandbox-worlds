using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class AIComponent : _BaseComponent {
    private List<_BaseGoal> _goals = new();
    public List<string> goals = new();

    public override void OnInitialize() {
        foreach (var goal in goals) {
            Type type = Type.GetType(goal + "Goal");
            if (type != null) {
                if (typeof(_BaseGoal).IsAssignableFrom(type)) {
                    // get the constructor that takes a BaseEntity as a parameter
                    ConstructorInfo constructor = type.GetConstructor(new Type[] { typeof(BaseEntity) });

                    if (constructor != null) {
                        // create the instance and pass the BaseEntity as a parameter
                        _BaseGoal goalInstance = (Activator.CreateInstance(type, new object[] { Entity })) as _BaseGoal;

                        if (goalInstance != null) {
                            _goals.Add(goalInstance);
                        } else {
                            Debug.LogError($"[AIComponent] Failed to create an instance of goal '{goal}Goal'.");
                        }
                    } else {
                        Debug.LogError($"[AIComponent] Goal '{goal}Goal' does not have a constructor that accepts a BaseEntity.");
                    }
                } else {
                    Debug.LogError($"[AIComponent] '{goal}Goal' does not inherit '_BaseGoal'!");
                }
            } else {
                Debug.LogError($"[AIComponent] Could not find goal script named '{goal}Goal'!");
            }
        }

        _goals[0].Activate();
    }

    [SerializeField] private EntityBehavior _behavior;

    public string Behavior {
        private get {
            return null;
        }
        set {
            if (!System.Enum.TryParse<EntityBehavior>(value, true, out _behavior)) {
                Debug.LogError($"[AIComponent] No EntityBehavior of type '{value}' was found!");
            }
        }
    }

    private void Update() {
        _goals[0].UpdateGoal();
    }

    public enum EntityBehavior {
        None,
        Passive,
        Neutral,
        Agressive
    }
}