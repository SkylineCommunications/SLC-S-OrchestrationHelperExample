namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers
{
	using System;
	using System.Collections.Generic;
	using Models;

	public class OrchestrationHelperWithInfo
	{
		private readonly Dictionary<string, ParameterInfo> values;

		public OrchestrationHelperWithInfo(Dictionary<string, ParameterInfo> values)
		{
			this.values = values ?? throw new ArgumentNullException(nameof(values));
		}

		public IEnumerable<ParameterInfo> GetParameterInfos() => values.Values;

		public object GetParameterValue(string id)
		{
			if (!values.TryGetValue(id, out var parameterInfo))
			{
				throw new InvalidOperationException($"Parameter with ID '{id}' is missing");
			}

			return parameterInfo.Value ?? throw new InvalidOperationException($"There's no value for parameter '{id}'");
		}
	}
}