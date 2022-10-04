using System;
using System.Collections;
using UnityEngine;
using Visuals;

namespace Weapons
{
    [RequireComponent(typeof(Collider))]
    public class Bomb : Projectile
    {
        [SerializeField] private float _explosionRadius = 2f;
        [SerializeField] private AnimationCurve _explosionFalloff = AnimationCurve.Constant(0f, 1f, 1f);
        [SerializeField] private float _fuseDuration = 5f;
        private float _fuseTime = 0f;
        private bool _fuseStarted;
        
        [SerializeField] private Animator _animator;
        private bool _hasAnimator;

        [SerializeField] private ParticleSystem _particles;
        private bool _hasParticles;

        private int _playerLayer;
        private Collider[] _playerColliders;

        private int _terrainLayer;
        
        private Animator Animator
        {
            get => _animator;
            set
            {
                _animator = value;
                _hasAnimator = value != null;
            }
        }
        
        private ParticleSystem Particles
        {
            get => _particles;
            set
            {
                _particles = value;
                _hasParticles = value != null;
            }
        }
        
        private void Awake()
        {
            Animator = _animator;
            if (!_hasAnimator)
                Animator = GetComponent<Animator>();

            Particles = _particles;
            if (!_hasParticles)
                Particles = GetComponentInChildren<ParticleSystem>();

            _playerLayer = LayerMask.NameToLayer("Player");
            _playerColliders = new Collider[PlayerManager.Players.Length];
            
            _terrainLayer = LayerMask.NameToLayer("Terrain");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_hasAnimator)
                Animator.SetTrigger(AnimatorParameters.Impact);

            if (!_fuseStarted)
                StartCoroutine(FuseStart());
            
            //Debug.Log("OnCollisionEnter");
        }

        public void Explode()
        {
            if (!enabled || !gameObject.activeInHierarchy)
                return;

            Vector3 explosionPosition = transform.position;
            int playerCount = Physics.OverlapSphereNonAlloc(explosionPosition, _explosionRadius, _playerColliders, 1 << _playerLayer);

            for (int i = 0; i < playerCount; i++)
            {
                Player.Player player = _playerColliders[i].GetComponent<Player.Player>();

                
                Vector3 closestPoint = _playerColliders[i].ClosestPoint(explosionPosition);
                float distance = Vector3.Distance(explosionPosition, closestPoint);
                float normalizedDistance = distance / _explosionRadius;
                float strengthCoeff = _explosionFalloff.Evaluate(normalizedDistance);

                Debug.Log($"{player} took {Strength * strengthCoeff} damage.");
                player.Damage(Strength * strengthCoeff);
            }
            
            Particles.Play();
        }
        
        private IEnumerator FuseStart()
        {
            _fuseStarted = true;
            
            yield return StartCoroutine(TimerStart());

            Explode();
        }
        
        private bool w => _fuseTime < _fuseDuration;
        private IEnumerator TimerStart()
        {
            for (;w;) // Sad loop
            {
                _fuseTime += Time.deltaTime;
                
                if (_hasAnimator)
                    Animator.SetFloat(AnimatorParameters.Blend, _fuseTime / _fuseDuration);
                
                yield return null;
            }

            if (_hasAnimator)
                Animator.SetFloat(AnimatorParameters.Blend, _fuseTime / _fuseDuration);
            
            _fuseTime = _fuseDuration;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}