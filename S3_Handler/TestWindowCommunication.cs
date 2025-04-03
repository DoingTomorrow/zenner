// Decompiled with JetBrains decompiler
// Type: S3_Handler.TestWindowCommunication
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.Windows.Forms;

#nullable disable
namespace S3_Handler
{
  internal class TestWindowCommunication
  {
    internal TextBox textBoxState;
    internal bool breakRequest;
    internal bool singleStepOn;
    internal bool continueSingleStep;

    internal TestWindowCommunication(TextBox textBoxState) => this.textBoxState = textBoxState;
  }
}
