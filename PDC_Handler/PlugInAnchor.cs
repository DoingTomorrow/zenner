// Decompiled with JetBrains decompiler
// Type: PDC_Handler.PlugInAnchor
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using PlugInLib;
using StartupLib;

#nullable disable
namespace PDC_Handler
{
  [ComponentPath("Configuration/Handler")]
  internal class PlugInAnchor : GmmPlugIn
  {
    private PDC_HandlerFunctions MyFunctions;
    internal static string[] UsedRights = new string[2]
    {
      "Developer",
      "Right\\ReadOnly"
    };

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyFunctions = new PDC_HandlerFunctions();
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowHandlerWindow();

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("PDC_Handler", "Configuration", "Change PDC device settings", "View and change device settings.", new string[2]
      {
        "AsyncCom",
        "DeviceCollector"
      }, PlugInAnchor.UsedRights, (object) this.MyFunctions);
    }
  }
}
