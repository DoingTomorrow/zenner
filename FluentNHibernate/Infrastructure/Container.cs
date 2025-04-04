// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Infrastructure.Container
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Infrastructure
{
  public class Container
  {
    private readonly IDictionary<Type, Func<Container, object>> registeredTypes = (IDictionary<Type, Func<Container, object>>) new Dictionary<Type, Func<Container, object>>();

    public void Register<T>(Func<Container, object> instantiateFunc)
    {
      this.registeredTypes[typeof (T)] = instantiateFunc;
    }

    public object Resolve(Type type)
    {
      if (!this.registeredTypes.ContainsKey(type))
        throw new ResolveException(type);
      return this.registeredTypes[type](this);
    }

    public T Resolve<T>() => (T) this.Resolve(typeof (T));
  }
}
