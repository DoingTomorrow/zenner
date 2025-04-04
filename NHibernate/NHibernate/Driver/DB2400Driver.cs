// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.DB2400Driver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Driver
{
  public class DB2400Driver : ReflectionBasedDriver
  {
    public DB2400Driver()
      : base("IBM.Data.DB2.iSeries", "IBM.Data.DB2.iSeries.iDB2Connection", "IBM.Data.DB2.iSeries.iDB2Command")
    {
    }

    public override bool UseNamedPrefixInSql => false;

    public override bool UseNamedPrefixInParameter => false;

    public override string NamedPrefix => string.Empty;

    public override bool SupportsMultipleOpenReaders => false;
  }
}
