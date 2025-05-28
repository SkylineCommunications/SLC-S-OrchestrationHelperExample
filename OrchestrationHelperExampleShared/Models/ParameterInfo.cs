﻿namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Models
{
	using System;
	using Mvc.DisplayTypes;
	using Skyline.DataMiner.Net;

	public class ParameterInfo // todo move to info namespace
	{
		public string Description { get; set; }

		public IParameterDisplayInfo DisplayInfo { get; set; }

		public string Id { get; set; }

		public IDMAObjectRef Reference { get; set; }

		public string Type { get; set; }

		public object Value { get; set; }

		public Type ValueType { get; set; }

		public ParameterGroup Group { get; set; }

		public override string ToString()
		{
			return $"{Id}: {Type} with reference {Reference} ({Description}) / Value: {Value} ({ValueType})";
		}
	}
}