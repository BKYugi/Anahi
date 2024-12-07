using UnityEngine;

namespace FT
{
    public class Utility_DestroyAfterTime : MonoBehaviour
    {

        [SerializeField] float timeUntilDestroyed = 5;

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroyed);
        }

    }
}
