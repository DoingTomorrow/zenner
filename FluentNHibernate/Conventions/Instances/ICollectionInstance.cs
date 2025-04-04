// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ICollectionInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface ICollectionInstance : ICollectionInspector, IInspector
  {
    IKeyInstance Key { get; }

    IIndexInstanceBase Index { get; }

    IElementInstance Element { get; }

    IRelationshipInstance Relationship { get; }

    void Table(string tableName);

    void Name(string name);

    void Schema(string schema);

    void LazyLoad();

    void ExtraLazyLoad();

    void BatchSize(int batchSize);

    void ReadOnly();

    void AsArray();

    void AsBag();

    void AsList();

    void AsMap();

    void AsSet();

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    ICollectionInstance Not { get; }

    IAccessInstance Access { get; }

    ICacheInstance Cache { get; }

    ICollectionCascadeInstance Cascade { get; }

    IFetchInstance Fetch { get; }

    void OptimisticLock();

    void Check(string constraint);

    void CollectionType<T>();

    void CollectionType(string type);

    void CollectionType(Type type);

    void Generic();

    void Inverse();

    void Persister<T>();

    void Where(string whereClause);

    void OrderBy(string orderBy);

    void Sort(string sort);

    void Subselect(string subselect);

    void KeyNullable();
  }
}
