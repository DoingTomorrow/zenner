// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ManyToOneMapper
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
  public class ManyToOneMapper : 
    IManyToOneMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    IColumnsMapper
  {
    private readonly IAccessorPropertyMapper _entityPropertyMapper;
    private readonly HbmManyToOne _manyToOne;
    private readonly HbmMapping _mapDoc;
    private readonly MemberInfo _member;

    public ManyToOneMapper(MemberInfo member, HbmManyToOne manyToOne, HbmMapping mapDoc)
      : this(member, member == null ? (IAccessorPropertyMapper) new NoMemberPropertyMapper() : (IAccessorPropertyMapper) new AccessorPropertyMapper(member.DeclaringType, member.Name, (Action<string>) (x => manyToOne.access = x)), manyToOne, mapDoc)
    {
    }

    public ManyToOneMapper(
      MemberInfo member,
      IAccessorPropertyMapper accessorPropertyMapper,
      HbmManyToOne manyToOne,
      HbmMapping mapDoc)
    {
      this._member = member;
      this._manyToOne = manyToOne;
      this._mapDoc = mapDoc;
      if (member == null)
        this._manyToOne.access = "none";
      this._entityPropertyMapper = member == null ? (IAccessorPropertyMapper) new NoMemberPropertyMapper() : accessorPropertyMapper;
    }

    public void Class(Type entityType)
    {
      this._manyToOne.@class = this._member.GetPropertyOrFieldType().IsAssignableFrom(entityType) ? entityType.GetShortClassName(this._mapDoc) : throw new ArgumentOutOfRangeException(nameof (entityType), string.Format("The type is incompatible; expected assignable to {0}", (object) this._member.GetPropertyOrFieldType()));
    }

    public void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle)
    {
      this._manyToOne.cascade = cascadeStyle.Exclude(NHibernate.Mapping.ByCode.Cascade.DeleteOrphans).ToCascadeString();
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
      this._manyToOne.fetch = fetchMode.ToHbm();
      this._manyToOne.fetchSpecified = this._manyToOne.fetch == HbmFetchMode.Join;
    }

    public void Formula(string formula)
    {
      if (formula == null)
        return;
      this.ResetColumnPlainValues();
      this._manyToOne.Items = (object[]) null;
      string[] strArray = formula.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      if (strArray.Length > 1)
        this._manyToOne.Items = (object[]) new HbmFormula[1]
        {
          new HbmFormula() { Text = strArray }
        };
      else
        this._manyToOne.formula = formula;
    }

    public void Lazy(LazyRelation lazyRelation)
    {
      this._manyToOne.lazy = lazyRelation.ToHbm();
      this._manyToOne.lazySpecified = this._manyToOne.lazy != HbmLaziness.Proxy;
    }

    public void Update(bool consideredInUpdateQuery)
    {
      this._manyToOne.update = consideredInUpdateQuery;
    }

    public void Insert(bool consideredInInsertQuery)
    {
      this._manyToOne.insert = consideredInInsertQuery;
    }

    public void ForeignKey(string foreignKeyName) => this._manyToOne.foreignkey = foreignKeyName;

    public void PropertyRef(string propertyReferencedName)
    {
      this._manyToOne.propertyref = propertyReferencedName;
    }

    public void NotFound(NotFoundMode mode) => this._manyToOne.notfound = mode.ToHbm();

    public void Access(Accessor accessor) => this._entityPropertyMapper.Access(accessor);

    public void Access(Type accessorType) => this._entityPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this._manyToOne.optimisticlock = takeInConsiderationForOptimisticLock;
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      if (this._manyToOne.Columns.Count<HbmColumn>() > 1)
        throw new MappingException("Multi-columns property can't be mapped through single-column API.");
      this._manyToOne.formula = (string) null;
      HbmColumn hbmColumn = this._manyToOne.Columns.SingleOrDefault<HbmColumn>();
      if (hbmColumn == null)
        hbmColumn = new HbmColumn()
        {
          name = this._manyToOne.column,
          notnull = this._manyToOne.notnull,
          notnullSpecified = this._manyToOne.notnullSpecified,
          unique = this._manyToOne.unique,
          uniqueSpecified = this._manyToOne.unique,
          uniquekey = this._manyToOne.uniquekey,
          index = this._manyToOne.index
        };
      HbmColumn mapping = hbmColumn;
      string name = this._member.Name;
      columnMapper((IColumnMapper) new ColumnMapper(mapping, this._member != null ? name : "unnamedcolumn"));
      if (mapping.sqltype != null || mapping.@default != null || mapping.check != null)
      {
        this._manyToOne.Items = (object[]) new HbmColumn[1]
        {
          mapping
        };
        this.ResetColumnPlainValues();
      }
      else
      {
        this._manyToOne.column = name == null || !name.Equals(mapping.name) ? mapping.name : (string) null;
        this._manyToOne.notnull = mapping.notnull;
        this._manyToOne.notnullSpecified = mapping.notnullSpecified;
        this._manyToOne.unique = mapping.unique;
        this._manyToOne.uniquekey = mapping.uniquekey;
        this._manyToOne.index = mapping.index;
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
        string memberName = (this._member != null ? (object) this._member.Name : (object) "unnamedcolumn").ToString() + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this._manyToOne.Items = (object[]) hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (x => x.Name(name)));

    private void ResetColumnPlainValues()
    {
      this._manyToOne.column = (string) null;
      this._manyToOne.notnull = false;
      this._manyToOne.notnullSpecified = false;
      this._manyToOne.unique = false;
      this._manyToOne.uniquekey = (string) null;
      this._manyToOne.index = (string) null;
      this._manyToOne.formula = (string) null;
    }
  }
}
