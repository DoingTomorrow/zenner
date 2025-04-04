// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Providers.StandardProvider
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Parameters;
using Ninject.Planning;
using Ninject.Planning.Directives;
using Ninject.Planning.Targets;
using Ninject.Selection;
using Ninject.Selection.Heuristics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Activation.Providers
{
  public class StandardProvider : IProvider
  {
    public Type Type { get; private set; }

    public IPlanner Planner { get; private set; }

    public IConstructorScorer ConstructorScorer { get; private set; }

    public StandardProvider(Type type, IPlanner planner, IConstructorScorer constructorScorer)
    {
      Ensure.ArgumentNotNull((object) type, nameof (type));
      Ensure.ArgumentNotNull((object) planner, nameof (planner));
      Ensure.ArgumentNotNull((object) constructorScorer, nameof (constructorScorer));
      this.Type = type;
      this.Planner = planner;
      this.ConstructorScorer = constructorScorer;
    }

    public virtual object Create(IContext context)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      if (context.Plan == null)
        context.Plan = this.Planner.GetPlan(this.GetImplementationType(context.Request.Service));
      if (!context.Plan.Has<ConstructorInjectionDirective>())
        throw new ActivationException(ExceptionFormatter.NoConstructorsAvailable(context));
      IGrouping<int, ConstructorInjectionDirective> grouping = context.Plan.GetAll<ConstructorInjectionDirective>().GroupBy<ConstructorInjectionDirective, int>((Func<ConstructorInjectionDirective, int>) (option => this.ConstructorScorer.Score(context, option))).OrderByDescending<IGrouping<int, ConstructorInjectionDirective>, int>((Func<IGrouping<int, ConstructorInjectionDirective>, int>) (g => g.Key)).First<IGrouping<int, ConstructorInjectionDirective>>();
      ConstructorInjectionDirective injectionDirective = !grouping.Skip<ConstructorInjectionDirective>(1).Any<ConstructorInjectionDirective>() ? grouping.Single<ConstructorInjectionDirective>() : throw new ActivationException(ExceptionFormatter.ConstructorsAmbiguous(context, grouping));
      object[] array = ((IEnumerable<ITarget>) injectionDirective.Targets).Select<ITarget, object>((Func<ITarget, object>) (target => this.GetValue(context, target))).ToArray<object>();
      return injectionDirective.Injector(array);
    }

    public object GetValue(IContext context, ITarget target)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      Ensure.ArgumentNotNull((object) target, nameof (target));
      IConstructorArgument constructorArgument = context.Parameters.OfType<IConstructorArgument>().Where<IConstructorArgument>((Func<IConstructorArgument, bool>) (p => p.AppliesToTarget(context, target))).SingleOrDefault<IConstructorArgument>();
      return constructorArgument == null ? target.ResolveWithin(context) : constructorArgument.GetValue(context, target);
    }

    public Type GetImplementationType(Type service)
    {
      Ensure.ArgumentNotNull((object) service, nameof (service));
      return !this.Type.ContainsGenericParameters ? this.Type : this.Type.MakeGenericType(service.GetGenericArguments());
    }

    public static Func<IContext, IProvider> GetCreationCallback(Type prototype)
    {
      Ensure.ArgumentNotNull((object) prototype, nameof (prototype));
      return (Func<IContext, IProvider>) (ctx => (IProvider) new StandardProvider(prototype, ctx.Kernel.Components.Get<IPlanner>(), ctx.Kernel.Components.Get<ISelector>().ConstructorScorer));
    }

    public static Func<IContext, IProvider> GetCreationCallback(
      Type prototype,
      ConstructorInfo constructor)
    {
      Ensure.ArgumentNotNull((object) prototype, nameof (prototype));
      return (Func<IContext, IProvider>) (ctx => (IProvider) new StandardProvider(prototype, ctx.Kernel.Components.Get<IPlanner>(), (IConstructorScorer) new SpecificConstructorSelector(constructor)));
    }
  }
}
