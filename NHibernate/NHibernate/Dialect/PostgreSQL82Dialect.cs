// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.PostgreSQL82Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class PostgreSQL82Dialect : PostgreSQL81Dialect
  {
    public PostgreSQL82Dialect() => this.RegisterColumnType(DbType.Guid, "uuid");

    public override bool SupportsIfExistsBeforeTableName => true;

    public override string GetDropSequenceString(string sequenceName)
    {
      return "drop sequence if exists " + sequenceName;
    }
  }
}
