// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.SybaseSQLAnywhere12Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Schema;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect
{
  public class SybaseSQLAnywhere12Dialect : SybaseSQLAnywhere11Dialect
  {
    public SybaseSQLAnywhere12Dialect()
    {
      this.DefaultProperties["connection.driver_class"] = "NHibernate.Driver.SybaseSQLAnywhereDotNet4Driver";
      this.RegisterDateTimeTypeMappings();
      this.RegisterKeywords();
    }

    protected new void RegisterKeywords()
    {
      this.RegisterKeyword("NEAR");
      this.RegisterKeyword("LIMIT");
      this.RegisterKeyword("OFFSET");
      this.RegisterKeyword("DATETIMEOFFSET");
    }

    private new void RegisterDateTimeTypeMappings()
    {
      this.RegisterColumnType(DbType.DateTimeOffset, "DATETIMEOFFSET");
    }

    public override bool SupportsSequences => true;

    public override bool SupportsPooledSequences => true;

    public override string QuerySequencesString => "SELECT SEQUENCE_NAME FROM SYS.SYSSEQUENCE";

    public override string GetSequenceNextValString(string sequenceName)
    {
      return "SELECT " + this.GetSelectSequenceNextValString(sequenceName) + " FROM SYS.DUMMY";
    }

    public override string GetSelectSequenceNextValString(string sequenceName)
    {
      return sequenceName + ".NEXTVAL";
    }

    public override string GetCreateSequenceString(string sequenceName)
    {
      return "CREATE SEQUENCE " + sequenceName;
    }

    public override string GetDropSequenceString(string sequenceName)
    {
      return "DROP SEQUENCE " + sequenceName;
    }

    public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
    {
      return (IDataBaseSchema) new SybaseAnywhereDataBaseMetaData(connection);
    }
  }
}
