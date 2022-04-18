using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class GameManager : MonoSingleton<GameManager>
	{
		public static CinemachineBrain CameraBrain { get; private set; }

		[SerializeField] private CinemachineVirtualCamera startCamera;

		protected override void Awake()
		{
			base.Awake();
			CameraBrain = Camera.main.GetComponent<CinemachineBrain>();
		}

		public static void ResetCamera() => SwitchCamera(Inst.startCamera);
		public static void SwitchCamera(CinemachineVirtualCamera newCamera)
		{
			CameraBrain.ActiveVirtualCamera.Priority = 0;
			newCamera.Priority = 1;
		}
	}
}
