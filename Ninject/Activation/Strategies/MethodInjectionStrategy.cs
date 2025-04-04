// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Strategies.MethodInjectionStrategy
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Activation.Strategies
{
  public class MethodInjectionStrategy : ActivationStrategy
  {
    public override void Activate(IContext context, InstanceReference reference)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      Ensure.ArgumentNotNull((object) reference, nameof (reference));
      foreach (MethodInjectionDirective injectionDirective in context.Plan.GetAll<MethodInjectionDirective>())
      {
        IEnumerable<object> source = ((IEnumerable<ITarget>) injectionDirective.Targets).Select<ITarget, object>((Func<ITarget, object>) (target => target.ResolveWithin(context)));
        injectionDirective.Injector(reference.Instance, source.ToArray<object>());
      }
    }
  }
}
