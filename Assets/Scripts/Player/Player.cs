using System;
using UnityEngine;
using Visuals;
using Weapons;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public PlayerInputController InputController;
        public Transform WeaponContainer;
        
        public float MaxHealth = 100f;
        public float Health = 100f;
        
        private bool _hasWeapon;
        private Weapon _weapon;
        
        private Renderer _renderer;
        private MaterialPropertyBlock _propBlock;

        public Action<Player, float> OnDamage;
        public Action<Player> OnDeath;

        public bool HasWeapon => _hasWeapon;

        public Weapon Weapon
        {
            get => _weapon;
            set
            {
                if (HasWeapon)
                    _weapon.transform.SetParent(null, false);
                
                _weapon = value;
                _hasWeapon = value != null;
                if (HasWeapon)
                    _weapon.transform.SetParent(WeaponContainer, false);
            }
        }

        public float NormalizedHealth => Health / MaxHealth;
        
        public void Awake()
        {
            if (!InputController)
                InputController = GetComponent<PlayerInputController>();

            _propBlock = new MaterialPropertyBlock();
            _renderer = GetComponentInChildren<Renderer>();
        
            UpdateVisuals();
        }

        public bool IsAlive { get; private set; } = true;

        public void Damage(float points)
        {
            Health -= points;
            if (Health < 0f)
            {
                Health = 0f;
                if (IsAlive)
                    Kill();
            }

            UpdateVisuals();

            OnDamage?.Invoke(this, points);
        }
    
        private void Kill()
        {
            Health = 0f;
            IsAlive = false;
            
            OnDeath?.Invoke(this);
        }

        private void UpdateVisuals()
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(ShaderIDs.FluidHeight, Health / MaxHealth);
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}