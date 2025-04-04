// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IAnyInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IAnyInspector : IAccessInspector, IInspector
  {
    Cascade Cascade { get; }

    IEnumerable<IColumnInspector> IdentifierColumns { get; }

    string IdType { get; }

    bool Insert { get; }

    TypeReference MetaType { get; }

    IEnumerable<IMetaValueInspector> MetaValues { get; }

    string Name { get; }

    IEnumerable<IColumnInspector> TypeColumns { get; }

    bool Update { get; }

    bool LazyLoad { get; }

    bool OptimisticLock { get; }
  }
}
