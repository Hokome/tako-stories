using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TakoStories
{
	public class PlayerCursor : MonoBehaviour
	{
		[SerializeField] private float radius;
		[SerializeField] private float force;
		//[SerializeField] private float maxDragSetupTime;

		private SpriteRenderer sr;
		
		private Vector2 dragBegin;
		private Vector2 direction;
		
		private List<Rigidbody2D> affectedObjects = new();

		private bool enableDrag;
		public bool EnableDrag
		{
			get => enableDrag;
			set
			{
				if (enableDrag == value) return;
				enableDrag = value;
				if (value)
				{
					dragBegin = WorldPosition;
					sr.enabled = true;
					sr.size = Vector2.up * 0.5f;
					affectedObjects.Clear();
					Collider2D[] cs = Physics2D.OverlapCircleAll(transform.position, radius);
					affectedObjects.Capacity = cs.Length;
					foreach (var c in cs)
					{
						if (c.CompareTag("Draggable"))
							affectedObjects.Add(c.attachedRigidbody);
					}
				}
				else
				{
					direction = (Vector2)transform.position - dragBegin;
					sr.enabled = false;
					foreach (var r in affectedObjects)
					{
						if (r == null) continue;
						r.velocity = Vector2.zero;
						r.AddForce(direction.normalized * force, ForceMode2D.Impulse);
					}
				}
			}
		}

		private void Start()
		{
			sr = GetComponent<SpriteRenderer>();
			sr.enabled = false;
		}

		private void Update()
		{
			transform.position = WorldPosition;
			if (EnableDrag)
			{
				direction = (Vector2)transform.position - dragBegin;
				transform.right = direction;
				sr.size = new Vector2(direction.magnitude, 0.5f);
			}
		}

		public Vector2 WorldPosition => GameManager.Camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
	}
}
