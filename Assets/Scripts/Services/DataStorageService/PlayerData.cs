using System;

namespace Services.DataStorageService
{
    [Serializable]
    public class PlayerData
    {
        public PlayerProgressData ProgressData = new PlayerProgressData();
    }
}