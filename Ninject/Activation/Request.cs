// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Request
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Planning.Targets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Activation
{
  public class Request : IRequest
  {
    public Type Service { get; private set; }

    public IRequest ParentRequest { get; private set; }

    public IContext ParentContext { get; private set; }

    public ITarget Target { get; private set; }

    public Func<IBindingMetadata, bool> Constraint { get; private set; }

    public ICollection<IParameter> Parameters { get; private set; }

    public Stack<IBinding> ActiveBindings { get; private set; }

    public int Depth { get; private set; }

    public bool IsOptional { get; set; }

    public bool IsUnique { get; set; }

    public Func<object> ScopeCallback { get; private set; }

    public Request(
      Type service,
      Func<IBindingMetadata, bool> constraint,
      IEnumerable<IParameter> parameters,
      Func<object> scopeCallback,
      bool isOptional,
      bool isUnique)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      Ensure.ArgumentNotNull((object) parameters, nameof (parameters));
      this.Service = service;
      this.Constraint = constraint;
      this.Parameters = (ICollection<IParameter>) parameters.ToList<IParameter>();
      this.ScopeCallback = scopeCallback;
      this.ActiveBindings = new Stack<IBinding>();
      this.Depth = 0;
      this.IsOptional = isOptional;
      this.IsUnique = isUnique;
    }

    public Request(
      IContext parentContext,
      Type service,
      ITarget target,
      Func<object> scopeCallback)
    {
      Ensure.ArgumentNotNull((object) parentContext, nameof (parentContext));
      Ensure.ArgumentNotNull((object) service, nameof (service));
      Ensure.ArgumentNotNull((object) target, nameof (target));
      this.ParentContext = parentContext;
      this.ParentRequest = parentContext.Request;
      this.Service = service;
      this.Target = target;
      this.Constraint = target.Constraint;
      this.IsOptional = target.IsOptional;
      this.Parameters = (ICollection<IParameter>) parentContext.Parameters.Where<IParameter>((Func<IParameter, bool>) (p => p.ShouldInherit)).ToList<IParameter>();
      this.ScopeCallback = scopeCallback;
      this.ActiveBindings = new Stack<IBinding>((IEnumerable<IBinding>) this.ParentRequest.ActiveBindings);
      this.Depth = this.ParentRequest.Depth + 1;
    }

    public bool Matches(IBinding binding)
    {
      return this.Constraint == null || this.Constraint(binding.Metadata);
    }

    public object GetScope() => this.ScopeCallback != null ? this.ScopeCallback() : (object) null;

    public IRequest CreateChild(Type service, IContext parentContext, ITarget target)
    {
      return (IRequest) new Request(parentContext, service, target, this.ScopeCallback);
    }
  }
}
