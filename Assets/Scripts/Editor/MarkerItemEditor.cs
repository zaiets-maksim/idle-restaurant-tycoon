using Interactable;
using SpawnMarkers;
using StaticData;
using StaticData.TypeId;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LevelItemEditor : UnityEditor.Editor
    {
        private static readonly GUIStyle _textStyle = new()
        {
            fontSize = 20,
            normal =
            {
                textColor = Color.white
            
            },
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
    
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawKitchenItems(KitchenItemSpawnMarker kitchenItem, GizmoType gizmoType)
        {
            if (kitchenItem.TypeId == KitchenItemTypeId.Unknown)
                return;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                kitchenItem.transform.position + kitchenItem.PositionOffset,
                Quaternion.Euler(kitchenItem.transform.eulerAngles + kitchenItem.RoattionOffset), Vector3.one);
            Gizmos.matrix = rotationMatrix;	
        
            Gizmos.color = kitchenItem.Color;
            Gizmos.DrawCube(Vector3.zero, kitchenItem.Size);

            Handles.Label(kitchenItem.transform.position + kitchenItem.TextOffset, kitchenItem.TypeId.ToString(), _textStyle);
        }
        
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawKitchenItemInteractionPoints(KitchenItem kitchenItem, GizmoType gizmoType)
        {
            if (kitchenItem.TypeId == KitchenItemTypeId.Unknown)
                return;
           
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(kitchenItem.InteractionPoint.position, 0.3f);
        }
        
        
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawHallItems(HallItemSpawnMarker hallItem, GizmoType gizmoType)
        {
            if (hallItem.TypeId == HallItemTypeId.Unknown)
                return;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                hallItem.transform.position + hallItem.PositionOffset,
                Quaternion.Euler(hallItem.transform.eulerAngles + hallItem.RoattionOffset), Vector3.one);
            Gizmos.matrix = rotationMatrix;	
        
            Gizmos.color = hallItem.Color;
            Gizmos.DrawCube(Vector3.zero, hallItem.Size);

            Handles.Label(hallItem.transform.position + hallItem.TextOffset, hallItem.TypeId.ToString(), _textStyle);
        }
    }
}
