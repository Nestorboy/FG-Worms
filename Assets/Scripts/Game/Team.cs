
using System;
using UnityEngine;
using Visuals;
using Weapons;

namespace Game
{
    [Serializable]
    public class Team
    {
        public Color32 Color = UnityEngine.Color.black;
        public Player.Player[] Players;
        public Weapon[] Inventory;

        public int CurrentPlayerIndex = -1;

        private int _alivePlayerCount;
            
        public int PlayerCount() => Players.Length;

        public float GetTeamHealth()
        {
            float health = 0;
            foreach (Player.Player p in Players)
                health += p.Health;
            return health;
        }

        /// <summary>
        /// Returns the next living player, if none, returns null.
        /// </summary>
        /// <returns></returns>
        public Player.Player NextPlayer()
        {
            return NextPlayer(CurrentPlayerIndex);
        }

        /// <summary>
        /// Returns the next living player after the given player index, if none, returns null.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Player.Player NextPlayer(int index)
        {
            CurrentPlayerIndex = -1;
            Player.Player player = null;
            for (int i = 0; i < Players.Length; ++i)
            {
                int newIndex = index + i % Players.Length;
                Player.Player newPlayer = Players[newIndex];
                if (newPlayer.IsAlive)
                {
                    CurrentPlayerIndex = newIndex;
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
        public void SetPlayer(Player.Player player, int index)
        {
            Players[index] = player;
            if (player.IsAlive)
                _alivePlayerCount++;
            
            Renderer renderer = player.GetComponent<Renderer>();
            MaterialPropertyBlock pb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(pb);
            pb.SetColor(ShaderIDs.Color, Color);
            renderer.SetPropertyBlock(pb);
        }
    }
}
