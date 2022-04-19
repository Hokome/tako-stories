using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TakoStories
{
	[RequireComponent(typeof(Collider2D))]
	public class CursorInteractable : MonoBehaviour
	{
		public UnityEvent onClick;
		public UnityEvent onExit;
		public UnityEvent<bool> onActivate;

	}
}
