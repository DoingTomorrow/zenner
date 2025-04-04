// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Strategies.ActivationCacheStrategy
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation.Caching;
using Ninject.Components;
using System;

#nullable disable
namespace Ninject.Activation.Strategies
{
  public class ActivationCacheStrategy : IActivationStrategy, INinjectComponent, IDisposable
  {
    private readonly IActivationCache activationCache;

    public ActivationCacheStrategy(IActivationCache activationCache)
    {
      this.activationCache = activationCache;
    }

    public INinjectSettings Settings { get; set; }

    public void Dispose()
    {
    }

    public void Activate(IContext context, InstanceReference reference)
    {
      this.activationCache.AddActivatedInstance(reference.Instance);
    }

    public void Deactivate(IContext context, InstanceReference reference)
    {
      this.activationCache.AddDeactivatedInstance(reference.Instance);
    }
  }
}
