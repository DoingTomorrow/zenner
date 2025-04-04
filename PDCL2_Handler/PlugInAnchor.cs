// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.PlugInAnchor
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using CommunicationPort.UserInterface;
using GmmDbLib;
using PDCL2_Handler.UserInterface;
using PlugInLib;
using StartupLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace PDCL2_Handler
{
  [ComponentPath("Configuration/Handler")]
  public class PlugInAnchor : GmmPlugIn, IReadoutConfig
  {
    internal const string ShortHandlerDescription = "PDCL2 Handler";
    internal const string LongHandlerDescription = "PDCL2 Handler";
    internal const string ConfigIdentification = "PDCL2_Handler";
    internal static string[] UsedRights = new string[1]
    {
      "Right\\ReadOnly"
    };
    private PDCL2_HandlerWindowFunctions MyWindowFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyWindowFunctions = new PDCL2_HandlerWindowFunctions((CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface, DbBasis.PrimaryDB.BaseDbConnection);
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
      return new PlugInInfo("PDCL2_Handler", "NotUsed", "PDCL2 Handler", "PDCL2 Handler", new string[0], PlugInAnchor.UsedRights, (object) this.MyWindowFunctions);
    }

    internal enum ConfigSettingNames
    {
      ComPort,
    }
  }
}
