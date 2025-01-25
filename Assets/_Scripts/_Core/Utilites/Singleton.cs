using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                // Try to find an existing instance in the scene
                _instance = FindFirstObjectByType<T>();

                // If no instance is found, create a new GameObject
                if (_instance == null) {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake() {
        // If an instance already exists and it's not this instance, destroy this object
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this as T; // Set the instance to this object
        }
    }
}