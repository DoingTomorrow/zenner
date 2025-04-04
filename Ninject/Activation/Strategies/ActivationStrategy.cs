// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Strategies.ActivationStrategy
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using System;

#nullable disable
namespace Ninject.Activation.Strategies
{
  public abstract class ActivationStrategy : 
    NinjectComponent,
    IActivationStrategy,
    INinjectComponent,
    IDisposable
  {
    public virtual void Activate(IContext context, InstanceReference reference)
    {
    }

    public virtual void Deactivate(IContext context, InstanceReference reference)
    {
    }
  }
}
