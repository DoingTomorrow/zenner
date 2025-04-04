// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.DiagnosticResults
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class DiagnosticResults
  {
    public DiagnosticResults(
      IEnumerable<ScannedSource> scannedSources,
      IEnumerable<Type> fluentMappings,
      IEnumerable<Type> conventions,
      IEnumerable<SkippedAutomappingType> automappingSkippedTypes,
      IEnumerable<Type> automappingCandidateTypes,
      IEnumerable<AutomappingType> automappingTypes)
    {
      this.FluentMappings = (IEnumerable<Type>) fluentMappings.ToArray<Type>();
      this.ScannedSources = (IEnumerable<ScannedSource>) scannedSources.ToArray<ScannedSource>();
      this.Conventions = (IEnumerable<Type>) conventions.ToArray<Type>();
      this.AutomappingSkippedTypes = (IEnumerable<SkippedAutomappingType>) automappingSkippedTypes.ToArray<SkippedAutomappingType>();
      this.AutomappingCandidateTypes = (IEnumerable<Type>) automappingCandidateTypes.ToArray<Type>();
      this.AutomappedTypes = (IEnumerable<AutomappingType>) automappingTypes.ToArray<AutomappingType>();
    }

    public IEnumerable<Type> FluentMappings { get; private set; }

    public IEnumerable<ScannedSource> ScannedSources { get; private set; }

    public IEnumerable<Type> Conventions { get; private set; }

    public IEnumerable<SkippedAutomappingType> AutomappingSkippedTypes { get; private set; }

    public IEnumerable<Type> AutomappingCandidateTypes { get; private set; }

    public IEnumerable<AutomappingType> AutomappedTypes { get; private set; }
  }
}
