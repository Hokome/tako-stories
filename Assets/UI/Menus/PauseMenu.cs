using TakoStories.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

//Originally from AssetFactory
namespace TakoStories
{
    public class PauseMenu : MenuManager
	{
		public static PauseMenu Inst { get; private set; }

		/// <summary>
		/// Will return true if the time is stopped
		/// </summary>
		public static bool IsPaused => Inst.TimeStopped;

		[Delayed]
		[SerializeField] private float timeScale = 1f;

		private bool inPauseMenu;
		private bool timeStopped;

		public float TimeScale
		{
			get => timeScale;
			set
			{
				timeScale = Mathf.Max(0f, value);
				TimeStopped = TimeStopped;
			}
		}
		public bool Paused
		{
			get => inPauseMenu;
			set
			{
				inPauseMenu = value;
				if (value)
				{
					ToMain();
				}
				else
				{
					if (currentMenu != null)
						currentMenu.Display(false);
				}
				Cursor.visible = value;
				TimeStopped = value;
			}
		}
		public bool TimeStopped
		{
			get => timeStopped;
			set
			{
				timeStopped = value;
				Time.timeScale = value ? 0f : timeScale;
			}
		}


		public void TogglePauseMenu()
		{
			if (!isActiveAndEnabled)
				return;
			//Unpause only if in main screen
			if (Paused && navigationStack.Count > 0)
				return;

			Paused = !Paused;
		}
		public override void Back()
		{
			//Delay to avoid unpausing instead of backing out
			StartCoroutine(BackCoroutine());
		}
		IEnumerator BackCoroutine()
		{
			yield return null;
			base.Back();
		}
		private void Awake()
		{
			if (Inst == null)
			{
				Inst = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Debug.LogWarning($"Singleton for {typeof(PauseMenu)} already exists.");
				Destroy(gameObject);
			}
		}
		private void OnValidate()
		{
			TimeScale = timeScale;
		}

		//protected override void OnEnable()
		//{
		//	pauseAction.performed += _ => TogglePause();
		//	base.OnEnable();
		//}
		//protected override void OnDisable()
		//{
		//	if (CurrentMenu != null)
		//		CurrentMenu.Display(false);
		//	pauseAction.performed -= _ => TogglePause();
		//	base.OnDisable();
		//}
	}
}
