// Decompiled with JetBrains decompiler
// Type: EDC_Handler.DatabaseDeviceInfo
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public sealed class DatabaseDeviceInfo
  {
    public HardwareType HardwareType { get; set; }

    public MeterInfo MeterInfo { get; set; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.HardwareType != null)
        stringBuilder.AppendLine(this.HardwareType.ToString());
      if (this.MeterInfo != null)
        stringBuilder.AppendLine(this.MeterInfo.ToString());
      return stringBuilder.ToString();
    }

    public DatabaseDeviceInfo DeepCopy()
    {
      return new DatabaseDeviceInfo()
      {
        HardwareType = this.HardwareType != null ? this.HardwareType.DeepCopy() : (HardwareType) null,
        MeterInfo = this.MeterInfo != null ? this.MeterInfo.DeepCopy() : (MeterInfo) null
      };
    }
  }
}
