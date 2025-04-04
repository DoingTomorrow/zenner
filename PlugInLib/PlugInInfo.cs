// Decompiled with JetBrains decompiler
// Type: PlugInLib.PlugInInfo
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

#nullable disable
namespace PlugInLib
{
  public class PlugInInfo
  {
    public string Name;
    public string UsingPath;
    public string ShortDescription;
    public string Description;
    public string[] SubPlugInList;
    public string[] UsedRights;
    public object Interface;

    public PlugInInfo(
      string Name,
      string UsingPath,
      string ShortDescription,
      string Description,
      string[] SubPlugInList,
      string[] UsedRights,
      object Interface)
    {
      this.Name = Name;
      this.UsingPath = UsingPath;
      this.ShortDescription = ShortDescription;
      this.Description = Description;
      this.SubPlugInList = SubPlugInList;
      this.UsedRights = UsedRights;
      this.Interface = Interface;
    }
  }
}
