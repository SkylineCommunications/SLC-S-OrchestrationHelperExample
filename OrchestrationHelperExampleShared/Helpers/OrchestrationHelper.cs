namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers
{
	using Models;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Automation;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class OrchestrationHelper
	{
		public static ValueInfo GetValueInfo(IEnumerable<KeyValuePair<DMAObjectRef, object>> values)
		{
			var valueInfo = new ValueInfo(); // todo use DMAObjectRef

			foreach (var kvp in values)
			{
				if (kvp.Key is ProfileParameterID profileParameterId)
				{
					valueInfo.ProfileParameterValues[profileParameterId.Id] = kvp.Value;
				}

				// todo add other types
			}

			return valueInfo;
		}

		/// <summary>
		/// Executes the orchestration using the specified.
		/// </summary>
		/// <param name="engine">The engine used to execute the script.</param>
		/// <param name="scriptName">The name of the orchestration script to use.</param>
		/// <param name="prepareSubscript">An action to configure the subscript options, like script parameters and element dummies, before execution.</param>
		/// <param name="scriptInput">The input parameters required for the script execution.</param>
		/// <exception cref="ArgumentNullException">
		/// Thrown when <paramref name="engine"/>, <paramref name="scriptName"/>, or <paramref name="scriptInput"/> is null.
		/// </exception>
		public static void Orchestrate(IEngine engine, string scriptName, Action<SubScriptOptions> prepareSubscript, ScriptInput scriptInput, bool askMissingValues)
		{
			if (engine == null)
			{
				throw new ArgumentNullException(nameof(engine));
			}

			if (scriptName == null)
			{
				throw new ArgumentNullException(nameof(scriptName));
			}

			if (scriptInput == null)
			{
				throw new ArgumentNullException(nameof(scriptInput));
			}

			var factory = new OrchestrationHelperInfoFactory(engine);
			var action = askMissingValues ?
				OrchestrationScriptAction.PerformOrchestrationAskMissingValues :
				OrchestrationScriptAction.PerformOrchestration;
			var subscript = engine.PrepareSubScript(scriptName, factory.GetInputInfo(action, scriptInput));
			prepareSubscript?.Invoke(subscript);
			subscript.Synchronous = true;

			// todo error handling
			subscript.StartScript();

			factory.HandlePerformOrchestrationEntryPointOutput(subscript);
		}

		public static ScriptInfoBuilder GetScriptInfo(IEngine engine) // todo return IScriptInfoGatherer instead
		{
			return new ScriptInfoBuilder(engine);
		}

		public static List<ParameterInfo> RequestScriptInfo(IEngine engine, string scriptName)
		{
			if (scriptName == null)
			{
				throw new ArgumentNullException(nameof(scriptName));
			}

			var factory = new OrchestrationHelperInfoFactory(engine);
			var scriptInfo = factory.RequestScriptInfoFromScript(scriptName);

			return factory.CreateParameterInfos(scriptInfo, new ValueInfo());
		}
	}
}