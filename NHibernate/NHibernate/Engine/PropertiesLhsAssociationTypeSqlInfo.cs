// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.PropertiesLhsAssociationTypeSqlInfo
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Engine
{
  public class PropertiesLhsAssociationTypeSqlInfo : AbstractLhsAssociationTypeSqlInfo
  {
    private readonly int propertyIdx;

    public PropertiesLhsAssociationTypeSqlInfo(
      string alias,
      int propertyIdx,
      IOuterJoinLoadable persister,
      IMapping mapping)
      : base(alias, persister, mapping)
    {
      this.propertyIdx = propertyIdx;
    }

    protected override string[] GetAliasedColumns()
    {
      return this.Persister.ToColumns(this.Alias, this.propertyIdx);
    }

    protected override string[] GetColumns()
    {
      return this.Persister.GetSubclassPropertyColumnNames(this.propertyIdx);
    }

    public override string GetTableName(IAssociationType type)
    {
      if (type.UseLHSPrimaryKey)
        return this.Persister.TableName;
      string lhsPropertyName = type.LHSPropertyName;
      return lhsPropertyName == null ? this.Persister.GetSubclassPropertyTableName(this.propertyIdx) : this.Persister.GetPropertyTableName(lhsPropertyName) ?? this.Persister.GetSubclassPropertyTableName(this.propertyIdx);
    }
  }
}
