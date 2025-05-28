namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers
{
	using Models;
	using Mvc.Dialogs;
	using ParameterProvider;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Automation;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using ParameterInfo = Models.ParameterInfo;

	internal class OrchestrationHelperInfoFactory
	{
		public static readonly string OrchestrationScriptActionRequestScriptInfoKey = nameof(OrchestrationScriptAction);
		public static readonly string OrchestrationScriptInfoRequestScriptInfoKey = "OrchestrationScriptInfo";
		public static readonly string ScriptInputRequestScriptInfoKey = "OrchestrationScriptInput";
		public static readonly string ScriptOutputRequestScriptInfoKey = "OrchestrationScriptOutput";

		private readonly IEngine engine;
		private readonly List<IParameterLinker> linkers;

		public OrchestrationHelperInfoFactory(IEngine engine)
		{
			this.engine = engine ?? throw new ArgumentNullException(nameof(engine));
			this.linkers = new List<IParameterLinker> // todo automatic / reflection?
			{
				new ProfileParameterProvider(engine),
			};
		}

		// todo remove?
		public static ExecuteScriptMessage GetExecuteScriptMessageFromSubScriptOptions(SubScriptOptions subScriptOptions) // todo remove and implement custom SubScriptOptions
		{
			var scriptArgsFieldInfo = typeof(SubScriptOptions).GetField("_scriptArgs", BindingFlags.Instance | BindingFlags.NonPublic);
			var scriptArgs = scriptArgsFieldInfo.GetValue(subScriptOptions) as IEnumerable<string>;

			// locking options
			var flags = ScriptRunFlags.None;

			if (subScriptOptions.LockElements)
				flags |= ScriptRunFlags.Lock;
			if (subScriptOptions.ForceLockElements)
				flags |= ScriptRunFlags.ForceLock;
			if (!subScriptOptions.WaitWhenLocked)
				flags |= ScriptRunFlags.NoWait;

			var options = new List<string>(scriptArgs)
			{
				$"DEFER:{(subScriptOptions.Synchronous ? "FALSE" : "TRUE")}",
				$"CHECKSETS:{(subScriptOptions.PerformChecks ? "TRUE" : "FALSE")}",
				$"SKIP_STARTED_INFO_EVENT:{(subScriptOptions.SkipStartedInfoEvent ? "TRUE" : "FALSE")}",
				$"OPTIONS:{(int)flags}",
			};

			if (subScriptOptions.ExtendedErrorInfo)
			{
				options.Add("EXTENDED_ERROR_INFO");
			}

			return new ExecuteScriptMessage(subScriptOptions.ScriptName)
			{
				Options = new SA(options.ToArray()),
			};
		}

		public List<ParameterInfo> CreateParameterInfos(ScriptInfo scriptInfo, ValueInfo valueInfo)
		{
			var parameterInfos = new List<ParameterInfo>(scriptInfo.ProfileParameters.Count);
			var profileParameterInfos = new Dictionary<ProfileParameterID, ParameterInfo>(scriptInfo.ProfileParameters.Count); // todo remove

			foreach (var profileParameter in scriptInfo.ProfileParameters)
			{
				var reference = new ProfileParameterID(profileParameter.Value);
				var info = new ParameterInfo
				{
					Id = profileParameter.Key,
					Reference = reference,
					Value = valueInfo.ProfileParameterValues.TryGetValue(profileParameter.Value, out var value) ? value : null,
				};

				profileParameterInfos[reference] = info;

				parameterInfos.Add(info);
			}

			linkers.ForEach(x => x.LinkParameters(scriptInfo, parameterInfos)); // todo: pre group per linker?

			return parameterInfos;
		}

		public void HandlePerformOrchestrationEntryPointOutput(RequestScriptInfoSubScriptOptions subscript)
		{
			var result = subscript.Output?.Data;
			if (result is null ||
				!result.TryGetValue(ScriptOutputRequestScriptInfoKey, out var serializedScriptOutput))
			{
				throw new InvalidOperationException($"Performing the orchestration with script '{subscript.ScriptName}' didn't return info");
			}

			var scriptOutput = SerializeModelsHelper.DeserializeScriptOutput(serializedScriptOutput);

			if (scriptOutput.ExceptionString != null)
			{
				throw new InvalidOperationException($"Performing the orchestration with script '{subscript.ScriptName}' failed: {scriptOutput.ExceptionString}");
			}
		}

		public OrchestrationHelperWithInfo GetHelper(List<ParameterInfo> infos) => new OrchestrationHelperWithInfo(infos.ToDictionary(x => x.Id));

		public IEnumerable<ParameterInfo> GetIncompleteInfos(IEnumerable<ParameterInfo> infos) => infos.Where(x => x.Value is null);

		public ScriptInfo GetOrchestrationScriptInfo(OrchestrationScript orchestrationScript)
		{
			if (orchestrationScript is null)
			{
				throw new ArgumentNullException(nameof(orchestrationScript));
			}

			var parameters = orchestrationScript.GetParameters(engine);
			var scriptInfoBuilder = new ScriptInfoBuilder(engine);
			foreach (var parameter in parameters)
			{
				parameter.AddParameter(scriptInfoBuilder);
			}

			return scriptInfoBuilder.Build();
		}

		public void GetValuesFromUser(List<ParameterInfo> infos)
		{
			var controller = new InteractiveController(engine);
			var dialog = new GetOrchestrationValuesDialog(engine, infos);

			dialog.Button.Pressed += (sender, args) =>
			{
				controller.Stop();
				dialog.UpdateValues();
			};

			controller.ShowDialog(dialog);
		}

		public Dictionary<string, string> HandleRequestInfoEntryPoint(OrchestrationScript orchestrationScript, IReadOnlyDictionary<string, string> metaData)
		{
			if (orchestrationScript is null)
			{
				throw new ArgumentNullException(nameof(orchestrationScript));
			}

			string unparsedOrchestrationScriptAction = null;
			if (metaData is null ||
				!metaData.TryGetValue(OrchestrationScriptActionRequestScriptInfoKey, out unparsedOrchestrationScriptAction) ||
				!Enum.TryParse(unparsedOrchestrationScriptAction, out OrchestrationScriptAction orchestrationScriptAction))
			{
				throw new InvalidOperationException($"No orchestration script action was provided (got {unparsedOrchestrationScriptAction}");
			}

			switch (orchestrationScriptAction)
			{
				case OrchestrationScriptAction.OrchestrationScriptInfo:
					{
						var scriptInfo = GetOrchestrationScriptInfo(orchestrationScript);
						var serializedScriptInfo = SerializeModelsHelper.Serialize(scriptInfo);
						return new Dictionary<string, string> { { OrchestrationScriptInfoRequestScriptInfoKey, serializedScriptInfo } };
					}

				case OrchestrationScriptAction.PerformOrchestration:
				case OrchestrationScriptAction.PerformOrchestrationAskMissingValues:
					{
						var scriptOutput = PerformOrchestrationFromEntryPoint(orchestrationScript, metaData, orchestrationScriptAction == OrchestrationScriptAction.PerformOrchestrationAskMissingValues);
						var serializedScriptInfo = SerializeModelsHelper.Serialize(scriptOutput);
						return new Dictionary<string, string> { { ScriptOutputRequestScriptInfoKey, serializedScriptInfo } };
					}

				default:
					throw new NotSupportedException($"No support for orchestration script action {orchestrationScriptAction}");
			}
		}

		public ScriptInfo RequestScriptInfoFromScript(string scriptName)
		{
			var info = GetInputInfo(OrchestrationScriptAction.OrchestrationScriptInfo);


			try
			{
				var subScriptInfo = engine.PrepareSubScript(scriptName, info);
				subScriptInfo.StartScript();
				// todo had error
				//var response = ExecuteScript(msg);

				if (!(subScriptInfo.Output?.Data is IReadOnlyDictionary<string, string> returnedInfo))
				{
					throw new InvalidOperationException("Invalid result received");
				}

				return GetScriptInfo(returnedInfo, $"Script '{OrchestrationScriptInfoRequestScriptInfoKey}'");
			}
			catch (Exception e)
			{
				throw new InvalidOperationException($"Requesting script info from '{scriptName}' failed: {e.Message}", e);
			}
		}

		public RequestScriptInfoInput GetInputInfo(OrchestrationScriptAction action, ScriptInput scriptInput) // todo static
		{
			return GetInputInfo(
				action,
				new Dictionary<string, string>
				{
					{ScriptInputRequestScriptInfoKey, SerializeModelsHelper.Serialize(scriptInput)},
				});
		}

		public static RequestScriptInfoInput GetInputInfo(OrchestrationScriptAction action, IDictionary<string, string> extraMetaData = null)
		{
			var metaData = extraMetaData is null
				? new Dictionary<string, string>(1)
				: new Dictionary<string, string>(extraMetaData);

			metaData[OrchestrationScriptActionRequestScriptInfoKey] = action.ToString();

			return new RequestScriptInfoInput
			{
				Data = metaData
			};
		}

		private static ScriptInfo GetScriptInfo(IReadOnlyDictionary<string, string> resultDictionary, string source) // todo better naming
		{
			if (!resultDictionary.TryGetValue(OrchestrationScriptInfoRequestScriptInfoKey, out var serializedScriptInfo))
			{
				throw new InvalidOperationException($"{source} didn't build the scriptInfo");
			}

			return SerializeModelsHelper.DeserializeScriptInfo(serializedScriptInfo);
		}

		private ExecuteScriptResponseMessage ExecuteScript(ExecuteScriptMessage message)
		{
			var response = engine.SendSLNetSingleResponseMessage(message) as ExecuteScriptResponseMessage;

			if (response is null)
			{
				throw new InvalidOperationException($"No response received when trying to execute '{message.ScriptName}'");
			}

			if (response.HadError || response.ErrorMessages?.Length > 0)
			{
				throw new InvalidOperationException($"An error occured while executing '{message.ScriptName}': {string.Join(Environment.NewLine, response.ErrorMessages ?? Enumerable.Empty<string>())}");
			}

			return response;
		}

		private ScriptOutput PerformOrchestrationFromEntryPoint(OrchestrationScript orchestrationScript, IReadOnlyDictionary<string, string> metaData, bool askMissingValues)
		{
			if (orchestrationScript is null)
			{
				throw new ArgumentNullException(nameof(orchestrationScript));
			}

			var scriptOutput = new ScriptOutput();

			try
			{
				var scriptInfo = GetOrchestrationScriptInfo(orchestrationScript);

				ScriptInput scriptInput = null;
				if (metaData.TryGetValue(ScriptInputRequestScriptInfoKey, out var serializedScriptInputRequestScriptInfo))
				{
					scriptInput = SerializeModelsHelper.DeserializeScriptInput(serializedScriptInputRequestScriptInfo);
				}

				var parameterInfos = CreateParameterInfos(scriptInfo, scriptInput?.ValueInfo ?? new ValueInfo());

				if (askMissingValues)
				{
					var incompleteInfos = GetIncompleteInfos(parameterInfos).ToList();
					if (incompleteInfos.Any())
					{
						GetValuesFromUser(incompleteInfos);
					}
				}

				var helper = GetHelper(parameterInfos);
				orchestrationScript.Orchestrate(engine, helper);
			}
			catch (Exception e)
			{
				scriptOutput.ExceptionString = e.ToString();
			}

			return scriptOutput;
		}
	}
}