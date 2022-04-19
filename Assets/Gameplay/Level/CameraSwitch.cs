using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	[RequireComponent(typeof(Collider2D))]
	public class CameraSwitch : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera transitionTo;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				if (transitionTo != null)
					GameManager.SwitchCamera(transitionTo);
				else
					GameManager.ResetCamera();
			}
		}
	}
}
