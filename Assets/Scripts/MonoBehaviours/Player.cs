using System;
using Input;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputController inputController;
    public Transform weaponContainer;

    public float maxHealth = 100f;
    public float health = 100f;
    
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    public float NormalizedHealth => health / maxHealth;
    
    public void Awake()
    {
        if (!inputController)
            inputController = GetComponent<PlayerInputController>();

        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponentInChildren<Renderer>();
        
        UpdateVisuals();
    }

    public bool IsAlive { get; private set; } = true;

    public void Damage(float points)
    {
        health -= points;
        
        if (health < 0f)
            Kill();

        UpdateVisuals();
    }
    
    private void Kill()
    {
        health = 0f;
        IsAlive = false;
    }

    private void UpdateVisuals()
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_FluidHeight", health / maxHealth);
        _renderer.SetPropertyBlock(_propBlock);
    }
}