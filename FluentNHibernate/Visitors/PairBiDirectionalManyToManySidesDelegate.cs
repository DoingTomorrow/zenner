// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Visitors.PairBiDirectionalManyToManySidesDelegate
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Visitors
{
  public delegate void PairBiDirectionalManyToManySidesDelegate(
    CollectionMapping current,
    IEnumerable<CollectionMapping> possibles,
    bool wasResolved);
}
