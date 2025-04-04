// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.DiagnosticsConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class DiagnosticsConfiguration
  {
    private readonly IDiagnosticMessageDispatcher dispatcher;
    private readonly Action<IDiagnosticLogger> setLogger;

    public DiagnosticsConfiguration(
      IDiagnosticMessageDispatcher dispatcher,
      Action<IDiagnosticLogger> setLogger)
    {
      this.dispatcher = dispatcher;
      this.setLogger = setLogger;
    }

    public DiagnosticsConfiguration Enable(bool enable)
    {
      if (enable)
        this.Enable();
      else
        this.Disable();
      return this;
    }

    public DiagnosticsConfiguration Enable()
    {
      this.setLogger((IDiagnosticLogger) new DefaultDiagnosticLogger(this.dispatcher));
      return this;
    }

    public DiagnosticsConfiguration Disable()
    {
      this.setLogger((IDiagnosticLogger) new NullDiagnosticsLogger());
      return this;
    }

    public DiagnosticsConfiguration RegisterListener(IDiagnosticListener listener)
    {
      this.dispatcher.RegisterListener(listener);
      return this;
    }

    public DiagnosticsConfiguration OutputToConsole()
    {
      this.RegisterListener((IDiagnosticListener) new ConsoleOutputListener());
      return this;
    }

    public DiagnosticsConfiguration OutputToConsole(IDiagnosticResultsFormatter formatter)
    {
      this.RegisterListener((IDiagnosticListener) new ConsoleOutputListener(formatter));
      return this;
    }

    public DiagnosticsConfiguration OutputToFile(string path)
    {
      this.RegisterListener((IDiagnosticListener) new FileOutputListener(path));
      return this;
    }

    public DiagnosticsConfiguration OutputToFile(IDiagnosticResultsFormatter formatter, string path)
    {
      this.RegisterListener((IDiagnosticListener) new FileOutputListener(formatter, path));
      return this;
    }
  }
}
