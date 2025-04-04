// Decompiled with JetBrains decompiler
// Type: Excel.Core.BinaryFormat.BIFFTYPE
// Assembly: Excel.4.5, Version=2.1.2.0, Culture=neutral, PublicKeyToken=93517dbe6a4012fa
// MVID: FC72B9E7-E35A-4A43-9AA0-53802BC5FDE7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Excel.4.5.dll

#nullable disable
namespace Excel.Core.BinaryFormat
{
  internal enum BIFFTYPE : ushort
  {
    WorkbookGlobals = 5,
    VBModule = 6,
    Worksheet = 16, // 0x0010
    Chart = 32, // 0x0020
    v4MacroSheet = 64, // 0x0040
    v4WorkbookGlobals = 256, // 0x0100
  }
}
