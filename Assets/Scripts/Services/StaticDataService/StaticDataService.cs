using System.Collections.Generic;
using System.Linq;
using StaticData;
using StaticData.Configs;
using UnityEngine;

namespace Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private const string GameConfigPath = "StaticData/GameConfig";
        private const string BalanceConfigPath = "StaticData/Balance";
        private const string WindowsStaticDataPath = "StaticData/WindowsStaticData";
        private const string KitchenItemStaticDataPath = "StaticData/KitchenItemStaticData";
        private const string LevelStaticDataPath = "StaticData/LevelStaticData";
        private const string PopUpWindowsStaticDataPath = "StaticData/PopUpWindowsStaticData";
        
        // private GameStaticData _gameStaticData;
        // private BalanceStaticData _balanceStaticData;
        private Dictionary<WindowTypeId, WindowConfig> _windowConfigs;
        private Dictionary<KitchenItemTypeId, KitchenItemConfig> _kitchenItemConfigs;
        private LevelStaticData _levelStaticData;

        // private Dictionary<PopUpWindowTypeId, PopUpWindowConfig> _popUpWindowConfigs;

        public void LoadData()
        {
            // _gameStaticData = Resources
            //     .Load<GameStaticData>(GameConfigPath);
            //
            // _balanceStaticData = Resources
            //     .Load<BalanceStaticData>(BalanceConfigPath);
            
            _windowConfigs = Resources
                .Load<WindowStaticData>(WindowsStaticDataPath)
                .Configs.ToDictionary(x => x.WindowTypeId, x => x);
            
            _kitchenItemConfigs = Resources
                .Load<KitchenItemStaticData>(KitchenItemStaticDataPath)
                .Configs.ToDictionary(x => x.TypeId, x => x);
            
            _levelStaticData = Resources
                .Load<LevelStaticData>(LevelStaticDataPath);
            
            // _popUpWindowConfigs = Resources
            //     .Load<PopUpWindowStaticData>(PopUpWindowsStaticDataPath)
            //     .Configs.ToDictionary(x => x.PopUpWindowTypeId, x => x);
        }

        // public BalanceStaticData Balance() => 
        //     _balanceStaticData;
        
        public WindowConfig ForWindow(WindowTypeId windowTypeId) => 
            _windowConfigs[windowTypeId];
        
        public KitchenItemConfig ForKitchenItem(KitchenItemTypeId kitchenItemTypeId) => 
            _kitchenItemConfigs[kitchenItemTypeId];
        
        public LevelStaticData LevelConfig() => 
            _levelStaticData;
        

        // public PopUpWindowConfig ForPopUpWindow(PopUpWindowTypeId popUpWindowTypeId) => 
        //     _popUpWindowConfigs[popUpWindowTypeId];
        
        // public GameStaticData GameConfig() =>
        //     _gameStaticData;
    }
}