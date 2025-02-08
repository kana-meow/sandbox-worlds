using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NoiseLayer))]
public class NoiseLayerEditor : Editor {
    private bool showTextures = false;

    public override void OnInspectorGUI() {
        NoiseLayer noiseLayer = (NoiseLayer)target;
        base.OnInspectorGUI();

        showTextures = EditorGUILayout.Foldout(showTextures, "Noise Layers");
        if (showTextures) {
            EditorGUI.indentLevel++;
            if (GUILayout.Button("Add Layer")) {
                noiseLayer.noises.Add(new NoiseSettings());
            }

            for (int i = 0; i < noiseLayer.noises.Count; i++) {
                NoiseSettings noise = noiseLayer.noises[i];

                // Generate noise texture based on its index

                noise.texture = noiseLayer.GenerateNoiseTexture(i);

                EditorGUILayout.BeginHorizontal();

                float noiseTextureSize = 100f;
                Rect rect = GUILayoutUtility.GetRect(noiseTextureSize, noiseTextureSize, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

                GUI.DrawTexture(rect, noise.texture, ScaleMode.ScaleToFit);

                EditorGUILayout.BeginVertical();
                noise.type = (NoiseType)EditorGUILayout.EnumPopup("Type", noise.type);
                noise.scale = EditorGUILayout.Slider("Scale", noise.scale, .01f, 1);
                noise.amplitude = EditorGUILayout.Slider("Amplitude", noise.amplitude, .01f, 1);
                noise.offset = EditorGUILayout.Vector2Field("Offset", noise.offset);
                if (GUILayout.Button("Remove")) {
                    noiseLayer.noises.RemoveAt(i);
                    break;
                }
                GUILayout.Space(10);
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Final Texture", EditorStyles.boldLabel);
            float finalSize = 100f;
            Rect finalRect = GUILayoutUtility.GetRect(finalSize, finalSize, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

            GUI.DrawTexture(finalRect, noiseLayer.GenerateFinalNoiseTexture(), ScaleMode.ScaleToFit);
        }
    }
}