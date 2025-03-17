using StaticData.TypeId;
using UnityEngine;

namespace SpawnMarkers
{
    public class CharacterSpawnMarker : MonoBehaviour
    {
        [SerializeField] private CharacterTypeId _typeId;
        public CharacterTypeId TypeId => _typeId;
    }
}
