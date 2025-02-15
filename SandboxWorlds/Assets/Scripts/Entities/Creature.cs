using Base.Component;
using UnityEngine;

namespace Base {

    [RequireComponent(typeof(Health), typeof(Rigidbody))]
    public class Creature : Entity {
    }
}