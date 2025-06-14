﻿namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Mvc.DisplayTypes
{
	using System.Collections.Generic;
	using Sections;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	internal class DropdownParameterDisplayInfo : IParameterDisplayInfo
	{
		public string Label { get; set; }

		public List<Option<object>> Options { get; set; }

		public ParameterSection CreateParameterSection()
		{
			return new DropdownParameterSection(this);
		}
	}
}