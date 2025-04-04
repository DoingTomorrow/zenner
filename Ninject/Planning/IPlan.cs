// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.IPlan
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Planning.Directives;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Planning
{
  public interface IPlan
  {
    Type Type { get; }

    void Add(IDirective directive);

    bool Has<TDirective>() where TDirective : IDirective;

    TDirective GetOne<TDirective>() where TDirective : IDirective;

    IEnumerable<TDirective> GetAll<TDirective>() where TDirective : IDirective;
  }
}
