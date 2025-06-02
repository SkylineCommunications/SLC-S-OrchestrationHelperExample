namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Setup.TestProfilesInfo
{
	using System;
	using System.Collections.Generic;
	using Skyline.DataMiner.Net.Profiles;

	public static class Instances
	{
		public static IEnumerable<ProfileInstance> GetAllInstances()
		{
			yield return Sat1Dvbs.Instance;
			yield return Sat1Dvbs2.Instance;
			yield return Sat1Ns3.Instance;
			yield return Sat2Dvbs.Instance;
		}

		public static class Sat1Dvbs
		{
			public static readonly Guid Id = new Guid("e147cec4-db1e-4dd8-b310-f616c3c1a176");
			public static readonly string Name = "OrchestrationHelperExample - Sat 1 DVB-S";

			public static ProfileInstance Instance => new ProfileInstance
			{
				ID = Id,
				IsValueCopy = false,
				AppliesToID = TestDefinition.Id,
				Name = Name,
				Values = new[]
				{
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfModulation.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.String,
							StringValue = "DVB-S",
						},
						CapabilityUsageValue = new CapabilityUsageParameterValue
						{
							RequiredDiscreet = "DVB-S",
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfFrequency.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.Double,
							DoubleValue = 10720.750,
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfRollOff.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.String,
							StringValue = "25%",
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfSymbolRate.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.Double,
							DoubleValue = 13.3,
						},
					},
				},
			};
		}

		public static class Sat1Dvbs2
		{
			public static readonly Guid Id = new Guid("d9a23dda-1dbb-4982-b542-d298fe4aaae9");
			public static readonly string Name = "OrchestrationHelperExample - Sat 1 DVB-S2";

			public static ProfileInstance Instance => new ProfileInstance
			{
				ID = Id,
				IsValueCopy = false,
				AppliesToID = TestDefinition.Id,
				Name = Name,
				Values = new[]
				{
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfModulation.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.String,
							StringValue = "DVB-S2",
						},
						CapabilityUsageValue = new CapabilityUsageParameterValue
						{
							RequiredDiscreet = "DVB-S2",
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfFrequency.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.Double,
							DoubleValue = 10720.750,
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfRollOff.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.String,
							StringValue = "25%",
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfSymbolRate.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.Double,
							DoubleValue = 13.3,
						},
					},
				},
			};
		}

		public static class Sat1Ns3
		{
			public static readonly Guid Id = new Guid("af74e255-f299-4753-94b2-f4c19c49613e");
			public static readonly string Name = "OrchestrationHelperExample - Sat 1 NS3";

			public static ProfileInstance Instance => new ProfileInstance
			{
				ID = Id,
				IsValueCopy = false,
				AppliesToID = TestDefinition.Id,
				Name = Name,
				Values = new[]
				{
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfModulation.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.String,
							StringValue = "NS3",
						},
						CapabilityUsageValue = new CapabilityUsageParameterValue
						{
							RequiredDiscreet = "NS3",
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfFrequency.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.Double,
							DoubleValue = 10720.750,
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfRollOff.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.String,
							StringValue = "25%",
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfSymbolRate.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.Double,
							DoubleValue = 13.3,
						},
					},
				},
			};
		}

		public static class Sat2Dvbs
		{
			public static readonly Guid Id = new Guid("8ba5294a-22ac-4f5f-bca0-7bcfaf836c86");
			public static readonly string Name = "OrchestrationHelperExample - Sat 2 DVB-S";

			public static ProfileInstance Instance => new ProfileInstance
			{
				ID = Id,
				IsValueCopy = false,
				AppliesToID = TestDefinition.Id,
				Name = Name,
				Values = new[]
				{
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfModulation.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.String,
							StringValue = "DVB-S",
						},
						CapabilityUsageValue = new CapabilityUsageParameterValue
						{
							RequiredDiscreet = "DVB-S",
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfFrequency.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.Double,
							DoubleValue = 19720.750,
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfRollOff.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.String,
							StringValue = "5%",
						},
					},
					new ProfileParameterEntry
					{
						Parameter = Parameters.RfSymbolRate.Parameter,
						Value = new ParameterValue
						{
							Type = ParameterValue.ValueType.Double,
							DoubleValue = 30,
						},
					},
				},
			};
		}
	}
}