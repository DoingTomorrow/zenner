// Decompiled with JetBrains decompiler
// Type: Ninject.Selection.Heuristics.IConstructorScorer
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Components;
using Ninject.Planning.Directives;
using System;

#nullable disable
namespace Ninject.Selection.Heuristics
{
  public interface IConstructorScorer : INinjectComponent, IDisposable
  {
    int Score(IContext context, ConstructorInjectionDirective directive);
  }
}
