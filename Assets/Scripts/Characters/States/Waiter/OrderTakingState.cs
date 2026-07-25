using System.Threading;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.PersonStateMachine;
using UnityEngine;

namespace Characters.States.Waiter
{
    public class OrderTakingState : PersonBaseState
    {
        private WaiterBehavior _waiterBehavior;
        private Transform _transform;
        private PersonMover _personMover;
        private PersonAnimator _personAnimator;

        public OrderTakingState(WaiterBehavior waiterBehavior, Transform transform, PersonMover personMover, PersonAnimator personAnimator)
        {
            _personAnimator = personAnimator;
            _personMover = personMover;
            _transform = transform;
        }

        protected override Task Enter(CancellationToken ct)
        {
            return Task.CompletedTask;
        }
        

        public override void Exit()
        {
        }
    }
}
