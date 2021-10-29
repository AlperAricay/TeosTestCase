using System.Collections;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private float _bigBulletScale;
        [SerializeField] private float _defaultScale = 0.1f;
        [SerializeField] private Material _redMaterial;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ParticleSystem _explosionParticle;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionForce;
        [SerializeField] private LayerMask _explosionTargetLayer;
        
        private readonly WaitForSeconds _recycleDelay = new WaitForSeconds(1.5f);
        private readonly WaitForSeconds _explosionDelay = new WaitForSeconds(.5f);

        private Rigidbody _rigidbody;

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        public void Initialize(bool isExplosive, bool isBig, bool isRed)
        {
            transform.localScale = isBig ? Vector3.one * _bigBulletScale : Vector3.one * _defaultScale;
            _meshRenderer.material = isRed ? _redMaterial : _defaultMaterial;
            gameObject.SetActive(true);
            StartCoroutine(isExplosive ? ExplosionRoutine() : RecycleRoutine());
        }

        private IEnumerator ExplosionRoutine()
        {
            yield return _explosionDelay;
            StartCoroutine(Explode());
        }

        private IEnumerator Explode()
        {
            _rigidbody.velocity = Vector3.zero;
            _meshRenderer.enabled = false;
            _explosionParticle.Play(true);
            
            const int maxColliders = 10;
            var hitColliders = new Collider[maxColliders];
            
            var timePassed = 0f;
            while (timePassed < _explosionParticle.main.duration / 2)
            {
                var size = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, hitColliders, _explosionTargetLayer);
                for (int i = 0; i < size; i++)
                    if (hitColliders[i].TryGetComponent<Rigidbody>(out var rb))
                        rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1f, ForceMode.Acceleration);

                timePassed += Time.deltaTime;
                yield return null;
            }
            Disable();
        }

        private IEnumerator RecycleRoutine()
        {
            yield return _recycleDelay;
            Disable();
        }

        private void Disable()
        {
            gameObject.SetActive(false);
            _meshRenderer.enabled = true;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            _rigidbody.useGravity = false;
            _rigidbody.velocity = Vector3.zero;
        }

        private void OnCollisionEnter() => _rigidbody.useGravity = true;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}
