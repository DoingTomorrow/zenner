// Decompiled with JetBrains decompiler
// Type: S3_Handler.S3P_Device_Setup_2
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

#nullable disable
namespace S3_Handler
{
  internal class S3P_Device_Setup_2
  {
    private S3_Parameter myParameter;
    private ushort Device_Setup_2;
    private const ushort FlowBit = 1;
    private const ushort VolumeFactorMatrix = 160;
    internal static ushort DEVICE_SETUP2_PT_Mask = 6;
    internal static ushort DEVICE_SETUP2_PT1000 = 0;
    internal static ushort DEVICE_SETUP2_PT500 = 2;
    internal static ushort DEVICE_SETUP2_PT100 = 4;
    internal static ushort DEVICE_SETUP2_VM_Mask = 73;
    internal static ushort DEVICE_SETUP2_FLOW_LINE = 1;
    internal static ushort DEVICE_SETUP2_FLOW_LINE_NOT_SELECTED = 8;
    internal static ushort DEVICE_SETUP2_CHANGE_TEMP_IF_FLOW_LINE_SET = 64;
    internal static ushort DEVICE_SETUP2_TDC_MatrixMask = 160;
    internal static ushort DEVICE_SETUP2_TDC_PW_OFFSET_ADJUST_TEST = 32;
    internal static ushort DEVICE_SETUP2_TDC_MAP_INTERPOLATION = 128;
    internal static ushort DEVICE_SETUP2_TDC_ErrorBits = 16;
    internal static ushort DEVICE_SETUP2_TDC_TSUM_TO_C = 16;

    internal S3P_Device_Setup_2(S3_Meter myMeter)
    {
      this.myParameter = myMeter.MyParameters.ParameterByName[S3_ParameterNames.Device_Setup_2.ToString()];
      this.Device_Setup_2 = this.myParameter.GetUshortValue();
    }

    public bool VolumeMeterFlowMounting
    {
      get => ((uint) this.Device_Setup_2 & 1U) > 0U;
      set
      {
        if (value)
          this.Device_Setup_2 |= (ushort) 1;
        else
          this.Device_Setup_2 &= (ushort) 65534;
        this.myParameter.SetUshortValue(this.Device_Setup_2);
      }
    }

    internal bool UltrasonicFactorMatrixUsed
    {
      get => ((int) this.Device_Setup_2 & 160) == 160;
      set
      {
        if (value)
          this.Device_Setup_2 |= (ushort) 160;
        else
          this.Device_Setup_2 &= (ushort) 65375;
        this.myParameter.SetUshortValue(this.Device_Setup_2);
      }
    }
  }
}
