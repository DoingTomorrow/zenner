// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.DefaultDiagnosticMessageDispatcher
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class DefaultDiagnosticMessageDispatcher : IDiagnosticMessageDispatcher
  {
    private readonly List<IDiagnosticListener> listeners = new List<IDiagnosticListener>();

    public void RegisterListener(IDiagnosticListener listener) => this.listeners.Add(listener);

    public void Publish(DiagnosticResults results)
    {
      foreach (IDiagnosticListener listener in this.listeners)
        listener.Receive(results);
    }
  }
}
