using System;
using System.Collections;
using Infrastructure;
using StudentHistory.Scripts;
using UnityEngine;

namespace Services.SceneLoader
{
    public interface ISceneLoader
    {
        SceneTypeId LastSceneTypeId { get; }
        SceneTypeId CurrentSceneTypeId { get; }
        SceneTypeId NextSceneTypeId { get; }

        void SetCurrentScene(SceneTypeId sceneTypeId);
        void SetNextScene(SceneTypeId sceneTypeId);
        
        IEnumerator LoadScene(SceneTypeId sceneTypeId, GameObject stateMachine);
        IEnumerator LoadScene(SceneTypeId sceneTypeId);
        void Load(SceneTypeId sceneTypeId, GameObject stateMachine);
        void Load(SceneTypeId sceneTypeId, Action OnLevelLoad);
        void LoadFirstScene();
    }
}