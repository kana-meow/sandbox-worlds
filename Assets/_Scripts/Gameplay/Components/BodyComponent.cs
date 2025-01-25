using UnityEngine;

public class BodyComponent : _BaseComponent {
    public float width = 1;
    public float height = 1;

    private MeshRenderer meshRenderer;

    public override void OnInitialize() {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(width, height, width);
        collider.center = Vector3.up * (height / 2);

        // temp mesh
        GameObject meshChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshChild.name = "Mesh";
        Destroy(meshChild.GetComponent<Collider>());
        meshChild.transform.parent = transform;
        meshRenderer = meshChild.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial.color = Color.gray;

        meshChild.transform.localScale = new Vector3(width, height, width);
        meshChild.transform.localPosition = Vector3.up * (height / 2);

        if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out UnityEngine.AI.NavMeshAgent agent)) {
            agent.height = height;
        }
    }

    public void ChangeMeshColor(Color color) {
        meshRenderer.material.color = color;
    }
}