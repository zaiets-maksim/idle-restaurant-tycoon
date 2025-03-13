using System;
using UnityEngine;

namespace StaticData.Configs
{
    [Serializable]
    public class MarketItem
    {
        public string Name;
        public string Description;
        public int Price;
        public Sprite Icon;
    }
}