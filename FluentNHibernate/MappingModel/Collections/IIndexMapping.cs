// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.IIndexMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Visitors;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.MappingModel.Collections
{
  public interface IIndexMapping
  {
    void AcceptVisitor(IMappingModelVisitor visitor);

    IEnumerable<ColumnMapping> Columns { get; }
  }
}
