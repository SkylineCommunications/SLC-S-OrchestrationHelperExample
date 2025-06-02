/*
****************************************************************************
*  Copyright (c) 2025,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

28-05-2025	1.0.0.1		Skyline2\PVP, Skyline	Initial version
****************************************************************************
*/

namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.OrchestrationScript.Example
{
	using System.Collections.Generic;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers;

	/// <summary>
	/// Represents a DataMiner orchestration script.
	/// </summary>
	public class Script : OrchestrationScript
	{
		public override IEnumerable<IParameter> GetParameters(IEngine engine)
		{
			return new IParameter[]
			{
				new ProfileDefinitionName(
					"OrchestrationHelperExample - Test Definition",
					("Frequency", "OrchestrationHelperExample - RF Frequency"),
					("Modulation", "OrchestrationHelperExample - RF Modulation"),
					("Roll Off", "OrchestrationHelperExample - RF Roll Off"),
					("Symbol Rate", "OrchestrationHelperExample - RF Symbol Rate")),
				new ProfileParameterId("Bit rate", "09a66ca2-8c12-4c76-9db5-5a415471871d"),
			};
		}

		public override void Orchestrate(IEngine engine, OrchestrationHelperWithInfo helper)
		{
			// .ShowUI( (DCP257459)
			engine.SetFlag(RunTimeFlags.NoInformationEvents); // Suggest to leave this in the final orchestration script template/example, since it's best to avoid these in production
			engine.SetFlag(RunTimeFlags.NoKeyCaching);

			var device = engine.GetDummy("device");
			var frequency = helper.GetParameterValue("Frequency");
			device.SetParameter("Numeric Value (Entries)", "Frequency", frequency);

			var modulation = helper.GetParameterValue("Modulation");
			device.SetParameter("Text Value (Entries)", "Modulation", modulation);

			var rollOff = helper.GetParameterValue("Roll Off");
			device.SetParameter("Text Value (Entries)", "Roll Off", rollOff);

			var symbolRate = helper.GetParameterValue("Symbol Rate");
			device.SetParameter("Numeric Value (Entries)", "Symbol Rate", symbolRate);

			var bitRate = helper.GetParameterValue("Bit rate");
			device.SetParameter("Numeric Value (Entries)", "Bit rate", bitRate);
		}
	}
}