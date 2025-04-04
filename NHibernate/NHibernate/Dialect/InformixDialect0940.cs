// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.InformixDialect0940
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class InformixDialect0940 : InformixDialect
  {
    public InformixDialect0940()
    {
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "CLOB");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "BLOB");
      this.RegisterColumnType(DbType.Binary, "BLOB");
      this.RegisterColumnType(DbType.String, int.MaxValue, "CLOB");
    }

    public override string QuerySequencesString
    {
      get => "select tabname from systables where tabtype='Q'";
    }

    public override bool SupportsSequences => true;

    public override bool SupportsPooledSequences => true;

    public override string GetSequenceNextValString(string sequenceName)
    {
      return "select " + this.GetSelectSequenceNextValString(sequenceName) + " from systables where tabid=1";
    }

    public override string GetDropSequenceString(string sequenceName)
    {
      return "drop sequence " + sequenceName;
    }

    public override string GetSelectSequenceNextValString(string sequenceName)
    {
      return sequenceName + ".nextval";
    }

    public override string GetCreateSequenceString(string sequenceName)
    {
      return "create sequence " + sequenceName;
    }

    public override JoinFragment CreateOuterJoinFragment() => (JoinFragment) new ANSIJoinFragment();

    public override bool SupportsLimit => false;

    public override bool SupportsLimitOffset => false;
  }
}
