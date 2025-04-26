using System;

namespace Connect4.Scripts.Infrastructure
{
    public interface ILoadingCurtain
    {
        public void SetDelay(float delay);
        public void SetAnimationSpeed(float speed);
        event Action OnComplete;
        void Show();
        void Hide();
    }
}