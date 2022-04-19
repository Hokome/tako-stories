using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class TogglableMove : MonoBehaviour
	{
		[SerializeField] private Vector2 destination;
		[SerializeField] private float speed;
		[SerializeField] private bool activated;
		public bool Activated
		{
			get => activated;
			set => activated = value;
		}

		private Rigidbody2D rb;
		private Vector2 start;
		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			start = rb.position;
			destination += rb.position;
		}

		private void FixedUpdate()
		{
			if (Activated)
			{
				Vector2 direction = destination - rb.position;
				direction.Normalize();
				rb.velocity = direction * speed;
			}
			else
			{
				Vector2 direction = start - rb.position;
				direction.Normalize();
				rb.velocity = direction * speed;
			}
		}

		private void OnDrawGizmos()
		{
			if (Application.isPlaying) return;
			Debug.DrawLine(transform.position, destination + (Vector2)transform.position, Color.cyan);
		}
	}
}
