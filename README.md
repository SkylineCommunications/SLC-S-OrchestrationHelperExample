# Technical Documentation for the OrchestrationHelper - Example

This is a custom solution for DataMiner to demo the 'OnRequestScriptInfo' entry point. Details about the entry point are available here: [TODO](https://docs.dataminer.services/entry-points/orchestration-helper/on-request-script-info/).

## Contents

Installing the solution package will deploy the following things:

- Connector: Generic Dynamic Table (1.0.0.4)
- Element: 'OrchestrationHelperExample - Test Device'
- [Scripts](#scripts) part of the example

## Scripts

The scripts part of this example can be used to develop an orchestration helper based on that entry point. Each use case will be explained here.

> [!NOTE]
> The scripts are available in the folder *OrchestrationHelperExample* and are also prefixed with <em>OrchestrationHelperExample - </em>.

### [Common](/OrchestrationHelperExample.Common)

The library script of this example that contains the common logic and gets referenced by each script.

### [Orchestration script example](/OrchestrationHelperExample%20-%20Orchestration%20script%20example/OrchestrationHelperExample%20-%20Orchestration%20script%20example.cs)

This script is the actual orchestration script. The `Script` class implements the [`OrchestrationScript` class](OrchestrationHelperExampleShared/Helpers/OrchestrationScript.cs) so the complexity of implementing the entry point can be obfuscated.

Executing the script will display a basic dialog to request a value for each defined parameter. After the confirming the dialog those values get applied to the dummy that was selected when executing the script.

A high level overview:

```mermaid
sequenceDiagram
    actor U as User
    participant E as Orchestration script example
    participant O as OrchestrationScript (Common)
    U-->>+E: Select 'device' and execute
    E->>+O: 'Run' method gets executed
    O->>E: Get the parameters,<br/>needed for orchestration<br/>(GetParameters)
    O-->>U: Request values
    U-->>O: Confirm entered values
    O->>E: Apply the values to 'device'<br/>(Orchestrate)
```

#### `GetParameters(IEngine engine)`

Basically returns a list of parameters that the orchestration script needs, together with a link to the profile parameter.

Each parameter has an ID that is unique within the script. That parameter is then used in `Orchestrate` to get the value that was gathered.

Remarks:

- The script ID of each parameter isn't checked for its uniqueness. It might be worth adding such check to avoid confusion.
- The script ID of each parameter is matched case sensitive. To ease up the implementation that behavior could be reviewed.
- Ideally the profile parameters are referenced by their name. For a user it is easier to copy/paste these from UI and they'll be easy to understand. Profile parameter, definition and instance names are not required to be unique, even if the UI lets you believe otherwise. It could be that duplicate names exist within the cluster, for instance if these types are imported from mixed function packages. The orchestration solution might set this as requirement of course.
- In the example the `engine` parameter is available if some advanced logic is required, but that parameter could as well be removed.
- The dummy, or any other script parameter or memory file this script has defined, might not be available at the time this method gets executed. This method gets called most likely, when the 'OnRequestScriptInfo' entry point is executed. The script arguments aren't required to be set in that case.

#### `Orchestrate(IEngine engine, OrchestrationHelperWithInfo helper)`

The helper that gets passed allows to get parameter values that were provided or entered.

`helper.GetParameterValue(string id)` will return the value provided or entered for the Parameter with the specified ID.

`helper.GetParameterInfos()` will return the info for all Parameters together with its value.

Remarks:

- Ideally the orchestration get extended with a logging framework, so issues, debug info and timing can be easily checked within the solution that will execute the orchestration scripts. In this example nothing gets logged.
- `.ShowUI(` was added to trick the Automation module in believing this is an automation script that requires interactivity. Task [DCP257459](https://collaboration.dataminer.services/task/257459) will add a feature allowing to clearly enforce that option in the XML of the script. This script should eventually be marked as *Interactivity*/*Always*.
- When using *SetParameter* an automation script will generate an information event with the value that was set. Since generating these is strongly discouraged, it might be worth disabling that by adding the following in the `OrchestrationHelper` or in the template:
`engine.SetFlag(RunTimeFlags.NoInformationEvents)`

#### Link

The name of a script that was suited to orchestrate a specific device can be stored in the Profile Definition (ref. Scripts region in Cube). The Profile Definition could then be found, since it was defined on the Function of a Resource. Because this requires Function Resources, Functions and Profile Definition, and since the Profile Definition is now defined in the script itself, this option has become less viable.

The script name could for instance be defined on the Resource Pool level.

### [Get orchestration Info](/OrchestrationHelperExample%20-%20Get%20Orchestration%20Info/OrchestrationHelperExample%20-%20Get%20Orchestration%20Info.cs)

This example script will gather all the parameter info available in an orchestration script. When executing the script, the name of the orchestration script (i.e. 'OrchestrationHelperExample - Orchestration script example') should be provided as 'Script name'.

Flow:

```mermaid
sequenceDiagram
  box Get Orchestration Info
  participant I as Script
  participant H as OrchestrationHelper<br/>(Common)
  end
  
  I ->> H: RequestScriptInfo of<br>'Orchestration script example'

  box Orchestration script example
  participant O as OrchestrationScript<br/>(Common)
  participant E as Script
  end

  H ->>+ O: 'OnRequestScriptInfo'<br>w/action OrchestrationScriptInfo
  destroy E
  O <<->>+ E: Get the parameters,<br/>needed for orchestration<br/>(GetParameters)
  destroy O
  O ->>+ H: Return ScriptInfo<br/>(serialized)

  create participant P as ProfileParameterProvider<br/>(Common)
  
  H ->> P: Gather profile parameter info 
  destroy P
  P ->> H: Update Parameter info with<br/>type, display info and definition group
  
  H ->> I: Return parameter info with<br/>empty values
```

> [!IMPORTANT]
> This example makes use of GenerateInformation to easily see what parameters are defined in the script. It is strongly discouraged to make

> [!NOTE]
> This example can be used to gather which parameters a device requires. The flow could then be extended to ask a user what values these parameters should have. An example of that flow [is available here](/OrchestrationHelperExampleShared/Helpers/OrchestrationHelperInfoFactory.cs#L281).

### [Perform Orchestration](/OrchestrationHelperExample%20-%20Perform%20Orchestration/OrchestrationHelperExample%20-%20Perform%20Orchestration.cs)

This example script will orchestrate the test device. It has some predefined values declared, but is missing a value for the 'Bit rate' parameter.

Similar to the 'Get orchestration Info' script, this example script will gather all the parameter info available in an orchestration script. It will check if all values are available.

Since the 'askMissingValues' is set to true when executing 'Orchestrate' the value for 'Bit rate' will be requested before the orchestration is started. When 'askMissingValues' is set to false, the value for the missing parameters will be `null`.
Adding the following line the following line to value dictionary will set a value for that parameter. No values will be requested and the orchestration will immediately start.

```csharp
{ new ProfileParameterID(Parameters.BitRate.Id), 3d },
```

Remark: `.FindInteractiveClient(` was added to trick the Automation module in believing this is an automation script that might display interactivity. Task [DCP257459](https://collaboration.dataminer.services/task/257459) will add a feature allowing to clearly enforce that option in the XML of the script. This script should eventually be marked as *Interactivity*/*Optional*.

Flow:

```mermaid
sequenceDiagram
  box Perform Orchestration
  participant PO as Script
  participant H as OrchestrationHelper<br/>(Common)
  end
  
  PO ->> H: Orchestrate 'Test Device' using <br>'Orchestration script example'

  box Orchestration script example
  participant O as OrchestrationScript<br/>(Common)
  participant E as Script
  end

  H ->> O: 'OnRequestScriptInfo'<br>w/action PerformOrchestrationAskMissingValues
  
  O <<->> E: Get the parameters,<br/>needed for orchestration<br/>(GetParameters)

  create participant P as ProfileParameterProvider<br/>(Common)  
  O ->> P: Gather profile parameter info 
  destroy P
  P ->> O: Update Parameter info with<br/>type, display info and definition group

  create actor U as User
  O -->> U: Show dialog requesting<br/>missing parameter value(s)
  destroy U
  U -->> O: Confirms entered values  
  
  O ->> E: Orchestrate dummy<br/>using all available values

  O ->> H: Fail in case the orchestration didn't succeed
```
