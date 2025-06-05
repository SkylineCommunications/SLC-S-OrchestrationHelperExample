namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Mvc.DisplayTypes
{
	using Sections;

	public interface IParameterGroupDisplayInfo
	{
		string Label { get; }

		ParameterGroupSection CreateParameterGroupSection();
	}
}