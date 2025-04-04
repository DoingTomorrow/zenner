// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IManyToOneInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IManyToOneInspector : 
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    string Name { get; }

    IEnumerable<IColumnInspector> Columns { get; }

    Cascade Cascade { get; }

    TypeReference Class { get; }

    string Formula { get; }

    Fetch Fetch { get; }

    string ForeignKey { get; }

    bool Insert { get; }

    Laziness LazyLoad { get; }

    NotFound NotFound { get; }

    string PropertyRef { get; }

    bool Update { get; }

    bool Nullable { get; }

    bool OptimisticLock { get; }
  }
}
