using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public abstract class Tako : MonoBehaviour
	{
		[SerializeField] private Sprite regularSprite;
		[SerializeField] private Sprite powerSprite;

		protected SpriteRenderer sr;

		protected bool activated;
		public virtual bool Activated
		{
			get => activated;
			set
			{
				activated = value;
				sr.sprite = value ? powerSprite : regularSprite;
			}
		}

		protected virtual void Start()
		{
			sr = GetComponentInChildren<SpriteRenderer>();
			Player.Inst.takos.Add(this);
			if (regularSprite == null)
				regularSprite = sr.sprite;
			Activated = false;
		}
	}
}
