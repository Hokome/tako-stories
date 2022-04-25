using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TakoStories
{
	public class LogicGate : Activator
	{
		private List<bool> values = new List<bool>();

		public void SetValue(int id, bool value)
		{
			values[id] = value;
			Activated = values.All(v => v);
		}
		public void AddSwitch(SwitchIndicator indicator)
		{
			indicator.ID = values.Count;
			values.Add(indicator);
		}
	}
}
