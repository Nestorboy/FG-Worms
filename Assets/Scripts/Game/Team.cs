
using System;
using UnityEngine;
using Visuals;
using Weapons;

namespace Game
{
    [Serializable]
    public class Team
    {
        public Color32 TeamColor = Color.black;
        public Player.Player[] Players;
        public Weapon[] Inventory;

        public int CurrentPlayerIndex = -1;

        private int _alivePlayerCount;
        public bool IsDefeated { get; private set; }

        public Action<Team> OnDefeat;
        
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
                {
                    IsDefeated = true;
                    OnDefeat?.Invoke(this);
                }

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
            int i = 1;
            while (i <= Players.Length)
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

            CurrentPlayerIndex = -1;
            return null;
        }

        /// <summary>
        /// Assigns a player to an index in the team, also changes their appearance to fit.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="index"></param>
        public void SetPlayer(Player.Player player, int index)
        {
            player.OnDeath += OnPlayerDeath;
            
            Players[index] = player;
            if (player.IsAlive)
                AlivePlayerCount++;
            
            Renderer renderer = player.GetComponentInChildren<Renderer>();
            MaterialPropertyBlock pb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(pb);
            Color.RGBToHSV(TeamColor, out float h, out float _, out float _);
            pb.SetFloat(ShaderIDs.FluidHueOffset, h);
            renderer.SetPropertyBlock(pb);
        }

        public Weapon GetWeapon(int index)
        {
            if (index >= 0 && index < Inventory.Length)
                return Inventory[index];
            
            return null;
        }
        
        private void OnPlayerDeath(Player.Player player)
        {
            AlivePlayerCount--;
            
            if (player == TeamManager.ActivePlayer)
                TeamManager.NextTurn();
        }
    }
}
