using System;
using UnityEngine;

namespace Weapons
{
    public abstract class Projectile : MonoBehaviour
    {
        public event Action onHit;

        public float Strength;
    }
}