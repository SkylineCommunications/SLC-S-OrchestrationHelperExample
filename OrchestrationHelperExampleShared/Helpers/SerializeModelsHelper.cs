namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers
{
	using System;
	using Models;
	using Newtonsoft.Json;

	public static class SerializeModelsHelper // todo non static for context? or move to 
	{
		public static string Serialize(ScriptInfo info) => JsonConvert.SerializeObject(info ?? throw new ArgumentNullException(nameof(info)));

		public static string Serialize(ScriptInput info) => JsonConvert.SerializeObject(info ?? throw new ArgumentNullException(nameof(info)));

		public static string Serialize(ScriptOutput info) => JsonConvert.SerializeObject(info ?? throw new ArgumentNullException(nameof(info)));

		public static ScriptInput DeserializeScriptInput(string serializedInfo) => Deserialize<ScriptInput>(serializedInfo, false);

		public static ScriptOutput DeserializeScriptOutput(string serializedInfo) => Deserialize<ScriptOutput>(serializedInfo, false);

		public static ScriptInfo DeserializeScriptInfo(string serializedInfo) => Deserialize<ScriptInfo>(serializedInfo, false);

		private static T Deserialize<T>(string serializedInfo, bool optional)
		{
			if (string.IsNullOrWhiteSpace(serializedInfo))
			{
				if (optional)
				{
					return default;
				}

				throw new ArgumentException($"Expected info to deserialize {typeof(T)} but none was available", nameof(serializedInfo));
			}

			return JsonConvert.DeserializeObject<T>(serializedInfo);
		}
	}
}
