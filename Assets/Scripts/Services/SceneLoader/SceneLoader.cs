using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PiratesIdle.Scripts.Infrastructure
{
    public class SceneLoader : ISceneLoader
    {
        public void Load(string name, Action onLevelLoad)
        {
            CoroutineRunner.instance.StartCoroutine(LoadLevel(name, onLevelLoad));
        }

        private IEnumerator LoadLevel(string name, Action onLevelLoad)
        {
            Debug.Log(name);
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(name);
            while (!waitNextScene.isDone)
                yield return null;

            onLevelLoad?.Invoke();
        }
    }
}