namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Models
{
	using System;
	using System.Collections.Generic;

	public class ScriptInfo
	{
		public Dictionary<string, Guid> ProfileParameters { get; } = new Dictionary<string, Guid>();

		public List<Guid> ProfileDefinitions { get; } = new List<Guid>();
	}
}