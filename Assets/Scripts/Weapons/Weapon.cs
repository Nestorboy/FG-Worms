using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public float Strength = 10;
        public int Count = 4;
        
        public abstract void UsePrimary();
        public abstract void UseSecondary();
    }
}