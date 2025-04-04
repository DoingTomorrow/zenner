// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.PropertyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class PropertyMapper : 
    IPropertyMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    IColumnsMapper
  {
    private readonly IAccessorPropertyMapper entityPropertyMapper;
    private readonly MemberInfo member;
    private readonly HbmProperty propertyMapping;

    public PropertyMapper(
      MemberInfo member,
      HbmProperty propertyMapping,
      IAccessorPropertyMapper accessorMapper)
    {
      if (propertyMapping == null)
        throw new ArgumentNullException(nameof (propertyMapping));
      this.member = member;
      this.propertyMapping = propertyMapping;
      if (member == null)
        this.propertyMapping.access = "none";
      this.entityPropertyMapper = accessorMapper;
    }

    public PropertyMapper(MemberInfo member, HbmProperty propertyMapping)
    {
      if (propertyMapping == null)
        throw new ArgumentNullException(nameof (propertyMapping));
      this.member = member;
      this.propertyMapping = propertyMapping;
      if (member == null)
        this.propertyMapping.access = "none";
      if (member == null)
        this.entityPropertyMapper = (IAccessorPropertyMapper) new NoMemberPropertyMapper();
      else
        this.entityPropertyMapper = (IAccessorPropertyMapper) new AccessorPropertyMapper(member.DeclaringType, member.Name, (Action<string>) (x => propertyMapping.access = x));
    }

    public void Access(Accessor accessor) => this.entityPropertyMapper.Access(accessor);

    public void Access(System.Type accessorType) => this.entityPropertyMapper.Access(accessorType);

    public void OptimisticLock(bool takeInConsiderationForOptimisticLock)
    {
      this.propertyMapping.optimisticlock = takeInConsiderationForOptimisticLock;
    }

    public void Type(IType persistentType)
    {
      if (persistentType == null)
        return;
      this.propertyMapping.type1 = persistentType.Name;
      this.propertyMapping.type = (HbmType) null;
    }

    public void Type<TPersistentType>() => this.Type(typeof (TPersistentType), (object) null);

    public void Type<TPersistentType>(object parameters)
    {
      this.Type(typeof (TPersistentType), parameters);
    }

    public void Type(System.Type persistentType, object parameters)
    {
      if (persistentType == null)
        throw new ArgumentNullException(nameof (persistentType));
      if (!typeof (IUserType).IsAssignableFrom(persistentType) && !typeof (IType).IsAssignableFrom(persistentType) && !typeof (ICompositeUserType).IsAssignableFrom(persistentType))
        throw new ArgumentOutOfRangeException(nameof (persistentType), "Expected type implementing IUserType, ICompositeUserType or IType.");
      if (parameters != null)
      {
        this.propertyMapping.type1 = (string) null;
        this.propertyMapping.type = new HbmType()
        {
          name = persistentType.AssemblyQualifiedName,
          param = ((IEnumerable<PropertyInfo>) parameters.GetType().GetProperties()).Select(pi => new
          {
            pi = pi,
            pname = pi.Name
          }).Select(_param1 => new
          {
            \u003C\u003Eh__TransparentIdentifier4 = _param1,
            pvalue = _param1.pi.GetValue(parameters, (object[]) null)
          }).Select(_param0 => new HbmParam()
          {
            name = _param0.\u003C\u003Eh__TransparentIdentifier4.pname,
            Text = new string[1]
            {
              object.ReferenceEquals(_param0.pvalue, (object) null) ? "null" : _param0.pvalue.ToString()
            }
          }).ToArray<HbmParam>()
        };
      }
      else
      {
        this.propertyMapping.type1 = persistentType.AssemblyQualifiedName;
        this.propertyMapping.type = (HbmType) null;
      }
    }

    public void Column(Action<IColumnMapper> columnMapper)
    {
      if (this.propertyMapping.Columns.Count<HbmColumn>() > 1)
        throw new MappingException("Multi-columns property can't be mapped through singlr-column API.");
      this.propertyMapping.formula = (string) null;
      HbmColumn hbmColumn = this.propertyMapping.Columns.SingleOrDefault<HbmColumn>();
      if (hbmColumn == null)
        hbmColumn = new HbmColumn()
        {
          name = this.propertyMapping.column,
          length = this.propertyMapping.length,
          scale = this.propertyMapping.scale,
          precision = this.propertyMapping.precision,
          notnull = this.propertyMapping.notnull,
          notnullSpecified = this.propertyMapping.notnullSpecified,
          unique = this.propertyMapping.unique,
          uniqueSpecified = this.propertyMapping.unique,
          uniquekey = this.propertyMapping.uniquekey,
          index = this.propertyMapping.index
        };
      HbmColumn mapping = hbmColumn;
      string name = this.member.Name;
      columnMapper((IColumnMapper) new ColumnMapper(mapping, this.member != null ? name : "unnamedcolumn"));
      if (mapping.sqltype != null || mapping.@default != null || mapping.check != null)
      {
        this.propertyMapping.Items = (object[]) new HbmColumn[1]
        {
          mapping
        };
        this.ResetColumnPlainValues();
      }
      else
      {
        this.propertyMapping.column = name == null || !name.Equals(mapping.name) ? mapping.name : (string) null;
        this.propertyMapping.length = mapping.length;
        this.propertyMapping.precision = mapping.precision;
        this.propertyMapping.scale = mapping.scale;
        this.propertyMapping.notnull = mapping.notnull;
        this.propertyMapping.notnullSpecified = mapping.notnullSpecified;
        this.propertyMapping.unique = mapping.unique;
        this.propertyMapping.uniquekey = mapping.uniquekey;
        this.propertyMapping.index = mapping.index;
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
        string memberName = (this.member != null ? (object) this.member.Name : (object) "unnamedcolumn").ToString() + (object) num++;
        action((IColumnMapper) new ColumnMapper(mapping, memberName));
        hbmColumnList.Add(mapping);
      }
      this.propertyMapping.Items = (object[]) hbmColumnList.ToArray();
    }

    public void Column(string name) => this.Column((Action<IColumnMapper>) (x => x.Name(name)));

    public void Length(int length) => this.Column((Action<IColumnMapper>) (x => x.Length(length)));

    public void Precision(short precision)
    {
      this.Column((Action<IColumnMapper>) (x => x.Precision(precision)));
    }

    public void Scale(short scale) => this.Column((Action<IColumnMapper>) (x => x.Scale(scale)));

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

    public void Formula(string formula)
    {
      if (formula == null)
        return;
      this.ResetColumnPlainValues();
      this.propertyMapping.Items = (object[]) null;
      string[] strArray = formula.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      if (strArray.Length > 1)
        this.propertyMapping.Items = (object[]) new HbmFormula[1]
        {
          new HbmFormula() { Text = strArray }
        };
      else
        this.propertyMapping.formula = formula;
    }

    public void Update(bool consideredInUpdateQuery)
    {
      this.propertyMapping.update = consideredInUpdateQuery;
      this.propertyMapping.updateSpecified = !consideredInUpdateQuery;
    }

    public void Insert(bool consideredInInsertQuery)
    {
      this.propertyMapping.insert = consideredInInsertQuery;
      this.propertyMapping.insertSpecified = !consideredInInsertQuery;
    }

    public void Lazy(bool isLazy) => this.propertyMapping.lazy = isLazy;

    public void Generated(NHibernate.Mapping.ByCode.PropertyGeneration generation)
    {
      if (generation == null)
        return;
      this.propertyMapping.generated = generation.ToHbm();
    }

    private void ResetColumnPlainValues()
    {
      this.propertyMapping.column = (string) null;
      this.propertyMapping.length = (string) null;
      this.propertyMapping.precision = (string) null;
      this.propertyMapping.scale = (string) null;
      this.propertyMapping.notnull = this.propertyMapping.notnullSpecified = false;
      this.propertyMapping.unique = false;
      this.propertyMapping.uniquekey = (string) null;
      this.propertyMapping.index = (string) null;
      this.propertyMapping.formula = (string) null;
    }
  }
}
