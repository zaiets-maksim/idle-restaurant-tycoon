using Unity.AI.Navigation;
using UnityEngine;

namespace Services.SurfaceUpdaterService
{
    public class SurfaceUpdaterService : ISurfaceUpdaterService
    {
        private NavMeshSurface _navMesh;

        public void Init() => _navMesh = Object.FindObjectOfType<NavMeshSurface>();

        public void Update() => _navMesh.BuildNavMesh();
    }

    public interface ISurfaceUpdaterService
    {
        void Init();
        void Update();
    }
}