// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ICompositeIdentityInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface ICompositeIdentityInspector : IIdentityInspectorBase, IInspector
  {
    TypeReference Class { get; }

    IEnumerable<IKeyManyToOneInspector> KeyManyToOnes { get; }

    IEnumerable<IKeyPropertyInspector> KeyProperties { get; }

    bool Mapped { get; }
  }
}
