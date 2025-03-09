using System;
using UnityEngine;

namespace StaticData.Configs
{
    [Serializable]
    public class KitchenItemConfig
    {
        public KitchenItemConfigTypeId WindowTypeId;
        public GameObject Prefab;
        public Vector3 Position;
    }

    public enum KitchenItemConfigTypeId
    {
        Default,
    }
}