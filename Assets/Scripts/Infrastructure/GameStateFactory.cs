using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.States;

namespace Infrastructure
{
    public class GameStateFactory
    {
        private readonly LoadProgressState _loadProgressState;
        private readonly LoadMenuState _loadMenuFactory;
        private readonly LoadLevelState _loadLevelState;
        public Dictionary<Type, GameStateEntity> States { get; private set; }

        /// <summary>
        /// Get the requested game state entity
        /// </summary>
        /// <param name="gameState">State we want to get</param>
        /// <returns>The requested game state entity</returns>
        ///
        public GameStateEntity GetState<T>() where T : GameStateEntity => 
            States.GetValueOrDefault(typeof(T));

        public void InitStates(Dictionary<Type, GameStateEntity> gameStateEntities) => 
            States = gameStateEntities;
    }
}