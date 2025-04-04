// Decompiled with JetBrains decompiler
// Type: TH_Handler.PlugInAnchor
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using PlugInLib;
using StartupLib;

#nullable disable
namespace TH_Handler
{
  [ComponentPath("Configuration/Handler")]
  public class PlugInAnchor : GmmPlugIn
  {
    private TH_HandlerWindowFunctions window;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.window = new TH_HandlerWindowFunctions(new TH_HandlerFunctions());
    }

    public override string ShowMainWindow()
    {
      return this.window != null ? this.window.ShowMainWindow() : "Back";
    }

    public override void Dispose()
    {
      if (this.window == null)
        return;
      this.window.Handler.Dispose();
    }

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("TH_Handler", "NotUsed", "Handler for temperature and humidity sensor (Minocomfort).", "Handler", new string[0], new string[2]
      {
        "Demo",
        "Right\\ReadOnly"
      }, (object) this.window);
    }
  }
}
