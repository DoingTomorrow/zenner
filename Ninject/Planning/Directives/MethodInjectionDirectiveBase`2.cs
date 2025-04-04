// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Directives.MethodInjectionDirectiveBase`2
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Planning.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Planning.Directives
{
  public abstract class MethodInjectionDirectiveBase<TMethod, TInjector> : IDirective where TMethod : MethodBase
  {
    public TInjector Injector { get; private set; }

    public ITarget[] Targets { get; private set; }

    protected MethodInjectionDirectiveBase(TMethod method, TInjector injector)
    {
      Ensure.ArgumentNotNull((object) method, nameof (method));
      Ensure.ArgumentNotNull((object) injector, nameof (injector));
      this.Injector = injector;
      this.Targets = this.CreateTargetsFromParameters(method);
    }

    protected virtual ITarget[] CreateTargetsFromParameters(TMethod method)
    {
      return (ITarget[]) ((IEnumerable<ParameterInfo>) method.GetParameters()).Select<ParameterInfo, ParameterTarget>((Func<ParameterInfo, ParameterTarget>) (parameter => new ParameterTarget((MethodBase) method, parameter))).ToArray<ParameterTarget>();
    }
  }
}
