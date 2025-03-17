using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Characters.Behaviors;
using Characters.Customers;
using Characters.PersonStateMachine;
using Interactable;
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

        public override void Enter()
        {
            _tcs = new TaskCompletionSource<bool>();
        }
        

        public override void Exit()
        {
            
        }
    
    }
}
