using System;
using Player;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
    public static class TeamManager
    {
        private static int _currentTeamIndex = -1;
        private static int _currentPlayerIndex = -1;
        private static int _teamsLeft = 0;

        public static Team[] Teams { get; private set; }
        public static int TotalPlayerCount { get; private set; }

        public static Player.Player ActivePlayer
        {
            get
            {
                if ((Teams?.Length > 0) && (_currentTeamIndex > -1))
                {
                    Team activeTeam = Teams[_currentTeamIndex];
                    if ((activeTeam != null) && (activeTeam.Players?.Length > 0) && (activeTeam.CurrentPlayerIndex > -1))
                    {
                        return activeTeam.Players[activeTeam.CurrentPlayerIndex];
                    }
                }
                
                return null;
            }
        }

        public static void InitializeTeams(int teamCount, int playerCount)
        {
            DestroyTeams();
            
            teamCount = Math.Max(teamCount, 2);
            playerCount = Math.Max(playerCount, 1);
			
            Team[] newTeams = new Team[teamCount];
            for (int i = 0; i < teamCount; i++)
            {
                newTeams[i] = new Team();
                newTeams[i].Color = Color.HSVToRGB((float)i / teamCount, 1f, 1f);
					
                newTeams[i].Players = new Player.Player[playerCount];
                for (int j = 0; j < playerCount; j++)
                {
                    Vector3 spawnLocation = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
                    Player.Player newPlayer = PlayerSpawnManager.GetInstance().SpawnPlayer(spawnLocation);
                    newTeams[i].SetPlayer(newPlayer, j);
                }
            }

            TotalPlayerCount = teamCount * playerCount;
            _teamsLeft = newTeams.Length;
            Teams = newTeams;
            
            Debug.Log($"Added: {teamCount} teams with {playerCount} players.");
        }

        public static void DestroyTeams()
        {
            if (Teams == null)
                return;

            foreach (Team team in Teams)
            {
                foreach (Player.Player player in team.Players)
                {
                    Object.Destroy(player);
                }
            }

            _teamsLeft = 0;
            Teams = null;
        }

        public static Team NextTeam()
        {
            return NextTeam(_currentTeamIndex);
        }
        
        public static Team NextTeam(int index)
        {
            _currentTeamIndex = -1;
            int i = 1;
            while (i < _teamsLeft)
            {
                int nextIndex = (index + i) % Teams.Length;
                Team nextTeam = Teams[nextIndex];
                if (!nextTeam.IsDefeated)
                {
                    _currentTeamIndex = nextIndex;
                    return nextTeam;
                }

                i++;
            }

            return null;
        }
    }
}
