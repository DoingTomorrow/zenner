// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.CombinedAssemblyTypeSource
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace FluentNHibernate
{
  public class CombinedAssemblyTypeSource : ITypeSource
  {
    private readonly AssemblyTypeSource[] sources;

    public CombinedAssemblyTypeSource(IEnumerable<Assembly> sources)
      : this(sources.Select<Assembly, AssemblyTypeSource>((Func<Assembly, AssemblyTypeSource>) (x => new AssemblyTypeSource(x))))
    {
    }

    public CombinedAssemblyTypeSource(IEnumerable<AssemblyTypeSource> sources)
    {
      this.sources = sources.ToArray<AssemblyTypeSource>();
    }

    public IEnumerable<Type> GetTypes()
    {
      return (IEnumerable<Type>) ((IEnumerable<AssemblyTypeSource>) this.sources).SelectMany<AssemblyTypeSource, Type>((Func<AssemblyTypeSource, IEnumerable<Type>>) (x => x.GetTypes())).ToArray<Type>();
    }

    public void LogSource(IDiagnosticLogger logger)
    {
      foreach (AssemblyTypeSource source in this.sources)
        source.LogSource(logger);
    }

    public string GetIdentifier() => "Combined source";
  }
}
