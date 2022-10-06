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
			TeamManager.InitializeTeams(Random.Range(2, 3), Random.Range(2, 4));
			Team team = TeamManager.NextTeam();
			Player.Player player = team.NextPlayer(); 
			
			//PlayerManager.NextPlayer();
			_playerCamera.SetTarget(player);
		}
	
		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.X))
			{
				print($"Pressed: [Next Turn]");
				//PlayerManager.NextPlayer();
				TeamManager.ActivePlayer.InputController.Move(Vector2.zero);
				Team team = TeamManager.NextTeam();
				Player.Player player = team.NextPlayer();
				_playerCamera.SetTarget(player);
			}
		}
	}
}