// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Pipeline
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation.Caching;
using Ninject.Activation.Strategies;
using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Activation
{
  public class Pipeline : NinjectComponent, IPipeline, INinjectComponent, IDisposable
  {
    private readonly IActivationCache activationCache;

    public Pipeline(IEnumerable<IActivationStrategy> strategies, IActivationCache activationCache)
    {
      Ensure.ArgumentNotNull((object) strategies, nameof (strategies));
      this.Strategies = (IList<IActivationStrategy>) strategies.ToList<IActivationStrategy>();
      this.activationCache = activationCache;
    }

    public IList<IActivationStrategy> Strategies { get; private set; }

    public void Activate(IContext context, InstanceReference reference)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      if (this.activationCache.IsActivated(reference.Instance))
        return;
      this.Strategies.Map<IActivationStrategy>((Action<IActivationStrategy>) (s => s.Activate(context, reference)));
    }

    public void Deactivate(IContext context, InstanceReference reference)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      if (this.activationCache.IsDeactivated(reference.Instance))
        return;
      this.Strategies.Map<IActivationStrategy>((Action<IActivationStrategy>) (s => s.Deactivate(context, reference)));
    }
  }
}
