using UnityEngine;

namespace Infrastructure
{
    public static class ProjectBootstrap
    {
        private const string ProjectcontextPath = "Infrastructure/ProjectContext";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var prefab = Resources.Load<ProjectContext>(ProjectcontextPath);
            Object.Instantiate(prefab);
        }
    }
}