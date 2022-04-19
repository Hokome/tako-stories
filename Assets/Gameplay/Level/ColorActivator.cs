using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class ColorActivator : MonoBehaviour
	{
		[SerializeField] private Color activated = Color.white, deactivated = Color.white;
		[Space]
		[SerializeField] private SpriteRenderer spriteRenderer;
		public SpriteRenderer SpriteRenderer
		{
			get
			{
				if (spriteRenderer == null)
					spriteRenderer = GetComponent<SpriteRenderer>();
				return spriteRenderer;
			}
			private set => spriteRenderer = value;
		}

		public bool Activated
		{
			set
			{
				SpriteRenderer.color = value ? activated : deactivated;
			}
		}

		private void Start()
		{
			Activated = false;
		}
	}
}