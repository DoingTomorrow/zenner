// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.IDiagnosticMessageDispatcher
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public interface IDiagnosticMessageDispatcher
  {
    void RegisterListener(IDiagnosticListener listener);

    void Publish(DiagnosticResults results);
  }
}
