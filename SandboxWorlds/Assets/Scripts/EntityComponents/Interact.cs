using UnityEngine;
using UnityEngine.Events;

namespace Base.Component {

    public class Interact : EntityComponent {
        public UnityEvent<PlayerEntity> OnInteract;
        public UnityEvent<PlayerEntity> OnInteractAlt;
    }
}