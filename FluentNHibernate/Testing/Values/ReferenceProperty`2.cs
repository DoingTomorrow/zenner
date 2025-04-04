// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Testing.Values.ReferenceProperty`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;

#nullable disable
namespace FluentNHibernate.Testing.Values
{
  public class ReferenceProperty<T, TProperty>(Accessor property, TProperty propertyValue) : 
    Property<T, TProperty>(property, propertyValue)
  {
    public override void HasRegistered(PersistenceSpecification<T> specification)
    {
      specification.TransactionalSave((object) this.Value);
    }
  }
}
