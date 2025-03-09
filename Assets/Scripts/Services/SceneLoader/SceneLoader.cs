using System;
using System.Collections;
using StudentHistory.Scripts;
using StudentHistory.Scripts.Services.SceneLoader;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SceneLoader
{
    public class SceneLoader : ISceneLoader
    {
        private readonly IStaticDataService _staticData;
        private readonly CoroutineRunner _coroutineRunner;
        
        public SceneTypeId LastSceneTypeId { get; private set; }
        public SceneTypeId CurrentSceneTypeId { get; private set; }
        public SceneTypeId NextSceneTypeId { get; private set; }

        public SceneLoader(CoroutineRunner coroutineRunner, IStaticDataService staticData)
        {
            _staticData = staticData;
            _coroutineRunner = coroutineRunner;
        }

        public void SetCurrentScene(SceneTypeId sceneTypeId)
        {
            CurrentSceneTypeId = sceneTypeId;
        }

        public void SetNextScene(SceneTypeId sceneTypeId)
        {
            NextSceneTypeId = sceneTypeId;
        }

        public void Load(SceneTypeId sceneTypeId, GameObject stateMachine) =>
            _coroutineRunner.StartCoroutine(LoadScene(sceneTypeId, stateMachine));
        public void Load(SceneTypeId sceneTypeId, Action OnLevelLoad) =>
            _coroutineRunner.StartCoroutine(LoadScene(sceneTypeId, OnLevelLoad));

        public IEnumerator LoadScene(SceneTypeId sceneTypeId, GameObject stateMachine)
        {
            if (sceneTypeId == SceneTypeId.Unknown)
                yield break;

            // string scene = _staticDataService.ForLevel(sceneTypeId).Configs.Name;
            string scene = String.Empty;
            Scene currentScene = SceneManager.GetActiveScene();

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            
            while (!waitNextScene.isDone)
                yield return null;

            SetCurrentScene(sceneTypeId);

            SceneManager.MoveGameObjectToScene(stateMachine, SceneManager.GetSceneByName(scene));
            SceneManager.UnloadSceneAsync(currentScene);
        }

        public IEnumerator LoadScene(SceneTypeId sceneTypeId)
        {
            if (sceneTypeId == SceneTypeId.Unknown)
                yield break;

            string scene = sceneTypeId.ToString();
            Scene currentScene = SceneManager.GetActiveScene();

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            
            while (!waitNextScene.isDone)
                yield return null;

            SetCurrentScene(sceneTypeId);

            SceneManager.UnloadSceneAsync(currentScene);
        }
        private IEnumerator LoadScene(SceneTypeId sceneTypeId, Action onLevelLoad)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneTypeId.ToString(), LoadSceneMode.Additive);
            while (!waitNextScene.isDone)
                yield return null;
            
            SetCurrentScene(sceneTypeId);
            SceneManager.UnloadSceneAsync(currentScene);
            

            onLevelLoad?.Invoke();
        }

        public void LoadFirstScene() =>
            SceneManager.LoadScene(StateTypeId.Menu.ToString());
    }
}
