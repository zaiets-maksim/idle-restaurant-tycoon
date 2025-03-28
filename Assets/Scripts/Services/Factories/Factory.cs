using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Factories
{
    public abstract class Factory
    {
        protected GameObject InstantiateOnActiveScene(string prefabName)
        {
            var prefab = LoadPrefab(prefabName);
            GameObject gameObject = Object.Instantiate(prefab);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiateOnActiveScene(string prefabName, Transform parent)
        {
            var prefab = LoadPrefab(prefabName);
            GameObject gameObject = Object.Instantiate(prefab, parent);
            return MoveToCurrentScene(gameObject);
        }

        protected GameObject InstantiateOnActiveScene(string prefabName, Vector3 position, Quaternion rotation, Transform parent)
        {
            var prefab = LoadPrefab(prefabName);
            GameObject gameObject = Object.Instantiate(prefab, position, rotation, parent);
            return MoveToCurrentScene(gameObject);
        }
        
        protected T InstantiateOnActiveScene<T>(GameObject prefab, Vector3 position, Vector3 rotation, Transform parent)
        {
            GameObject gameObject = Object.Instantiate(prefab, position, Quaternion.Euler(rotation), parent);
            return MoveToCurrentScene(gameObject).GetComponent<T>();
        }

        protected GameObject MoveToCurrentScene(GameObject gameObject)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            return gameObject;
        }

        private GameObject LoadPrefab(string name)
        {
            return Resources.Load<GameObject>(name);
        }
    }
}