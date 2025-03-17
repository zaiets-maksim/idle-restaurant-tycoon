using System;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Balance", fileName = "Balance", order = 0)]
    public class BalanceStaticData : ScriptableObject
    {
        public Customers Customers = new();
    }

    [Serializable]
    public class Customers
    {
        [Header("(in seconds)")]
        public Vector2 SpawnInterval;
        public Vector2 Speed;
        public float DefaultSpeed;
        public Vector2 MealDurationInterval;
    }
}