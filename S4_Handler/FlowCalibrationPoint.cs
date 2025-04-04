// Decompiled with JetBrains decompiler
// Type: S4_Handler.FlowCalibrationPoint
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

#nullable disable
namespace S4_Handler
{
  public class FlowCalibrationPoint
  {
    public double Flow_Qm { get; set; }

    public double Error_Percent { get; set; }

    public FlowCalibrationPoint()
    {
    }

    public FlowCalibrationPoint(double flow_Qm, double error_Percent)
    {
      this.Flow_Qm = flow_Qm;
      this.Error_Percent = error_Percent;
    }
  }
}
