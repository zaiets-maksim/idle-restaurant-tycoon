using System.Linq;
using Crates;
using SpawnMarkers;
using StaticData.Levels;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private LevelStaticData _levelStaticData;

        private void OnEnable()
        {
            _levelStaticData = (LevelStaticData)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ShowLoadButtons();
        }

        private void ShowLoadButtons()
        {
            if (GUILayout.Button("Load Kitchen Items Data"))
                LoadKitchenItemsData();

            if (GUILayout.Button("Load Storage Items Data"))
                LoadStorageItemsData();

            if (GUILayout.Button("Load Hall Items Data"))
                LoadHallItemsData();
            
            if (GUILayout.Button("Load Characters Data"))
                LoadCharactersData();
        }

        private void LoadHallItemsData()
        {
            _levelStaticData.HallItemsData = FindHallItems();
            EditorUtility.SetDirty(_levelStaticData);
        }

        private void LoadKitchenItemsData()
        {
            _levelStaticData.KitchenItemsData = FindKitchemItems();
            EditorUtility.SetDirty(_levelStaticData);
        }

        private void LoadStorageItemsData()
        {
            _levelStaticData.StorageItemsData = FindStorageItems();
            EditorUtility.SetDirty(_levelStaticData);
        }

        private void LoadCharactersData()
        {
            _levelStaticData.CharactersData = FindCharactersData();
            EditorUtility.SetDirty(_levelStaticData);
        }

        private CharacterData[] FindCharactersData() =>
            FindObjectsOfType<CharacterSpawnMarker>()
                .Select(CharactersData).ToArray();

        private HallData[] FindHallItems() =>
            FindObjectsOfType<HallItemSpawnMarker>()
                .Select(HallItemsData).ToArray();
        
        private KitchenData[] FindKitchemItems() =>
            FindObjectsOfType<KitchenItemSpawnMarker>()
                .Select(KitchenItemsData).ToArray();

        private StorageData[] FindStorageItems() =>
            FindObjectsOfType<Crate>()
                .Select(StorageItemsData).ToArray();

        private CharacterData CharactersData(CharacterSpawnMarker characterSpawnMarker) =>
            new()
            {
                TypeId = characterSpawnMarker.TypeId,
                Position = characterSpawnMarker.transform.position,
                Rotation = characterSpawnMarker.transform.eulerAngles,
            };
        
        private HallData HallItemsData(HallItemSpawnMarker hallItem) =>
            new()
            {
                TypeId = hallItem.TypeId,
                PurchaseOrder = hallItem.PurchaseOrder,
                Position = hallItem.transform.position,
                Rotation = hallItem.transform.eulerAngles,
            };
        
        private KitchenData KitchenItemsData(KitchenItemSpawnMarker kitchenItem) =>
            new()
            {
                TypeId = kitchenItem.TypeId,
                PurchaseOrder = kitchenItem.PurchaseOrder,
                Position = kitchenItem.transform.position,
                Rotation = kitchenItem.transform.eulerAngles,
            };

        private StorageData StorageItemsData(Crate crate) =>
            new()
            {
                Position = crate.transform.position,
                Rotation = crate.transform.eulerAngles,
            };
        
        
    }
}