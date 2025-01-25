using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

public class BaseEntity : MonoBehaviour {
    public EntityData data;
    public Guid guid;

    private LookComponent _look;
    private MoveComponent _move;
    private JumpComponent _jump;
    private BodyComponent _body;
    private TargetComponent _target;

    public LookComponent Look {
        get {
            if (_look == null) {
                _look = GetComponent<LookComponent>();
            }
            return _look;
        }
    }

    public MoveComponent Move {
        get {
            if (_move == null) {
                _move = GetComponent<MoveComponent>();
            }
            return _move;
        }
    }

    public JumpComponent Jump {
        get {
            if (_jump == null) {
                _jump = GetComponent<JumpComponent>();
            }
            return _jump;
        }
    }

    public BodyComponent Body {
        get {
            if (_body == null) {
                _body = GetComponent<BodyComponent>();
            }
            return _body;
        }
    }

    public TargetComponent Target {
        get {
            if (_target == null) {
                _target = GetComponent<TargetComponent>();
            }
            return _target;
        }
    }

    public event Action OnInitializeComponent;

    public void Initialize(EntityData data) {
        this.data = data;
        guid = Guid.NewGuid();

        EntityComponentFactory.AddEntityComponents(this, data.Components);
    }

    public void InitializeComponents() {
        OnInitializeComponent?.Invoke();
    }
}

[System.Serializable]
public class EntityData {

    [JsonProperty("entityID")]
    public string EntityID { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("category")]
    [JsonConverter(typeof(StringEnumConverter))]
    public EntityCategory Category { get; set; }

    [JsonProperty("components")]
    public Dictionary<string, Dictionary<string, object>> Components { get; set; }

    public enum EntityCategory {
        Animal,
        Monster,
        Boss,
        Player,
    }
}