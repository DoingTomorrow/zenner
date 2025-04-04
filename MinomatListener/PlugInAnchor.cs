// Decompiled with JetBrains decompiler
// Type: MinomatListener.PlugInAnchor
// Assembly: MinomatListener, Version=1.0.0.1, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: BC91232A-BFD0-4DD3-8B1E-2FFF28E228D6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatListener.dll

using PlugInLib;

#nullable disable
namespace MinomatListener
{
  [ComponentPath("Reading")]
  public class PlugInAnchor : GmmPlugIn
  {
    private MinomatListenerFunctions MyFunctions;
    internal static string[] UsedRights = new string[1]
    {
      "Developer"
    };

    public PlugInAnchor() => this.MyFunctions = new MinomatListenerFunctions();

    public override void Dispose()
    {
    }

    public override string ShowMainWindow() => this.MyFunctions.ShowMainWindow();

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("MinomatListener", "Reading", "Minomat listener", "Minomat listener", new string[0], PlugInAnchor.UsedRights, (object) this.MyFunctions);
    }
  }
}
