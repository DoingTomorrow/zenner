// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.HardwareType
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class HardwareType
  {
    public int HardwareTypeID { get; set; }

    public int MapID { get; set; }

    public uint FirmwareVersion { get; set; }

    public string HardwareName { get; set; }

    public int HardwareVersion { get; set; }

    public string HardwareResource { get; set; }

    public string Description { get; set; }

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("HardwareTypeID: ".PadRight(spaces)).AppendLine(this.HardwareTypeID.ToString());
      stringBuilder.Append("MapID: ".PadRight(spaces)).AppendLine(this.MapID.ToString());
      stringBuilder.Append("FirmwareVersion: ".PadRight(spaces)).AppendLine(this.FirmwareVersion.ToString());
      if (!string.IsNullOrEmpty(this.HardwareName))
        stringBuilder.Append("HardwareName: ".PadRight(spaces)).AppendLine(this.HardwareName);
      stringBuilder.Append("HardwareVersion: ".PadRight(spaces)).AppendLine(this.HardwareVersion.ToString());
      if (!string.IsNullOrEmpty(this.HardwareResource))
        stringBuilder.Append("HardwareResource: ".PadRight(spaces)).AppendLine(this.HardwareResource);
      if (!string.IsNullOrEmpty(this.Description))
        stringBuilder.Append("HardwareDescription: ".PadRight(spaces)).AppendLine(this.Description);
      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      return "ID: " + this.HardwareTypeID.ToString() + ", " + (string.IsNullOrEmpty(this.Description) ? "" : this.Description);
    }

    public HardwareType DeepCopy() => this.MemberwiseClone() as HardwareType;
  }
}
