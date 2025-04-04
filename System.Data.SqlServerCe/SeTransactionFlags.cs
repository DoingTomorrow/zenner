// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SeTransactionFlags
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  [Flags]
  internal enum SeTransactionFlags
  {
    NOFLAGS = 0,
    SYSTEM = 1,
    GENERATEIDENTITY = 2,
    GENERATEROWGUID = 4,
    TRACK = 8,
    REPLACECOLUMN = 16, // 0x00000010
    DISABLETRIGGERS = 32, // 0x00000020
    COMPRESSEDLVSTREAM = 64, // 0x00000040
    VALIDFLAGS = COMPRESSEDLVSTREAM | DISABLETRIGGERS | REPLACECOLUMN | TRACK | GENERATEROWGUID | GENERATEIDENTITY | SYSTEM, // 0x0000007F
  }
}
