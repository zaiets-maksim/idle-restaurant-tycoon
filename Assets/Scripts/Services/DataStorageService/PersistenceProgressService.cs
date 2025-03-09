using Services.DataStorageService;

namespace StudentHistory.Scripts.Services.DataStorageService
{
    public class PersistenceProgressService : IPersistenceProgressService
    {
        public PlayerData PlayerData { get; set; }
    }
}