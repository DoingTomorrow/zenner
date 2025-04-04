// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.Resolvers.SelfBindingResolver
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Activation.Providers;
using Ninject.Components;
using Ninject.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Planning.Bindings.Resolvers
{
  public class SelfBindingResolver : 
    NinjectComponent,
    IMissingBindingResolver,
    INinjectComponent,
    IDisposable
  {
    public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, IRequest request)
    {
      Type service = request.Service;
      if (!this.TypeIsSelfBindable(service))
        return Enumerable.Empty<IBinding>();
      return (IEnumerable<IBinding>) new Binding[1]
      {
        new Binding(service)
        {
          ProviderCallback = StandardProvider.GetCreationCallback(service)
        }
      };
    }

    protected virtual bool TypeIsSelfBindable(Type service)
    {
      return !service.IsInterface && !service.IsAbstract && !service.IsValueType && service != typeof (string) && !service.ContainsGenericParameters;
    }
  }
}
