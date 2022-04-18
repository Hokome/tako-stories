using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Originally from AssetFactory
namespace TakoStories.Initialization
{
    public class SettingsInit : MonoBehaviour
    {
		private void Start()
		{
			Settings.current = new Settings();
			Settings.current.SetScreen();
		}
	}
}
