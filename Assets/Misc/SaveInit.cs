using System.IO;
using UnityEngine;

//Originally from AssetFactory
namespace TakoStories.Initialization
{
	public class SaveInit : MonoBehaviour
    {
		private void Start()
		{
			Directory.CreateDirectory($@"{Application.dataPath}/Save");
		}
	}
}
