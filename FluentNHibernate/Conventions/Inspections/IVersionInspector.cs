// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IVersionInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IVersionInspector : IInspector
  {
    string Name { get; }

    Access Access { get; }

    IEnumerable<IColumnInspector> Columns { get; }

    Generated Generated { get; }

    string UnsavedValue { get; }

    TypeReference Type { get; }

    int Length { get; }

    int Precision { get; }

    int Scale { get; }

    bool Nullable { get; }

    bool Unique { get; }

    string UniqueKey { get; }

    string SqlType { get; }

    string Index { get; }

    string Check { get; }

    string Default { get; }
  }
}
