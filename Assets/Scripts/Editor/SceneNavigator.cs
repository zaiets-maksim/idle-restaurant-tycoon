using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class SceneNavigator : EditorWindow
{
    private const int MaxHistory = 10;
    private const string CurrentScenePointer = " \u25c0";

    private readonly LinkedList<string> _sceneHistory = new();
    private static string _lastScene;
    private string _sceneName;
    
    private void OnEnable()
    {
        EditorSceneManager.sceneOpened += OnSceneOpened;
        _lastScene = SceneManager.GetActiveScene().path;
    }

    private void OnDisable()
    {
        EditorSceneManager.sceneOpened -= OnSceneOpened;
    }

    public static void ShowWindow() => GetWindow<SceneNavigator>("Scene Navigator");

    private void OnSceneOpened(Scene scene, OpenSceneMode mode)
    {
        if (!_sceneHistory.Contains(scene.path))
        {
            _sceneHistory.AddFirst(scene.path);
            if (_sceneHistory.Count > MaxHistory) 
                _sceneHistory.RemoveLast();
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
    }

    private void OpenScene(string scenePath)
    {
        if (scenePath != _lastScene)
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}


internal abstract class SceneNavigatorShortcut
{
    const string ShortcutId = "MyTools/OpenSceneNavigator";

    [Shortcut(ShortcutId, KeyCode.O, ShortcutModifiers.Control | ShortcutModifiers.Shift)]
    static void SpecialActionShortcut() => SceneNavigator.ShowWindow();
}