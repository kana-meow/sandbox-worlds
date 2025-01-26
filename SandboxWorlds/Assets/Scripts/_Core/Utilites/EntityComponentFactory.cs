using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Base.Factories {

    public static class EntityComponentFactory {

        public static void AddEntityComponents(Base.BaseEntity entity, Dictionary<string, Dictionary<string, object>> components) {
            foreach (var component in components) {
                // get component type
                Type componentType = Type.GetType(component.Key);
                if (componentType != null) {
                    // check if component type inherits from BaseComponent
                    if (typeof(_BaseComponent).IsAssignableFrom(componentType)) {
                        // add component
                        _BaseComponent newComponent = (_BaseComponent)entity.gameObject.AddComponent(componentType);
                        entity.OnInitializeComponent += () => {
                            newComponent.OnInitialize();
                        };

                        foreach (var param in component.Value) {
                            // find property by name and assign value
                            PropertyInfo property = componentType.GetProperty(param.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (property != null && property.CanWrite) {
                                object valueToAssign = Utils.ConvertToType(param.Value, property.PropertyType);
                                property.SetValue(newComponent, valueToAssign);
                                continue;
                            } else {
                                // if not a property, it might be a field
                                FieldInfo field = componentType.GetField(param.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                if (field != null) {
                                    object valueToAssign = Utils.ConvertToType(param.Value, field.FieldType);
                                    field.SetValue(newComponent, valueToAssign);
                                    continue;
                                }
                            }

                            Debug.LogError($"[EntityComponentFactory] No parameter with name '{param.Key}' could be found inside '{component.Key}'! (Make sure parameter is writable)");
                        }
                    } else {
                        Debug.LogError($"[EntityComponentFactory] Component of type '{component.Key}' does not inherit from '_BaseComponent'! (Make sure {component.Key} inherits _BaseComponent.)");
                    }
                } else {
                    Debug.LogError($"[EntityComponentFactory] Component of type '{component.Key}' couldn't be found!");
                }
            }

            entity.InitializeComponents();
        }
    }
}