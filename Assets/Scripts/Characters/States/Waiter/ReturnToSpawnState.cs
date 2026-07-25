using System.Threading;
using System.Threading.Tasks;
using Characters;
using Characters.PersonStateMachine;
using Characters.States;

public class ReturnToSpawnState : PersonBaseState
{
    private readonly PersonBehavior _personBehavior;
    private readonly Person _person;

    public ReturnToSpawnState(PersonBehavior personBehavior, Person person)
    {
        _person = person;
        _personBehavior = personBehavior;
    }
    
    protected override async Task Enter(CancellationToken ct)
    {
        await _person.MoveToSpawn();
        ct.ThrowIfCancellationRequested();
        
        _personBehavior.ChangeState<IdleState>();
    }

    public override void Exit()
    {
    }
}
