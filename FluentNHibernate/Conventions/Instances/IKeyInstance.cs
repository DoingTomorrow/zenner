// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IKeyInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IKeyInstance : IKeyInspector, IInspector
  {
    void Column(string columnName);

    void ForeignKey(string constraint);

    void PropertyRef(string property);

    new IEnumerable<IColumnInspector> Columns { get; }

    void CascadeOnDelete();
  }
}
