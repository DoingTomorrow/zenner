// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.PlugInAnchor
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using PlugInLib;
using StartupLib;
using System.Collections.Generic;
using System.Windows;
using ZR_ClassLibrary;

#nullable disable
namespace ReadoutConfiguration
{
  [ComponentPath("Reading")]
  public class PlugInAnchor : GmmPlugInByOwner
  {
    private ReadoutConfigFunctions MyFunctions;

    public PlugInAnchor()
    {
      if (!PlugInLoader.IsPluginLoaderInitialised())
        return;
      this.MyFunctions = new ReadoutConfigFunctions();
      this.MyFunctions.IsPluginObject = true;
    }

    public override void Dispose() => this.MyFunctions.GMM_Dispose();

    public override string ShowMainWindow() => this.MyFunctions.ShowMainWindow();

    public override string ShowMainWindow(Window owner) => this.MyFunctions.ShowMainWindow(owner);

    public override PlugInInfo GetPluginInfo()
    {
      return new PlugInInfo("ReadoutConfiguration", "Reading", "Byteübertragung", "Grundeinstellung der Schnittstelle und der Übertragungskanäle", new string[0], PlugInAnchor.GetUsedRights(), (object) this.MyFunctions);
    }

    private static string[] GetUsedRights()
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<OverrideID, ConfigurationParameter.ConPaInfo> keyValuePair in ConfigurationParameter.ConfigParametersByOverrideID)
      {
        if (keyValuePair.Value.SubdeviceNumber == 0)
        {
          stringList.Add("Right\\Configurator\\ConfigParamVisible_" + keyValuePair.Key.ToString() + "|" + keyValuePair.Value.EnabledAsDefaultInOldLicense.ToString() + "|EnumDesc_ZR_ClassLibrary.OverrideID_" + keyValuePair.Key.ToString());
          stringList.Add("Right\\Configurator\\ConfigParamEditable_" + keyValuePair.Key.ToString() + "|" + keyValuePair.Value.EnabledAsDefaultInOldLicense.ToString() + "|EnumDesc_ZR_ClassLibrary.OverrideID_" + keyValuePair.Key.ToString());
        }
      }
      stringList.Add("ReadoutConfiguration\\ShowStandardChangableParameter|false");
      return stringList.ToArray();
    }
  }
}
