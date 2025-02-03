using UnityEngine;

namespace Base {

    public abstract class EntityComponent : MonoBehaviour {
        private EntityObject entity;

        public EntityObject Entity {
            get {
                if (entity == null) {
                    entity = GetComponent<EntityObject>();
                }
                return entity;
            }
        }

        public abstract void OnInitialize();
    }
}