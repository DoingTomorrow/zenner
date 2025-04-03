// Decompiled with JetBrains decompiler
// Type: HandlerLib.FirmwareReleaseInfo
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using GmmDbLib.DataSets;
using System;

#nullable disable
namespace HandlerLib
{
  public class FirmwareReleaseInfo : IComparable<FirmwareReleaseInfo>
  {
    public int MapID { get; internal set; }

    public string FirmwareVersionString { get; internal set; }

    public int FirmwareVersion { get; internal set; }

    public string ProgFilName { get; internal set; }

    public string ReleaseText { get; internal set; }

    public string ReleaseDescription { get; internal set; }

    public int CompareTo(FirmwareReleaseInfo compareObject)
    {
      return ((uint) this.FirmwareVersion).CompareTo((uint) compareObject.FirmwareVersion) * -1;
    }

    public override string ToString()
    {
      return this.MapID.ToString() + "; " + this.FirmwareVersionString + "; " + this.ReleaseText;
    }

    public static FirmwareReleaseInfo ToFirmwareReleaseInfo(HardwareTypeTables.ProgFilesRow pfRow)
    {
      FirmwareReleaseInfo firmwareReleaseInfo = new FirmwareReleaseInfo()
      {
        MapID = pfRow.MapID,
        FirmwareVersion = pfRow.FirmwareVersion
      };
      firmwareReleaseInfo.FirmwareVersionString = new ZENNER.CommonLibrary.FirmwareVersion((uint) firmwareReleaseInfo.FirmwareVersion).ToString();
      firmwareReleaseInfo.ProgFilName = pfRow.IsProgFileNameNull() ? string.Empty : pfRow.ProgFileName;
      firmwareReleaseInfo.ReleaseText = pfRow.IsReleasedNameNull() ? string.Empty : pfRow.ReleasedName;
      firmwareReleaseInfo.ReleaseDescription = pfRow.IsReleaseCommentsNull() ? string.Empty : pfRow.ReleaseComments;
      return firmwareReleaseInfo;
    }
  }
}
