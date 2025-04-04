// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.DefaultDiagnosticLogger
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class DefaultDiagnosticLogger : IDiagnosticLogger
  {
    private readonly IDiagnosticMessageDispatcher dispatcher;
    private readonly List<ScannedSource> scannedSources = new List<ScannedSource>();
    private readonly List<Type> classMaps = new List<Type>();
    private readonly List<Type> conventions = new List<Type>();
    private readonly List<SkippedAutomappingType> automappingSkippedTypes = new List<SkippedAutomappingType>();
    private readonly List<Type> automappingCandidateTypes = new List<Type>();
    private readonly List<AutomappingType> automappingTypes = new List<AutomappingType>();
    private bool isDirty;

    public DefaultDiagnosticLogger(IDiagnosticMessageDispatcher dispatcher)
    {
      this.dispatcher = dispatcher;
    }

    public void Flush()
    {
      if (!this.isDirty)
        return;
      this.dispatcher.Publish(this.BuildResults());
    }

    private DiagnosticResults BuildResults()
    {
      return new DiagnosticResults((IEnumerable<ScannedSource>) this.scannedSources, (IEnumerable<Type>) this.classMaps, (IEnumerable<Type>) this.conventions, (IEnumerable<SkippedAutomappingType>) this.automappingSkippedTypes, (IEnumerable<Type>) this.automappingCandidateTypes, (IEnumerable<AutomappingType>) this.automappingTypes);
    }

    public void FluentMappingDiscovered(Type type)
    {
      this.isDirty = true;
      this.classMaps.Add(type);
    }

    public void ConventionDiscovered(Type type)
    {
      this.isDirty = true;
      this.conventions.Add(type);
    }

    public void LoadedFluentMappingsFromSource(ITypeSource source)
    {
      this.isDirty = true;
      this.scannedSources.Add(new ScannedSource()
      {
        Identifier = source.GetIdentifier(),
        Phase = ScanPhase.FluentMappings
      });
    }

    public void LoadedConventionsFromSource(ITypeSource source)
    {
      this.isDirty = true;
      this.scannedSources.Add(new ScannedSource()
      {
        Identifier = source.GetIdentifier(),
        Phase = ScanPhase.Conventions
      });
    }

    public void AutomappingSkippedType(Type type, string reason)
    {
      this.isDirty = true;
      this.automappingSkippedTypes.Add(new SkippedAutomappingType()
      {
        Type = type,
        Reason = reason
      });
    }

    public void AutomappingCandidateTypes(IEnumerable<Type> types)
    {
      this.isDirty = true;
      this.automappingCandidateTypes.AddRange(types);
    }

    public void BeginAutomappingType(Type type)
    {
      this.isDirty = true;
      this.automappingTypes.Add(new AutomappingType()
      {
        Type = type
      });
    }
  }
}
