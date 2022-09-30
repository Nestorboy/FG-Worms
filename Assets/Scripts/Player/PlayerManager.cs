using UnityEngine;

[DisallowMultipleComponent]
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    private static int _currentPlayerIndex = -1;
    
    public static Player.Player[] Players;
    public static Player.Player ActivePlayer;
    
    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else
            Destroy(gameObject);
    }

    public static void NextPlayer()
    {
        if (++_currentPlayerIndex >= Players.Length)
        {
            _currentPlayerIndex %= Players.Length;
            print("End of round!");
        }
        
        Player.Player nextPlayer = Players[_currentPlayerIndex];
        if (!nextPlayer.IsAlive)
        {
            NextPlayer();
            return;
        }
        
        ActivePlayer?.InputController.Move(new Vector2());
        ActivePlayer = Players[_currentPlayerIndex];
    }
}