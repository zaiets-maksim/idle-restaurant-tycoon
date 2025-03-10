using System.Collections.Generic;
using _Developer.Scripts.Utilities;
using Services.DataStorageService;
using StaticData;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class DataManipulatorTool
    {
        private const string PlayerProgressKey = "PlayerProgress";
        private const string LevelStaticDataPath = "StaticData/LevelStaticData";
        
        [MenuItem("Tools/Data Manipulator/Delete all data")]
        public static void DeleteAllData()
        {
            PlayerPrefs.DeleteAll();
        }
    
        [MenuItem("Tools/Data Manipulator/Buy all kitchen items")]
        public static void AddAllItems()
        {
            var levelStaticData = Resources
                .Load<LevelStaticData>(LevelStaticDataPath);

            List<KitchenItemInfo> purchasedKitchenItems = new List<KitchenItemInfo>();
            foreach (var data in levelStaticData.KitchenItemsData)
            {
                purchasedKitchenItems.Add(new KitchenItemInfo
                {
                    TypeId = data.TypeId,
                    PurchaseOrder = data.PurchaseOrder,
                    Position = data.Position,
                    Rotation = data.Rotation,
                    Parent = data.Parent
                });
            }
            
            PlayerData PlayerData = new PlayerData
            {
                ProgressData =
                {
                    PurchasedKitchenItems = purchasedKitchenItems
                }
            };
            
            PlayerPrefs.SetString(PlayerProgressKey, PlayerData.ToJson());
        }

    }
}
