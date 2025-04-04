// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.AbstractLhsAssociationTypeSqlInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;

#nullable disable
namespace NHibernate.Engine
{
  public abstract class AbstractLhsAssociationTypeSqlInfo : ILhsAssociationTypeSqlInfo
  {
    protected AbstractLhsAssociationTypeSqlInfo(
      string alias,
      IOuterJoinLoadable persister,
      IMapping mapping)
    {
      this.Alias = alias;
      this.Persister = persister;
      this.Mapping = mapping;
    }

    public string Alias { get; private set; }

    public IOuterJoinLoadable Persister { get; private set; }

    public IMapping Mapping { get; private set; }

    public string[] GetAliasedColumnNames(IAssociationType type, int begin)
    {
      if (type.UseLHSPrimaryKey)
        return StringHelper.Qualify(this.Alias, this.Persister.IdentifierColumnNames);
      string lhsPropertyName = type.LHSPropertyName;
      return lhsPropertyName == null ? ArrayHelper.Slice(this.GetAliasedColumns(), begin, type.GetColumnSpan(this.Mapping)) : ((IPropertyMapping) this.Persister).ToColumns(this.Alias, lhsPropertyName);
    }

    public string[] GetColumnNames(IAssociationType type, int begin)
    {
      if (type.UseLHSPrimaryKey)
        return this.Persister.IdentifierColumnNames;
      string lhsPropertyName = type.LHSPropertyName;
      return lhsPropertyName == null ? ArrayHelper.Slice(this.GetColumns(), begin, type.GetColumnSpan(this.Mapping)) : this.Persister.GetPropertyColumnNames(lhsPropertyName);
    }

    protected abstract string[] GetAliasedColumns();

    protected abstract string[] GetColumns();

    public abstract string GetTableName(IAssociationType type);
  }
}
