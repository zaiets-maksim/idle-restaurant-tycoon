using StaticData.TypeId;
using UnityEngine;

namespace SpawnMarkers
{
    public class HallItemSpawnMarker : MonoBehaviour
    {
        [SerializeField] private HallItemTypeId _typeId;
        [SerializeField] private int _purchaseOrder;
        // [SerializeField] private Transform _parent;

        public HallItemTypeId TypeId => _typeId;
        public int PurchaseOrder => _purchaseOrder;

        [Header("Draw settings")] 
        public Vector3 Size;
        public Vector3 PositionOffset;
        public Vector3 RoattionOffset;
        public Color Color = new(1f, 0f, 0.67f, 0.5f);
        public Vector3 TextOffset;


        [ContextMenu("Assign Chair Orders")]
        public void AssignChairOrders()
        {
            if (_typeId == HallItemTypeId.Chair)
            {
                var tablePurchaseOrder = transform.parent.GetComponent<HallItemSpawnMarker>().PurchaseOrder;
                var siblingIndex = transform.GetSiblingIndex() + 1;

                if (tablePurchaseOrder == 1)
                    _purchaseOrder = siblingIndex;
                else
                    _purchaseOrder = (tablePurchaseOrder - 1) * 4 + siblingIndex;
            }
        }
    }
}
