// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Testing.Values.ReferenceList`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Testing.Values
{
  public class ReferenceList<T, TListElement>(Accessor property, IEnumerable<TListElement> value) : 
    List<T, TListElement>(property, value)
  {
    public override void HasRegistered(PersistenceSpecification<T> specification)
    {
      foreach (TListElement propertyValue in this.Expected)
        specification.TransactionalSave((object) propertyValue);
    }
  }
}
