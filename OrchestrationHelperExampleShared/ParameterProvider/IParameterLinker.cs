namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.ParameterProvider
{
	using System.Collections.Generic;
	using Models;

	internal interface IParameterLinker
	{
		void LinkParameters(ScriptInfo scriptInfo, List<ParameterInfo> infos);
	}
}