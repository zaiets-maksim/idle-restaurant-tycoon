using Characters.PersonStateMachine;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PersonInfoEditor : UnityEditor.Editor
    {
        private static string _text;
        
        static PersonInfoEditor()
        {
        }

        private static readonly GUIStyle _textStyle = new()
        {
            fontSize = 15,
            normal =
            {
                textColor = Color.black
            },
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawPersonsState(PersonBehavior personBehavior, GizmoType gizmoType)
        {
            return;
            if (personBehavior.IsTransitioning)
                _text = "IsTransitioning" + $"\n{personBehavior.gameObject.GetInstanceID()}";
            else
                _text = personBehavior.CurrentState.GetType().Name + $"\n{personBehavior.gameObject.GetInstanceID()}";
                
            Vector3 worldPos = personBehavior.transform.position + Vector3.up;
            string labelText = _text;

            Handles.BeginGUI();

            Vector3 screenPos = HandleUtility.WorldToGUIPoint(worldPos);

            GUIContent content = new(labelText);
            Vector2 size = _textStyle.CalcSize(content);
            Rect rect = new(screenPos.x - size.x * 0.5f, screenPos.y - size.y * 0.5f, size.x + 10, size.y + 10);
            EditorGUI.DrawRect(rect, Color.white);
            GUI.Label(rect, content, _textStyle);

            Handles.EndGUI();
        }
    }
}