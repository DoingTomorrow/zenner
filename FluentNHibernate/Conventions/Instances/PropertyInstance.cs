// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.PropertyInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using NHibernate.UserTypes;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class PropertyInstance : 
    PropertyInspector,
    IPropertyInstance,
    IPropertyInspector,
    IReadOnlyInspector,
    IExposedThroughPropertyInspector,
    IInspector,
    IAccessInspector,
    IInsertInstance,
    IUpdateInstance,
    IReadOnlyInstance,
    INullableInstance
  {
    private const int layer = 1;
    private readonly PropertyMapping mapping;
    private bool nextBool = true;

    public PropertyInstance(PropertyMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public void Insert()
    {
      this.mapping.Set<bool>((Expression<Func<PropertyMapping, bool>>) (x => x.Insert), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Update()
    {
      this.mapping.Set<bool>((Expression<Func<PropertyMapping, bool>>) (x => x.Update), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void ReadOnly()
    {
      this.mapping.Set<bool>((Expression<Func<PropertyMapping, bool>>) (x => x.Insert), 1, (!this.nextBool ? 1 : 0) != 0);
      this.mapping.Set<bool>((Expression<Func<PropertyMapping, bool>>) (x => x.Update), 1, (!this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Nullable()
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, bool>> expression = (Expression<Func<ColumnMapping, bool>>) (x => x.NotNull);
        int num = !this.nextBool ? 1 : 0;
        column.Set<bool>(expression, 1, num != 0);
      }
      this.nextBool = true;
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<PropertyMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public void CustomType(TypeReference type) => this.CustomType(type, this.Property.Name + "_");

    public void CustomType(TypeReference type, string columnPrefix)
    {
      this.mapping.Set<TypeReference>((Expression<Func<PropertyMapping, TypeReference>>) (x => x.Type), 1, type);
      if (!typeof (ICompositeUserType).IsAssignableFrom(this.mapping.Type.GetUnderlyingSystemType()))
        return;
      this.AddColumnsForCompositeUserType(columnPrefix);
    }

    public void CustomType<T>(string columnPrefix) => this.CustomType(typeof (T), columnPrefix);

    public void CustomType<T>() => this.CustomType(typeof (T));

    public void CustomType(System.Type type) => this.CustomType(new TypeReference(type));

    public void CustomType(System.Type type, string columnPrefix)
    {
      this.CustomType(new TypeReference(type), columnPrefix);
    }

    public void CustomType(string type) => this.CustomType(new TypeReference(type));

    public void CustomType(string type, string columnPrefix)
    {
      this.CustomType(new TypeReference(type), columnPrefix);
    }

    public void CustomSqlType(string sqlType)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.SqlType);
        string str = sqlType;
        column.Set<string>(expression, 1, str);
      }
    }

    public void Precision(int precision)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, int>> expression = (Expression<Func<ColumnMapping, int>>) (x => x.Precision);
        int num = precision;
        column.Set<int>(expression, 1, num);
      }
    }

    public void Scale(int scale)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, int>> expression = (Expression<Func<ColumnMapping, int>>) (x => x.Scale);
        int num = scale;
        column.Set<int>(expression, 1, num);
      }
    }

    public void Default(string value)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.Default);
        string str = value;
        column.Set<string>(expression, 1, str);
      }
    }

    public void Unique()
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, bool>> expression = (Expression<Func<ColumnMapping, bool>>) (x => x.Unique);
        int num = this.nextBool ? 1 : 0;
        column.Set<bool>(expression, 1, num != 0);
      }
      this.nextBool = true;
    }

    public void UniqueKey(string keyName)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.UniqueKey);
        string str = keyName;
        column.Set<string>(expression, 1, str);
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IPropertyInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IPropertyInstance) this;
      }
    }

    public void Column(string columnName)
    {
      ColumnMapping columnMapping = this.mapping.Columns.FirstOrDefault<ColumnMapping>();
      ColumnMapping mapping = columnMapping == null ? new ColumnMapping() : columnMapping.Clone();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 1, columnName);
      this.mapping.AddColumn(1, mapping);
    }

    public void Formula(string formula)
    {
      this.mapping.Set<string>((Expression<Func<PropertyMapping, string>>) (x => x.Formula), 1, formula);
      this.mapping.MakeColumnsEmpty(2);
    }

    public IGeneratedInstance Generated
    {
      get
      {
        return (IGeneratedInstance) new GeneratedInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<PropertyMapping, string>>) (x => x.Generated), 1, value)));
      }
    }

    public void OptimisticLock()
    {
      this.mapping.Set<bool>((Expression<Func<PropertyMapping, bool>>) (x => x.OptimisticLock), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Length(int length)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, int>> expression = (Expression<Func<ColumnMapping, int>>) (x => x.Length);
        int num = length;
        column.Set<int>(expression, 1, num);
      }
    }

    public void LazyLoad()
    {
      this.mapping.Set<bool>((Expression<Func<PropertyMapping, bool>>) (x => x.Lazy), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Index(string value)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.Index);
        string str = value;
        column.Set<string>(expression, 1, str);
      }
    }

    public void Check(string constraint)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.Check);
        string str = constraint;
        column.Set<string>(expression, 1, str);
      }
    }

    private void AddColumnsForCompositeUserType(string columnPrefix)
    {
      ICompositeUserType instance = (ICompositeUserType) Activator.CreateInstance(this.mapping.Type.GetUnderlyingSystemType());
      if (instance.PropertyNames.Length <= 1)
        return;
      ColumnMapping columnMapping = this.mapping.Columns.Single<ColumnMapping>();
      this.mapping.MakeColumnsEmpty(1);
      foreach (string propertyName in instance.PropertyNames)
      {
        ColumnMapping mapping = columnMapping.Clone();
        mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 1, columnPrefix + propertyName);
        this.mapping.AddColumn(1, mapping);
      }
    }
  }
}
