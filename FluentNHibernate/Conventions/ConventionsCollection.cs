// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.ConventionsCollection
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public class ConventionsCollection : IEnumerable<Type>, IEnumerable
  {
    private readonly List<ConventionsCollection.AddedConvention> inner = new List<ConventionsCollection.AddedConvention>();
    private readonly List<Type> types = new List<Type>();

    public IEnumerator<Type> GetEnumerator() => (IEnumerator<Type>) this.types.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool Contains(Type type) => this.types.Contains(type);

    public void Add<T>(T instance) => this.Add(typeof (T), (object) instance);

    public void Add(Type type, object instance)
    {
      ConventionsCollection.AddedConvention addedConvention;
      if (this.Contains(type))
      {
        addedConvention = this.inner.Find((Predicate<ConventionsCollection.AddedConvention>) (x => x.Type == type));
      }
      else
      {
        addedConvention = new ConventionsCollection.AddedConvention(type);
        this.types.Add(type);
        this.inner.Add(addedConvention);
      }
      addedConvention.Instances.Add(instance);
    }

    public IEnumerable<object> this[Type type]
    {
      get
      {
        return this.inner.Find((Predicate<ConventionsCollection.AddedConvention>) (x => x.Type == type)).GetInstances();
      }
    }

    public void Merge(ConventionsCollection conventions)
    {
      conventions.inner.Each<ConventionsCollection.AddedConvention>(new Action<ConventionsCollection.AddedConvention>(this.inner.Add));
      conventions.types.Each<Type>(new Action<Type>(this.types.Add));
    }

    private class AddedConvention
    {
      public IList<object> Instances { get; private set; }

      public Type Type { get; private set; }

      public AddedConvention(Type type)
      {
        this.Type = type;
        this.Instances = (IList<object>) new List<object>();
      }

      public IEnumerable<object> GetInstances()
      {
        return (IEnumerable<object>) new List<object>((IEnumerable<object>) this.Instances);
      }
    }
  }
}
