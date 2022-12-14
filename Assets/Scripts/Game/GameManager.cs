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

		[SerializeField] public Weapons.Weapon[] Weapons;
		
		private void Awake()
		{
			TeamManager.GameManager = this;
			TeamManager.PlayerCamera = _playerCamera;
		}

		private void Start()
		{
			TeamManager.InitializeTeams(Random.Range(2, 5), Random.Range(2, 5));
			TeamManager.NextTurn();
		}
	
		private void Update()
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.X))
			{
				print($"Pressed: [Next Turn]");
				TeamManager.NextTurn();
			}
		}
	}
}