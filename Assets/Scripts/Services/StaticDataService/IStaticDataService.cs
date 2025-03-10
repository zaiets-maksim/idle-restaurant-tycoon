using StaticData;
using StaticData.Configs;

namespace Services.StaticDataService
{
    public interface IStaticDataService
    {
        void LoadData();
        // GameStaticData GameConfig();
        WindowConfig ForWindow(WindowTypeId windowTypeId);
        KitchenItemConfig ForKitchenItem(KitchenItemTypeId kitchenItemTypeId);
        LevelStaticData LevelConfig();
        // BalanceStaticData Balance();
    }
}
