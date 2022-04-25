using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(TargetJoint2D))]
	public class Draggable : MonoBehaviour
	{
		//[SerializeField] private float force;
		//[SerializeField] private float normalDrag;
		//[SerializeField] private float activeDrag;
		[SerializeField] private bool disableGravity = true;

		private bool activated;

		public bool Activated
		{
			get => activated;
			set
			{
				activated = value;
				tj.enabled = value;
				rb.gravityScale = disableGravity && value ? 0f : 1f;
				//rb.drag = value ? activeDrag : normalDrag;
			}
		}
		private TargetJoint2D tj;
		private Rigidbody2D rb;

		private void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			tj = GetComponent<TargetJoint2D>();
			Activated = false;
		}

		private void FixedUpdate()
		{
			if (Activated)
			{
				Vector2 target = PuzzleCursor.Inst.transform.position;
				tj.target = target;
				//Vector2 direction = (Vector2)PuzzleCursor.Inst.transform.position - rb.position;
				//rb.AddForce(direction.normalized * force);
			}
		}
		public void ResetVelocity()
		{
			Activated = false;
			rb.velocity = Vector2.zero;
		}
	}
}
