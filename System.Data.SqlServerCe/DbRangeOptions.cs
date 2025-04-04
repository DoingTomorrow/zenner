// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.DbRangeOptions
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  [Flags]
  public enum DbRangeOptions
  {
    InclusiveStart = 0,
    InclusiveEnd = 0,
    ExclusiveStart = 1,
    ExclusiveEnd = 2,
    ExcludeNulls = 4,
    Prefix = 8,
    Match = 16, // 0x00000010
    Default = 0,
  }
}
