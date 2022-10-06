
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
        public bool IsDefeated;

        public int AlivePlayerCount
        {
            get => _alivePlayerCount;
            set
            {
                if (IsDefeated)
                {
                    if (value > 0)
                        IsDefeated = false;
                }
                else if (value <= 0)
                    IsDefeated = false;

                _alivePlayerCount = value;
            }
        }
        
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
            int i = 1;
            while (i < AlivePlayerCount)
            {
                int nextIndex = (index + i) % Players.Length;
                Player.Player nextPlayer = Players[nextIndex];
                if (nextPlayer.IsAlive)
                {
                    CurrentPlayerIndex = nextIndex;
                    return nextPlayer;
                }

                i++;
            }

            return null;
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
                AlivePlayerCount++;
            
            Renderer renderer = player.GetComponentInChildren<Renderer>();
            MaterialPropertyBlock pb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(pb);
            pb.SetColor(ShaderIDs.Color, Color);
            renderer.SetPropertyBlock(pb);
        }
    }
}
