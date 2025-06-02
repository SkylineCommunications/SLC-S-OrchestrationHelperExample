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

26-05-2025	1.0.0.1		PVP, Skyline	Initial version
****************************************************************************
*/

using System;
using System.Linq;
using Skyline.AppInstaller;
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Net.AppPackages;
using Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Setup;
using Skyline.DataMiner.Utils.OrchestrationHelperExample.Common.Setup.TestProfilesInfo;
using Skyline.DataMiner.Core.DataMinerSystem.Automation;
using Skyline.DataMiner.Core.DataMinerSystem.Common;
using Skyline.DataMiner.Net.Profiles;

/// <summary>
/// DataMiner Script Class.
/// </summary>
internal class Script
{
	/// <summary>
	/// The script entry point.
	/// </summary>
	/// <param name="engine">Provides access to the Automation engine.</param>
	/// <param name="context">Provides access to the installation context.</param>
	[AutomationEntryPoint(AutomationEntryPointType.Types.InstallAppPackage)]
	public void Install(IEngine engine, AppInstallContext context)
	{
		try
		{
			engine.Timeout = new TimeSpan(0, 10, 0);
			engine.GenerateInformation("Starting installation");
			var installer = new AppInstaller(Engine.SLNetRaw, context);
			installer.InstallDefaultContent();

			PrepareTestDevice(engine);
			CreateProfiles(engine);
		}
		catch (Exception e)
		{
			engine.ExitFail($"Exception encountered during installation: {e}");
		}
	}

	private static void CreateProfiles(IEngine engine)
	{
		Logger.Log("Creating profiles...");
		var profileHelper = new ProfileHelper(engine.SendSLNetMessages);
		profileHelper.ProfileParameters.AddOrUpdateBulk(Parameters.GetAllParameters().ToArray());
		profileHelper.ProfileDefinitions.AddOrUpdateBulk(TestDefinition.Definition);
		profileHelper.ProfileInstances.AddOrUpdateBulk(Instances.GetAllInstances().ToArray());
		Logger.Log("Done creating profiles...");
	}

	private static void PrepareTestDevice(IEngine engine)
	{
		Logger.Log($"Preparing '{TestDeviceInfo.ElementName}'...");
		var testDevice = engine.FindElement(TestDeviceInfo.ElementName);

		if (testDevice is null)
		{
			var dms = engine.GetDms();
			var dma = dms.GetAgent(engine.GetUserConnection().ServerDetails.AgentID);
			var protocol = dms.GetProtocol(TestDeviceInfo.ProtocolName, TestDeviceInfo.ProtocolVersion);
			var elementConfiguration = new ElementConfiguration(dms, TestDeviceInfo.ElementName, protocol);
			_ = dma.CreateElement(elementConfiguration);
		}

		object commandDelimiterRaw = null;
		for (var i = 1; i <= 10 && (testDevice?.IsActive != true || commandDelimiterRaw is null); i++)
		{
			testDevice = engine.FindElement(TestDeviceInfo.ElementName);
			testDevice?.Start();
			Logger.Log($"Attempt {i}: Waiting for the test device '{TestDeviceInfo.ElementName}' to become active...");
			engine.Sleep(3_000);
			if (testDevice?.IsActive == true)
			{
				commandDelimiterRaw = testDevice.GetParameter(TestDeviceInfo.CommandDelimiterParameterName);
			}
		}

		if (testDevice?.IsActive != true)
		{
			engine.ExitFail($"Failed to create or activate the test device '{TestDeviceInfo.ElementName}' after multiple attempts.");
			return;
		}

		Logger.Log($"Test device '{testDevice.ElementName}' ({testDevice.DmaId}/{testDevice.ElementId}) is available and active...");

		var commandDelimiter = commandDelimiterRaw as string
			?? throw new InvalidOperationException($"Command delimiter of '{testDevice.ElementName}' ({testDevice.DmaId}/{testDevice.ElementId}) isn't available. (Raw value: {commandDelimiterRaw})");
		var entryKeys = string.Join(commandDelimiter, TestDeviceInfo.EntryKeys);
		testDevice.SetParameter(TestDeviceInfo.AddEntriesParameterName, entryKeys);
	}
}