using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class PuzzleCursor : MonoSingleton<PuzzleCursor>
	{
		[SerializeField] private float radius;
		[Space]
		[SerializeField] private float activationScale = 1f;
		[SerializeField] private float deactivationScale = 1f;
		[SerializeField] private float activationTime;
		[SerializeField] private float deactivationTime;

		private LTDescr anim;

		private bool activated;
		public bool Activated
		{
			get => activated;
			set
			{
				if (activated == value) return;
				activated = value;

				if (anim != null)
					LeanTween.cancel(anim.uniqueId);

				if (activated)
				{
					foreach (var hit in Physics2D.OverlapCircleAll(transform.position, radius))
					{
						if (hit.gameObject.TryGetComponent(out CursorInteractable ci))
						{
							ci.onClick.Invoke();
							ci.onActivate.Invoke(true);
							interactables.Add(ci);
						}
					}
					anim = transform.LeanScale(Vector3.one * activationScale, activationTime);
				}
				else
				{
					foreach (var ci in interactables)
					{
						ci.onExit.Invoke();
						ci.onActivate.Invoke(false);
					}
					interactables.Clear();
					anim = transform.LeanScale(Vector3.one * deactivationScale, deactivationTime);
				}

				anim.setEase(LeanTweenType.easeOutCubic);
			}
		}

		private List<CursorInteractable> interactables = new List<CursorInteractable>();

		private void Start()
		{
			transform.localScale = Vector3.one * deactivationScale;
		}
	}
}
