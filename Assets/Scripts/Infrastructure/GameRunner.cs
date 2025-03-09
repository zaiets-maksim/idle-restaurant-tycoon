using JetBrains.Annotations;
using UnityEngine;

namespace Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField, NotNull] private ProjectContext projectContext;

        private void Awake()
        {
            if (!FindObjectOfType<ProjectContext>()) 
                Instantiate(projectContext);

            Destroy(gameObject);
        }
    }
}