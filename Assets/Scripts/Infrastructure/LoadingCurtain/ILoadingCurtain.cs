using System;

namespace Infrastructure
{
    public interface ILoadingCurtain
    {
        event Action OnComplete;
        void Show();
        void Hide();
    }
}