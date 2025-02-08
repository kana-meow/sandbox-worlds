using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor {

    public override void OnInspectorGUI() {
        WorldGenerator worldGenerator = (WorldGenerator)target;

        EditorGUILayout.LabelField("Base fields", EditorStyles.boldLabel);
        base.OnInspectorGUI();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Constants", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Octree Chunk Size", WorldData.OCTREE_CHUNK_SIZE.ToString());
        EditorGUILayout.LabelField("Min Voxels Per Node", WorldData.MIN_VOXELS_PER_NODE.ToString());

        GUILayout.Space(10);

        if (GUILayout.Button("Generate")) {
            worldGenerator.Generate();
        }
        if (GUILayout.Button("Reset")) {
            worldGenerator.Reset();
        }
    }
}