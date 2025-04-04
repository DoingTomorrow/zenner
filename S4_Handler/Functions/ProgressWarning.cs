// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.ProgressWarning
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler.Functions
{
  public class ProgressWarning
  {
    public string WarningMessage { get; private set; }

    public ProgressWarning(string warningMessage) => this.WarningMessage = warningMessage;

    public override string ToString() => this.WarningMessage;
  }
}
