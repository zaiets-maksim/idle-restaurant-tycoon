using System.Collections.Generic;
using StaticData.Configs;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/KitchenItem", fileName = "KitchenItemStaticData", order = 0)]
    public class KitchenItemStaticData : ScriptableObject
    {
        public List<KitchenItemConfig> Configs = new();
    }
}