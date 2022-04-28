using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	[RequireComponent(typeof(Collider2D))]
	public class LevelScreen : MonoBehaviour
	{
		private CinemachineVirtualCamera cam;
		public CinemachineVirtualCamera Cam
		{
			get
			{
				if (cam == null)
					cam = GetComponentInChildren<CinemachineVirtualCamera>();
				return cam;
			}
			private set => cam = value;
		}

		public void ResetScreen()
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).TryGetComponent(out IResettable r))
				{
					r.OnReset();
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
				GameManager.Inst.SwitchScreen(this);
		}
	}
}
