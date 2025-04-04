// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.PlugInAnchor
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using PlugInLib;
using StartupLib;

#nullable disable
namespace SmokeDetectorHandler
{
  [ComponentPath("Configuration/Handler")]
  internal class PlugInAnchor : GmmPlugIn
  {
    private SmokeDetectorHandlerFunctions MyFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyFunctions = new SmokeDetectorHandlerFunctions();
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowSmokeDetectorWindow();

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("SmokeDetectorHandler", "Configuration", "Change smoke detector settings", "View and change device settings.", new string[2]
      {
        "AsyncCom",
        "DeviceCollector"
      }, new string[3]
      {
        "Developer",
        "SmokeDetectorHandler",
        "Right\\ReadOnly"
      }, (object) this.MyFunctions);
    }
  }
}
