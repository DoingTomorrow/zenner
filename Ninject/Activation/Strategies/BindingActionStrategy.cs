// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Strategies.BindingActionStrategy
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using System;

#nullable disable
namespace Ninject.Activation.Strategies
{
  public class BindingActionStrategy : ActivationStrategy
  {
    public override void Activate(IContext context, InstanceReference reference)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      context.Binding.ActivationActions.Map<Action<IContext, object>>((Action<Action<IContext, object>>) (action => action(context, reference.Instance)));
    }

    public override void Deactivate(IContext context, InstanceReference reference)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      context.Binding.DeactivationActions.Map<Action<IContext, object>>((Action<Action<IContext, object>>) (action => action(context, reference.Instance)));
    }
  }
}
