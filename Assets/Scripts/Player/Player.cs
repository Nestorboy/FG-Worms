using System;
using Game;
using Input;
using UnityEngine;
using Visuals;

namespace Player
{
    public class Player : MonoBehaviour
    {
        public PlayerInputController InputController;
        public Transform WeaponContainer;
        
        public float MaxHealth = 100f;
        public float Health = 100f;

        private Renderer _renderer;
        private MaterialPropertyBlock _propBlock;

        public Action<Player, float> onDamage;
        
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
                Kill();

            UpdateVisuals();
            
            onDamage?.Invoke(this, points);
        }
    
        private void Kill()
        {
            Health = 0f;
            IsAlive = false;
        }

        private void UpdateVisuals()
        {
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(ShaderIDs.FluidHeight, Health / MaxHealth);
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}