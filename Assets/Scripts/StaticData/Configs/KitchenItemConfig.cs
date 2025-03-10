using System;
using UnityEngine;

namespace StaticData.Configs
{
    [Serializable]
    public class KitchenItemConfig
    {
        public KitchenItemTypeId TypeId;
        public GameObject Prefab;
    }
}