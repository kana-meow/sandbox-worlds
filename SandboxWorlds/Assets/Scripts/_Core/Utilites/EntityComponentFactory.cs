using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Base.Factories {

    public static class EntityComponentFactory {

        public static void AddEntityComponents(Base.BaseEntity entity, Dictionary<string, Dictionary<string, object>> components) {
            foreach (var component in components) {
                // get component type
                System.Type componentType = Utils.GetTypeFromString(component.Key);
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
                                // check if JsonConverterAttribute is applied to property
                                JsonConverterAttribute converterAttribute = property.GetCustomAttribute(typeof(JsonConverterAttribute)) as JsonConverterAttribute;
                                if (converterAttribute != null) {
                                    // use JsonConverter specified in attribute
                                    JsonConverter converter = (JsonConverter)Activator.CreateInstance(converterAttribute.ConverterType);
                                    object valueToAssign = converter.ReadJson(new JsonTextReader(new StringReader(param.Value.ToString())), property.PropertyType, null, new JsonSerializer());
                                    property.SetValue(newComponent, valueToAssign);
                                } else {
                                    object valueToAssign = Utils.ConvertToType(param.Value, property.PropertyType);
                                    property.SetValue(newComponent, valueToAssign);
                                }
                                continue;
                            } else {
                                // if not a property, it might be a field
                                FieldInfo field = componentType.GetField(param.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                if (field != null) {
                                    // check if JsonConverterAttribute is applied to field
                                    JsonConverterAttribute converterAttribute = field.GetCustomAttribute(typeof(JsonConverterAttribute)) as JsonConverterAttribute;
                                    if (converterAttribute != null) {
                                        // use JsonConverter specified in field
                                        JsonConverter converter = (JsonConverter)Activator.CreateInstance(converterAttribute.ConverterType);
                                        object valueToAssign = converter.ReadJson(new JsonTextReader(new StringReader(param.Value.ToString())), field.FieldType, null, new JsonSerializer());
                                        field.SetValue(newComponent, valueToAssign);
                                    } else {
                                        object valueToAssign = Utils.ConvertToType(param.Value, field.FieldType);
                                        field.SetValue(newComponent, valueToAssign);
                                    }
                                    continue;
                                }
                            }

                            Debug.LogError($"[EntityComponentFactory] No parameter with name '{param.Key}' could be found inside '{componentType}'! (Make sure parameter is writable)");
                        }
                    } else {
                        Debug.LogError($"[EntityComponentFactory] Component of type '{componentType}' does not inherit from '_BaseComponent'! (Make sure {componentType} inherits _BaseComponent.)");
                    }
                } else {
                    Debug.LogError($"[EntityComponentFactory] Component of type '{componentType}' couldn't be found!");
                }
            }

            entity.InitializeComponents();
        }
    }
}