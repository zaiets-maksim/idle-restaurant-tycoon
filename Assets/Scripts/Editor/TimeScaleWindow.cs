using UnityEditor;
using UnityEngine;

public class TimeScaleWindow : EditorWindow
{
    private static TimeScaleWindow _window;
    private float _timeScale = 1f;
    private float _minScale = 0f;
    private float _maxScale = 1f;

    [MenuItem("Tools/TimeScale Window 🕓")]
    public static void OpenWindow()
    {
        if (_window == null)
        {
            _window = GetWindow<TimeScaleWindow>("Time Scale");
            _window.minSize = new Vector2(250f, 125f);
            _window.maxSize = new Vector2(500f, 125f);
        }
        _window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Adjust Time Scale", EditorStyles.boldLabel);

        _minScale = EditorGUILayout.FloatField("Min Scale", _minScale);
        _maxScale = EditorGUILayout.FloatField("Max Scale", _maxScale);

        if (_minScale > _maxScale) 
            _minScale = _maxScale;

        _timeScale = EditorGUILayout.Slider("Time Scale", Time.timeScale, _minScale, _maxScale);
        Time.timeScale = _timeScale;
    }
}
