// Decompiled with JetBrains decompiler
// Type: EDC_Handler.PlugInAnchor
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using PlugInLib;
using StartupLib;

#nullable disable
namespace EDC_Handler
{
  [ComponentPath("Configuration/Handler")]
  internal class PlugInAnchor : GmmPlugIn
  {
    private EDC_HandlerFunctions MyFunctions;
    internal static string[] UsedRights = new string[6]
    {
      "Developer",
      "EDC_Handler",
      "EDC_Handler.View.Expert",
      "EDC_Handler.View.TestCommandos",
      "EDC_Handler.View.FirmwareUpdate",
      "Right\\ReadOnly"
    };

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyFunctions = new EDC_HandlerFunctions();
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowHandlerWindow();

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("EDC_Handler", "Configuration", "Change EDC device settings", "View and change device settings.", new string[2]
      {
        "AsyncCom",
        "DeviceCollector"
      }, PlugInAnchor.UsedRights, (object) this.MyFunctions);
    }
  }
}
