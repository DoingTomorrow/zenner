// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ICollectionInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface ICollectionInspector : IInspector
  {
    IKeyInspector Key { get; }

    IIndexInspectorBase Index { get; }

    string Sort { get; }

    string TableName { get; }

    bool IsMethodAccess { get; }

    MemberInfo Member { get; }

    IRelationshipInspector Relationship { get; }

    Cascade Cascade { get; }

    Fetch Fetch { get; }

    bool OptimisticLock { get; }

    bool Generic { get; }

    bool Inverse { get; }

    Access Access { get; }

    int BatchSize { get; }

    ICacheInspector Cache { get; }

    string Check { get; }

    Type ChildType { get; }

    TypeReference CollectionType { get; }

    ICompositeElementInspector CompositeElement { get; }

    IElementInspector Element { get; }

    Lazy LazyLoad { get; }

    string Name { get; }

    TypeReference Persister { get; }

    string Schema { get; }

    string Where { get; }

    string OrderBy { get; }

    Collection Collection { get; }
  }
}
