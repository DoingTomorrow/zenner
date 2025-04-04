// Decompiled with JetBrains decompiler
// Type: S4_Handler.DiagnosticStatusEntry
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System.Text;

#nullable disable
namespace S4_Handler
{
  internal class DiagnosticStatusEntry
  {
    internal uint Last;
    internal uint SecondLast;
    internal uint ThirdLast;
    internal bool threeValueLogger = false;

    internal string ToString(uint statusValue)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.threeValueLogger)
      {
        S4_FunctionalState s4FunctionalState = new S4_FunctionalState((ushort) statusValue);
        stringBuilder.Append(s4FunctionalState.ToString());
      }
      else
      {
        stringBuilder.Append("0x" + statusValue.ToString("x08"));
        if ((statusValue & 16U) > 0U)
          stringBuilder.Append("; NTAG I2C fault");
        if ((statusValue & 32U) > 0U)
          stringBuilder.Append("; Display interpreter error");
        if ((statusValue & 64U) > 0U)
          stringBuilder.Append("; Lo battery calculated");
        if ((statusValue & 128U) > 0U)
          stringBuilder.Append("; Lo battery by voltage monitor");
        if ((statusValue & 256U) > 0U)
          stringBuilder.Append("; Write protection not set");
        if ((statusValue & 512U) > 0U)
          stringBuilder.Append("; Ultrasonic channel 1 corrupt");
        if ((statusValue & 1024U) > 0U)
          stringBuilder.Append("; Ultrasonic channel 2 corrupt");
        if ((statusValue & 8192U) > 0U)
          stringBuilder.Append("; Reverse flow");
        if ((statusValue & 16384U) > 0U)
          stringBuilder.Append("; Temperature out of range");
        if ((statusValue & 32768U) > 0U)
          stringBuilder.Append("; Flow out of range");
        if ((statusValue & 4194304U) > 0U)
          stringBuilder.Append("; Bubbles detected");
        if ((statusValue & 8388608U) > 0U)
          stringBuilder.Append("; Empty tube (or ultrasonic defect)");
        if ((statusValue & 536870912U) > 0U)
          stringBuilder.Append("; Temperature sensor defect");
      }
      return stringBuilder.ToString();
    }
  }
}
