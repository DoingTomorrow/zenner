// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.VersionInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class VersionInstance : VersionInspector, IVersionInstance, IVersionInspector, IInspector
  {
    private const int layer = 1;
    private readonly VersionMapping mapping;
    private bool nextBool = true;

    public VersionInstance(VersionMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<VersionMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public IGeneratedInstance Generated
    {
      get
      {
        return (IGeneratedInstance) new GeneratedInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<VersionMapping, string>>) (x => x.Generated), 1, value)));
      }
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IVersionInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IVersionInstance) this;
      }
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
      this.mapping.Set<string>((Expression<Func<VersionMapping, string>>) (x => x.UnsavedValue), 1, unsavedValue);
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

    public void CustomType(string type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<VersionMapping, TypeReference>>) (x => x.Type), 1, new TypeReference(type));
    }

    public void CustomType(System.Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<VersionMapping, TypeReference>>) (x => x.Type), 1, new TypeReference(type));
    }

    public void CustomType<T>() => this.CustomType(typeof (T));
  }
}
