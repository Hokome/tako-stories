using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Draggable : MonoBehaviour
	{
		[SerializeField] private float force;
		[SerializeField] private bool disableGravity;

		private bool activated;

		public bool Activated
		{
			get => activated;
			set
			{
				activated = value;
				rb.gravityScale = disableGravity && value ? 0f : 1f;
			}
		}


		private Rigidbody2D rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			if (Activated)
			{
				Vector2 direction = PuzzleCursor.Inst.transform.position;
				direction -= rb.position;
				rb.AddForce(direction.normalized * force);
			}
		}
	}
}
