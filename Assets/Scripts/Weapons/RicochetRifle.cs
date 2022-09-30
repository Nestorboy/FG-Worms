using UnityEngine;

namespace Weapons
{
    public class RicochetRifle : Weapon
    {
        private TrailRenderer _trail;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
                UsePrimary();
        }

        public override void UsePrimary()
        {
            _trail = new GameObject("Ricochet Trail", typeof(TrailRenderer)).GetComponent<TrailRenderer>();
            _trail.widthMultiplier = 0.01f;
            _trail.time = 1f;
            _trail.autodestruct = true;

            Vector3 position = transform.position;
            Vector3 forward = transform.forward;

            for (int i = 0; i < 5; i++)
            {
                _trail.AddPosition(position);
                if (Physics.Raycast(position, forward, out RaycastHit hitInfo, 20f))
                {
                    _trail.AddPosition(position);
                    Debug.DrawLine(position, hitInfo.point, Color.black, 5f);
                    position = hitInfo.point;
                    forward = Vector3.Reflect(forward, hitInfo.normal);

                    if (hitInfo.transform.TryGetComponent(out Player.Player player))
                    {
                        OnHit(player, 10f + 10f * i); // 10 base +10 per bounce
                        break;
                    }
                }
                else
                {
                    position = position + forward * 20f;
                    break;
                }
            }
        }

        public override void UseSecondary()
        {
            
        }

        public void OnHit(Player.Player player, float damage)
        {
            player.Damage(damage);
        }
    }
}
