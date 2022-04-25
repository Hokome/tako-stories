using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class CameraSwitch : MonoBehaviour
	{
		private CinemachineVirtualCamera cam;
		//private bool activated;
		//Vector2 min;
		//Vector2 max;

		private void Start()
		{
			cam = GetComponent<CinemachineVirtualCamera>();
			//float h = GameManager.Camera.orthographicSize;
			//Vector2 hsize = new Vector2(h * GameManager.Camera.aspect, h);
			//Vector2 pos = transform.position;
			//min = pos - hsize;
			//max = pos + hsize;
		}

		//private void Update()
		//{
		//	if (activated)
		//	{
		//		Vector2 player = Player.Inst.transform.position;
		//	}
		//}

		public void SetCamera(bool v)
		{
			if (v)
				GameManager.SwitchCamera(cam);
			else
				GameManager.ResetCamera();
		}
	}
}
