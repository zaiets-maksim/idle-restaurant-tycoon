using System;
using Interactable;
using UnityEngine;

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
        public Crate Crate;
        public Vector2 Position;
        public Vector3 Rotation;
    }

    [Serializable]
    public class KitchenData
    {
        public KitchenItem KitchenItem;
        public KitchenItemTypeId KitchenItemTypeId ;
        public Vector2 Position;
        public Vector3 Rotation;
    }
}