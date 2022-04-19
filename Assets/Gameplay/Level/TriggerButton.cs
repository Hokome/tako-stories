using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	[RequireComponent(typeof(Collider2D))]
	public class TriggerButton : Activator
	{
		[SerializeField] private LayerMask mask;

		private int triggerCount;
		private int TriggerCount
		{
			get => triggerCount;
			set
			{
				triggerCount = value;
				Activated = triggerCount > 0;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (Utility.IsInLayerMask(collision.gameObject.layer, mask))
			{
				TriggerCount++;
			}
		}
		private void OnTriggerExit2D(Collider2D collision)
		{
			if (Utility.IsInLayerMask(collision.gameObject.layer, mask))
			{
				TriggerCount--;
			}
		}
	}
}
