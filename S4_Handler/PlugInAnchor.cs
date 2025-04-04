// Decompiled with JetBrains decompiler
// Type: S4_Handler.PlugInAnchor
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommunicationPort.UserInterface;
using GmmDbLib;
using PlugInLib;
using S4_Handler.Functions;
using S4_Handler.UserInterface;
using StartupLib;
using System.Windows;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler
{
  [ComponentPath("Configuration/Handler")]
  public class PlugInAnchor : GmmPlugInByOwner, IReadoutConfig
  {
    internal const string ShortHandlerDescription = "Handler for Series 4 devices (S4)";
    internal const string LongHandlerDescription = "This handler is for use with IUW BULK and Residential water meter.";
    internal const string ConfigIdentification = "S4Handler";
    internal static string[] UsedRights = new string[3]
    {
      UserManager.Right_ReadOnly + UserManager.Right_ReadOnly_Init,
      UserManager.Role_Developer + UserManager.Role_Developer_Init,
      S4_HandlerFunctions.Right_AllHandlersEnabled + "|False|Like developer for all Handlers"
    };
    private S4_HandlerWindowFunctions MyWindowFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyWindowFunctions = new S4_HandlerWindowFunctions((CommunicationPortWindowFunctions) PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface, DbBasis.PrimaryDB.BaseDbConnection);
      this.MyWindowFunctions.IsPluginObject = true;
    }

    public override void Dispose() => this.MyWindowFunctions.DisposeComponent();

    public void SetReadoutConfiguration(ConfigList configList)
    {
      this.MyWindowFunctions.SetReadoutConfiguration(configList);
    }

    public ConfigList GetReadoutConfiguration() => this.MyWindowFunctions.GetReadoutConfiguration();

    public override string ShowMainWindow() => this.MyWindowFunctions.ShowMainWindow(true);

    public override string ShowMainWindow(Window owner)
    {
      return this.MyWindowFunctions.ShowMainWindow(true, owner);
    }

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("S4_Handler", "NotUsed", "Handler for Series 4 devices (S4)", "This handler is for use with IUW BULK and Residential water meter.", new string[0], PlugInAnchor.UsedRights, (object) this.MyWindowFunctions);
    }

    internal enum ConfigSettingNames
    {
      ComPort,
    }
  }
}
