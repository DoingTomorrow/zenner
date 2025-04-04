// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.TrackingOptions
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  [Flags]
  public enum TrackingOptions
  {
    None = 0,
    Insert = 1,
    Update = 2,
    Delete = 4,
    All = Delete | Update | Insert, // 0x00000007
    Max = 8,
  }
}
