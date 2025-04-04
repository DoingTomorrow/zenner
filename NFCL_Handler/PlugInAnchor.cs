// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.PlugInAnchor
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using CommunicationPort.UserInterface;
using GmmDbLib;
using PlugInLib;
using StartupLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace NFCL_Handler
{
  [ComponentPath("Configuration/Handler")]
  public class PlugInAnchor : GmmPlugIn, IReadoutConfig
  {
    internal const string ShortHandlerDescription = "NFCL Handler";
    internal const string LongHandlerDescription = "NFCL Handler";
    internal const string ConfigIdentification = "NFCLHandler";
    internal static string[] UsedRights = new string[1]
    {
      "Right\\ReadOnly"
    };
    private NFCL_HandlerWindowFunctions MyWindowFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyWindowFunctions = new NFCL_HandlerWindowFunctions((CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface, DbBasis.PrimaryDB.BaseDbConnection);
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
      return new PlugInInfo("NFCL_Handler", "NotUsed", "NFCL Handler", "NFCL Handler", new string[0], PlugInAnchor.UsedRights, (object) this.MyWindowFunctions);
    }

    internal enum ConfigSettingNames
    {
      ComPort,
    }
  }
}
