// Decompiled with JetBrains decompiler
// Type: CommunicationPort.PlugInAnchor
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using CommunicationPort.UserInterface;
using PlugInLib;
using StartupLib;
using System.Windows;
using ZENNER.CommonLibrary;

#nullable disable
namespace CommunicationPort
{
  [ComponentPath("Reading")]
  public class PlugInAnchor : GmmPlugInByOwner, IReadoutConfig
  {
    internal static string[] UsedRights = new string[0];
    private CommunicationPortWindowFunctions MyWindowFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyWindowFunctions = new CommunicationPortWindowFunctions();
      this.MyWindowFunctions.portFunctions.IsPluginObject = true;
      this.MyWindowFunctions.IsPluginObject = true;
    }

    public override void Dispose() => this.MyWindowFunctions.DisposeComponent();

    public override string ShowMainWindow() => this.MyWindowFunctions.ShowMainWindow(true);

    public override string ShowMainWindow(Window owner)
    {
      return this.MyWindowFunctions.ShowMainWindow(true, owner);
    }

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("CommunicationPort", "NotUsed", "Central component for byte transfer to devices", "Includes communication by SerialPort and other different byte transfere streams", new string[0], PlugInAnchor.UsedRights, (object) this.MyWindowFunctions);
    }

    public void SetReadoutConfiguration(ConfigList configList)
    {
      this.MyWindowFunctions.SetReadoutConfiguration(configList);
    }

    public ConfigList GetReadoutConfiguration() => this.MyWindowFunctions.GetReadoutConfiguration();
  }
}
