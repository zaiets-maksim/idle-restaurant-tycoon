using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class StateMachine : IStateMachine
    {
        private GameStateEntity _currentState;
        private GameStateEntity _previousState;
        private GameStateEntity _nextState;
        private GameStateEntity _gameStateEntity;
        
        private readonly GameStateFactory _gameStateFactory;
        
        public StateMachine(GameStateFactory gameStateFactory)
        {
            _gameStateFactory = gameStateFactory;
        }

        public void Enter<T>(string payload = null) where T : GameStateEntity
        {
            GameStateEntity state = _gameStateFactory.GetState<T>();

            if (state == _currentState)
                return;
            
            _currentState?.Exit();
            state.Enter();
            
            Debug.Log($"{state} Enter");
            Debug.Log($"{_currentState} Exit");
            
            _currentState = state;
        }
    }

    public interface IStateMachine
    {
        void Enter<T>(string payload = null) where T : GameStateEntity;
    }
}