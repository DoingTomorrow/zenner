// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MsSql2012Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;

#nullable disable
namespace NHibernate.Dialect
{
  public class MsSql2012Dialect : MsSql2008Dialect
  {
    public override bool SupportsSequences => true;

    public override bool SupportsPooledSequences => true;

    public override string GetCreateSequenceString(string sequenceName)
    {
      return this.GetCreateSequenceString(sequenceName, 1, 1);
    }

    protected override string GetCreateSequenceString(
      string sequenceName,
      int initialValue,
      int incrementSize)
    {
      return string.Format("create sequence {0} as int start with {1} increment by {2}", (object) sequenceName, (object) initialValue, (object) incrementSize);
    }

    public override string GetDropSequenceString(string sequenceName)
    {
      return "drop sequence " + sequenceName;
    }

    public override string GetSequenceNextValString(string sequenceName)
    {
      return "select " + this.GetSelectSequenceNextValString(sequenceName) + " as seq";
    }

    public override string GetSelectSequenceNextValString(string sequenceName)
    {
      return "next value for " + sequenceName;
    }

    public override string QuerySequencesString => "select name from sys.sequences";

    protected override void RegisterFunctions()
    {
      base.RegisterFunctions();
      this.RegisterFunction("iif", (ISQLFunction) new StandardSafeSQLFunction("iif", 3));
    }
  }
}
