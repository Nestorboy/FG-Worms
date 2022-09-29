using UnityEngine;

namespace Weapons
{
    public abstract class WeaponSO : ScriptableObject
    {
        [SerializeField] public float strength = 10;
        [SerializeField] public int count = 4;

        public abstract void UsePrimary();
        public abstract void UseSecondary();
    }
}