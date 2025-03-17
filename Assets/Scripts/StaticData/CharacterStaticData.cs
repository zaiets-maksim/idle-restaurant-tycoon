using System.Collections.Generic;
using StaticData.Configs;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Character", fileName = "CharacterStaticData", order = 0)]
    public class CharacterStaticData : ScriptableObject
    {
        public List<CharacterConfig> Configs = new();
        [Header("Only for customers")]
        public List<CustomerAppearance> Appearances = new();
    }
}
