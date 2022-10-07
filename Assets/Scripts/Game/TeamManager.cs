using System;
using Player;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
    public static class TeamManager
    {
        public static PlayerCamera PlayerCamera;
        
        private static int _currentTeamIndex = -1;
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

            float hueOffset = Random.Range(0f, 1f);
            Team[] newTeams = new Team[teamCount];
            for (int i = 0; i < teamCount; i++)
            {
                newTeams[i] = new Team();
                newTeams[i].TeamColor = Color.HSVToRGB((hueOffset + (float)i / teamCount) % 1, 1f, 1f);
                
                newTeams[i].OnDefeat += OnTeamDefeated;
                
                //newTeams[i].Inventory
                //for (int j = 0; j < )
                
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
                    Object.Destroy(player.gameObject);
                }
            }

            _currentTeamIndex = -1;
            _teamsLeft = 0;
            Teams = null;
        }

        public static Team NextTeam()
        {
            return NextTeam(_currentTeamIndex);
        }
        
        public static Team NextTeam(int index)
        {
            int i = 1;
            while (i <= Teams.Length)
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

            _currentTeamIndex = -1;
            return null;
        }

        public static void NextTurn()
        {
            int prevTeam = _currentTeamIndex;
            Team team = NextTeam();
            Player.Player player = team.NextPlayer();
            if (_currentTeamIndex < 0)
            {
                Debug.Log($"No team has won!");
            }
            else if ((prevTeam == _currentTeamIndex) || (_teamsLeft == 1))
            {
                Debug.Log($"Team {_currentTeamIndex + 1} has won!");
            }
            else
            {
                PlayerCamera.SetTarget(player);
                return;
            }
            
            InitializeTeams(Random.Range(2, 3), Random.Range(2, 4));
            NextTurn();
            //team = NextTeam();
            //player = team.NextPlayer();
            //PlayerCamera.SetTarget(player);
        }
        
        private static void OnTeamDefeated(Team team)
        {
            _teamsLeft--;
        }
    }
}
