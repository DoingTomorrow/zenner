// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.IDiagnosticLogger
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public interface IDiagnosticLogger
  {
    void Flush();

    void FluentMappingDiscovered(Type type);

    void ConventionDiscovered(Type type);

    void LoadedFluentMappingsFromSource(ITypeSource source);

    void LoadedConventionsFromSource(ITypeSource source);

    void AutomappingSkippedType(Type type, string reason);

    void AutomappingCandidateTypes(IEnumerable<Type> types);

    void BeginAutomappingType(Type type);
  }
}
