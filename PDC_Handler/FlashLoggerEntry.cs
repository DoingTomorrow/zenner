// Decompiled with JetBrains decompiler
// Type: PDC_Handler.FlashLoggerEntry
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public sealed class FlashLoggerEntry
  {
    public const int SIZE = 12;

    public FlashLoggerType Type { get; set; }

    public uint? ValueInputA { get; set; }

    public uint? ValueInputB { get; set; }

    public DateTime? Timepoint { get; set; }

    public bool IsCrcOK { get; set; }

    public ushort Address { get; set; }

    public static FlashLoggerEntry Parse(byte[] memory, ushort address, FlashLoggerType type)
    {
      bool flag = (int) BitConverter.ToUInt16(memory, (int) address + 10) == (int) Util.CalculatesCRC16_CC430(memory, (int) address, 10);
      if (!flag)
        return (FlashLoggerEntry) null;
      return new FlashLoggerEntry()
      {
        Type = type,
        Address = address,
        ValueInputA = new uint?(BitConverter.ToUInt32(memory, (int) address)),
        ValueInputB = new uint?(BitConverter.ToUInt32(memory, (int) address + 4)),
        Timepoint = Util.ConvertToDate_MBus_CP16_TypeG(memory, (int) address + 8),
        IsCrcOK = flag
      };
    }

    public override string ToString()
    {
      if (!this.IsCrcOK)
        return string.Format("0x{0:X4} {1}, NULL", (object) this.Address, (object) this.Type);
      return this.Timepoint.HasValue ? string.Format("0x{0:X4} {1}, {2:d}, A: {3}, B: {4}", (object) this.Address, (object) this.Type, (object) this.Timepoint, (object) this.ValueInputA, (object) this.ValueInputB) : string.Format("0x{0:X4} {1}, Invalid Timestamp, A: {2}, B: {3}", (object) this.Address, (object) this.Type, (object) this.ValueInputA, (object) this.ValueInputB);
    }
  }
}
