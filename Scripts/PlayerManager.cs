using UnityEngine;

[DisallowMultipleComponent]
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    private static int _currentPlayerIndex = -1;
    
    public static Player[] Players;
    public static Player ActivePlayer;
    
    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public static void NextPlayer()
    {
        _currentPlayerIndex = ++_currentPlayerIndex % Players.Length;
        
        ActivePlayer?.playerController.Move(new Vector2());
        ActivePlayer = Players[_currentPlayerIndex];
    }
}