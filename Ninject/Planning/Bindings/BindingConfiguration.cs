// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.BindingConfiguration
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Parameters;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Planning.Bindings
{
  public class BindingConfiguration : IBindingConfiguration
  {
    public BindingConfiguration()
    {
      this.Metadata = (IBindingMetadata) new BindingMetadata();
      this.Parameters = (ICollection<IParameter>) new List<IParameter>();
      this.ActivationActions = (ICollection<Action<IContext, object>>) new List<Action<IContext, object>>();
      this.DeactivationActions = (ICollection<Action<IContext, object>>) new List<Action<IContext, object>>();
      this.ScopeCallback = StandardScopeCallbacks.Transient;
    }

    public IBindingMetadata Metadata { get; private set; }

    public bool IsImplicit { get; set; }

    public bool IsConditional => this.Condition != null;

    public BindingTarget Target { get; set; }

    public Func<IRequest, bool> Condition { get; set; }

    public Func<IContext, IProvider> ProviderCallback { get; set; }

    public Func<IContext, object> ScopeCallback { get; set; }

    public ICollection<IParameter> Parameters { get; private set; }

    public ICollection<Action<IContext, object>> ActivationActions { get; private set; }

    public ICollection<Action<IContext, object>> DeactivationActions { get; private set; }

    public IProvider GetProvider(IContext context)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      return this.ProviderCallback(context);
    }

    public object GetScope(IContext context)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      return this.ScopeCallback(context);
    }

    public bool Matches(IRequest request)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      return this.Condition == null || this.Condition(request);
    }
  }
}
