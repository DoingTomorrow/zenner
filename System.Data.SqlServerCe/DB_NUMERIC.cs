// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.DB_NUMERIC
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  internal struct DB_NUMERIC
  {
    public byte precision;
    public byte scale;
    public byte sign;
    public unsafe fixed byte val[16];
  }
}
