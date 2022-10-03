using UnityEngine;

namespace Weapons
{
    public class RicochetRifle : Weapon
    {
        private LineRenderer _line;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
                UsePrimary();
        }

        public override void UsePrimary()
        {
            _line = new GameObject("Ricochet Trail", typeof(LineRenderer)).GetComponent<LineRenderer>();
            _line.widthMultiplier = 0.01f;

            Vector3 position = transform.position;
            Vector3 forward = transform.forward;

            Vector3[] positions = new Vector3[5];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = position;
                if (Physics.Raycast(position, forward, out RaycastHit hitInfo, 20f))
                {
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
            
            _line.SetPositions(positions);
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
