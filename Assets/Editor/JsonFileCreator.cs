using UnityEngine;
using UnityEditor;
using System.IO;

public class JsonFileCreator {

    [MenuItem("Assets/Create/New .json File")]
    public static void CreateJsonFile() {
        // Get the selected folder in the Project window
        string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        // Ensure it's a folder
        if (Directory.Exists(folderPath) || string.IsNullOrEmpty(folderPath)) {
            // Define the new file's path (with default name)
            string filePath = folderPath + "/file.json";

            // Create a new empty JSON file
            File.WriteAllText(filePath, "{}");

            // Refresh the AssetDatabase to show the new file
            AssetDatabase.Refresh();

            // Select the new file in the Project window
            Object newJsonFile = AssetDatabase.LoadAssetAtPath<Object>(filePath);
            Selection.activeObject = newJsonFile;
        } else {
            Debug.LogError("Please select a valid folder to create the file.");
        }
    }
}