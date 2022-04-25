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
		[SerializeField] private CinemachineVirtualCamera playerCamera;

		[HideInInspector] public List<Resettable> resettables = new List<Resettable>();

		protected override void Awake()
		{
			base.Awake();
			Camera = Camera.main;
			CameraBrain = Camera.GetComponent<CinemachineBrain>();

			player = Instantiate(player);
			playerCamera = Instantiate(playerCamera);
			playerCamera.Follow = player.transform;
		}

		public static void ResetCamera() => SwitchCamera(Inst.playerCamera);
		public static void SwitchCamera(CinemachineVirtualCamera newCamera)
		{
			CameraBrain.ActiveVirtualCamera.Priority = 0;
			newCamera.Priority = 1;
		}
		public void LoadLevel(int level)
		{
			SceneTransitioner.Inst.LoadScene(level);
		}
		public void ResetPuzzle()
		{
			resettables.ForEach(r => r.ResetObject());
		}
	}
}
