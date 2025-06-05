namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Mvc.DisplayTypes
{
	using Sections;

	public interface IParameterDisplayInfo
	{
		string Label { get; }

		ParameterSection CreateParameterSection();
	}
}