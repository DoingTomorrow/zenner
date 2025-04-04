// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SEOPENFLAGS
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  [Flags]
  internal enum SEOPENFLAGS
  {
    MODE_READ = 1,
    MODE_WRITE = 2,
    MODE_SHARE_DENY_READ = 4,
    MODE_SHARE_DENY_WRITE = 8,
  }
}
