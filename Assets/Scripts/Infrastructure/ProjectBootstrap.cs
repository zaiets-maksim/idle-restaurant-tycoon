using UnityEngine;

namespace Infrastructure
{
    public static class ProjectBootstrap
    {
        private const string ProjectContextPath = "Infrastructure/ProjectContext";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            Application.targetFrameRate = 60;

            var prefab = Resources.Load<ProjectContext>(ProjectContextPath);
            Object.Instantiate(prefab);
        }
    }
}