// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3P_Sta_Status
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

#nullable disable
namespace S3_Handler
{
  internal class S3P_Sta_Status
  {
    private S3_Parameter myParameter;
    private ushort Sta_Status;
    private const ushort TdcStatusMask = 240;

    internal S3P_Sta_Status(S3_Meter myMeter)
    {
      this.myParameter = myMeter.MyParameters.ParameterByName[S3_ParameterNames.Sta_Status.ToString()];
      this.Sta_Status = this.myParameter.GetUshortValue();
    }

    public bool Reset()
    {
      this.Sta_Status = (ushort) 0;
      return this.myParameter.SetUshortValue(this.Sta_Status);
    }

    public UltrasonicStateEnum UltrasonicState
    {
      get => (UltrasonicStateEnum) (((int) this.Sta_Status & 240) >> 4);
    }
  }
}
