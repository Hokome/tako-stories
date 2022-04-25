using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakoStories
{
	public class SwitchIndicator : ColorActivator
	{
		private LogicGate lg;

		public int ID { get; set; }

		public override bool Activated
		{
			set
			{
				base.Activated = value;
				lg.SetValue(ID, value);
			}
		}
		protected override void Start()
		{
			lg = GetComponentInParent<LogicGate>();
			lg.AddSwitch(this);
			base.Start();
		}
	}
}
