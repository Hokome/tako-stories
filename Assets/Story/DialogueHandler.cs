using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

namespace TakoStories
{
	[RequireComponent(typeof(CanvasGroup))]
	public class DialogueHandler : DialogueViewBase
	{
		[SerializeField] private TMP_Text dialogueField;
		[SerializeField] private TMP_Text nameField;

		public static DialogueHandler Inst { get; private set; }
		public DialogueRunner Runner { get; private set; }
		private CanvasGroup group;
		private CanvasGroup nameGroup;

		private void Awake()
		{
			if (Inst == null)
			{
				Inst = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Debug.LogWarning($"Singleton for {typeof(DialogueHandler)} already exists.");
				Destroy(gameObject);
			}
		}

		private Action lineFinished;

		private void Start()
		{
			Runner = GetComponent<DialogueRunner>();
			group = GetComponent<CanvasGroup>();
			nameGroup = nameField.GetComponentInParent<CanvasGroup>();
			group.Display(false);
			dialogueField.text = "";
			SetCharacter("NONE");
		}

		public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
		{
			if (dialogueLine.CharacterName != null)
				dialogueField.text = dialogueLine.CharacterName;
			dialogueField.text = dialogueLine.Text.Text;
			lineFinished = onDialogueLineFinished;
		}
		public override void DialogueStarted()
		{
			group.Display(true);
			Player.Inst.Frozen = true;
		}
		public override void DialogueComplete()
		{
			group.Display(false);
			Player.Inst.Frozen = false;
		}
		public override void UserRequestedViewAdvancement()
		{
			if (Runner.IsDialogueRunning)
				lineFinished.Invoke();
		}

		#region Commands
		[YarnCommand("setchar")]
		public static void SetCharacter(string name)
		{
			Inst.nameGroup.Display(name != "NONE", false);
			Inst.nameField.text = name;
		}
		#endregion
	}
}
