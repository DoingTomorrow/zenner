// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IdentityInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class IdentityInstance : 
    IdentityInspector,
    IIdentityInstance,
    IIdentityInspector,
    IExposedThroughPropertyInspector,
    IIdentityInspectorBase,
    IInspector
  {
    private readonly IdMapping mapping;
    private bool nextBool = true;

    public IdentityInstance(IdMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public void Column(string columnName)
    {
      ColumnMapping columnMapping = this.mapping.Columns.FirstOrDefault<ColumnMapping>();
      ColumnMapping mapping = columnMapping == null ? new ColumnMapping() : columnMapping.Clone();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 1, columnName);
      this.mapping.AddColumn(1, mapping);
    }

    public void UnsavedValue(string unsavedValue)
    {
      this.mapping.Set((Expression<Func<IdMapping, object>>) (x => x.UnsavedValue), 1, (object) unsavedValue);
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

    public void CustomType(string type)
    {
      this.mapping.Set((Expression<Func<IdMapping, object>>) (x => x.Type), 1, (object) new TypeReference(type));
    }

    public void CustomType(System.Type type)
    {
      this.mapping.Set((Expression<Func<IdMapping, object>>) (x => x.Type), 1, (object) new TypeReference(type));
    }

    public void CustomType<T>() => this.CustomType(typeof (T));

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set((Expression<Func<IdMapping, object>>) (x => x.Access), 1, (object) value)));
      }
    }

    public IGeneratorInstance GeneratedBy
    {
      get
      {
        this.mapping.Set((Expression<Func<IdMapping, object>>) (x => x.Generator), 1, (object) new GeneratorMapping());
        return (IGeneratorInstance) new GeneratorInstance(this.mapping.Generator, this.mapping.Type.GetUnderlyingSystemType());
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IIdentityInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IIdentityInstance) this;
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

    public void UniqueKey(string columns)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.UniqueKey);
        string str = columns;
        column.Set<string>(expression, 1, str);
      }
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

    public void Index(string index)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.Index);
        string str = index;
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

    public void Default(object value)
    {
      foreach (ColumnMapping column in this.mapping.Columns)
      {
        Expression<Func<ColumnMapping, string>> expression = (Expression<Func<ColumnMapping, string>>) (x => x.Default);
        string str = value.ToString();
        column.Set<string>(expression, 1, str);
      }
    }
  }
}
