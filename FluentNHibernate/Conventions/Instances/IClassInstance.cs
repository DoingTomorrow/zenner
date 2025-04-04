// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IClassInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IClassInstance : 
    IClassInspector,
    ILazyLoadInspector,
    IReadOnlyInspector,
    IInspector
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IClassInstance Not { get; }

    IOptimisticLockInstance OptimisticLock { get; }

    ICacheInstance Cache { get; }

    ISchemaActionInstance SchemaAction { get; }

    void Table(string tableName);

    void DynamicInsert();

    void DynamicUpdate();

    void BatchSize(int size);

    void LazyLoad();

    void ReadOnly();

    void Schema(string schema);

    void Where(string where);

    void Subselect(string subselectSql);

    void Proxy<T>();

    void Proxy(Type type);

    void Proxy(string type);

    void ApplyFilter(string name, string condition);

    void ApplyFilter(string name);

    void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new();

    void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new();
  }
}
