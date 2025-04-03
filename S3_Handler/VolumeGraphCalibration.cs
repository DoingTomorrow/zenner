// Decompiled with JetBrains decompiler
// Type: S3_Handler.VolumeGraphCalibration
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using DeviceCollector;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class VolumeGraphCalibration
  {
    protected S3_HandlerFunctions MyFunctions;

    public static void GarantObjectAvailable(S3_HandlerFunctions MyFunctions)
    {
      if (MyFunctions.MyMeters.WorkMeter == null)
        return;
      if (MyFunctions.MyMeters.WorkMeter.MyIdentification.GetVolumeMeterType() == ParameterService.HardwareMaskElements.Ultrasonic || MyFunctions.MyMeters.WorkMeter.MyIdentification.GetVolumeMeterType() == ParameterService.HardwareMaskElements.UltrasonicDirect)
      {
        if (MyFunctions.volumeGraphCalibration != null && MyFunctions.volumeGraphCalibration is TDC_Calibration)
          return;
        MyFunctions.volumeGraphCalibration = (VolumeGraphCalibration) new TDC_Calibration(MyFunctions);
      }
      else if (MyFunctions.volumeGraphCalibration == null || MyFunctions.volumeGraphCalibration is TDC_Calibration)
        MyFunctions.volumeGraphCalibration = new VolumeGraphCalibration(MyFunctions);
    }

    public VolumeGraphCalibration()
    {
    }

    public VolumeGraphCalibration(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
    }

    internal bool SendVolumeTestActivate()
    {
      this.MyFunctions.MyMeters.MeterJobStart(MeterJob.DeviceTestModeActivateVolumeTest);
      bool flag = this.MyFunctions.MyCommands.FlyingTestActivate();
      this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.DeviceTestModeActivateVolumeTest);
      return flag;
    }

    internal bool ReadTestVolume(out float ReadVolume)
    {
      this.MyFunctions.MyMeters.MeterJobStart(MeterJob.DeviceTestReadTestVolume);
      ReadVolume = 0.0f;
      bool flag;
      for (int index = 0; index < 5; ++index)
      {
        MBusDeviceState state;
        flag = this.MyFunctions.MyCommands.FlyingTestReadVolume(out ReadVolume, out state);
        if (flag)
        {
          switch (state)
          {
            case MBusDeviceState.NoError:
              goto label_7;
            case MBusDeviceState.Busy:
              Thread.Sleep(1000);
              continue;
            default:
              flag = false;
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Flying test not complete!!");
              goto label_7;
          }
        }
        else
          goto label_7;
      }
      flag = false;
label_7:
      this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.DeviceTestReadTestVolume);
      return flag;
    }

    internal virtual bool AdjustVolumeFactor(float ErrorInPercent)
    {
      S3_Parameter s3Parameter = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_VolFactor.ToString()];
      float floatValue = s3Parameter.GetFloatValue();
      if ((double) floatValue == double.NaN)
        return false;
      float NewValue = floatValue / (float) (1.0 + (double) ErrorInPercent / 100.0);
      return s3Parameter.SetFloatValue(NewValue);
    }

    internal virtual bool AdjustVolumeCalibration(
      float Qi,
      float QiErrorInPercent,
      float Q,
      float QErrorInPercent,
      float Qp,
      float QpErrorInPercent)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return true;
    }

    internal virtual bool AdjustVolumeFactorQi(float Qi, float Q_UpperLimit, float ErrorInPercent)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }
  }
}
