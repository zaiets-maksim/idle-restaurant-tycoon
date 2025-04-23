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
    
    public override async void Enter()
    {
        await _person.MoveToSpawn();
        
        _personBehavior.ChangeState<IdleState>();
    }

    public override void Exit()
    {
        
    }
}
