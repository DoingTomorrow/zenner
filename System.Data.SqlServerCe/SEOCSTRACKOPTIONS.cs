// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SEOCSTRACKOPTIONS
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  [Flags]
  internal enum SEOCSTRACKOPTIONS
  {
    NONE = 0,
    UPSERT = 1,
    INSERTUPDATE = 2,
    DELETE = 4,
    COLUMNS = 8,
    ALL = COLUMNS | DELETE | INSERTUPDATE | UPSERT, // 0x0000000F
  }
}
