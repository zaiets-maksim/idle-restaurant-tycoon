using System;


namespace Infrastructure
{
    public abstract class GameStateEntity
    {
        public virtual void Enter()
        {
        }
 
        public virtual void OnLevelLoad()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void Exit()
        {
        }
    }
}