using Interfaces;
using UnityEngine;
using Visuals;

namespace Game
{
    public class Team
    {
        public Color32 Color = UnityEngine.Color.black;
        public Player[] Players;
        public IWeapon[] Inventory;

        public int CurrentPlayerIndex = -1;

        private int _alivePlayerCount;
            
        public int PlayerCount() => Players.Length;
        
        public int AlivePlayerCount() => _alivePlayerCount;

        public float GetTeamHealth()
        {
            float health = 0;
            foreach (Player p in Players)
                health += p.health;
            return health;
        }

        /// <summary>
        /// Returns the next living player, if none, returns null.
        /// </summary>
        /// <returns></returns>
        public Player NextPlayer()
        {
            return NextPlayer(CurrentPlayerIndex);
        }

        /// <summary>
        /// Returns the next living player after the given player index, if none, returns null.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Player NextPlayer(int index)
        {
            CurrentPlayerIndex = -1;
            Player player = null;
            for (int i = 0; i < Players.Length; ++i)
            {
                int newIndex = index + i % Players.Length;
                Player newPlayer = Players[newIndex];
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
        public void SetPlayer(Player player, int index)
        {
            Players[index] = player;
            if (player.IsAlive)
                _alivePlayerCount++;
            
            Renderer renderer = player.GetComponent<Renderer>();
            MaterialPropertyBlock pb = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(pb);
            pb.SetColor(ShaderIDs.color, Color);
            renderer.SetPropertyBlock(pb);
        }
    }
}
