namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.ParameterProvider
{
	using Models;
	using Mvc.DisplayTypes;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Profiles;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Parameter = Skyline.DataMiner.Net.Profiles.Parameter;
	using ValueOption = Skyline.DataMiner.Utils.InteractiveAutomationScript.Option<object>;
	using GroupPresetOption = Skyline.DataMiner.Utils.InteractiveAutomationScript.Option<Mvc.DisplayTypes.PresetGroupDisplayInfo.PresetInfo>;

	internal class ProfileParameterProvider : IParameterLinker
	{
		private readonly ProfileHelper profileHelper;

		public ProfileParameterProvider(IEngine engine)
		{
			profileHelper = new ProfileHelper(engine.SendSLNetMessages);
		}

		public void AddProfileDefinition(ScriptInfo scriptInfo, string profileDefinitionName, IReadOnlyCollection<(string id, string profileParameterName)> links)
		{
			var matchingProfileDefinitions = profileHelper.ProfileDefinitions.Read(ProfileDefinitionExposers.Name.Equal(profileDefinitionName));

			if (matchingProfileDefinitions.Count == 1)
			{
				Func<Parameter, string> getId;
				if (links is null || links.Count == 0)
				{
					getId = x => x.Name;
				}
				else
				{
					var linksByName = links.ToDictionary(x => x.profileParameterName, x => x.id);
					getId = x => linksByName.TryGetValue(x.Name, out var id) ? id : null;
				}

				AddProfileDefinition(scriptInfo, matchingProfileDefinitions.First(), getId);
				return;
			}

			if (matchingProfileDefinitions.Count > 1)
			{
				throw new InvalidOperationException($"Multiple profile definitions found with name '{profileDefinitionName}': {string.Join(";", matchingProfileDefinitions.Select(x => $"{x.Name} ({x.ID})"))}");
			}

			throw new InvalidOperationException($"No profile definition found with name '{profileDefinitionName}'");
		}

		public void AddProfileDefinition(ScriptInfo scriptInfo, ProfileDefinition profileDefinition, Func<Parameter, string> getParameterName)
		{
			if (scriptInfo == null)
			{
				throw new ArgumentNullException(nameof(scriptInfo));
			}

			if (profileDefinition == null)
			{
				throw new ArgumentNullException(nameof(profileDefinition));
			}

			if (getParameterName == null)
			{
				throw new ArgumentNullException(nameof(getParameterName));
			}

			foreach (var parameter in profileDefinition.Parameters) // todo safe to use definition.Parameters, or is it inefficient?
			{
				var name = getParameterName(parameter);
				if (name is null)
				{
					continue;
				}

				scriptInfo.ProfileParameters[name] = parameter.ID;
			}

			scriptInfo.ProfileDefinitions.Add(profileDefinition.ID);
		}

		public void LinkParameters(ScriptInfo scriptInfo, List<ParameterInfo> infos)
		{
			var profileParameterInfos = infos
				.Where(x => x.Reference is ProfileParameterID)
				.ToDictionary(x => (x.Reference as ProfileParameterID).Id);

			var profileParameters = Tools.RetrieveBigOrFilter(
				profileParameterInfos.Keys.ToList(),
				x => ParameterExposers.ID.Equal(x),
				profileHelper.ProfileParameters.Read);

			foreach (var parameter in profileParameters)
			{
				if (!profileParameterInfos.TryGetValue(parameter.ID, out var parameterInfo))
				{
					throw new InvalidOperationException($"Parameter {parameter.ID} wasn't requested");
				}

				parameterInfo.Description = parameter.Name;
				parameterInfo.Type = "ProfileParameter";
				switch (parameter.InterpreteType.Type)
				{
					case InterpreteType.TypeEnum.Double:
						parameterInfo.ValueType = typeof(double);
						break;

					case InterpreteType.TypeEnum.String:
						parameterInfo.ValueType = typeof(string);
						break;

					case InterpreteType.TypeEnum.HighNibble: // todo say what?
					case InterpreteType.TypeEnum.Undefined:
					default:
						parameterInfo.ValueType = typeof(object); // todo review
						break;
				}

				switch (parameter.Type)
				{
					case Parameter.ParameterType.Discrete:
						{
							var queue = new Queue<string>(parameter.DiscreetDisplayValues);
							var options = new List<ValueOption>();
							foreach (var discreet in parameter.Discretes)
							{
								if (discreet.GetType() != parameterInfo.ValueType)
								{
									continue; // todo warn or fail
								}

								var display = queue.Dequeue();
								options.Add(new ValueOption(display, discreet));
							}

							parameterInfo.DisplayInfo = new DropdownParameterDisplayInfo
							{
								Label = parameterInfo.Id,
								Options = options,
							};
						}

						break;

					case Parameter.ParameterType.Number:
						{
							parameterInfo.DisplayInfo = new NumericParameterDisplayInfo()
							{
								Label = parameterInfo.Id,
								Min = parameter.RangeMin,
								Max = parameter.RangeMax,
								Step = parameter.Stepsize,
								Decimals = parameter.Decimals,
								Unit = parameter.Units,
							};
						}

						break;

					default:
						throw new NotSupportedException($"Unsupported parameter type {parameter.Type}");
				}
			}

			// todo warn/fail for missing parameters

			AssignProfileDefinitionGroups(scriptInfo.ProfileDefinitions, profileParameterInfos);
		}

		private void AssignProfileDefinitionGroups(List<Guid> profileDefinitionIds, Dictionary<Guid, ParameterInfo> parameters)
		{
			var profileDefinitions = Tools.RetrieveBigOrFilter(
				profileDefinitionIds,
				x => ProfileDefinitionExposers.ID.Equal(x),
				profileHelper.ProfileDefinitions.Read);

			foreach (var definition in profileDefinitions)
			{
				// todo possible improvement to get all instances for all definitions

				var instances = profileHelper.ProfileInstances.Read(ProfileInstanceExposers.AppliesToID.Equal(definition.ID));

				var presets = new List<GroupPresetOption>(instances.Count);
				foreach (var instance in instances)
				{
					var presetInfo = new PresetGroupDisplayInfo.PresetInfo();
					presets.Add(new GroupPresetOption(instance.Name, presetInfo)); // todo name should be unique

					foreach (var value in instance.Values)
					{
						if (!parameters.TryGetValue(value.ParameterID, out var parameter))
						{
							continue;
						}

						switch (value.Value.Type)
						{
							case ParameterValue.ValueType.Double:
								presetInfo.ParameterValues.Add((parameter, value.Value.DoubleValue));
								break;

							case ParameterValue.ValueType.String:
								presetInfo.ParameterValues.Add((parameter, value.Value.StringValue));
								break;

							default:
								throw new NotSupportedException($"No support for type {value.Value.Type} (Parameter ID: {value.ParameterID}; Profile Instance ID: {instance.ID})"); // todo or skip?
						}
					}
				}

				var group = new ParameterGroup
				{
					Description = definition.Name,
					Reference = new ProfileDefinitionID(definition.ID),
					Type = "ProfileDefinition",
					DisplayInfo = new PresetGroupDisplayInfo
					{
						Label = definition.Name,
						Presets = presets,
					},
				};

				foreach (var parameterId in definition.ParameterIDs)
				{
					if (!parameters.TryGetValue(parameterId, out var parameter))
					{
						continue;
					}

					parameter.Group = group;
				}
			}
		}
	}
}