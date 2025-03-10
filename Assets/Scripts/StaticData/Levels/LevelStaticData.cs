using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelStaticData", menuName = "StaticData/Levels/Level", order = 0)]   
    public class LevelStaticData : ScriptableObject
    {
        [Header("Data")]
        public KitchenData[] KitchenItemsData;
        public StorageData[] StorageItemsData;
    }

    [Serializable]
    public class StorageData
    {
        // public CrateTypeId CrateTypeId;
        public Vector2 Position;
        public Vector3 Rotation;
    }

    [Serializable]
    public class KitchenData
    {
        [FormerlySerializedAs("KitchenItemTypeId")] public KitchenItemTypeId TypeId ;
        public Vector3 Position;
        public Vector3 Rotation;
        public int PurchaseOrder;
        public Transform Parent;
    }
}