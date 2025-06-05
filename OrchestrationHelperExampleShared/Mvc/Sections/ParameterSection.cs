﻿namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Mvc.Sections
{
	using System;
	using DisplayTypes;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public abstract class ParameterSection : Section
	{
		public Label Label { get; }

		protected ParameterSection(IParameterDisplayInfo displayInfo)
		{
			if (displayInfo == null)
			{
				throw new ArgumentNullException(nameof(displayInfo));
			}

			// Init widgets
			Label = new Label(displayInfo.Label);
		}

		public void InitializeSection()
		{
			DefineLayout();
		}

		protected virtual void DefineLayout()
		{
			AddWidget(Label, 0, 0);
		}

		public abstract void SetValue(object value);

		public abstract object GetValue();
	}
}