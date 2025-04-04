// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Blocks.ActivationBlock
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Syntax;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Activation.Blocks
{
  public class ActivationBlock : 
    DisposableObject,
    IActivationBlock,
    IResolutionRoot,
    IFluentSyntax,
    INotifyWhenDisposed,
    IDisposableObject,
    IDisposable
  {
    public IResolutionRoot Parent { get; private set; }

    public event EventHandler Disposed;

    public ActivationBlock(IResolutionRoot parent)
    {
      Ensure.ArgumentNotNull((object) parent, nameof (parent));
      this.Parent = parent;
    }

    public override void Dispose(bool disposing)
    {
      lock (this)
      {
        if (disposing && !this.IsDisposed)
        {
          EventHandler disposed = this.Disposed;
          if (disposed != null)
            disposed((object) this, EventArgs.Empty);
          this.Disposed = (EventHandler) null;
        }
        base.Dispose(disposing);
      }
    }

    public bool CanResolve(IRequest request)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      return this.Parent.CanResolve(request);
    }

    public bool CanResolve(IRequest request, bool ignoreImplicitBindings)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      return this.Parent.CanResolve(request, ignoreImplicitBindings);
    }

    public IEnumerable<object> Resolve(IRequest request)
    {
      Ensure.ArgumentNotNull((object) request, nameof (request));
      return this.Parent.Resolve(request);
    }

    public virtual IRequest CreateRequest(
      Type service,
      Func<IBindingMetadata, bool> constraint,
      IEnumerable<IParameter> parameters,
      bool isOptional,
      bool isUnique)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      Ensure.ArgumentNotNull((object) parameters, nameof (parameters));
      return (IRequest) new Request(service, constraint, parameters, (Func<object>) (() => (object) this), isOptional, isUnique);
    }

    Type IFluentSyntax.GetType() => this.GetType();
  }
}
