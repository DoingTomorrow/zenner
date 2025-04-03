// Decompiled with JetBrains decompiler
// Type: PDC_Handler.DatabaseDeviceInfo
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
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
