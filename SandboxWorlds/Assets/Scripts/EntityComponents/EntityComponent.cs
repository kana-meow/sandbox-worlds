using UnityEngine;

namespace Base {

    public abstract class EntityComponent : MonoBehaviour {
        private Entity entity;

        public Entity Entity {
            get {
                if (entity == null) {
                    entity = GetComponent<Entity>();
                }
                return entity;
            }
        }
    }
}