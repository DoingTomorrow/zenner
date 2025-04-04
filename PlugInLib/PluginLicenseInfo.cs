// Decompiled with JetBrains decompiler
// Type: PlugInLib.PluginLicenseInfo
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

using System;

#nullable disable
namespace PlugInLib
{
  [Serializable]
  public class PluginLicenseInfo
  {
    public string Plugin;
    public bool Enable;

    public PluginLicenseInfo()
    {
      this.Plugin = string.Empty;
      this.Enable = false;
    }

    public override string ToString()
    {
      return !string.IsNullOrEmpty(this.Plugin) ? this.Plugin + " " + (object) this.Enable : string.Empty;
    }
  }
}
