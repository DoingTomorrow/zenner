// Decompiled with JetBrains decompiler
// Type: PlugInLib.PermissionInfo
// Assembly: PlugInLib, Version=2.0.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 0D0A1C6E-D587-46FA-A431-5DFCE0ADBD53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PlugInLib.dll

#nullable disable
namespace PlugInLib
{
  public class PermissionInfo
  {
    public int PermissionId { get; set; }

    public string PermissionName { get; set; }

    public bool PermissionValue { get; set; }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.PermissionName))
        return string.Empty;
      return this.PermissionId.ToString() + " " + this.PermissionName + " " + (object) this.PermissionValue;
    }
  }
}
