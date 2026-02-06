using Infrastructure;
using Services.PurchasedItemRegistry;
using Services.StaticDataService;
using UnityEngine;

public class TestService : MonoBehaviour, ITestService
{
    private void Start()
    {
        Debug.Log("TestService initialized!");
        var a = ProjectContext.Instance.Container.Resolve<IStaticDataService>();
        var b = ProjectContext.Get<IPurchasedItemRegistry>();
        Debug.Log(a.Balance().Customers.Speed);
        Debug.Log(b.Stuff.Capacity);
    }
}

public interface ITestService
{
}