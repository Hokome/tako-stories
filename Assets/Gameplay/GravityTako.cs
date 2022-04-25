using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class GravityTako : Tako
	{
		[SerializeField] private float radius;
		[SerializeField] private float force;
		[SerializeField] private LayerMask mask;

		private Rigidbody2D rb;
		public Rigidbody2D Rb
		{
			get
			{
				if (rb == null)
					rb = GetComponent<Rigidbody2D>();
				return rb;
			}
			private set => rb = value;
		}

		private void FixedUpdate()
		{
			if (!Activated) return;

			Collider2D[] cs = Physics2D.OverlapCircleAll(Rb.position, radius, mask);
			foreach (var c in cs)
			{
				if (c.gameObject == gameObject) continue;
				Rigidbody2D r = c.attachedRigidbody;
				Debug.Log(r.name);
				Vector2 direction = Rb.position - r.position;
				r.AddForce(direction.normalized * force);
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(transform.position, radius);
		}
	}
}
