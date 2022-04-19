using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
    public class SpriteFlicker : MonoBehaviour
    {
		[SerializeField] private float duration = 2f;
		[SerializeField] private float flickerFrequency = 0.1f;
		[SerializeField] private SpriteRenderer sprite;

		private Coroutine routine;
		public void StartFlicker() => StartFlicker(duration);
		public void StartFlicker(float duration)
		{
			if (routine != null)
				StopCoroutine(routine);
			routine = StartCoroutine(Flicker(duration));
		}
		private IEnumerator Flicker(float duration)
		{
			float flickerStart = Time.time;
			while (Time.time - flickerStart < duration)
			{
				sprite.enabled = !sprite.enabled;
				yield return new WaitForSeconds(flickerFrequency);
			}
			sprite.enabled = true;
		}
	}
}
