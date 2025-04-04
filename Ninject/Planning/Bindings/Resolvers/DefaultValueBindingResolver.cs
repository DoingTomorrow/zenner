// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.Resolvers.DefaultValueBindingResolver
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Planning.Targets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Planning.Bindings.Resolvers
{
  public class DefaultValueBindingResolver : 
    NinjectComponent,
    IMissingBindingResolver,
    INinjectComponent,
    IDisposable
  {
    public IEnumerable<IBinding> Resolve(Multimap<Type, IBinding> bindings, IRequest request)
    {
      Type service = request.Service;
      if (!DefaultValueBindingResolver.HasDefaultValue(request.Target))
        return Enumerable.Empty<IBinding>();
      return (IEnumerable<IBinding>) new Binding[1]
      {
        new Binding(service)
        {
          Condition = (Func<IRequest, bool>) (r => DefaultValueBindingResolver.HasDefaultValue(r.Target)),
          ProviderCallback = (Func<IContext, IProvider>) (_ => (IProvider) new DefaultValueBindingResolver.DefaultParameterValueProvider(service))
        }
      };
    }

    private static bool HasDefaultValue(ITarget target) => target != null && target.HasDefaultValue;

    private class DefaultParameterValueProvider : IProvider
    {
      public DefaultParameterValueProvider(Type type) => this.Type = type;

      public Type Type { get; private set; }

      public object Create(IContext context) => context.Request.Target?.DefaultValue;
    }
  }
}
