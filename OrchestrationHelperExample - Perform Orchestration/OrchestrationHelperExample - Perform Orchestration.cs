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

27-05-2025	1.0.0.1		PVP, Skyline	Initial version
****************************************************************************
*/

namespace Skyline.DataMiner.Utils.OrchestrationHelperExample.Script.GetOrchestrationInfo
{
	using System;
	using System.Collections.Generic;
	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Automation;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Helpers;
	using Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Models;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(IEngine engine)
		{
			try
			{
				RunSafe(engine);
			}
			catch (ScriptAbortException)
			{
				// Catch normal abort exceptions (engine.ExitFail or engine.ExitSuccess)
				throw; // Comment if it should be treated as a normal exit of the script.
			}
			catch (ScriptForceAbortException)
			{
				// Catch forced abort exceptions, caused via external maintenance messages.
				throw;
			}
			catch (ScriptTimeoutException)
			{
				// Catch timeout exceptions for when a script has been running for too long.
				throw;
			}
			catch (InteractiveUserDetachedException)
			{
				// Catch a user detaching from the interactive script by closing the window.
				// Only applicable for interactive scripts, can be removed for non-interactive scripts.
				throw;
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}

		private void RunSafe(IEngine engine)
		{
			// todo DCP257459 .FindInteractiveClient(
			var valueInfo = OrchestrationHelper.GetValueInfo(new Dictionary<DMAObjectRef, object>
			{
				{ new ProfileParameterID(new Guid("b1377a1d-f886-45bc-8329-90523c6ffe7c")), 12_345d },
				{ new ProfileParameterID(new Guid("51824d5b-ac6f-4cb7-a553-71c385935b16")), "DVB-S2" },
				{ new ProfileParameterID(new Guid("58f1ab17-ed09-40f6-8aa2-2f8e9fd721ae")), "15%" },
				{ new ProfileParameterID(new Guid("8cff7984-2028-4147-ac5c-3acd4a623c2f")), 1d },
			});

			var device = engine.FindElement("OrchestrationHelperExample - Device");
			OrchestrationHelper.Orchestrate(
				engine,
				"OrchestrationHelperExample - Orchestration script example",
				x =>
				{
					x.SelectDummy("device", device);
					x.PerformChecks = false;
				},
				new ScriptInput { ValueInfo = valueInfo },
				true);
		}
	}
}
