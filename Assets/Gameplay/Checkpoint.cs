using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	[RequireComponent(typeof(Collider2D))]
	//Could be optimized by storing a list of resettables and only reset them
	public class Checkpoint : MonoBehaviour
	{
		[SerializeField] private Vector2 offset;
		[SerializeField] private LayerMask rayMask;

		private Vector2 Center => (Vector2)transform.position + offset;
		public Vector2 SpawnPoint => Physics2D.Raycast(Center, Vector2.down, 100f, rayMask).point;
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				Player.Inst.Checkpoint = this;
			}
		}

		private void OnDrawGizmos()
		{
			Vector2 point = SpawnPoint;
			Debug.DrawLine(Center, point, Color.blue);
		}
	}
}
