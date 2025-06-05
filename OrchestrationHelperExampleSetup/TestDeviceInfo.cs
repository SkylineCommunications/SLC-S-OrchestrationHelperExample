namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Setup
{
	using System.Collections.Generic;

	public static class TestDeviceInfo
	{
		public static readonly string ProtocolName = "Generic Dynamic Table";
		public static readonly string ProtocolVersion = "1.0.0.4";
		public static readonly string ElementName = "OrchestrationHelperExample - Test Device";

		public static readonly string CommandDelimiterParameterName = "Automation Command Delimiter";
		public static readonly string AddEntriesParameterName = "Automation Add Entries";
		public static readonly IEnumerable<string> EntryKeys = new[]
		{
			"Frequency",
			"Modulation",
			"Roll Off",
			"Symbol Rate",
			"Bit rate",
		};
	}
}