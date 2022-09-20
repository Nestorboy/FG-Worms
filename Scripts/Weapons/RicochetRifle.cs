using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class RicochetRifle : MonoBehaviour, IWeapon
{
    private TrailRenderer trail;

    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
            UsePrimary();
    }

    public void UsePrimary()
    {
        trail = new GameObject("Ricochet Trail", typeof(TrailRenderer)).GetComponent<TrailRenderer>();
        trail.widthMultiplier = 0.01f;
        trail.time = 1f;
        trail.autodestruct = true;

        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        for (int i = 0; i < 5; i++)
        {
            trail.AddPosition(position);
            if (Physics.Raycast(position, forward, out RaycastHit hitInfo, 20f))
            {
                trail.AddPosition(position);
                Debug.DrawLine(position, hitInfo.point, Color.black, 5f);
                position = hitInfo.point;
                forward = Vector3.Reflect(forward, hitInfo.normal);

                if (hitInfo.transform.TryGetComponent(out Player player))
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

    public void UseSecondary()
    {
        
    }

    public void OnHit(Player player, float damage)
    {
        player.Damage(damage);
    }
}
