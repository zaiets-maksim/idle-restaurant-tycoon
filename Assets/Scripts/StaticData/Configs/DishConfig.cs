using System;
using UnityEngine;

namespace StaticData.Configs
{
    [Serializable]
    public class DishConfig
    {
        public DishTypeId TypeId;
        public GameObject Prefab;
        public int Price;
    }
}
