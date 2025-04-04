// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.Resolvers.StandardBindingResolver
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Planning.Bindings.Resolvers
{
  public class StandardBindingResolver : 
    NinjectComponent,
    IBindingResolver,
    INinjectComponent,
    IDisposable
  {
    public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, Type service)
    {
      return bindings[service].ToEnumerable<IBinding>();
    }
  }
}
