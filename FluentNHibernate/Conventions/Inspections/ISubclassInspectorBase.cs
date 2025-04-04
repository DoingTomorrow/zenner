// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ISubclassInspectorBase
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface ISubclassInspectorBase : IInspector
  {
    bool Abstract { get; }

    IEnumerable<IAnyInspector> Anys { get; }

    IEnumerable<ICollectionInspector> Collections { get; }

    IEnumerable<IJoinInspector> Joins { get; }

    IEnumerable<IOneToOneInspector> OneToOnes { get; }

    IEnumerable<IPropertyInspector> Properties { get; }

    IEnumerable<IManyToOneInspector> References { get; }

    IEnumerable<ISubclassInspectorBase> Subclasses { get; }

    bool DynamicInsert { get; }

    bool DynamicUpdate { get; }

    Type Extends { get; }

    bool LazyLoad { get; }

    string Name { get; }

    string Proxy { get; }

    bool SelectBeforeUpdate { get; }

    Type Type { get; }
  }
}
