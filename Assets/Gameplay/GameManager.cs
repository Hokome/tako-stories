using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class GameManager : MonoSingleton<GameManager>
	{
		public static CinemachineBrain CameraBrain { get; private set; }
		public static Camera Camera { get; private set; }

		[SerializeField] private Player player;
		[SerializeField] private LevelScreen levelScreen;
		[SerializeField] private Checkpoint firstCheckpoint;

		//[HideInInspector] public List<Resettable> resettables = new List<Resettable>();

		protected override void Awake()
		{
			base.Awake();
			Camera = Camera.main;
			CameraBrain = Camera.GetComponent<CinemachineBrain>();

			player = Instantiate(player);
			player.Checkpoint = firstCheckpoint;
			player.transform.position = firstCheckpoint.SpawnPoint;
			SwitchScreen(levelScreen);
			//playerCamera = Instantiate(playerCamera);
			//playerCamera.Follow = player.transform;
		}
		public void SwitchScreen(LevelScreen newScreen)
		{
			if (levelScreen != null)
			{
				levelScreen.Cam.Priority = 0;
				//levelScreen.gameObject.SetActive(false);
			}
			levelScreen = newScreen;
			//newScreen.gameObject.SetActive(true);
			newScreen.Cam.Priority = 1;
		}
		public void LoadLevel(int level)
		{
			SceneTransitioner.Inst.LoadScene(level);
		}
		//public void ResetPuzzle()
		//{
		//	resettables.ForEach(r => r.ResetObject());
		//}
	}
}
