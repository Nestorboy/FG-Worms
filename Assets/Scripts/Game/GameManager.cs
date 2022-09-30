using System;
using System.Collections;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
	[DisallowMultipleComponent]
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private PlayerCamera _playerCamera;
		public Team[] Teams;
		public Player.Player Player;

		private void Start()
		{
			InitializeGame(Random.Range(2, 3), Random.Range(2, 4));
			PlayerManager.NextPlayer();
			_playerCamera.UpdateInitialValues();
		}
	
		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.X))
			{
				print($"Pressed: [Next Turn]");
				PlayerManager.NextPlayer();
				_playerCamera.UpdateInitialValues();
			}
		}

		private void InitializeGame(int teamCount, int playerCount)
		{
			/*
			teamCount = Math.Max(teamCount, 2);
			playerCount = Math.Max(playerCount, 1);
			
			Team[] newTeams = new Team[teamCount];
			for (int i = 0; i < teamCount; i++)
			{
				newTeams[i] = new Team();
				newTeams[i].Color = Color.HSVToRGB((float)i / teamCount, 1f, 1f);
					
				newTeams[i].Players = new Player[playerCount];
				for (int j = 0; j < playerCount; j++)
				{
					Vector3 spawnLocation = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
					Player newPlayer = PlayerSpawnManager.GetInstance().SpawnPlayer(spawnLocation);
					newTeams[i].SetPlayer(newPlayer, j);
				}
			}*/
			
			Player.Player[] newPlayers = new Player.Player[playerCount];
			for (int i = 0; i < newPlayers.Length; i++)
			{
				Vector3 spawnLocation = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
				Player.Player newPlayer = PlayerSpawnManager.GetInstance().SpawnPlayer(spawnLocation);
				newPlayers[i] = newPlayer;
			}

			PlayerManager.Players = newPlayers;
		
			print($"Added: {teamCount} teams with {playerCount} players.");
		}
	}
}