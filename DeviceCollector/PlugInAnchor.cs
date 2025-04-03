// Decompiled with JetBrains decompiler
// Type: DeviceCollector.PlugInAnchor
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using PlugInLib;
using StartupLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace DeviceCollector
{
  [ComponentPath("Reading")]
  public class PlugInAnchor : GmmPlugIn, IReadoutConfig
  {
    private DeviceCollectorFunctions MyFunctions;
    internal static string[] UsedRights = new string[5]
    {
      "DeviceCollector",
      "Designer",
      "Configurator",
      "Developer",
      "ZennerDefaultKey"
    };

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyFunctions = new DeviceCollectorFunctions();
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowBusWindow("");

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("DeviceCollector", "Communication", "Übertragungsprotokolle", "Datentelegramme, je nach angeschlossenem Geräten.", new string[1]
      {
        "AsyncCom"
      }, PlugInAnchor.UsedRights, (object) this.MyFunctions);
    }

    public void SetReadoutConfiguration(ConfigList configList)
    {
      this.MyFunctions.SetReadoutConfiguration(configList);
    }

    public ConfigList GetReadoutConfiguration() => this.MyFunctions.GetReadoutConfiguration();
  }
}
