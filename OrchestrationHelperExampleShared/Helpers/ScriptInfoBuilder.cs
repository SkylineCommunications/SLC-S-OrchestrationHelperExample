namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers
{
	using Models;
	using ParameterProvider;
	using Skyline.DataMiner.Automation;
	using System;

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
			// todo validate if IDs already exists: fail? where?
			// todo how to gather errors: does definition exist? does parameter exist?
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

			// todo validate if ID already exists: fail? where?
			scriptInfo.ProfileParameters[id] = profileParameterId;
			return this;
		}
	}
}