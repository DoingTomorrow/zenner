// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Plan
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Planning.Directives;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Planning
{
  public class Plan : IPlan
  {
    public Type Type { get; private set; }

    public ICollection<IDirective> Directives { get; private set; }

    public Plan(Type type)
    {
      Ensure.ArgumentNotNull((object) type, nameof (type));
      this.Type = type;
      this.Directives = (ICollection<IDirective>) new List<IDirective>();
    }

    public void Add(IDirective directive)
    {
      Ensure.ArgumentNotNull((object) directive, nameof (directive));
      this.Directives.Add(directive);
    }

    public bool Has<TDirective>() where TDirective : IDirective
    {
      return this.GetAll<TDirective>().Count<TDirective>() > 0;
    }

    public TDirective GetOne<TDirective>() where TDirective : IDirective
    {
      return this.GetAll<TDirective>().SingleOrDefault<TDirective>();
    }

    public IEnumerable<TDirective> GetAll<TDirective>() where TDirective : IDirective
    {
      return this.Directives.OfType<TDirective>();
    }
  }
}
