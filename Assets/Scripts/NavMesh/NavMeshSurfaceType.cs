using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Serialization;

    public class NavMeshSurfaceType : MonoBehaviour
    {
        [SerializeField] private NavMeshTypeId _typeId;
        [SerializeField] private NavMeshSurface surface;
        
        public NavMeshTypeId TypeId => _typeId;
        public NavMeshSurface Surface => surface;
    }

