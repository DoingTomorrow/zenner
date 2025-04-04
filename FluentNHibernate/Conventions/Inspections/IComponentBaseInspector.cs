// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IComponentBaseInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IComponentBaseInspector : 
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    IParentInspector Parent { get; }

    bool Insert { get; }

    bool Update { get; }

    IEnumerable<IAnyInspector> Anys { get; }

    IEnumerable<ICollectionInspector> Collections { get; }

    IEnumerable<IComponentBaseInspector> Components { get; }

    string Name { get; }

    bool OptimisticLock { get; }

    bool Unique { get; }

    Type Type { get; }

    TypeReference Class { get; }

    IEnumerable<IOneToOneInspector> OneToOnes { get; }

    IEnumerable<IPropertyInspector> Properties { get; }

    IEnumerable<IManyToOneInspector> References { get; }
  }
}
