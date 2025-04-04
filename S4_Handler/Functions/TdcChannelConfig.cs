// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.TdcChannelConfig
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using System;

#nullable disable
namespace S4_Handler.Functions
{
  internal class TdcChannelConfig
  {
    private const double Water70_Degree_MetersPerSecond = 1554.534705;
    private const double ClockTime = 2.5E-07;
    private const ushort DN50_StartTime = 240;
    private const double DN50_Current = 50.0;
    private const ushort DN300_StartTime = 599;
    private const double DN300_Current = 60.0;
    private const ushort DN25_StartTime = 165;
    private const double BulkStartTimeFactor = 0.00055710306406685228;
    private const double ResidentialStartTimeFactor = 0.00027855153203342614;
    internal bool TwoTransducersUsed;
    internal bool parallel_TOF_eval;
    internal ushort StartClocks;
    internal ushort TimeoutMicroSeconds;
    internal double MinTransducerDistance;
    internal double MaxTransducerDistance;

    internal TdcChannelConfig(S4_DeviceMemory memory)
    {
      S4_DeviceMemory.ConfigFlagRegisterBits parameterValue = (S4_DeviceMemory.ConfigFlagRegisterBits) memory.GetParameterValue<ushort>(S4_Params.ConfigFlagRegister);
      this.TwoTransducersUsed = (parameterValue & S4_DeviceMemory.ConfigFlagRegisterBits.US_ONLY_ONE_CHANAL) == (S4_DeviceMemory.ConfigFlagRegisterBits) 0;
      this.parallel_TOF_eval = (parameterValue & S4_DeviceMemory.ConfigFlagRegisterBits.TDC_USE_PARALLEL_TOF_EVAL) != 0;
      byte[] numArray = (byte[]) null;
      byte[] data;
      if (this.TwoTransducersUsed)
      {
        data = memory.GetData(S4_Params.tdcConfigTwoChannelUnit2);
        numArray = memory.GetData(S4_Params.tdcConfigTwoChannelUnit1);
      }
      else
        data = memory.GetData(S4_Params.tdcConfigOneChannelUnit2);
      this.StartClocks = BitConverter.ToUInt16(data, 18);
      this.MinTransducerDistance = 1554.534705 * ((double) this.StartClocks * 2.5E-07) * 1000.0;
      this.TimeoutMicroSeconds = (ushort) (128 << ((int) BitConverter.ToUInt16(data, 2) & 7));
      this.MaxTransducerDistance = 1554.534705 * (double) this.TimeoutMicroSeconds / 1000.0;
      if (numArray != null && (int) this.StartClocks != (int) BitConverter.ToUInt16(numArray, 18))
        throw new Exception("Start time configuration different for the two tdc channels");
    }

    internal double GetTdcBatteryFactor()
    {
      double tdcBatteryFactor = !this.TwoTransducersUsed ? 1.0 + (double) ((int) this.StartClocks - 165) * 0.00027855153203342614 : 1.0 + (double) ((int) this.StartClocks - 240) * 0.00055710306406685228;
      if (tdcBatteryFactor < 1.0)
        tdcBatteryFactor = 1.0 - (1.0 - tdcBatteryFactor) / 4.0;
      return tdcBatteryFactor;
    }
  }
}
