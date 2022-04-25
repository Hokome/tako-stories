using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	[RequireComponent(typeof(Collider2D))]
	//Could be optimized by storing a list of resettables and only reset them
	public class Checkpoint : MonoBehaviour
	{
		[SerializeField] private Vector2 spawnPoint;
		public Vector2 SpawnPoint => spawnPoint + (Vector2)transform.position;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				Player.Inst.Checkpoint = this;
			}
		}

		private void OnDrawGizmos()
		{
			DebugEx.DrawCross(SpawnPoint, Color.blue);
		}
	}
}
