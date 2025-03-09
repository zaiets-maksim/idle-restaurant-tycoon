using StaticData.Configs;

public interface IStaticDataService
{
    void LoadData();
    // GameStaticData GameConfig();
    WindowConfig ForWindow(WindowTypeId windowTypeId);
    // BalanceStaticData Balance();
}
