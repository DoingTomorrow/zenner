// Decompiled with JetBrains decompiler
// Type: MinolHandler.PlugInAnchor
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using PlugInLib;
using StartupLib;

#nullable disable
namespace MinolHandler
{
  [ComponentPath("Configuration/Handler")]
  internal class PlugInAnchor : GmmPlugIn
  {
    private MinolHandlerFunctions MyFunctions;
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
      this.MyFunctions = new MinolHandlerFunctions();
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowMinolHandlerWindow();

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("MinolHandler", "Configuration", "Change Minol device settings", "View and change device settings of all Minol devices.", new string[2]
      {
        "AsyncCom",
        "DeviceCollector"
      }, PlugInAnchor.UsedRights, (object) this.MyFunctions);
    }
  }
}
