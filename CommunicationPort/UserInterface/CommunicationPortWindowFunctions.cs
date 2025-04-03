// Decompiled with JetBrains decompiler
// Type: CommunicationPort.UserInterface.CommunicationPortWindowFunctions
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using CommunicationPort.Functions;
using StartupLib;
using System.Collections.Generic;
using System.Windows;
using ZENNER.CommonLibrary;

#nullable disable
namespace CommunicationPort.UserInterface
{
  public class CommunicationPortWindowFunctions : IWindowFunctions, IReadoutConfig
  {
    internal const string ConfigNameSpace = "CommunicationPort";
    public CommunicationPortFunctions portFunctions;
    internal CommunicationPortWindow MyWindow;
    private bool isPluginObject = false;

    internal bool IsPluginObject
    {
      set => this.isPluginObject = value;
      get => this.isPluginObject;
    }

    public CommunicationPortWindowFunctions()
    {
      this.portFunctions = new CommunicationPortFunctions();
    }

    public object GetFunctions() => (object) this.portFunctions;

    public void Close() => this.portFunctions.Close();

    public string ShowMainWindow() => this.ShowMainWindow(false);

    public string ShowMainWindow(bool isPluginCall, Window owner = null)
    {
      this.MyWindow = new CommunicationPortWindow(this, isPluginCall && this.portFunctions.IsPluginObject);
      if (owner != null)
        this.MyWindow.Owner = owner;
      this.MyWindow.ShowDialog();
      return this.MyWindow.NextPlugin;
    }

    public void SetReadoutConfiguration(SortedList<string, string> configList)
    {
      this.portFunctions.SetReadoutConfiguration(configList);
    }

    public void SetReadoutConfiguration(ConfigList configList)
    {
      this.portFunctions.SetReadoutConfiguration(configList);
    }

    public ConfigList GetReadoutConfiguration() => this.portFunctions.GetReadoutConfiguration();

    public void DisposeComponent() => this.portFunctions.Dispose();

    internal enum ConfigVariables
    {
      MiConBLE_TestPort,
    }
  }
}
