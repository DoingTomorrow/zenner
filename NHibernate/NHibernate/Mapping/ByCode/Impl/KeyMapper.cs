// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.KeyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class KeyMapper : IKeyMapper, IColumnsMapper
  {
    private readonly HbmKey mapping;
    private readonly Type ownerEntityType;

    public KeyMapper(Type ownerEntityType, HbmKey mapping)
    {
      this.ownerEntityType = ownerEntityType;
      this.mapping = mapping;
      this.mapping.column1 = KeyMapper.DefaultColumnName(ownerEntityType);
    }

    public static string DefaultColumnName(Type ownerEntityType)
    {
      return ownerEntityType.Name.ToLowerInvariant() + "_key";
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      HbmColumn hbmColumn1 = this.mapping.Columns.Count<HbmColumn>() <= 1 ? this.mapping.Columns.SingleOrDefault<HbmColumn>() : throw new MappingException("Multi-columns property can't be mapped through single-column API.");
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.mapping.column1,
          notnull = this.mapping.notnull,
          unique = this.mapping.unique,
          uniqueSpecified = this.mapping.unique
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, KeyMapper.DefaultColumnName(this.ownerEntityType)));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.mapping.column = new HbmColumn[1]{ hbmColumn2 };
        this.ResetColumnPlainValues();
      }
      else
      {
        this.mapping.column1 = !KeyMapper.DefaultColumnName(this.ownerEntityType).Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
        this.NotNullable(hbmColumn2.notnull);
        this.Unique(hbmColumn2.unique);
      }
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.ResetColumnPlainValues();
      int num = 1;
      List<HbmColumn> hbmColumnList = new List<HbmColumn>(columnMapper.Length);
      foreach (Action<IColumnMapper> action in columnMapper)
      {
        HbmColumn mapping = new HbmColumn();
        string memberName = KeyMapper.DefaultColumnName(this.ownerEntityType) + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this.mapping.column = hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (x => x.Name(name)));

    public void OnDelete(OnDeleteAction deleteAction)
    {
      switch (deleteAction)
      {
        case OnDeleteAction.NoAction:
          this.mapping.ondelete = HbmOndelete.Noaction;
          break;
        case OnDeleteAction.Cascade:
          this.mapping.ondelete = HbmOndelete.Cascade;
          break;
      }
    }

    public void PropertyRef(MemberInfo property)
    {
      if (property == null)
      {
        this.mapping.propertyref = (string) null;
      }
      else
      {
        if (!this.ownerEntityType.Equals(property.DeclaringType) && !this.ownerEntityType.Equals(property.ReflectedType))
          throw new ArgumentOutOfRangeException(nameof (property), "Can't reference a property of another entity.");
        this.mapping.propertyref = property.Name;
      }
    }

    public void Update(bool consideredInUpdateQuery)
    {
      this.mapping.update = consideredInUpdateQuery;
      this.mapping.updateSpecified = true;
    }

    public void NotNullable(bool notnull)
    {
      this.mapping.notnull = this.mapping.notnullSpecified = notnull;
    }

    public void Unique(bool unique) => this.mapping.unique = this.mapping.uniqueSpecified = unique;

    public void ForeignKey(string foreignKeyName)
    {
      if (foreignKeyName == null)
      {
        this.mapping.foreignkey = (string) null;
      }
      else
      {
        string str = foreignKeyName.Trim();
        if (string.Empty.Equals(str))
          this.mapping.foreignkey = "none";
        else
          this.mapping.foreignkey = str;
      }
    }

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null || hbm.length != null || hbm.precision != null || hbm.scale != null;
    }

    private void ResetColumnPlainValues()
    {
      this.mapping.column1 = (string) null;
      this.mapping.notnull = false;
      this.mapping.unique = false;
    }
  }
}
