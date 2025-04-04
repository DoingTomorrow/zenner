// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Context
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation.Caching;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Planning.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Activation
{
  public class Context : IContext
  {
    private WeakReference cachedScope;

    public IKernel Kernel { get; set; }

    public IRequest Request { get; set; }

    public IBinding Binding { get; set; }

    public IPlan Plan { get; set; }

    public ICollection<IParameter> Parameters { get; set; }

    public Type[] GenericArguments { get; private set; }

    public bool HasInferredGenericArguments { get; private set; }

    public ICache Cache { get; private set; }

    public IPlanner Planner { get; private set; }

    public IPipeline Pipeline { get; private set; }

    public Context(
      IKernel kernel,
      IRequest request,
      IBinding binding,
      ICache cache,
      IPlanner planner,
      IPipeline pipeline)
    {
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      Ensure.ArgumentNotNull((object) request, nameof (request));
      Ensure.ArgumentNotNull((object) binding, nameof (binding));
      Ensure.ArgumentNotNull((object) cache, nameof (cache));
      Ensure.ArgumentNotNull((object) planner, nameof (planner));
      Ensure.ArgumentNotNull((object) pipeline, nameof (pipeline));
      this.Kernel = kernel;
      this.Request = request;
      this.Binding = binding;
      this.Parameters = (ICollection<IParameter>) request.Parameters.Union<IParameter>((IEnumerable<IParameter>) binding.Parameters).ToList<IParameter>();
      this.Cache = cache;
      this.Planner = planner;
      this.Pipeline = pipeline;
      if (!binding.Service.IsGenericTypeDefinition)
        return;
      this.HasInferredGenericArguments = true;
      this.GenericArguments = request.Service.GetGenericArguments();
    }

    public object GetScope()
    {
      if (this.cachedScope == null)
        this.cachedScope = new WeakReference(this.Request.GetScope() ?? this.Binding.GetScope((IContext) this));
      return this.cachedScope.Target;
    }

    public IProvider GetProvider() => this.Binding.GetProvider((IContext) this);

    public object Resolve()
    {
      lock (this.Binding)
      {
        if (this.Request.ActiveBindings.Contains(this.Binding))
          throw new ActivationException(ExceptionFormatter.CyclicalDependenciesDetected((IContext) this));
        object obj = this.Cache.TryGet((IContext) this);
        if (obj != null)
          return obj;
        this.Request.ActiveBindings.Push(this.Binding);
        InstanceReference reference = new InstanceReference()
        {
          Instance = this.GetProvider().Create((IContext) this)
        };
        this.Request.ActiveBindings.Pop();
        if (reference.Instance == null)
        {
          if (!this.Kernel.Settings.AllowNullInjection)
            throw new ActivationException(ExceptionFormatter.ProviderReturnedNull((IContext) this));
          if (this.Plan == null)
            this.Plan = this.Planner.GetPlan(this.Request.Service);
          return (object) null;
        }
        if (this.GetScope() != null)
          this.Cache.Remember((IContext) this, reference);
        if (this.Plan == null)
          this.Plan = this.Planner.GetPlan(reference.Instance.GetType());
        this.Pipeline.Activate((IContext) this, reference);
        return reference.Instance;
      }
    }
  }
}
