using _Developer.Scripts.Utilities;
using Services.DataStorageService;
using UnityEngine;

namespace Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string PlayerProgressKey = "PlayerProgress";

        private readonly IPersistenceProgressService _progress;

        public SaveLoadService(IPersistenceProgressService progress)
        {
            _progress = progress;
        }

        public void SaveProgress() => 
            PlayerPrefs.SetString(PlayerProgressKey, _progress.PlayerData.ToJson());

        public PlayerData LoadProgress() =>
            PlayerPrefs.GetString(PlayerProgressKey)?
                .ToDeserialize<PlayerData>();

        public void ResetProgress() => PlayerPrefs.DeleteKey(PlayerProgressKey);
    }

    public interface ISaveLoadService
    {
        PlayerData LoadProgress();
        void SaveProgress();
        void ResetProgress();
    }
}