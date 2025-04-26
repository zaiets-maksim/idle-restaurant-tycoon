using System.Collections.Generic;
using System.Linq;
using Connect4.Scripts.StaticData;
using StaticData;
using StaticData.Configs;
using StaticData.Levels;
using StaticData.TypeId;
using UnityEngine;

namespace Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameConfigPath = "StaticData/GameConfig";
        private const string BalanceConfigPath = "StaticData/Balance";
        private const string WindowsStaticDataPath = "StaticData/WindowsStaticData";
        private const string KitchenItemStaticDataPath = "StaticData/KitchenItemStaticData";
        private const string HallItemStaticDataPath = "StaticData/HallItemStaticData";
        private const string CharacterStaticDataPath = "StaticData/CharacterStaticData";
        private const string DishStaticDataPath = "StaticData/DishStaticData";
        private const string LevelStaticDataPath = "StaticData/LevelStaticData";
        private const string PopUpWindowsStaticDataPath = "StaticData/PopUpWindowsStaticData";
        
        private GameStaticData _gameStaticData;
        private BalanceStaticData _balanceStaticData;
        private LevelStaticData _levelStaticData;
        
        private Dictionary<WindowTypeId, WindowConfig> _windowConfigs;
        private Dictionary<KitchenItemTypeId, KitchenItemConfig> _kitchenItemConfigs;
        private Dictionary<HallItemTypeId, HallItemConfig> _hallItemConfigs;
        private CharacterStaticData _characterStaticData;
        private Dictionary<DishTypeId, DishConfig> _dishConfigs;
        private Dictionary<CharacterTypeId, CharacterConfig> _characterConfigs;
        private Dictionary<CustomerTypeId, CustomerAppearance> _customerAppearances;
        // private Dictionary<PopUpWindowTypeId, PopUpWindowConfig> _popUpWindowConfigs;

        public void LoadData()
        {
            _gameStaticData = Resources
                .Load<GameStaticData>(GameConfigPath);

            _balanceStaticData = Resources
                .Load<BalanceStaticData>(BalanceConfigPath);
            
            _windowConfigs = Resources
                .Load<WindowStaticData>(WindowsStaticDataPath)
                .Configs.ToDictionary(x => x.WindowTypeId, x => x);
            
            _kitchenItemConfigs = Resources
                .Load<KitchenItemStaticData>(KitchenItemStaticDataPath)
                .Configs.ToDictionary(x => x.TypeId, x => x);
            
            _hallItemConfigs = Resources
                .Load<HallItemStaticData>(HallItemStaticDataPath)
                .Configs.ToDictionary(x => x.TypeId, x => x);
            
            _dishConfigs = Resources
                .Load<DishStaticData>(DishStaticDataPath)
                .Configs.ToDictionary(x => x.TypeId, x => x);

            _characterStaticData = Resources.Load<CharacterStaticData>(CharacterStaticDataPath);
            _characterConfigs = _characterStaticData.Configs.ToDictionary(x => x.TypeId, x => x);
            _customerAppearances = _characterStaticData.Appearances.ToDictionary(x => x.TypeId, x => x);
            
            _levelStaticData = Resources
                .Load<LevelStaticData>(LevelStaticDataPath);
            
            // _popUpWindowConfigs = Resources
            //     .Load<PopUpWindowStaticData>(PopUpWindowsStaticDataPath)
            //     .Configs.ToDictionary(x => x.PopUpWindowTypeId, x => x);
        }

        public BalanceStaticData Balance() => 
            _balanceStaticData;

        public CharacterConfig ForCharacter(CharacterTypeId typeId) => 
            _characterConfigs[typeId];

        public DishConfig ForDish(DishTypeId typeId) => _dishConfigs[typeId];

        public CustomerAppearance ForCharacterAppearance(CustomerTypeId typeId) => 
            _customerAppearances[typeId];

        public WindowConfig ForWindow(WindowTypeId typeId) => 
            _windowConfigs[typeId];
        
        public KitchenItemConfig ForKitchenItem(KitchenItemTypeId typeId) => 
            _kitchenItemConfigs[typeId];

        public HallItemConfig ForHallItem(HallItemTypeId typeId) => 
            _hallItemConfigs[typeId];

        public LevelStaticData LevelConfig() => 
            _levelStaticData;

        public CustomerTypeId[] GetCustomerTypeIdsInAppearance() => 
            _customerAppearances.Select(x => x.Key).ToArray();
        
        public DishTypeId[] GetDishTypeIds() => 
            _dishConfigs.Select(x => x.Key).ToArray();

        public GameStaticData GameConfig() => _gameStaticData;


        // public PopUpWindowConfig ForPopUpWindow(PopUpWindowTypeId popUpWindowTypeId) => 
        //     _popUpWindowConfigs[popUpWindowTypeId];
    }
}