using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers;
using Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Models;

namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Tests
{
	public class SerializeModelsHelperTests
	{
		[Test]
		public void ScriptInfoTest()
		{
			// Arrange
			var toSerialize = new ScriptInfo();
			toSerialize.ProfileParameters.Add("Frequency", new Guid("b1377a1d-f886-45bc-8329-90523c6ffe7c"));
			toSerialize.ProfileParameters.Add("Modulation", new Guid("51824d5b-ac6f-4cb7-a553-71c385935b16"));

			// Act
			var serializedInfo = SerializeModelsHelper.Serialize(toSerialize);
			Console.WriteLine($"serializedInfo: {serializedInfo}");
			var deserializedInfo = SerializeModelsHelper.DeserializeScriptInfo(serializedInfo);

			// Assert
			AssertScriptInfo(toSerialize, deserializedInfo);
		}

		[Test]
		public void ScriptInfoTestDefinitions()
		{
			// Arrange
			var toSerialize = new ScriptInfo();
			toSerialize.ProfileParameters.Add("Frequency", new Guid("b1377a1d-f886-45bc-8329-90523c6ffe7c"));
			toSerialize.ProfileParameters.Add("Modulation", new Guid("51824d5b-ac6f-4cb7-a553-71c385935b16"));
			toSerialize.ProfileDefinitions.Add(new Guid("15daeaa6-2385-48dd-a9c0-ae7bfc4804ac"));

			// Act
			var serializedInfo = SerializeModelsHelper.Serialize(toSerialize);
			Console.WriteLine($"serializedInfo: {serializedInfo}");
			var deserializedInfo = SerializeModelsHelper.DeserializeScriptInfo(serializedInfo);

			// Assert
			AssertScriptInfo(toSerialize, deserializedInfo);
		}

		[Test]
		public void ScriptInputTest()
		{
			// Arrange
			var toSerialize = new ScriptInput();
			toSerialize.ValueInfo = new ValueInfo();
			toSerialize.ValueInfo.ProfileParameterValues.Add(new Guid("b1377a1d-f886-45bc-8329-90523c6ffe7c"), 15.2d);
			toSerialize.ValueInfo.ProfileParameterValues.Add(new Guid("2bb8bdd6-95ec-45fc-a2a6-cb39b2e0b414"), "TestValue");

			// Act
			var serializedInfo = SerializeModelsHelper.Serialize(toSerialize);
			Console.WriteLine($"serializedInfo: {serializedInfo}");
			var deserializedInfo = SerializeModelsHelper.DeserializeScriptInput(serializedInfo);

			// Assert
			CollectionAssert.AreEqual(toSerialize.ValueInfo.ProfileParameterValues, deserializedInfo.ValueInfo.ProfileParameterValues);
		}

		[Test]
		public void ScriptOutputTest()
		{
			// Arrange
			var toSerialize = new ScriptOutput { ExceptionString = new InvalidOperationException("An error occured").ToString() };

			// Act
			var serializedInfo = SerializeModelsHelper.Serialize(toSerialize);
			Console.WriteLine($"serializedInfo: {serializedInfo}");
			var deserializedInfo = SerializeModelsHelper.DeserializeScriptOutput(serializedInfo);

			// Assert
			ClassicAssert.AreEqual(toSerialize.ExceptionString, deserializedInfo.ExceptionString);
		}

		[Test]
		public void ScriptInputEmpty()
		{
			// Arrange
			var serializedInfo = "{}";

			// Act
			Console.WriteLine($"serializedInfo: {serializedInfo}");
			var deserializedInfo = SerializeModelsHelper.DeserializeScriptInput(serializedInfo);

			// Assert
			Assert.That(deserializedInfo.ValueInfo, Is.Null);
		}

		// Tip: add a test deserializing null

		private static void AssertScriptInfo(ScriptInfo expectedInfo, ScriptInfo actualInfo)
		{
			CollectionAssert.AreEqual(expectedInfo.ProfileParameters, actualInfo.ProfileParameters);
			CollectionAssert.AreEqual(expectedInfo.ProfileDefinitions, actualInfo.ProfileDefinitions);
		}
	}
}