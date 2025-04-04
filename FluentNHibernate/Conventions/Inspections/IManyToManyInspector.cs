// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IManyToManyInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IManyToManyInspector : IRelationshipInspector, IInspector
  {
    IEnumerable<IColumnInspector> Columns { get; }

    Type ChildType { get; }

    Fetch Fetch { get; }

    string ForeignKey { get; }

    bool LazyLoad { get; }

    NotFound NotFound { get; }

    Type ParentType { get; }

    string Where { get; }

    string OrderBy { get; }
  }
}
