using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.AI;
using UnityEngine;

namespace Base {

    public class Factories {

        public static class AIGoals {

            public static List<Goal> GetGoalsFromJson(EntityObject entity, List<JObject> goalObjects) {
                List<Goal> goals = new();

                foreach (JObject goalObject in goalObjects) {
                    string goalKey = goalObject.Properties().FirstOrDefault().Name;
                    Type goalType = Utils.GetTypeFromString(goalKey);

                    JObject goalData = (JObject)goalObject[goalKey];

                    if (typeof(Goal).IsAssignableFrom(goalType)) {
                        Goal goalInstance = (Goal)Activator.CreateInstance(goalType, new object[] { entity });

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

        public static class EntityComponents {

            public static void AddEntityComponents(EntityObject entity, Dictionary<string, JObject> components) {
                // go through each component
                foreach (var component in components) {
                    // get component type
                    Type componentType = Utils.GetTypeFromString(component.Key);
                    if (componentType == null) {
                        Debug.LogError($"[EntityComponentFactory] Type '{component.Key}' could not be found.");
                        continue;
                    }

                    // ensure that componentType inherits from _BaseComponent
                    if (!typeof(EntityComponent).IsAssignableFrom(componentType)) {
                        Debug.LogError($"[EntityComponentFactory] Type '{componentType}' does not inherit '_BaseComponent'.");
                        continue;
                    }

                    // add component to entity and subscribe it to OnInitializeComponent
                    EntityComponent newComponent = (EntityComponent)entity.gameObject.AddComponent(componentType);
                    entity.OnInitializeComponent += newComponent.OnInitialize;

                    // deserialize component's data with JObject
                    JObject componentData = component.Value;
                    try {
                        JsonSerializer serializer = JsonSerializer.CreateDefault();
                        serializer.Populate(componentData.CreateReader(), newComponent);
                    }
                    catch (Exception e) {
                        Debug.LogError($"[EntityComponentFactory] Failed to populate component '{componentType}': {e.Message}");
                    }
                }

                entity.InitializeComponents();
            }
        }
    }
}