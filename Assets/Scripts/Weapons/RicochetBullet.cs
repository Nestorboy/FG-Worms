using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Weapons
{
    public class RicochetBullet : Projectile
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _maxDistance = 20f;
        [SerializeField] private int _maxBounces = 5;
        [SerializeField] private AnimationCurve _bounceStrengthMultiplier = AnimationCurve.Linear(0f, 1f, 1f, 2f);

        private float _distanceTraveled = 0f;
        private int _bounceCount = 0;

        private void Awake()
        {
            StartCoroutine(BulletSweep(transform.position, transform.forward));
        }

        private bool _ => (_distanceTraveled < _maxDistance) && (_bounceCount < _maxBounces);
        private WaitForFixedUpdate _waitForFixedUpdate;
        private IEnumerator BulletSweep(Vector3 pos, Vector3 dir)
        {
            Vector3 rayPos = pos;
            Vector3 rayDir = dir;
            
            for (;_;)
            {
                float distanceToTravel = Mathf.Min(_speed * Time.fixedDeltaTime, _maxDistance - _distanceTraveled);
                float distanceLeft = distanceToTravel;
                while (distanceLeft > 0)
                {
                    float traveled;
                    if (Physics.Raycast(rayPos, rayDir, out RaycastHit hit, distanceLeft))
                    {
                        traveled = Vector3.Distance(rayPos, hit.point);
                        
                        Player.Player player;
                        if (hit.collider.TryGetComponent(out player))
                        {
                            float bounceMultiplier = _bounceStrengthMultiplier.Evaluate((float)_bounceCount / _maxBounces);
                            float knockbackFalloff = Mathf.Clamp01(Vector3.Dot(rayDir, -hit.normal));
                            player.Damage(Strength * bounceMultiplier);
                            player.InputController.ImpulseVelocity(rayDir * (Knockback * knockbackFalloff * bounceMultiplier));
                        }

                        rayPos = hit.point;
                        rayDir = Vector3.Reflect(rayDir, hit.normal);
                        
                        if (_bounceCount >= _maxBounces)
                            break;
                        _bounceCount++;
                    }
                    else
                    {
                        traveled = distanceLeft;

                        Vector3 travelVector = rayDir * distanceLeft;
                        rayPos += travelVector;
                    }

                    _distanceTraveled += traveled;
                    distanceLeft -= traveled;
                }
                
                transform.SetPositionAndRotation(rayPos, Quaternion.LookRotation(rayDir));
                
                yield return _waitForFixedUpdate;
            }
        }
    }
}
