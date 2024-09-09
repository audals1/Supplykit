using UnityEngine;

namespace Actor
{
    public class DestroyAfterPlaying : MonoBehaviour
    {
        private ParticleSystem _ps;
        
        private void Awake()
        {
            _ps = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (!_ps.isPlaying) Destroy(gameObject);
        }
    }
}
