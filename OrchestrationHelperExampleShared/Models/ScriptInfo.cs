namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Models
{
	using System;
	using System.Collections.Generic;

	public class ScriptInfo
	{
		public Dictionary<string, Guid> ProfileParameters { get; } = new Dictionary<string, Guid>(); // todo only profile parameters are supported for now; store as serialized IDMAObjectRef
		
		public List<Guid> ProfileDefinitions { get; } = new List<Guid>(); // todo hard linking with profile parameters?; store as serialized IDMAObjectRef
	}
}