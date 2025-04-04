// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IPropertyInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IPropertyInspector : 
    IReadOnlyInspector,
    IExposedThroughPropertyInspector,
    IInspector,
    IAccessInspector
  {
    bool Insert { get; }

    bool Update { get; }

    int Length { get; }

    bool Nullable { get; }

    string Formula { get; }

    TypeReference Type { get; }

    string SqlType { get; }

    bool Unique { get; }

    string UniqueKey { get; }

    string Name { get; }

    bool OptimisticLock { get; }

    Generated Generated { get; }

    IEnumerable<IColumnInspector> Columns { get; }

    string Index { get; }

    bool LazyLoad { get; }

    string Check { get; }

    string Default { get; }

    int Precision { get; }

    int Scale { get; }
  }
}
