// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.IdPropertiesLhsAssociationTypeSqlInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Engine
{
  public class IdPropertiesLhsAssociationTypeSqlInfo(
    string alias,
    IOuterJoinLoadable persister,
    IMapping mapping) : AbstractLhsAssociationTypeSqlInfo(alias, persister, mapping)
  {
    protected override string[] GetAliasedColumns()
    {
      return this.Persister.ToIdentifierColumns(this.Alias);
    }

    protected override string[] GetColumns() => this.Persister.IdentifierColumnNames;

    public override string GetTableName(IAssociationType type) => this.Persister.TableName;
  }
}
