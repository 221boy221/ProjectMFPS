using UnityEngine;
using System.Collections;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameSettings : ScriptableObject {

    public const string ClientVersion = "1.0";
    public const string path = "/Resources/GameSettings.asset";

    protected static GameSettings _Instance;
    public static GameSettings Instance {
        get {
            if (_Instance == null) {
                Debug.Log("Instance is Null!");
#if UNITY_EDITOR
                if (File.Exists(Application.dataPath + path)) {
                    Debug.Log("File exists though...");
                    // If the file exists, load it
                    _Instance = Resources.Load("GameSettings") as GameSettings;

                    Debug.Log("Found: " + _Instance);
                } else {
                    Debug.Log("File does NOT exist");
                    // If the file doesn't exist, create it

                    // Creates the ScriptableObject Instance
                    var asset = ScriptableObject.CreateInstance<GameSettings>();

                    // Creates the Asset, refreshes the AssetDatabase and then saves all changes
                    AssetDatabase.CreateAsset(asset, Application.dataPath + path);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();

                    Debug.Log("GameSettings: Asset file didn't exist and was created at (" + path + ")");

                    // Save reference
                    _Instance = asset;
                }
#elif !UNITY_EDITOR
                	_Instance = Resources.Load("GameSettings") as GameSettings;
#endif
            }
            return _Instance;
        }

    }

    
}
