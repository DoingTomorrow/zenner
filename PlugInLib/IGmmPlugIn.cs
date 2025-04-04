// Decompiled with JetBrains decompiler
// Type: PlugInLib.IGmmPlugIn
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

#nullable disable
namespace PlugInLib
{
  public interface IGmmPlugIn : ILicensePlugIn
  {
    void Dispose();

    string ShowMainWindow();
  }
}
