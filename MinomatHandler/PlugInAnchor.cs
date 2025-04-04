// Decompiled with JetBrains decompiler
// Type: MinomatHandler.PlugInAnchor
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using PlugInLib;
using StartupLib;

#nullable disable
namespace MinomatHandler
{
  [ComponentPath("Configuration/Handler")]
  internal class PlugInAnchor : GmmPlugIn
  {
    private MinomatHandlerFunctions MyFunctions;
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
      this.MyFunctions = new MinomatHandlerFunctions();
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowMinomatHandlerWindow();

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("MinomatHandler", "Configuration", "Change Minomat settings", "View and change device settings Minomat settings.", new string[2]
      {
        "AsyncCom",
        "DeviceCollector"
      }, PlugInAnchor.UsedRights, (object) this.MyFunctions);
    }
  }
}
