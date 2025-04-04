// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ClassInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class ClassInstance : 
    ClassInspector,
    IClassInstance,
    IClassInspector,
    ILazyLoadInspector,
    IReadOnlyInspector,
    IInspector
  {
    private readonly ClassMapping mapping;
    private bool nextBool = true;

    public ClassInstance(ClassMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IClassInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (IClassInstance) this;
      }
    }

    public ISchemaActionInstance SchemaAction
    {
      get
      {
        return (ISchemaActionInstance) new SchemaActionInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.SchemaAction), 1, value)));
      }
    }

    public void Table(string tableName)
    {
      this.mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.TableName), 1, tableName);
    }

    public void DynamicInsert()
    {
      this.mapping.Set<bool>((Expression<Func<ClassMapping, bool>>) (x => x.DynamicInsert), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void DynamicUpdate()
    {
      this.mapping.Set<bool>((Expression<Func<ClassMapping, bool>>) (x => x.DynamicUpdate), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public IOptimisticLockInstance OptimisticLock
    {
      get
      {
        return (IOptimisticLockInstance) new OptimisticLockInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.OptimisticLock), 1, value)));
      }
    }

    public void BatchSize(int size)
    {
      this.mapping.Set<int>((Expression<Func<ClassMapping, int>>) (x => x.BatchSize), 1, size);
    }

    public void LazyLoad()
    {
      this.mapping.Set<bool>((Expression<Func<ClassMapping, bool>>) (x => x.Lazy), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void ReadOnly()
    {
      this.mapping.Set<bool>((Expression<Func<ClassMapping, bool>>) (x => x.Mutable), 1, (!this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Schema(string schema)
    {
      this.mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.Schema), 1, schema);
    }

    public void Where(string where)
    {
      this.mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.Where), 1, where);
    }

    public void Subselect(string subselectSql)
    {
      this.mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.Subselect), 1, subselectSql);
    }

    public void Proxy<T>() => this.Proxy(typeof (T));

    public void Proxy(Type type) => this.Proxy(type.AssemblyQualifiedName);

    public void Proxy(string type)
    {
      this.mapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.Proxy), 1, type);
    }

    public void ApplyFilter(string name, string condition)
    {
      FilterMapping mapping = new FilterMapping();
      mapping.Set<string>((Expression<Func<FilterMapping, string>>) (x => x.Name), 1, name);
      mapping.Set<string>((Expression<Func<FilterMapping, string>>) (x => x.Condition), 1, condition);
      this.mapping.AddFilter(mapping);
    }

    public void ApplyFilter(string name) => this.ApplyFilter(name, (string) null);

    public void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
    {
      this.ApplyFilter(new TFilter().Name, condition);
    }

    public void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
    {
      this.ApplyFilter<TFilter>((string) null);
    }
  }
}
