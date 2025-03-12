using Characters.PersonStateMachine;

namespace Characters.States
{
    public class IdleState : PersonBaseState
    {
        private readonly PersonAnimator _personAnimator;

        public IdleState(PersonAnimator personAnimator)
        {
            _personAnimator = personAnimator;
        }
        
        public override async void Enter()
        {
            _personAnimator.Idle();
        }

        public override void Exit()
        {
            
        }
    }
}
