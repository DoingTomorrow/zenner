// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.OleDbDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data;
using System.Data.OleDb;

#nullable disable
namespace NHibernate.Driver
{
  public class OleDbDriver : DriverBase
  {
    public override IDbConnection CreateConnection() => (IDbConnection) new OleDbConnection();

    public override IDbCommand CreateCommand() => (IDbCommand) new OleDbCommand();

    public override bool UseNamedPrefixInSql => false;

    public override bool UseNamedPrefixInParameter => false;

    public override string NamedPrefix => string.Empty;

    public override bool SupportsMultipleOpenReaders => false;
  }
}
