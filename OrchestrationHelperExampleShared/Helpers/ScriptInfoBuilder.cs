namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers
{
	using System;
	using Models;
	using ParameterProvider;
	using Skyline.DataMiner.Automation;

	public class ScriptInfoBuilder
	{
		private readonly IEngine engine;
		private readonly ProfileParameterProvider profileParameterProvider;
		private ScriptInfo scriptInfo = new ScriptInfo();

		public ScriptInfoBuilder(IEngine engine)
		{
			engine = engine ?? throw new ArgumentNullException(nameof(engine));
			profileParameterProvider = new ProfileParameterProvider(engine);
		}

		public ScriptInfo Build()
		{
			var current = scriptInfo;
			scriptInfo = new ScriptInfo();
			return current;
		}

		public ScriptInfoBuilder WithProfileDefinition(string profileDefinitionName, params (string id, string profileParameterName)[] links)
		{
			// Tip: add validation if id already exists
			profileParameterProvider.AddProfileDefinition(scriptInfo, profileDefinitionName, links);
			return this;
		}

		public ScriptInfoBuilder WithProfileParameterId(string id, string profileParameterId)
					=> WithProfileParameterId(id, new Guid(profileParameterId));

		public ScriptInfoBuilder WithProfileParameterId(string id, Guid profileParameterId)
		{
			if (id == null)
			{
				throw new ArgumentNullException(nameof(id));
			}

			// Tip: add validation if id already exists
			scriptInfo.ProfileParameters[id] = profileParameterId;
			return this;
		}
	}
}