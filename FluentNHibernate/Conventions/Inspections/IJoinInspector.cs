// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.IJoinInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public interface IJoinInspector : IInspector
  {
    IEnumerable<IAnyInspector> Anys { get; }

    Fetch Fetch { get; }

    bool Inverse { get; }

    IKeyInspector Key { get; }

    bool Optional { get; }

    IEnumerable<IPropertyInspector> Properties { get; }

    IEnumerable<IManyToOneInspector> References { get; }

    IEnumerable<ICollectionInspector> Collections { get; }

    string Schema { get; }

    string TableName { get; }

    string Catalog { get; }

    string Subselect { get; }
  }
}
