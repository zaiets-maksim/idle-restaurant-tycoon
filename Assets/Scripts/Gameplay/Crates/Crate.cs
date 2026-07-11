using UnityEngine;

namespace Crates
{
    public class Crate : MonoBehaviour
    {
        [SerializeField] private Transform _interactionPoint;

        public Transform InteractionPoint => _interactionPoint;
    }
}
