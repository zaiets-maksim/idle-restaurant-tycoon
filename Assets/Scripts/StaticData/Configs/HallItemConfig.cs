using System;
using StaticData.Configs;
using StaticData.TypeId;
using UnityEngine;

[Serializable]
public class HallItemConfig
{
    public HallItemTypeId TypeId;
    public GameObject Prefab;
    public MarketItem MarketItem;
}
