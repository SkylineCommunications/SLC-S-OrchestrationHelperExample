namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Models
{
	using Mvc.DisplayTypes;
	using Skyline.DataMiner.Net;

	public class ParameterGroup
	{
		public string Description { get; set; }

		public IParameterGroupDisplayInfo DisplayInfo { get; set; }

		public IDMAObjectRef Reference { get; set; }

		public string Type { get; set; }

		public override string ToString()
		{
			return $"{Type} with reference {Reference} ({Description})";
		}
	}
}