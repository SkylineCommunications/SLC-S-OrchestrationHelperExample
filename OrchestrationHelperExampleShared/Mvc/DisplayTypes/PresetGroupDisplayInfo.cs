namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Mvc.DisplayTypes
{
	using System.Collections.Generic;
	using Models;
	using Sections;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	internal class PresetGroupDisplayInfo : IParameterGroupDisplayInfo
	{
		public string Label { get; set; }

		public List<Option<PresetInfo>> Presets { get; set; }

		public ParameterGroupSection CreateParameterGroupSection()
		{
			return new PresetGroupSection(this);
		}

		internal class PresetInfo
		{
			public List<(ParameterInfo info, object value)> ParameterValues { get; } = new List<(ParameterInfo info, object value)>();
		}
	}
}