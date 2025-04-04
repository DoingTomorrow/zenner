// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.FORMULAERROR
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal enum FORMULAERROR : byte
  {
    NULL = 0,
    DIV0 = 7,
    VALUE = 15, // 0x0F
    REF = 23, // 0x17
    NAME = 29, // 0x1D
    NUM = 36, // 0x24
    NA = 42, // 0x2A
  }
}
