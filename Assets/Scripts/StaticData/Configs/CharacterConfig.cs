using System;
using StaticData.TypeId;
using UnityEngine;

namespace StaticData.Configs
{
    [Serializable]
    public class CharacterConfig
    {
        public CharacterTypeId TypeId;
        public GameObject Prefab;
        public MarketItem MarketItem;
    }

    [Serializable]
    public class CustomerAppearance
    {
        public CustomerTypeId TypeId;
        public Mesh Mesh;
        public Material[] Materials;
    }
}
