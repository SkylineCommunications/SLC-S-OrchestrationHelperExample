namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Setup.TestProfilesInfo
{
	using System;
	using Skyline.DataMiner.Net.Profiles;

	public static class TestDefinition
	{
		public static readonly Guid Id = new Guid("0f7afc12-bc48-42b6-ab88-6e6175709a84");
		public static readonly string Name = "OrchestrationHelperExample - Test Definition";

		public static ProfileDefinition Definition
		{
			get
			{
				var profileDefinition = new ProfileDefinition
				{
					ID = Id,
					Name = Name,
				};

				profileDefinition.ParameterIDs.Add(Parameters.RfModulation.Id);
				profileDefinition.ParameterIDs.Add(Parameters.RfFrequency.Id);
				profileDefinition.ParameterIDs.Add(Parameters.RfRollOff.Id);
				profileDefinition.ParameterIDs.Add(Parameters.RfSymbolRate.Id);

				return profileDefinition;
			}
		}
	}
}