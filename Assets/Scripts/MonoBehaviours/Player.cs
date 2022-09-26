using System;
using Input;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputController playerController;
    public Transform weaponContainer;

    public float playerMaxHealth = 100f;
    public float playerHealth = 100f;
    
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    
    public void Awake()
    {
        if (!playerController)
            playerController = GetComponent<PlayerInputController>();

        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponentInChildren<Renderer>();
        
        UpdateVisuals();
    }

    public bool IsAlive { get; private set; } = true;

    public void Damage(float points)
    {
        playerHealth -= points;
        
        if (playerHealth < 0f)
            Kill();

        UpdateVisuals();
    }
    
    private void Kill()
    {
        playerHealth = 0f;
        IsAlive = false;
    }

    private void UpdateVisuals()
    {
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat("_FluidHeight", playerHealth / playerMaxHealth);
        _renderer.SetPropertyBlock(_propBlock);
    }
}