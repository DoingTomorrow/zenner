// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.GlobalDeviceId
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class GlobalDeviceId
  {
    public ValueIdent.ValueIdPart_MeterType MeterType;
    public string Manufacturer;
    public string Generation;
    public string Serialnumber;
    public int Address = -1;
    public string MeterNumber;
    public string DeviceTypeName;
    public string FirmwareVersion;
    public string DeviceDetails;
    public bool IsRegistered;
    public List<GlobalDeviceId> SubDevices;

    public GlobalDeviceId() => this.SubDevices = new List<GlobalDeviceId>();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Serialnumber);
      stringBuilder.Append(";");
      if (this.Manufacturer != null && this.Manufacturer != "???")
      {
        stringBuilder.Append(this.Manufacturer);
        stringBuilder.Append(";");
      }
      stringBuilder.Append(this.DeviceTypeName);
      if (this.Address >= 0)
      {
        stringBuilder.Append(";");
        stringBuilder.Append(this.Address.ToString());
      }
      return stringBuilder.ToString().Trim(';');
    }
  }
}
