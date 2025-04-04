// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.NullDiagnosticsLogger
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class NullDiagnosticsLogger : IDiagnosticLogger
  {
    public void Flush()
    {
    }

    public void FluentMappingDiscovered(Type type)
    {
    }

    public void ConventionDiscovered(Type type)
    {
    }

    public void LoadedFluentMappingsFromSource(ITypeSource source)
    {
    }

    public void LoadedConventionsFromSource(ITypeSource source)
    {
    }

    public void AutomappingSkippedType(Type type, string reason)
    {
    }

    public void AutomappingCandidateTypes(IEnumerable<Type> types)
    {
    }

    public void BeginAutomappingType(Type type)
    {
    }
  }
}
