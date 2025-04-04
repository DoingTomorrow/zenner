// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.Binding
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
  public class Binding : IBinding, IBindingConfiguration
  {
    public Binding(Type service)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      this.Service = service;
      this.BindingConfiguration = (IBindingConfiguration) new Ninject.Planning.Bindings.BindingConfiguration();
    }

    public Binding(Type service, IBindingConfiguration configuration)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      Ensure.ArgumentNotNull((object) configuration, nameof (configuration));
      this.Service = service;
      this.BindingConfiguration = configuration;
    }

    public IBindingConfiguration BindingConfiguration { get; private set; }

    public Type Service { get; private set; }

    public IBindingMetadata Metadata => this.BindingConfiguration.Metadata;

    public BindingTarget Target
    {
      get => this.BindingConfiguration.Target;
      set => this.BindingConfiguration.Target = value;
    }

    public bool IsImplicit
    {
      get => this.BindingConfiguration.IsImplicit;
      set => this.BindingConfiguration.IsImplicit = value;
    }

    public bool IsConditional => this.BindingConfiguration.IsConditional;

    public Func<IRequest, bool> Condition
    {
      get => this.BindingConfiguration.Condition;
      set => this.BindingConfiguration.Condition = value;
    }

    public Func<IContext, IProvider> ProviderCallback
    {
      get => this.BindingConfiguration.ProviderCallback;
      set => this.BindingConfiguration.ProviderCallback = value;
    }

    public Func<IContext, object> ScopeCallback
    {
      get => this.BindingConfiguration.ScopeCallback;
      set => this.BindingConfiguration.ScopeCallback = value;
    }

    public ICollection<IParameter> Parameters => this.BindingConfiguration.Parameters;

    public ICollection<Action<IContext, object>> ActivationActions
    {
      get => this.BindingConfiguration.ActivationActions;
    }

    public ICollection<Action<IContext, object>> DeactivationActions
    {
      get => this.BindingConfiguration.DeactivationActions;
    }

    public IProvider GetProvider(IContext context)
    {
      return this.BindingConfiguration.GetProvider(context);
    }

    public object GetScope(IContext context) => this.BindingConfiguration.GetScope(context);

    public bool Matches(IRequest request) => this.BindingConfiguration.Matches(request);
  }
}
