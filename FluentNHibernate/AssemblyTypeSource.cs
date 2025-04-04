// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.AssemblyTypeSource
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
  public class AssemblyTypeSource : ITypeSource
  {
    private readonly Assembly source;

    public AssemblyTypeSource(Assembly source)
    {
      this.source = source != null ? source : throw new ArgumentNullException(nameof (source));
    }

    public IEnumerable<Type> GetTypes()
    {
      return (IEnumerable<Type>) ((IEnumerable<Type>) this.source.GetTypes()).OrderBy<Type, string>((Func<Type, string>) (x => x.FullName));
    }

    public void LogSource(IDiagnosticLogger logger)
    {
      if (logger == null)
        throw new ArgumentNullException(nameof (logger));
      logger.LoadedFluentMappingsFromSource((ITypeSource) this);
    }

    public string GetIdentifier() => this.source.GetName().FullName;

    public override int GetHashCode() => this.source.GetHashCode();
  }
}
