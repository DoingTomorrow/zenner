// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DeviceSearchFilter
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using ZENNER.CommonLibrary;

#nullable disable
namespace GmmDbLib
{
  public sealed class DeviceSearchFilter
  {
    public DateTime? ProductionStartDate { get; set; }

    public DateTime? ProductionEndDate { get; set; }

    public DateTime? ApprovalStartDate { get; set; }

    public DateTime? ApprovalEndDate { get; set; }

    public string Serialnumber { get; set; }

    public string MeterID { get; set; }

    public string OrderNumber { get; set; }

    public string HardwareName { get; set; }

    public FirmwareType FwType { get; set; }

    public bool IsOldVersion { get; set; }

    public DeviceSearchFilter() => this.IsOldVersion = false;
  }
}
