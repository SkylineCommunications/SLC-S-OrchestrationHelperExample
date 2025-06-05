namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers
{
	using System;
	using Models;
	using Newtonsoft.Json;
	using Skyline.DataMiner.Utils.SecureCoding.SecureSerialization.Json.Newtonsoft;

	public static class SerializeModelsHelper
	{
		private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
		{
			TypeNameHandling = TypeNameHandling.None,
			Formatting = Formatting.None,
			NullValueHandling = NullValueHandling.Ignore,
		};

		public static string Serialize(ScriptInfo info) => InnerSerialize(info ?? throw new ArgumentNullException(nameof(info)));

		public static string Serialize(ScriptInput info) => InnerSerialize(info ?? throw new ArgumentNullException(nameof(info)));

		public static string Serialize(ScriptOutput info) => InnerSerialize(info ?? throw new ArgumentNullException(nameof(info)));

		public static ScriptInput DeserializeScriptInput(string serializedInfo) => InnerDeserialize<ScriptInput>(serializedInfo, false);

		public static ScriptOutput DeserializeScriptOutput(string serializedInfo) => InnerDeserialize<ScriptOutput>(serializedInfo, false);

		public static ScriptInfo DeserializeScriptInfo(string serializedInfo) => InnerDeserialize<ScriptInfo>(serializedInfo, false);

		private static string InnerSerialize(object data)
		{
			if (data is null)
			{
				return null;
			}

			return JsonConvert.SerializeObject(data, SerializerSettings);
		}

		private static T InnerDeserialize<T>(string serializedInfo, bool optional)
		{
			if (string.IsNullOrWhiteSpace(serializedInfo))
			{
				if (optional)
				{
					return default;
				}

				throw new ArgumentException($"Expected info to deserialize {typeof(T)} but none was available", nameof(serializedInfo));
			}

			return SecureNewtonsoftDeserialization.DeserializeObject<T>(serializedInfo);
		}
	}
}