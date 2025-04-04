// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Diagnostics.StringLambdaOutputListener
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;

#nullable disable
namespace FluentNHibernate.Diagnostics
{
  public class StringLambdaOutputListener : IDiagnosticListener
  {
    private readonly Action<string> raiseMessage;
    private IDiagnosticResultsFormatter outputFormatter = (IDiagnosticResultsFormatter) new DefaultOutputFormatter();

    public StringLambdaOutputListener(Action<string> raiseMessage)
    {
      this.raiseMessage = raiseMessage;
    }

    public void Receive(DiagnosticResults results)
    {
      this.raiseMessage(this.outputFormatter.Format(results));
    }

    public void SetFormatter(IDiagnosticResultsFormatter formatter)
    {
      this.outputFormatter = formatter;
    }
  }
}
