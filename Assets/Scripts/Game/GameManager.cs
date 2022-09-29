using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
	[DisallowMultipleComponent]
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private PlayerCamera playerCamera;

		private void Start()
		{
			StartGame(Random.Range(4, 8));
			PlayerManager.NextPlayer();
			playerCamera.UpdateInitialValues();
		}
	
		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.X))
			{
				print($"Pressed: [Next Turn]");
				PlayerManager.NextPlayer();
				playerCamera.UpdateInitialValues();
			}
		}

		private void StartGame(int playerCount)
		{
			Player[] newPlayers = new Player[playerCount];
			for (int i = 0; i < newPlayers.Length; i++)
			{
				Vector3 spawnLocation = new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
				Player newPlayer = PlayerSpawnManager.GetInstance().SpawnPlayer(spawnLocation);
				newPlayers[i] = newPlayer;
			}

			PlayerManager.Players = newPlayers;
		
			print($"Added: {playerCount} players");
		}
	}
}