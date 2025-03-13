using Unity.AI.Navigation;
using UnityEngine;

namespace Services.SurfaceUpdaterService
{
    public class SurfaceUpdaterService : ISurfaceUpdaterService
    {
        private NavMeshSurface _navMeshKitchen;
        private NavMeshSurface _navMeshHall;
        private NavMeshSurface _navMeshCommon;

        public void Init()
        {
            // var navMesh = Object.FindObjectsOfType<NavMeshSurfaceType>();
            // _navMeshKitchen = navMesh.FirstOrDefault(x => x.TypeId == NavMeshTypeId.Kitchen)?.Surface;
            // _navMeshHall = navMesh.FirstOrDefault(x => x.TypeId == NavMeshTypeId.Hall)?.Surface;
            
            _navMeshCommon = Object.FindObjectOfType<NavMeshSurface>();
        }

        public void UpdateKitchen() => _navMeshKitchen.BuildNavMesh();
        public void UpdateHall() => _navMeshHall.BuildNavMesh();
        public void UpdateCommon() => _navMeshCommon.BuildNavMesh();
    }

    public interface ISurfaceUpdaterService
    {
        void Init();
        void UpdateKitchen();
        void UpdateHall();
        void UpdateCommon();
    }
}