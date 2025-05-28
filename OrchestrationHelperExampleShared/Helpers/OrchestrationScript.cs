namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers
{
	using System;
	using System.Collections.Generic;
	using Models;
	using System.Linq;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Automation;

	public abstract class OrchestrationScript
	{
		public interface IParameter
		{
			void AddParameter(ScriptInfoBuilder builder);
		}

		public abstract IEnumerable<IParameter> GetParameters(IEngine engine);

		public abstract void Orchestrate(IEngine engine, OrchestrationHelperWithInfo helper);

		[AutomationEntryPoint(AutomationEntryPointType.Types.OnRequestScriptInfo)]
		public RequestScriptInfoOutput OnRequestScriptInfoRequest(IEngine engine, RequestScriptInfoInput inputData)
		{
			var helper = new OrchestrationHelperInfoFactory(engine);
			return new RequestScriptInfoOutput
			{
				Data = helper.HandleRequestInfoEntryPoint(this, inputData.Data),
			};
		}

		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{
				RunSafe(engine);
			}
			catch (ScriptAbortException)
			{
				// Catch normal abort exceptions (engine.ExitFail or engine.ExitSuccess)
				throw; // Comment if it should be treated as a normal exit of the script.
			}
			catch (ScriptForceAbortException)
			{
				// Catch forced abort exceptions, caused via external maintenance messages.
				throw;
			}
			catch (ScriptTimeoutException)
			{
				// Catch timeout exceptions for when a script has been running for too long.
				throw;
			}
			catch (InteractiveUserDetachedException)
			{
				// Catch a user detaching from the interactive script by closing the window.
				// Only applicable for interactive scripts, can be removed for non-interactive scripts.
				throw;
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}

		private void RunSafe(IEngine engine)
		{
			var factory = new OrchestrationHelperInfoFactory(engine);

			var scriptInfo = factory.GetOrchestrationScriptInfo(this);

			var parameterInfos = factory.CreateParameterInfos(scriptInfo, new ValueInfo());

			if (factory.GetIncompleteInfos(parameterInfos).Any())
			{
				factory.GetValuesFromUser(parameterInfos);
			}

			var helper = factory.GetHelper(parameterInfos);

			Orchestrate(engine, helper);
		}

		public class ProfileDefinitionName : IParameter
		{
			public ProfileDefinitionName(string profileDefinitionName, params (string id, string profileParameterName)[] links)
			{
				DefinitionName = profileDefinitionName;
				Links = links;
			}

			public string DefinitionName { get; }

			public (string id, string profileParameterName)[] Links { get; }

			public void AddParameter(ScriptInfoBuilder builder)
			{
				builder.WithProfileDefinition(DefinitionName, Links);
			}
		}

		public class ProfileParameterId : IParameter
		{
			public ProfileParameterId(string parameterId, string profileParameterId) : this(parameterId, new Guid(profileParameterId))
			{
			}

			public ProfileParameterId(string parameterId, Guid profileParameterId)
			{
				Id = parameterId;
				ParameterId = profileParameterId;
			}

			public string Id { get; }

			public Guid ParameterId { get; }

			public void AddParameter(ScriptInfoBuilder builder)
			{
				builder.WithProfileParameterId(Id, ParameterId);
			}
		}
	}
}