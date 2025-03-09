using System;
using UnityEngine;

namespace StaticData.Configs
{
    [Serializable]
    public class WindowConfig
    {
        public WindowTypeId WindowTypeId;
        public GameObject Prefab;
    }

    public enum WindowTypeId
    {
        Default,
        Menu
    }
}