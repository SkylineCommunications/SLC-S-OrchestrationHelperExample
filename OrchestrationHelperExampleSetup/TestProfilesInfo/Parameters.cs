namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Setup.TestProfilesInfo
{
	using System;
	using System.Collections.Generic;
	using Skyline.DataMiner.Net.Profiles;

	public static class Parameters
	{
		public static IEnumerable<Parameter> GetAllParameters()
		{
			yield return RfModulation.Parameter;
			yield return RfFrequency.Parameter;
			yield return RfRollOff.Parameter;
			yield return RfSymbolRate.Parameter;
			yield return BitRate.Parameter;
		}

		public static class BitRate
		{
			public static readonly Guid Id = new Guid("09a66ca2-8c12-4c76-9db5-5a415471871d");
			public static readonly string Name = "OrchestrationHelperExample - Bit rate";

			public static Parameter Parameter => new Parameter
			{
				ID = Id,
				Type = Parameter.ParameterType.Number,
				Categories = ProfileParameterCategory.Configuration,
				Name = Name,
				InterpreteType = new InterpreteType
				{
					RawType = InterpreteType.RawTypeEnum.NumericText,
					Type = InterpreteType.TypeEnum.Double,
				},
				DefaultValue = new ParameterValue
				{
					Type = ParameterValue.ValueType.Double,
				},
				Units = "Mbps",
				RangeMin = 0.0,
				RangeMax = 100_000.0,
				Stepsize = 0.001,
				Decimals = 3,
			};
		}

		public static class RfModulation
		{
			public static readonly Guid Id = new Guid("c0047a1d-1305-4da6-a161-247053294edb");
			public static readonly string Name = "OrchestrationHelperExample - RF Modulation";

			public static Parameter Parameter => new Parameter
			{
				ID = Id,
				Type = Parameter.ParameterType.Discrete,
				Categories = ProfileParameterCategory.Capability | ProfileParameterCategory.Configuration,
				Name = Name,
				DefaultValue = new ParameterValue
				{
					Type = ParameterValue.ValueType.String,
					StringValue = "DVB-S2",
				},
				Discretes = new List<string> { "DVB-S", "DVB-S2", "NS3" },
				InterpreteType = new InterpreteType
				{
					RawType = InterpreteType.RawTypeEnum.Other,
					Type = InterpreteType.TypeEnum.String,
				},
				DiscreetDisplayValues = new List<string> { "DVB-S", "DVB-S2", "NS3" },
			};
		}

		public static class RfFrequency
		{
			public static readonly Guid Id = new Guid("edd0f644-5de3-418c-aa0a-0f0be60c304c");

			public static readonly string Name = "OrchestrationHelperExample - RF Frequency";

			public static Parameter Parameter => new Parameter
			{
				ID = Id,
				Type = Parameter.ParameterType.Number,
				Categories = ProfileParameterCategory.Capacity | ProfileParameterCategory.Configuration,
				Name = Name,
				InterpreteType = new InterpreteType
				{
					RawType = InterpreteType.RawTypeEnum.NumericText,
					Type = InterpreteType.TypeEnum.Double,
				},
				DefaultValue = new ParameterValue
				{
					Type = ParameterValue.ValueType.Double,
				},
				Units = "MHz",
				RangeMin = 4000.0,
				RangeMax = 18000.0,
				Stepsize = 0.001,
				Decimals = 3,
			};
		}

		public static class RfRollOff
		{
			public static readonly Guid Id = new Guid("4453763b-6d93-46bd-94c2-6e01242b8ba1");

			public static readonly string Name = "OrchestrationHelperExample - RF Roll Off";

			public static Parameter Parameter => new Parameter
			{
				ID = Id,
				Type = Parameter.ParameterType.Discrete,
				Categories = ProfileParameterCategory.Capability | ProfileParameterCategory.Configuration,
				Name = Name,
				InterpreteType = new InterpreteType
				{
					RawType = InterpreteType.RawTypeEnum.Other,
					Type = InterpreteType.TypeEnum.String,
				},
				DefaultValue = new ParameterValue
				{
					Type = ParameterValue.ValueType.String,
					StringValue = "25%",
				},
				Discretes = new List<string> { "5%", "10%", "15%", "20%", "25%", "35%" },
				DiscreetDisplayValues = new List<string> { "5%", "10%", "15%", "20%", "25%", "35%" },
				RangeMin = 1.0,
				RangeMax = 45.0,
				Stepsize = 0.001,
				Decimals = 3,
			};
		}

		public static class RfSymbolRate
		{
			public static readonly Guid Id = new Guid("23542bd6-95be-40ef-b1dd-fc6f0666a512");

			public static readonly string Name = "OrchestrationHelperExample - RF Symbol Rate";

			public static Parameter Parameter => new Parameter
			{
				ID = Id,
				Type = Parameter.ParameterType.Number,
				Categories = ProfileParameterCategory.Capacity | ProfileParameterCategory.Configuration,
				Name = Name,
				InterpreteType = new InterpreteType
				{
					RawType = InterpreteType.RawTypeEnum.NumericText,
					Type = InterpreteType.TypeEnum.Double,
				},
				DefaultValue = new ParameterValue
				{
					Type = ParameterValue.ValueType.Double,
				},
				Units = "MSymps",
				RangeMin = 1.0,
				RangeMax = 45.0,
				Stepsize = 0.001,
				Decimals = 3,
			};
		}
	}
}