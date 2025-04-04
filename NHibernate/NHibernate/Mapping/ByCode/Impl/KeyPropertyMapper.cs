// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.KeyPropertyMapper
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
  public class KeyPropertyMapper : 
    IPropertyMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper,
    IColumnsMapper
  {
    private readonly IAccessorPropertyMapper entityPropertyMapper;
    private readonly MemberInfo member;
    private readonly HbmKeyProperty propertyMapping;

    public KeyPropertyMapper(MemberInfo member, HbmKeyProperty propertyMapping)
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
      if (!typeof (IUserType).IsAssignableFrom(persistentType) && !typeof (IType).IsAssignableFrom(persistentType))
        throw new ArgumentOutOfRangeException(nameof (persistentType), "Expected type implementing IUserType or IType.");
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
      HbmColumn hbmColumn1 = this.propertyMapping.Columns.Count<HbmColumn>() <= 1 ? this.propertyMapping.Columns.SingleOrDefault<HbmColumn>() : throw new MappingException("Multi-columns property can't be mapped through singlr-column API.");
      if (hbmColumn1 == null)
        hbmColumn1 = new HbmColumn()
        {
          name = this.propertyMapping.column1,
          length = this.propertyMapping.length
        };
      HbmColumn hbmColumn2 = hbmColumn1;
      string name = this.member.Name;
      columnMapper((IColumnMapper) new ColumnMapper(hbmColumn2, this.member != null ? name : "unnamedcolumn"));
      if (this.ColumnTagIsRequired(hbmColumn2))
      {
        this.propertyMapping.column = new HbmColumn[1]
        {
          hbmColumn2
        };
        this.ResetColumnPlainValues();
      }
      else
      {
        this.propertyMapping.column1 = name == null || !name.Equals(hbmColumn2.name) ? hbmColumn2.name : (string) null;
        this.propertyMapping.length = hbmColumn2.length;
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
      this.propertyMapping.column = hbmColumnList.ToArray();
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
    }

    public void Update(bool consideredInUpdateQuery)
    {
    }

    public void Insert(bool consideredInInsertQuery)
    {
    }

    public void Lazy(bool isLazy)
    {
    }

    public void Generated(NHibernate.Mapping.ByCode.PropertyGeneration generation)
    {
    }

    private bool ColumnTagIsRequired(HbmColumn hbm)
    {
      return hbm.precision != null || hbm.scale != null || hbm.notnull || hbm.unique || hbm.uniquekey != null || hbm.sqltype != null || hbm.index != null || hbm.@default != null || hbm.check != null;
    }

    private void ResetColumnPlainValues()
    {
      this.propertyMapping.column1 = (string) null;
      this.propertyMapping.length = (string) null;
    }
  }
}
