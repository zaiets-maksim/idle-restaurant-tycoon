using System;
using UnityEngine;

namespace tetris.Scripts.Services.PauseService
{
    public class PauseService : IPauseService
    {
        public event Action OnPause;
        public event Action OnResume;
        public bool IsPause { get; private set; }

        public void Pause()
        {
            Time.timeScale = 0f;
            OnPause?.Invoke();
            IsPause = true;
        }

        public void Resume()
        {
            Time.timeScale = 1f;
            OnResume?.Invoke();
            IsPause = false;
        }
    }

    public interface IPauseService
    {
        void Pause();
        void Resume();
        bool IsPause { get; }
    
        event Action OnPause;
        event Action OnResume;
    }
}