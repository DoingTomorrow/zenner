// Decompiled with JetBrains decompiler
// Type: GMM_Handler.PlugInAnchor
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using PlugInLib;
using StartupLib;

#nullable disable
namespace GMM_Handler
{
  [ComponentPath("Configuration/Handler")]
  internal class PlugInAnchor : GmmPlugIn
  {
    private ZR_HandlerFunctions MyFunctions;
    internal static string[] UsedRights = new string[5]
    {
      "DeviceCollector",
      "DesignerChangeMenu",
      "ProfessionalConfig",
      "Developer",
      "Right\\ReadOnly"
    };

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyFunctions = new ZR_HandlerFunctions();
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowHandlerWindow("");

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("GMM_Handler", "Configuration", "Change serie2 device settings", "View and change device settings.", new string[2]
      {
        "AsyncCom",
        "DeviceCollector"
      }, PlugInAnchor.UsedRights, (object) this.MyFunctions);
    }
  }
}
