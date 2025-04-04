// Decompiled with JetBrains decompiler
// Type: EDC_Handler.FlashLoggerEntry
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using System;

#nullable disable
namespace EDC_Handler
{
  public sealed class FlashLoggerEntry
  {
    public FlashLoggerType Type { get; set; }

    public DateTime Timepoint { get; set; }

    public int? Value { get; set; }

    public byte? Month { get; set; }

    public byte? Reserved_Byte5 { get; set; }

    public int Index { get; set; }

    public bool IsCrcOK { get; set; }

    public ushort Address { get; set; }

    public byte[] Buffer { get; set; }
  }
}
