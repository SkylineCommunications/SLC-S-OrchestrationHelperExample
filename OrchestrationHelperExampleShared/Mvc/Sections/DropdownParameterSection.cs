﻿namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Mvc.Sections
{
	using System;
	using System.Linq;
	using DisplayTypes;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	internal class DropdownParameterSection : ParameterSection
	{
		public DropDown<object> Value { get; }

		public DropdownParameterSection(DropdownParameterDisplayInfo info) : base(info)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			// Init widgets
			Value = new DropDown<object>(info.Options);
		}

		protected override void DefineLayout()
		{
			base.DefineLayout();
			AddWidget(Value, 0, 1);
		}

		public override void SetValue(object value)
		{
			Value.SelectedOption = Value.Options.SingleOrDefault(x => Equals(value, x.Value));
		}

		public override object GetValue()
		{
			return Value.Selected;
		}
	}
}