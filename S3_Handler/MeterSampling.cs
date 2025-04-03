// Decompiled with JetBrains decompiler
// Type: S3_Handler.MeterSampling
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class MeterSampling
  {
    private S3_HandlerFunctions MyFunctions;
    internal MeterSamplingData SamplingData;

    internal MeterSampling(S3_HandlerFunctions MyFunctions) => this.MyFunctions = MyFunctions;

    internal bool RunSampling(out MeterSamplingData samplingData)
    {
      ZR_ClassLibMessages.ClearErrors();
      samplingData = (MeterSamplingData) null;
      ByteField MonitorData;
      if (!this.MyFunctions.MyCommands.GetMeterMonitorData(out MonitorData))
        return false;
      if (MonitorData.Count != 504)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal number of bytes");
      if (MonitorData.Data[0] != (byte) 85 || MonitorData.Data[1] != (byte) 170)
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal data header");
      this.SamplingData = new MeterSamplingData();
      for (int index = 0; index < 500; ++index)
      {
        byte num = MonitorData.Data[index + 4];
        this.SamplingData.SamplingValues[index] = num;
        if ((int) num > (int) this.SamplingData.MaxValue)
          this.SamplingData.MaxValue = num;
        if ((int) num < (int) this.SamplingData.MinValue)
          this.SamplingData.MinValue = num;
      }
      bool flag = true;
      byte num1 = this.SamplingData.MinValue;
      for (int index = 1; index < 500; ++index)
      {
        if (flag)
        {
          if ((int) this.SamplingData.SamplingValues[index] - (int) num1 > 4)
          {
            ++this.SamplingData.Pulses;
            num1 = this.SamplingData.SamplingValues[index];
            flag = false;
          }
        }
        else if ((int) num1 - (int) this.SamplingData.SamplingValues[index] > 4)
        {
          ++this.SamplingData.Pulses;
          num1 = this.SamplingData.SamplingValues[index];
          flag = true;
        }
      }
      samplingData = this.SamplingData;
      return true;
    }
  }
}
