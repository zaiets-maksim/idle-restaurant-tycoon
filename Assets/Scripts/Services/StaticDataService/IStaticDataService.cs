using Connect4.Scripts.StaticData;
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

        BalanceStaticData Balance();
        CharacterConfig ForCharacter(CharacterTypeId typeId);
        DishConfig ForDish(DishTypeId typeId);
        CustomerAppearance ForCharacterAppearance(CustomerTypeId typeId);
        CustomerTypeId[] GetCustomerTypeIdsInAppearance();
        DishTypeId[] GetDishTypeIds();
        GameStaticData GameConfig();
    }
}
