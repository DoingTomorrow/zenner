// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.MEOPENINFO
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  internal struct MEOPENINFO
  {
    internal IntPtr pwszFileName;
    internal IntPtr pwszPassword;
    internal IntPtr pwszTempPath;
    internal int lcidLocale;
    internal int cbBufferPool;
    internal int fEncrypt;
    internal int dwAutoShrinkPercent;
    internal int dwFlushInterval;
    internal int cMaxPages;
    internal int cMaxTmpPages;
    internal int dwDefaultTimeout;
    internal int dwDefaultEscalation;
    internal SEOPENFLAGS dwFlags;
    internal int dwEncryptionMode;
    internal int dwLocaleFlags;
  }
}
