using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TakoStories
{
	public class WeightButton : Activator
	{
		[SerializeField] private float triggerHeight = 0f;
		[SerializeField] private float force = 40f;

		private Rigidbody2D rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			rb.AddForce(transform.up * force);

			Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
			localVelocity.y = 0;

			rb.velocity = transform.TransformDirection(localVelocity);

			Vector2 p = transform.localPosition;
			p.x = 0;
			transform.localPosition = p;

			if (p.y < triggerHeight)
			{
				Activated = true;
			}
			else if (p.y > triggerHeight)
			{
				Activated = false;
			}

		}
	}
}
