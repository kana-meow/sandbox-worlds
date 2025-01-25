using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class EntityComponentFactory {

    public static void AddEntityComponents(BaseEntity entity, Dictionary<string, Dictionary<string, object>> components) {
        foreach (var component in components) {
            // get component type
            Type componentType = Type.GetType(component.Key + "Component");
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
                            object valueToAssign = ConvertToType(param.Value, property.PropertyType);
                            property.SetValue(newComponent, valueToAssign);
                            continue;
                        } else {
                            // if not a property, it might be a field
                            FieldInfo field = componentType.GetField(param.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            if (field != null) {
                                object valueToAssign = ConvertToType(param.Value, field.FieldType);
                                field.SetValue(newComponent, valueToAssign);
                                continue;
                            }
                        }

                        Debug.LogError($"[EntityComponentFactory] No parameter with name '{param.Key}' could be found inside '{component.Key}Component'! (Make sure parameter is writable)");
                    }
                } else {
                    Debug.LogError($"[EntityComponentFactory] Component of type '{component.Key}Component' does not inherit from '_BaseComponent'! (Make sure {component.Key} inherits _BaseComponent.)");
                }
            } else {
                Debug.LogError($"[EntityComponentFactory] Component of type '{component.Key}Component' couldn't be found!");
            }
        }

        entity.InitializeComponents();
    }

    private static object ConvertToType(object value, Type targetType) {
        if (value == null) return null;

        // Handle List<T>
        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)) {
            Type elementType = targetType.GetGenericArguments()[0]; // Get the type of T in List<T>
            var list = Activator.CreateInstance(targetType) as System.Collections.IList;

            if (value is IEnumerable<object> enumerable) {
                foreach (var item in enumerable) {
                    list.Add(Convert.ChangeType(item, elementType));
                }
            }

            return list;
        }

        // Handle other types (primitive and compatible types)
        return Convert.ChangeType(value, targetType);
    }
}