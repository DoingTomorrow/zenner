// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.MEDBBINDING
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  internal struct MEDBBINDING
  {
    public int iOrdinal;
    public int obValue;
    public int obSize;
    public int obStatus;
    public IntPtr pObject;
    public int cbMaxLen;
    public uint dwFlags;
    public SETYPE type;
    public uint bPrecision;
    public uint bScale;
    public int minor_pError;
  }
}
