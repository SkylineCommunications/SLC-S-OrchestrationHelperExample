namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Mvc.DisplayTypes
{
	using Sections;

	public class NumericParameterDisplayInfo : IParameterDisplayInfo
	{
		public string Label { get; set; }

		public double Max { get; set; } = double.MaxValue;

		public double Min { get; set; } = double.MinValue;

		public double Step { get; set; } = 1;

		public string Unit { get; set; }

		public int Decimals { get; set; }

		public ParameterSection CreateParameterSection()
		{
			return new NumericParameterSection(this);
		}
	}
}