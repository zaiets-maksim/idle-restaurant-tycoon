using System.Threading;
using System.Threading.Tasks;
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
        
        protected override Task Enter(CancellationToken ct)
        {
            _personAnimator.Idle();
            return Task.CompletedTask;
        }

        public override void Exit()
        {
        }
    }
}
