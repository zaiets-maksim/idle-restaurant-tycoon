using StaticData;
using StaticData.Configs;
using StaticData.Levels;
using StaticData.TypeId;

namespace Services.StaticDataService
{
    public interface IStaticDataService
    {
        void LoadData();
        // GameStaticData GameConfig();
        WindowConfig ForWindow(WindowTypeId typeId);
        KitchenItemConfig ForKitchenItem(KitchenItemTypeId typeId);
        HallItemConfig ForHallItem(HallItemTypeId typeId);
        LevelStaticData LevelConfig();

        // BalanceStaticData Balance();
    }
}
