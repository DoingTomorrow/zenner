// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ManyToManyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ManyToManyMapper : IManyToManyMapper, IColumnsMapper
  {
    private readonly Type elementType;
    private readonly HbmManyToMany manyToMany;
    private readonly HbmMapping mapDoc;

    public ManyToManyMapper(Type elementType, HbmManyToMany manyToMany, HbmMapping mapDoc)
    {
      if (elementType == null)
        throw new ArgumentNullException(nameof (elementType));
      if (manyToMany == null)
        throw new ArgumentNullException(nameof (manyToMany));
      this.elementType = elementType;
      this.manyToMany = manyToMany;
      this.mapDoc = mapDoc;
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      if (this.manyToMany.Columns.Count<HbmColumn>() > 1)
        throw new MappingException("Multi-columns property can't be mapped through single-column API.");
      this.manyToMany.formula = (string) null;
      HbmColumn hbmColumn1 = this.manyToMany.Columns.SingleOrDefault<HbmColumn>();
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.manyToMany.column,
          unique = this.manyToMany.unique,
          uniqueSpecified = this.manyToMany.unique
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      string name = this.elementType.Name;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, name));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.manyToMany.Items = (object[]) new HbmColumn[1]
        {
          hbmColumn2
        };
        this.ResetColumnPlainValues();
      }
      else
      {
        this.manyToMany.column = name == null || !name.Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
        this.manyToMany.unique = hbmColumn2.unique;
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
        string memberName = this.elementType.Name + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this.manyToMany.Items = (object[]) hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (x => x.Name(name)));

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.length != null || hbm.precision != null || hbm.scale != null || hbm.notnull || hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null;
    }

    private void ResetColumnPlainValues()
    {
      this.manyToMany.column = (string) null;
      this.manyToMany.unique = false;
      this.manyToMany.formula = (string) null;
    }

    public void Class(Type entityType)
    {
      this.manyToMany.@class = this.elementType.IsAssignableFrom(entityType) ? entityType.GetShortClassName(this.mapDoc) : throw new ArgumentOutOfRangeException(nameof (entityType), string.Format("The type is incompatible; expected assignable to {0}", (object) this.elementType));
    }

    public void EntityName(string entityName) => this.manyToMany.entityname = entityName;

    public void NotFound(NotFoundMode mode)
    {
      if (mode == null)
        return;
      this.manyToMany.notfound = mode.ToHbm();
    }

    public void Formula(string formula)
    {
      if (formula == null)
        return;
      this.ResetColumnPlainValues();
      this.manyToMany.Items = (object[]) null;
      string[] strArray = formula.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      if (strArray.Length > 1)
        this.manyToMany.Items = (object[]) new HbmFormula[1]
        {
          new HbmFormula() { Text = strArray }
        };
      else
        this.manyToMany.formula = formula;
    }

    public void Lazy(LazyRelation lazyRelation)
    {
      switch (lazyRelation.ToHbm())
      {
        case HbmLaziness.False:
          this.manyToMany.lazy = HbmRestrictedLaziness.False;
          this.manyToMany.lazySpecified = true;
          break;
        case HbmLaziness.Proxy:
          this.manyToMany.lazy = HbmRestrictedLaziness.Proxy;
          this.manyToMany.lazySpecified = true;
          break;
        case HbmLaziness.NoProxy:
          this.manyToMany.lazy = HbmRestrictedLaziness.False;
          this.manyToMany.lazySpecified = true;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void ForeignKey(string foreignKeyName) => this.manyToMany.foreignkey = foreignKeyName;
  }
}
