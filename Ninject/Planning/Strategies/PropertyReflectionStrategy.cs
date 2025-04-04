// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Strategies.PropertyReflectionStrategy
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Injection;
using Ninject.Planning.Directives;
using Ninject.Selection;
using System;
using System.Reflection;

#nullable disable
namespace Ninject.Planning.Strategies
{
  public class PropertyReflectionStrategy : 
    NinjectComponent,
    IPlanningStrategy,
    INinjectComponent,
    IDisposable
  {
    public ISelector Selector { get; private set; }

    public IInjectorFactory InjectorFactory { get; set; }

    public PropertyReflectionStrategy(ISelector selector, IInjectorFactory injectorFactory)
    {
      Ensure.ArgumentNotNull((object) selector, nameof (selector));
      Ensure.ArgumentNotNull((object) injectorFactory, nameof (injectorFactory));
      this.Selector = selector;
      this.InjectorFactory = injectorFactory;
    }

    public void Execute(IPlan plan)
    {
      Ensure.ArgumentNotNull((object) plan, nameof (plan));
      foreach (PropertyInfo propertyInfo in this.Selector.SelectPropertiesForInjection(plan.Type))
        plan.Add((IDirective) new PropertyInjectionDirective(propertyInfo, this.InjectorFactory.Create(propertyInfo)));
    }
  }
}
