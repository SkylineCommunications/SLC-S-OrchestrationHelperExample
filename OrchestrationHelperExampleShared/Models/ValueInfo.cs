namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Models
{
	using System;
	using System.Collections.Generic;

	public class ValueInfo
	{
		public Dictionary<Guid, object> ProfileParameterValues { get; } = new Dictionary<Guid, object>(); // todo only profile parameters are supported for now; create a dynamic parameter type
	}
}
