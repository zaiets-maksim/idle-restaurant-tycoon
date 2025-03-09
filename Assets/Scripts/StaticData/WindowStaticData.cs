using System.Collections.Generic;
using StaticData.Configs;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Windows", fileName = "WindowsStaticData", order = 0)]
    public class WindowStaticData : ScriptableObject
    {
        public List<WindowConfig> Configs = new();
    }
}