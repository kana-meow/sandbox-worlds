using UnityEngine;

public abstract class _BaseComponent : MonoBehaviour {
    private Base.BaseEntity entity;

    public Base.BaseEntity Entity {
        get {
            if (entity == null) {
                entity = GetComponent<Base.BaseEntity>();
            }
            return entity;
        }
    }

    public abstract void OnInitialize();
}