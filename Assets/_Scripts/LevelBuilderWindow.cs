using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class LevelBuilderWindow : EditorWindow
{
    private List<GameObject> _objectsToPlace = new List<GameObject>();
    private int _selectedObjectIndex = 0;
    private GameObject _currentObject;
    private int _rotationOptionIndex = 0;
    private bool _isPlacingObjects = false;
    private string[] _rotationOptions = { "0°", "90°", "180°", "270°", "Random" };
    private int[] _rotationAngles = { 0, 90, 180, 270 };

    [MenuItem("Tools/Level Builder")]
    public static void ShowWindow()
    {
        GetWindow<LevelBuilderWindow>("Level Builder");
    }

    private void OnEnable()
    {
        LoadPrefabs();
        if (_objectsToPlace.Count > 0)
        {
            _selectedObjectIndex = 0;
            _currentObject = _objectsToPlace[_selectedObjectIndex];
        }
    }

    private void LoadPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/BuildingBlocks" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
            {
                _objectsToPlace.Add(prefab);
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Object to Place", EditorStyles.boldLabel);

        if (_objectsToPlace.Count == 0)
        {
            GUILayout.Label("No objects found in Assets/Prefabs/BuildingBlocks.");
            return;
        }

        _selectedObjectIndex = EditorGUILayout.Popup("Object", _selectedObjectIndex, GetObjectNames());
        _currentObject = _objectsToPlace[_selectedObjectIndex];

        _rotationOptionIndex = EditorGUILayout.Popup("Rotation", _rotationOptionIndex, _rotationOptions);

        if (GUILayout.Button("Place Object"))
        {
            _isPlacingObjects = true;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        if (GUILayout.Button("Stop Placing Objects"))
        {
            StopPlacingObjects();
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (_currentObject == null || !_isPlacingObjects)
            return;

        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject placedObject = (GameObject)PrefabUtility.InstantiatePrefab(_currentObject);
                Vector3 roundedPosition = new Vector3(
                    Mathf.Round(hit.point.x),
                    Mathf.Round(hit.point.y),
                    Mathf.Round(hit.point.z)
                );
                placedObject.transform.position = roundedPosition;

                int rotationAngle = GetRotationAngle();
                placedObject.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);

                Undo.RegisterCreatedObjectUndo(placedObject, "Place Object");
            }
            e.Use();
        }
        else if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
        {
            StopPlacingObjects();
            e.Use();
        }
    }

    private int GetRotationAngle()
    {
        if (_rotationOptionIndex == _rotationOptions.Length - 1)
        {
            // Random rotation
            return _rotationAngles[Random.Range(0, _rotationAngles.Length)];
        }
        else
        {
            // Specific rotation
            return _rotationAngles[_rotationOptionIndex];
        }
    }

    private void StopPlacingObjects()
    {
        _isPlacingObjects = false;
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private string[] GetObjectNames()
    {
        string[] names = new string[_objectsToPlace.Count];
        for (int i = 0; i < _objectsToPlace.Count; i++)
        {
            names[i] = _objectsToPlace[i].name;
        }
        return names;
    }
}
