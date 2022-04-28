using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class Breakable : MonoBehaviour, IKillable
	{
		[SerializeField] private float minVelocity;
		[SerializeField] private LayerMask mask;

		public void Kill()
		{
			Destroy(gameObject);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (mask.CheckLayer(collision.gameObject.layer))
			{
				Vector2 normal = collision.GetContact(0).normal;
				Vector2 projectedVelocity = Vector3.Project(collision.relativeVelocity, normal);
				if (projectedVelocity.sqrMagnitude > minVelocity * minVelocity)
				{
					Kill();
				}
			}
		}
	}
}
