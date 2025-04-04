// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.PlugInAnchor
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using CommunicationPort.UserInterface;
using EDCL_Handler.UserInterface;
using GmmDbLib;
using PlugInLib;
using StartupLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace EDCL_Handler
{
  [ComponentPath("Configuration/Handler")]
  public class PlugInAnchor : GmmPlugIn, IReadoutConfig
  {
    internal const string ShortHandlerDescription = "EDCL Handler";
    internal const string LongHandlerDescription = "EDCL Handler";
    internal const string ConfigIdentification = "EDCL_Handler";
    internal static string[] UsedRights = new string[1]
    {
      "Right\\ReadOnly"
    };
    private EDCL_HandlerWindowFunctions MyWindowFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyWindowFunctions = new EDCL_HandlerWindowFunctions((CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface, DbBasis.PrimaryDB.BaseDbConnection);
      this.MyWindowFunctions.IsPluginObject = true;
    }

    public override void Dispose() => this.MyWindowFunctions.DisposeComponent();

    public override string ShowMainWindow() => this.MyWindowFunctions.ShowMainWindow(true);

    public void SetReadoutConfiguration(ConfigList configList)
    {
      this.MyWindowFunctions.MyFunctions.SetReadoutConfiguration(configList);
    }

    public ConfigList GetReadoutConfiguration()
    {
      return this.MyWindowFunctions.MyFunctions.GetReadoutConfiguration();
    }

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("EDCL_Handler", "NotUsed", "EDCL Handler", "EDCL Handler", new string[0], PlugInAnchor.UsedRights, (object) this.MyWindowFunctions);
    }

    internal enum ConfigSettingNames
    {
      ComPort,
    }
  }
}
