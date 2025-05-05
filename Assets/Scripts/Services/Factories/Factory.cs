using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Connect4.Scripts.Services.Factories
{
    public abstract class Factory
    {
        protected IInstantiator _instantiator;

        protected Factory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        protected GameObject InstantiateOnActiveScene(string uiRootPath)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiateOnActiveScene(string uiRootPath, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath, parent);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiateOnActiveScene(string uiRootPath, Vector3 position, Vector3 eulerAngles, Transform parent)
        {
            GameObject gameObject = _instantiator.InstantiatePrefabResource(uiRootPath, position, Quaternion.Euler(eulerAngles), parent);
            return MoveToCurrentScene(gameObject);
        }

        protected T InstantiateOnActiveScene<T>(GameObject prefab, Vector3 position, Vector3 eulerAngles, Transform parent)
        {
            var transform = _instantiator.InstantiatePrefab(prefab, position, Quaternion.Euler(eulerAngles), parent).transform;
            transform.position = position;
            transform.eulerAngles = eulerAngles;
            // transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles));
            transform.SetParent(parent);
            MoveToCurrentScene(transform.gameObject);
            
            return transform.gameObject.GetComponent<T>();
        }

        protected GameObject InstantiatePrefabOnActiveScene(GameObject prefab)
        {
            GameObject gameObject = _instantiator.InstantiatePrefab(prefab);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiatePrefab(GameObject prefab, Transform parent) => 
            _instantiator.InstantiatePrefab(prefab, parent);

        private GameObject MoveToCurrentScene(GameObject gameObject)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            return gameObject;
        }
    }
}