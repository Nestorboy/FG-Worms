using System;
using Input;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputController playerController;
    public Transform playerHand;

    public float playerHealth = 100f;
    
    public void Awake()
    {
        if (!playerController)
            playerController = GetComponent<PlayerInputController>();
    }

    public bool IsAlive { get; private set; } = true;

    public void Damage(float points)
    {
        playerHealth -= points;
        
        if (playerHealth < 0f)
            Kill();
    }
    
    private void Kill()
    {
        playerHealth = 0f;
        IsAlive = false;
    }
}