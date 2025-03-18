using System.Threading.Tasks;
using Characters.Customers;
using Characters.PersonStateMachine;

internal class LeaveState : PersonBaseState
{
    private readonly Customer _customer;

    public LeaveState(Customer customer)
    {
        _customer = customer;
    }

    public override async void Enter()
    {
        _tcs = new TaskCompletionSource<bool>();

        _customer.LeaveChair();
        _customer.EnableAgent();
        _customer.LeaveRestaurant();
    }

    public override void Exit()
    {
    }
}