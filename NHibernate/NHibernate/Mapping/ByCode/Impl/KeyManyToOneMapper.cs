// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.KeyManyToOneMapper
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
  public class KeyManyToOneMapper : 
    IManyToOneMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    IColumnsMapper
  {
    private readonly IAccessorPropertyMapper _entityPropertyMapper;
    private readonly HbmKeyManyToOne _manyToOne;
    private readonly HbmMapping _mapDoc;
    private readonly MemberInfo _member;

    public KeyManyToOneMapper(MemberInfo member, HbmKeyManyToOne manyToOne, HbmMapping mapDoc)
    {
      this._member = member;
      this._manyToOne = manyToOne;
      this._mapDoc = mapDoc;
      if (member == null)
        this._manyToOne.access = "none";
      if (member == null)
        this._entityPropertyMapper = (IAccessorPropertyMapper) new NoMemberPropertyMapper();
      else
        this._entityPropertyMapper = (IAccessorPropertyMapper) new AccessorPropertyMapper(member.DeclaringType, member.Name, (Action<string>) (x => manyToOne.access = x));
    }

    public void Class(Type entityType)
    {
      this._manyToOne.@class = this._member.GetPropertyOrFieldType().IsAssignableFrom(entityType) ? entityType.GetShortClassName(this._mapDoc) : throw new ArgumentOutOfRangeException(nameof (entityType), string.Format("The type is incompatible; expected assignable to {0}", (object) this._member.GetPropertyOrFieldType()));
    }

    public void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle)
    {
    }

    public void NotNullable(bool notnull)
    {
      this.Column((Action<IColumnMapper>) (x => x.NotNullable(notnull)));
    }

    public void Unique(bool unique) => this.Column((Action<IColumnMapper>) (x => x.Unique(unique)));

    public void UniqueKey(string uniquekeyName)
    {
      this.Column((Action<IColumnMapper>) (x => x.UniqueKey(uniquekeyName)));
    }

    public void Index(string indexName)
    {
      this.Column((Action<IColumnMapper>) (x => x.Index(indexName)));
    }

    public void Fetch(FetchKind fetchMode)
    {
    }

    public void Formula(string formula)
    {
    }

    public void Lazy(LazyRelation lazyRelation)
    {
      switch (lazyRelation.ToHbm())
      {
        case HbmLaziness.False:
          this._manyToOne.lazy = HbmRestrictedLaziness.False;
          this._manyToOne.lazySpecified = true;
          break;
        case HbmLaziness.Proxy:
          this._manyToOne.lazy = HbmRestrictedLaziness.Proxy;
          this._manyToOne.lazySpecified = true;
          break;
        case HbmLaziness.NoProxy:
          this._manyToOne.lazy = HbmRestrictedLaziness.False;
          this._manyToOne.lazySpecified = true;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public void Update(bool consideredInUpdateQuery)
    {
    }

    public void Insert(bool consideredInInsertQuery)
    {
    }

    public void ForeignKey(string foreignKeyName) => this._manyToOne.foreignkey = foreignKeyName;

    public void PropertyRef(string propertyReferencedName)
    {
    }

    public void NotFound(NotFoundMode mode) => this._manyToOne.notfound = mode.ToHbm();

    public void Access(Accessor accessor) => this._entityPropertyMapper.Access(accessor);

    public void Access(Type accessorType) => this._entityPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      HbmColumn hbmColumn1 = this._manyToOne.Columns.Count<HbmColumn>() <= 1 ? this._manyToOne.Columns.SingleOrDefault<HbmColumn>() : throw new MappingException("Multi-columns property can't be mapped through single-column API.");
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this._manyToOne.column1
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      string name = this._member.Name;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, this._member != null ? name : "unnamedcolumn"));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this._manyToOne.column = new HbmColumn[1]
        {
          hbmColumn2
        };
        this.ResetColumnPlainValues();
      }
      else
        this._manyToOne.column1 = name == null || !name.Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
    }

    public void Columns(params Action<IColumnMapper>[] columnMapper)
    {
      this.ResetColumnPlainValues();
      int num = 1;
      List<HbmColumn> hbmColumnList = new List<HbmColumn>(columnMapper.Length);
      foreach (Action<IColumnMapper> action in columnMapper)
      {
        HbmColumn mapping = new HbmColumn();
        string memberName = (this._member != null ? (object) this._member.Name : (object) "unnamedcolumn").ToString() + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this._manyToOne.column = hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (x => x.Name(name)));

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.length != null || hbm.precision != null || hbm.scale != null || hbm.notnull || hbm.unique || hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null;
    }

    private void ResetColumnPlainValues() => this._manyToOne.column1 = (string) null;
  }
}
