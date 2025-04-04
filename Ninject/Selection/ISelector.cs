// Decompiled with JetBrains decompiler
// Type: Ninject.Selection.ISelector
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Selection.Heuristics;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Ninject.Selection
{
  public interface ISelector : INinjectComponent, IDisposable
  {
    IConstructorScorer ConstructorScorer { get; set; }

    ICollection<IInjectionHeuristic> InjectionHeuristics { get; }

    IEnumerable<ConstructorInfo> SelectConstructorsForInjection(Type type);

    IEnumerable<PropertyInfo> SelectPropertiesForInjection(Type type);

    IEnumerable<MethodInfo> SelectMethodsForInjection(Type type);
  }
}
