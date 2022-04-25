using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TakoStories
{
	public class Resettable : MonoBehaviour
	{
		[SerializeField] private UnityEvent onReset;

		public Vector2 InitialPosition { get; set; }

		private void Start()
		{
			GameManager.Inst.resettables.Add(this);
			InitialPosition = transform.position;
		}
		private void OnDestroy()
		{
			if (GameManager.Exists)
				GameManager.Inst.resettables.Remove(this);
		}

		public void ResetObject()
		{
			onReset.Invoke();
		}
		public void ResetPosition()
		{
			transform.position = InitialPosition;
		}
	}

	//public interface IResettable
	//{
	//	void ResetObject();
	//}
}
