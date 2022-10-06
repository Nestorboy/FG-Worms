using UnityEngine;

namespace Weapons
{
    public class RicochetRifle : Weapon
    {
        [SerializeField] private Transform _projectileOrigin;
        [SerializeField] private GameObject _projectilePrefab;
        
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Z))
                UsePrimary();
        }

        public override void UsePrimary()
        {
            if (Count-- > 0)
            {
                Projectile projectile = Instantiate(_projectilePrefab, _projectileOrigin.position, _projectileOrigin.rotation).GetComponent<Projectile>();
                projectile.Strength = Strength;
            }
        }

        public override void UseSecondary()
        {
            
        }
    }
}
