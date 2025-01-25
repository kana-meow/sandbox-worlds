using UnityEngine;

public abstract class _BaseComponent : MonoBehaviour {
    private BaseEntity entity;

    public BaseEntity Entity {
        get {
            if (entity == null) {
                entity = GetComponent<BaseEntity>();
            }
            return entity;
        }
    }

    public abstract void OnInitialize();
}