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

        private LineRenderer _line;
        private Vector3[] _positions = new Vector3[3];
        private float _distanceTraveled = 0f;
        private int _bounceCount = 0;

        private void Awake()
        {
            _line = this.AddComponent<LineRenderer>();
            _line.widthMultiplier = 0.01f;
            _line.positionCount = 3;
            for (int i = 0; i < _positions.Length; i++)
                _positions[i] = transform.position;

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
                        if (hit.transform.TryGetComponent(out player))
                        {
                            player.Damage(Strength * _bounceCount);
                            float knockbackFalloff = Mathf.Clamp01(Vector3.Dot(rayDir, hit.normal));
                            player.InputController.ImpulseVelocity(rayDir * (Knockback * knockbackFalloff));
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
