// Decompiled with JetBrains decompiler
// Type: HandlerLib.CompatibilityInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Text;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class CompatibilityInfo : IComparable<CompatibilityInfo>
  {
    public int MapID;
    public uint Firmware;
    public int CompatibleToMapID;
    public uint CompatibleToFirmware;
    public bool IsFullCompatible = false;
    public List<string> CompatibleGroupShortcuts;

    public int CompareTo(CompatibilityInfo compareInfo)
    {
      int num = new FirmwareVersion(this.Firmware).CompareTo((object) new FirmwareVersion(compareInfo.Firmware));
      return num != 0 ? num : new FirmwareVersion(this.CompatibleToFirmware).CompareTo((object) new FirmwareVersion(compareInfo.CompatibleToFirmware));
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.Firmware > 0U)
        stringBuilder.Append("FirmwareVersion: " + new FirmwareVersion(this.Firmware).ToString() + " / ");
      stringBuilder.Append("MapID:" + this.MapID.ToString() + " compatible to -> ");
      if (this.CompatibleToFirmware > 0U)
        stringBuilder.Append("FirmwareVersion :" + new FirmwareVersion(this.CompatibleToFirmware).ToString() + " / ");
      stringBuilder.Append("MapID:" + this.CompatibleToMapID.ToString());
      if (this.IsFullCompatible)
        stringBuilder.Append(" # Full compatible ");
      else if (this.CompatibleGroupShortcuts != null && this.CompatibleGroupShortcuts.Count > 0)
      {
        stringBuilder.Append(" # Compatible groups: ");
        bool flag = true;
        foreach (string compatibleGroupShortcut in this.CompatibleGroupShortcuts)
        {
          if (flag)
            flag = false;
          else
            stringBuilder.Append(";");
          stringBuilder.Append(compatibleGroupShortcut);
        }
      }
      return stringBuilder.ToString();
    }
  }
}
