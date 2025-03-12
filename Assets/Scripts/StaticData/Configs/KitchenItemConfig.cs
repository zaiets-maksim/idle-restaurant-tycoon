using System;
using UnityEngine;

namespace StaticData.Configs
{
    [Serializable]
    public class KitchenItemConfig
    {
        public KitchenItemTypeId TypeId;
        public GameObject Prefab;
        public MarketItem MarketItem;
    }

    [Serializable]
    public class MarketItem
    {
        public string Name;
        public string Description;
        public int Price;
        public Sprite Icon;
    }
}