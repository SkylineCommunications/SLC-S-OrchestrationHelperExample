namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Mvc.DisplayTypes
{
	using Sections;

	public interface IParameterDisplayInfo // todo we go for the section immediately
	{
		string Label { get; }

		ParameterSection CreateParameterSection();
	}
}
