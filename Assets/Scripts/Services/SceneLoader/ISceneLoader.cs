using System;

namespace PiratesIdle.Scripts.Infrastructure
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLevelLoad);
    }
}