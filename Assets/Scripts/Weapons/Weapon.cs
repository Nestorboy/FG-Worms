using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public float strength = 10;
        public int count = 4;
        
        public abstract void UsePrimary();
        public abstract void UseSecondary();
    }
}