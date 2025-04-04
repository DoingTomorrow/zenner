// Decompiled with JetBrains decompiler
// Type: Ninject.Selection.Heuristics.SpecificConstructorSelector
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Components;
using Ninject.Planning.Directives;
using System;
using System.Reflection;

#nullable disable
namespace Ninject.Selection.Heuristics
{
  public class SpecificConstructorSelector : 
    NinjectComponent,
    IConstructorScorer,
    INinjectComponent,
    IDisposable
  {
    private readonly ConstructorInfo constructorInfo;

    public SpecificConstructorSelector(ConstructorInfo constructorInfo)
    {
      this.constructorInfo = constructorInfo;
    }

    public virtual int Score(IContext context, ConstructorInjectionDirective directive)
    {
      return !directive.Constructor.Equals((object) this.constructorInfo) ? 0 : 1;
    }
  }
}
