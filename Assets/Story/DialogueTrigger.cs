using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class DialogueTrigger : MonoBehaviour
	{
		[SerializeField] private string node;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				DialogueHandler.Inst.Runner.StartDialogue(node);
			}
		}
	}
}
