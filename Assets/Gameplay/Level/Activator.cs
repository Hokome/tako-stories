using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TakoStories
{
	public class Activator : MonoBehaviour
	{
		[SerializeField] protected UnityEvent onActivate;
		[SerializeField] protected UnityEvent onDeactivate;
		[SerializeField] protected UnityEvent<bool> onActivateDynamic;

		private bool activated;
		public virtual bool Activated
		{
			get => activated;
			set
			{
				if (activated == value) return;

				activated = value;

				if (value)
					onActivate.Invoke();
				else
					onDeactivate.Invoke();

				onActivateDynamic.Invoke(value);
			}
		}
	}
}
