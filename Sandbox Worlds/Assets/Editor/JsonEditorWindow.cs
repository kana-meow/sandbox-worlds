using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonEditorWindow : EditorWindow {
    private string filePath = "";
    private string fileContent = "";
    private Vector2 scrollPos;

    [MenuItem("Tools/JSON Editor")]
    public static void ShowWindow() {
        GetWindow<JsonEditorWindow>("JSON Editor");
    }

    private void OnGUI() {
        EditorGUILayout.LabelField("Drag and drop a JSON file below to edit it.", EditorStyles.boldLabel);

        EditorGUILayout.Space(10);

        // drag-and-drop area
        Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "-Drop JSON file here-", EditorStyles.helpBox);

        if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform) {
            if (dropArea.Contains(Event.current.mousePosition)) {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (Event.current.type == EventType.DragPerform) {
                    DragAndDrop.AcceptDrag();
                    if (DragAndDrop.paths.Length > 0) {
                        string draggedPath = DragAndDrop.paths[0];
                        if (Path.GetExtension(draggedPath).ToLower() == ".json") {
                            filePath = draggedPath;
                            LoadFile();
                        } else {
                            Debug.LogError("[JsonEditorWindow] Only JSON files are supported.");
                        }
                    }
                    Event.current.Use();
                }
            }
        }

        if (!string.IsNullOrEmpty(filePath)) {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Editing File:", filePath, EditorStyles.wordWrappedLabel);

            Undo.RecordObject(this, "Edit JSON Content");

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            fileContent = EditorGUILayout.TextArea(fileContent, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);

            if (GUILayout.Button("Save")) {
                SaveFile();
            }

            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Undo")) {
                Undo.PerformUndo();
            }
            if (GUILayout.Button("Redo")) {
                Undo.PerformRedo();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void LoadFile() {
        try {
            fileContent = File.ReadAllText(filePath);
        }
        catch (System.Exception e) {
            Debug.LogError("Failed to load file: " + e.Message);
        }
    }

    private void SaveFile() {
        try {
            File.WriteAllText(filePath, fileContent);
            Debug.Log("File saved successfully: " + filePath);
        }
        catch (System.Exception e) {
            Debug.LogError("Failed to save file: " + e.Message);
        }
    }
}