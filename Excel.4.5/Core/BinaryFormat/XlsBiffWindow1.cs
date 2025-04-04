// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.XlsBiffWindow1
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal class XlsBiffWindow1 : XlsBiffRecord
  {
    internal XlsBiffWindow1(byte[] bytes, uint offset, ExcelBinaryReader reader)
      : base(bytes, offset, reader)
    {
    }

    public ushort Left => this.ReadUInt16(0);

    public ushort Top => this.ReadUInt16(2);

    public ushort Width => this.ReadUInt16(4);

    public ushort Height => this.ReadUInt16(6);

    public XlsBiffWindow1.Window1Flags Flags => (XlsBiffWindow1.Window1Flags) this.ReadUInt16(8);

    public ushort ActiveTab => this.ReadUInt16(10);

    public ushort FirstVisibleTab => this.ReadUInt16(12);

    public ushort SelectedTabCount => this.ReadUInt16(14);

    public ushort TabRatio => this.ReadUInt16(16);

    [System.Flags]
    public enum Window1Flags : ushort
    {
      Hidden = 1,
      Minimized = 2,
      HScrollVisible = 8,
      VScrollVisible = 16, // 0x0010
      WorkbookTabs = 32, // 0x0020
    }
  }
}
