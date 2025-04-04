// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.DbColumnFlags
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  [Flags]
  internal enum DbColumnFlags
  {
    ISBOOKMARK = 1,
    MAYDEFER = 2,
    WRITE = 4,
    WRITEUNKNOWN = 8,
    ISFIXEDLENGTH = 16, // 0x00000010
    ISNULLABLE = 32, // 0x00000020
    MAYBENULL = 64, // 0x00000040
    ISLONG = 128, // 0x00000080
    ISROWID = 256, // 0x00000100
    ISROWVER = 512, // 0x00000200
    CACHEDEFERRED = 4096, // 0x00001000
  }
}
