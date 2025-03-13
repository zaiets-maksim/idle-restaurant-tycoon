using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/HallItem", fileName = "HallItemStaticData", order = 0)]
    public class HallItemStaticData : ScriptableObject
    {
        public List<HallItemConfig> Configs = new();
    }
}
