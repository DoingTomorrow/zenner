// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IClassInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IClassInspector : ILazyLoadInspector, IReadOnlyInspector, IInspector
  {
    string TableName { get; }

    OptimisticLock OptimisticLock { get; }

    SchemaAction SchemaAction { get; }

    string Schema { get; }

    bool DynamicUpdate { get; }

    bool DynamicInsert { get; }

    int BatchSize { get; }

    bool Abstract { get; }

    string Check { get; }

    object DiscriminatorValue { get; }

    string Name { get; }

    string Persister { get; }

    Polymorphism Polymorphism { get; }

    string Proxy { get; }

    string Where { get; }

    string Subselect { get; }

    bool SelectBeforeUpdate { get; }

    IIdentityInspectorBase Id { get; }

    ICacheInspector Cache { get; }

    IDiscriminatorInspector Discriminator { get; }

    IVersionInspector Version { get; }

    IEnumerable<IAnyInspector> Anys { get; }

    IEnumerable<ICollectionInspector> Collections { get; }

    IEnumerable<IComponentBaseInspector> Components { get; }

    IEnumerable<IJoinInspector> Joins { get; }

    IEnumerable<IOneToOneInspector> OneToOnes { get; }

    IEnumerable<IPropertyInspector> Properties { get; }

    IEnumerable<IManyToOneInspector> References { get; }

    IEnumerable<ISubclassInspectorBase> Subclasses { get; }
  }
}
