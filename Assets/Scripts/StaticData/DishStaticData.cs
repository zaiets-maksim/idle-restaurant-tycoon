using System.Collections;
using System.Collections.Generic;
using StaticData.Configs;
using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/Dish", fileName = "DishStaticData", order = 0)]
public class DishStaticData : ScriptableObject
{
    public List<DishConfig> Configs = new();
}
