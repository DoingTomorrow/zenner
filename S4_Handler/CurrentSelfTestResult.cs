// Decompiled with JetBrains decompiler
// Type: S4_Handler.CurrentSelfTestResult
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System.Text;

#nullable disable
namespace S4_Handler
{
  public class CurrentSelfTestResult
  {
    public readonly string AllUnits = "µA";
    public int TDC2_Pure_Current;
    public int TDC1_Pure_Current;
    public int TDC_StandBy_Current;
    public int TDC2_activ;
    public int TDC1_activ;
    public int Controller_Current;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("TDC 2 Pure current: " + this.TDC2_Pure_Current.ToString() + this.AllUnits);
      stringBuilder.AppendLine("TDC 1 Pure current: " + this.TDC1_Pure_Current.ToString() + this.AllUnits);
      stringBuilder.AppendLine("TDC standby current: " + this.TDC_StandBy_Current.ToString() + this.AllUnits);
      stringBuilder.AppendLine("TDC 2 activ current: " + this.TDC2_activ.ToString() + this.AllUnits);
      stringBuilder.AppendLine("TDC 1 activ current: " + this.TDC1_activ.ToString() + this.AllUnits);
      stringBuilder.AppendLine("Controller current: " + this.Controller_Current.ToString() + this.AllUnits);
      return stringBuilder.ToString();
    }
  }
}
