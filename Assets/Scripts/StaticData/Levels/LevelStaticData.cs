using System;
using UnityEngine;

namespace StaticData.Levels
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
        public KitchenItemTypeId TypeId ;
        public int PurchaseOrder;
        public Vector3 Position;
        public Vector3 Rotation;
        public Transform Parent;
    }
}