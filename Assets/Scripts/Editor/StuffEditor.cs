using Characters;
using Characters.Behaviors;
using Characters.Personal;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class StuffEditor : UnityEditor.Editor
    {
        private static readonly GUIStyle _textStyle = new()
        {
            fontSize = 20,
            normal =
            {
                textColor = Color.black

            },
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawChefState(Chef chef, GizmoType gizmoType)
        {
            if(chef.ChefBehavior.CurrentState == null)
                return;
            Handles.Label(chef.transform.position + Vector3.up,
                chef.ChefBehavior.CurrentState.GetType().Name +
                $"\n{chef.gameObject.GetInstanceID()}", _textStyle);
        }
        
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawChefState(Waiter waiter, GizmoType gizmoType)
        {
            if(waiter.WaiterBehavior.CurrentState == null)
                return;
            Handles.Label(waiter.transform.position + Vector3.up,
                waiter.WaiterBehavior.CurrentState.GetType().Name +
                $"\n{waiter.gameObject.GetInstanceID()}", _textStyle);
        }

    }
}
