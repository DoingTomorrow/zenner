// Decompiled with JetBrains decompiler
// Type: M8_Handler.PlugInAnchor
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

using CommunicationPort.UserInterface;
using GmmDbLib;
using M8_Handler.UserInterface;
using PlugInLib;
using StartupLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace M8_Handler
{
  [ComponentPath("Configuration/Handler")]
  public class PlugInAnchor : GmmPlugIn, IReadoutConfig
  {
    internal const string ShortHandlerDescription = "M8 Handler";
    internal const string LongHandlerDescription = "M8 Handler";
    internal const string ConfigIdentification = "M8Handler";
    internal static string[] UsedRights = new string[1]
    {
      "Right\\ReadOnly"
    };
    private M8_HandlerWindowFunctions MyWindowFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyWindowFunctions = new M8_HandlerWindowFunctions((CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface, DbBasis.PrimaryDB.BaseDbConnection);
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
      return new PlugInInfo("M8_Handler", "NotUsed", "M8 Handler", "M8 Handler", new string[0], PlugInAnchor.UsedRights, (object) this.MyWindowFunctions);
    }

    internal enum ConfigSettingNames
    {
      ComPort,
    }
  }
}
