// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.ResultSetOptions
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  [Flags]
  public enum ResultSetOptions
  {
    None = 0,
    Updatable = 1,
    Scrollable = 2,
    Sensitive = 4,
    Insensitive = 8,
  }
}
