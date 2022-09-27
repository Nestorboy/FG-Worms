using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Visuals;

public class Team
{
    public Color32 color = Color.black;
    public Player[] players;
    public IWeapon[] inventory;

    public int currentPlayerIndex = -1;
    
    public int PlayerCount() => players.Length;

    public float GetTeamHealth()
    {
        float health = 0;
        foreach (Player p in players)
            health += p.playerHealth;
        return health;
    }

    /// <summary>
    /// Returns the next living player, if none, returns null.
    /// </summary>
    /// <returns></returns>
    public Player NextPlayer()
    {
        return NextPlayer(currentPlayerIndex);
    }

    /// <summary>
    /// Returns the next living player after the given player index, if none, returns null.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Player NextPlayer(int index)
    {
        currentPlayerIndex = -1;
        Player player = null;
        for (int i = 0; i < players.Length; ++i)
        {
            int newIndex = index + i % players.Length;
            Player newPlayer = players[newIndex];
            if (newPlayer.IsAlive)
            {
                currentPlayerIndex = newIndex;
                player = newPlayer;
                break;
            }
        }

        return player;
    }

    /// <summary>
    /// Assigns a player to an index in the team, also changes their appearance to fit.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="index"></param>
    public void SetPlayer(Player player, int index)
    {
        players[index] = player;
        Renderer renderer = player.GetComponent<Renderer>();
        MaterialPropertyBlock pb = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(pb);
        pb.SetColor(ShaderIDs.color, color);
        renderer.SetPropertyBlock(pb);
    }
}
