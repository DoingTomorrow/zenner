// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Testing.Values.Property`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections;

#nullable disable
namespace FluentNHibernate.Testing.Values
{
  public abstract class Property<T>
  {
    public IEqualityComparer EntityEqualityComparer { get; set; }

    public abstract void SetValue(T target);

    public abstract void CheckValue(object target);

    public virtual void HasRegistered(PersistenceSpecification<T> specification)
    {
    }
  }
}
