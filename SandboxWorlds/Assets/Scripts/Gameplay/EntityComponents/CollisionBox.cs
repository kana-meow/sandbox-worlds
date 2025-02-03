using UnityEngine;

namespace Base.Component {

    public class CollisionBox : EntityComponent {
        public float width, height;

        public override void OnInitialize() {
            // for now there will be a primitve instead of a mesh from a geo.json file
            GameObject meshObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Destroy(meshObject.GetComponent<Collider>());
            meshObject.name = "Mesh Object";
            meshObject.transform.parent = gameObject.transform;
            meshObject.transform.localScale = new Vector3(width, height, width);
            meshObject.transform.localPosition = Vector3.up * (height / 2);

            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(width, height, width);
            collider.center = Vector3.up * (height / 2);
        }
    }
}