using System.Collections.Generic;
using System.Linq;
using _Developer.Scripts.Utilities;
using Services.DataStorageService;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class SceneNavigator : EditorWindow
{
    private const int MaxHistory = 10;
    private const string CurrentScenePointer = " \u25c0";
    private const string HistoryKey = "SceneNavigatorHistory";
    private const string CloseAfterOpenSceneKey = "CloseAfterOpenSceneKey";
    private LinkedList<string> _sceneHistory = new();
    private static string _lastScene;
    private string _sceneName;
    private bool _closeAfterOpenScene;

    private void OnEnable()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
        _sceneHistory = LoadHistory();
        _closeAfterOpenScene = EditorPrefs.GetBool(CloseAfterOpenSceneKey);
        _lastScene = SceneManager.GetActiveScene().path;
    }

    private void OnDisable()
    {
        EditorSceneManager.sceneOpened -= OnSceneOpened;
        SaveHistory();
    }

    public static void ShowWindow() => GetWindow<SceneNavigator>("Scene Navigator");

    private void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        if (!_sceneHistory.Contains(scene.path))
        {
            _sceneHistory.AddFirst(scene.path);
            if (_sceneHistory.Count > MaxHistory)
                _sceneHistory.RemoveLast();

            SaveHistory();
        }

        _lastScene = scene.path;
    }

    private void OnGUI()
    {
        GUILayout.Label("Scenes in Build Settings:", EditorStyles.boldLabel);

        foreach (var scene in EditorBuildSettings.scenes)
        {
            _sceneName = scene.path;
            if (SceneManager.GetActiveScene().path == scene.path)
                _sceneName = $"{scene.path}{CurrentScenePointer}";
            if (GUILayout.Button($"Open {_sceneName}"))
                OpenScene(scene.path);
        }

        GUILayout.Space(10);
        GUILayout.Label("Recent Scenes:", EditorStyles.boldLabel);

        foreach (var scenePath in _sceneHistory)
            if (GUILayout.Button($"Open {scenePath}"))
                OpenScene(scenePath);

        GUILayout.FlexibleSpace();
        Rect rect = EditorGUILayout.GetControlRect();
        _closeAfterOpenScene = EditorGUI.ToggleLeft(rect, "Close after open scene", _closeAfterOpenScene);
        EditorPrefs.SetBool(CloseAfterOpenSceneKey, _closeAfterOpenScene);
    }

    private void OpenScene(string scenePath)
    {
        if (scenePath != _lastScene)
        {
            EditorSceneManager.OpenScene(scenePath);
            if(_closeAfterOpenScene)
                Close();
        }
    }

    private void SaveHistory()
    {
        var list = _sceneHistory.ToList();
        var json = JsonUtility.ToJson(new SerializableList<string> { items = list });
        EditorPrefs.SetString(HistoryKey, json);
    }

    private LinkedList<string> LoadHistory()
    {
        string json = EditorPrefs.GetString(HistoryKey, null);
        if (!string.IsNullOrEmpty(json))
        {
            var serializableList = JsonUtility.FromJson<SerializableList<string>>(json);
            return new LinkedList<string>(serializableList.items);
        }

        return new LinkedList<string>();
    }
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> items;
}


internal abstract class SceneNavigatorShortcut
{
    const string ShortcutId = "MyTools/OpenSceneNavigator";

    [Shortcut(ShortcutId, KeyCode.O, ShortcutModifiers.Control | ShortcutModifiers.Shift)]
    static void SpecialActionShortcut() => SceneNavigator.ShowWindow();
}