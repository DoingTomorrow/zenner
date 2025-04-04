// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.IBindingConfiguration
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Parameters;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Planning.Bindings
{
  public interface IBindingConfiguration
  {
    IBindingMetadata Metadata { get; }

    BindingTarget Target { get; set; }

    bool IsImplicit { get; set; }

    bool IsConditional { get; }

    Func<IRequest, bool> Condition { get; set; }

    Func<IContext, IProvider> ProviderCallback { get; set; }

    Func<IContext, object> ScopeCallback { get; set; }

    ICollection<IParameter> Parameters { get; }

    ICollection<Action<IContext, object>> ActivationActions { get; }

    ICollection<Action<IContext, object>> DeactivationActions { get; }

    IProvider GetProvider(IContext context);

    object GetScope(IContext context);

    bool Matches(IRequest request);
  }
}
