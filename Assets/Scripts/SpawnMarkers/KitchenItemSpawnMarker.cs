using System.Linq;
using StaticData;
using UnityEngine;

namespace SpawnMarkers
{
    [ExecuteInEditMode]
    public class KitchenItemSpawnMarker : MonoBehaviour
    {
        [SerializeField] private KitchenItemTypeId _typeId;
        [SerializeField] private int _purchaseOrder;
        // [SerializeField] private Transform _parent;

        public KitchenItemTypeId TypeId => _typeId;
        public int PurchaseOrder => _purchaseOrder;

        [Header("Draw settings")] public Vector3 Size;
        public Vector3 PositionOffset;
        public Vector3 RoattionOffset;
        public Color Color = new Color(1f, 0f, 0.67f, 0.5f);
        public Vector3 TextOffset;


        private void OnEnable()
        {
            UpdateIndex();
        }

        private void UpdateIndex()
        {
            if (_purchaseOrder != 0)
                return;

            var index = FindObjectsOfType<KitchenItemSpawnMarker>()
                .Where(x => x.TypeId == _typeId)
                .Max(x => x.PurchaseOrder);
            _purchaseOrder = ++index;
        }
    }
}