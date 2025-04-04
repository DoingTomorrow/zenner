// Decompiled with JetBrains decompiler
// Type: THL_Handler.PlugInAnchor
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using CommunicationPort.UserInterface;
using GmmDbLib;
using PlugInLib;
using StartupLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace THL_Handler
{
  [ComponentPath("Configuration/Handler")]
  public class PlugInAnchor : GmmPlugIn, IReadoutConfig
  {
    internal const string ShortHandlerDescription = "THL Handler";
    internal const string LongHandlerDescription = "THL Handler";
    internal const string ConfigIdentification = "THLHandler";
    internal static string[] UsedRights = new string[1]
    {
      "Right\\ReadOnly"
    };
    private THL_HandlerWindowFunctions MyWindowFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyWindowFunctions = new THL_HandlerWindowFunctions((CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface, DbBasis.PrimaryDB.BaseDbConnection);
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
      return new PlugInInfo("THL_Handler", "NotUsed", "THL Handler", "THL Handler", new string[0], PlugInAnchor.UsedRights, (object) this.MyWindowFunctions);
    }

    internal enum ConfigSettingNames
    {
      ComPort,
    }
  }
}
