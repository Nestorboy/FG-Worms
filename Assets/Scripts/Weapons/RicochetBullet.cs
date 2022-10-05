using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Weapons
{
    public class RicochetBullet : Projectile
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _knockback = 2f;
        
        private LineRenderer _line;
        private Vector3[] _positions = new Vector3[3];

        private void Awake()
        {
            _line = this.AddComponent<LineRenderer>();
            _line.widthMultiplier = 0.01f;
            _line.positionCount = 3;
            for (int i = 0; i < _positions.Length; i++)
                _positions[i] = transform.position;
        }

        private void FixedUpdate()
        {
            float travelDistance = _speed * Time.fixedDeltaTime;
            Vector3 rayPos = transform.position;
            Vector3 rayDir = transform.forward;
            int bounceCount = 0;

            _positions[0] = rayPos;
            while (travelDistance > 0f)
            {
                float traveled;
                if (Physics.Raycast(rayPos, rayDir, out RaycastHit hit, travelDistance))
                {
                    if (bounceCount++ < 1)
                        _positions[1] = hit.point;

                    traveled = Vector3.Distance(rayPos, hit.point);
                    
                    rayPos = hit.point;
                    rayDir = Vector3.Reflect(rayDir, hit.normal);
                }
                else
                {
                    traveled = travelDistance;

                    _positions[1] = rayPos + rayDir * travelDistance * 0.5f;
                    
                    rayPos += rayDir * travelDistance;
                }
                
                travelDistance -= traveled;
            }
            
            _positions[2] = rayPos;
            
            transform.SetPositionAndRotation(rayPos, Quaternion.LookRotation(rayDir));
            _line.SetPositions(_positions);
        }
    }
}
