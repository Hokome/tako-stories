using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TakoStories
{
	public class PuzzleCursor : MonoSingleton<PuzzleCursor>
	{
		[SerializeField] private float radius;
		[SerializeField] private float maxDistance;
		[Space]
		[SerializeField] private Transform targetPosition;
		[Header("Animation")]
		[SerializeField] private float activationScale = 1f;
		[SerializeField] private float standardScale = 1f;
		[SerializeField] private float powerOnScale = 1f;
		[SerializeField] private float activationTime;
		[SerializeField] private float deactivationTime;
		[SerializeField] private float powerOnTime;
		[SerializeField] private float powerOffTime;
		//[Space]
		//[SerializeField] private ParticleSystem powerParticles;

		private LTDescr anim;
		private Rigidbody2D rb;

		private bool dragEnabled;
		public bool DragEnabled
		{
			get => dragEnabled;
			set
			{
				if (dragEnabled == value || Player.Inst.Frozen) return;
				if (value && powerEnabled) return;

				dragEnabled = value;

				if (anim != null)
					LeanTween.cancel(anim.uniqueId);

				if (dragEnabled)
				{
					foreach (var hit in Physics2D.OverlapCircleAll(rb.position, radius))
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
					anim = transform.LeanScale(Vector3.one * standardScale, deactivationTime);
				}

				anim.setEase(LeanTweenType.easeOutCubic);
			}
		}

		private bool powerEnabled;
		public bool PowerEnabled
		{
			get => powerEnabled;
			set
			{
				if (powerEnabled == value || Player.Inst.Frozen) return;
				if (value && dragEnabled) return;

				powerEnabled = value;

				if (anim != null)
					LeanTween.cancel(anim.uniqueId);

				if (value)
				{
					foreach (var hit in Physics2D.OverlapCircleAll(rb.position, radius))
					{
						if (hit.gameObject.TryGetComponent(out Tako t))
						{
							t.Activated = true;
							poweredTako = t;
						}
					}
					anim = transform.LeanScale(Vector3.one * powerOnScale, powerOnTime);
				}
				else
				{
					if (poweredTako != null)
						poweredTako.Activated = false;
					anim = transform.LeanScale(Vector3.one * standardScale, powerOffTime);
				}

				anim.setEase(LeanTweenType.easeOutCubic);
			}
		}

		private Tako poweredTako;

		private List<CursorInteractable> interactables = new List<CursorInteractable>();

		private void Start()
		{
			transform.localScale = Vector3.one * standardScale;
			rb = GetComponent<Rigidbody2D>();
			targetPosition = Instantiate(targetPosition);
		}

		private void Update()
		{
			Vector2 dir = (Vector2)targetPosition.position - rb.position;
			targetPosition.gameObject.SetActive(dir.sqrMagnitude > maxDistance * maxDistance);
			if (Player.Inst.Frozen)
			{
				DragEnabled = false;
				PowerEnabled = false;
			}
		}
		private void FixedUpdate()
		{
			if (PauseMenu.IsPaused) return;
			Vector2 pos = GameManager.Camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

			Vector2 dir = (pos - rb.position) / Time.fixedDeltaTime;
			rb.velocity = dir;
			targetPosition.position = pos;
		}

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		static extern bool SetCursorPos(int X, int Y);
		public void ResetPosition()
		{
			//GameManager.CameraBrain.ActiveVirtualCamera.VirtualCameraGameObject.transform.position = Player.Inst.transform.position;
			Vector2 pos = GameManager.Camera.WorldToScreenPoint(Player.Inst.transform.position);
			int x = Mathf.RoundToInt(pos.x);
			int y = Mathf.RoundToInt(pos.y);
#if UNITY_EDITOR
			Type t = Type.GetType("UnityEditor.GameView,UnityEditor");
			EditorWindow gw = EditorWindow.GetWindow(t);
			Rect r = gw.position;
			x += Mathf.RoundToInt(r.x + (r.width - Screen.width) / 2);
			y += Mathf.RoundToInt(r.y + (r.height - Screen.height)/ 2);
#endif
			SetCursorPos(x, y);

			rb.Sleep();
			transform.position = Player.Inst.transform.position;
			rb.WakeUp();
		}

	}
}
