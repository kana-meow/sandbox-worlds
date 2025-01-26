using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Base.Factories {

    public static class EntityComponentFactory {

        public static void AddEntityComponents(Base.BaseEntity entity, Dictionary<string, JObject> components) {
            // go through each component
            foreach (var component in components) {
                // get component type
                Type componentType = Utils.GetTypeFromString(component.Key);
                if (componentType == null) {
                    Debug.LogError($"[EntityComponentFactory] Type '{component.Key}' could not be found.");
                    continue;
                }

                // ensure that componentType inherits from _BaseComponent
                if (!typeof(_BaseComponent).IsAssignableFrom(componentType)) {
                    Debug.LogError($"[EntityComponentFactory] Type '{componentType}' does not inherit '_BaseComponent'.");
                    continue;
                }

                // add component to entity and subscribe it to OnInitializeComponent
                _BaseComponent newComponent = (_BaseComponent)entity.gameObject.AddComponent(componentType);
                entity.OnInitializeComponent += newComponent.OnInitialize;

                // deserialize component's data with JObject
                JObject componentData = component.Value;
                try {
                    // use JObject.ToObject to populate the component
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