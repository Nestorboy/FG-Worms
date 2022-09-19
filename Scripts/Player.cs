using System;
using Input;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputController playerController;

    public float playerHealth = 100f;
    
    public void Awake()
    {
        if (!playerController)
            playerController = GetComponent<PlayerInputController>();
    }

    public bool IsAlive { get; private set; } = true;

    private void Kill()
    {
        IsAlive = false;
    }
}