// Decompiled with JetBrains decompiler
// Type: Devices.Series3DeviceByHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using S3_Handler;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class Series3DeviceByHandler(DeviceManager MyDeviceManager) : MBusDeviceHandler(MyDeviceManager)
  {
    private S3_HandlerFunctions MyS3Handler = (S3_HandlerFunctions) null;
    private bool typeMode;

    public override event EventHandlerEx<int> OnProgress;

    public override object GetHandler() => (object) this.MyS3Handler;

    private void GarantS3HandlerLoaded()
    {
      if (this.MyS3Handler == null)
      {
        if (this.MyDeviceManager.IsPlugin)
        {
          this.MyS3Handler = (S3_HandlerFunctions) PlugInLoader.GetPlugIn("S3_Handler").GetPluginInfo().Interface;
        }
        else
        {
          this.MyS3Handler = new S3_HandlerFunctions((IDeviceCollector) this.MyDeviceManager.MyBus);
          this.MyS3Handler.meterBackupOnRead = false;
          this.MyS3Handler.meterBackupOnWrite = false;
          this.MyS3Handler.loadLastSettingsOnStart = false;
          this.MyS3Handler.onlyOneReadBackupPerDay = false;
          this.MyS3Handler.saveLastSettingsOnExit = false;
        }
      }
      this.MyS3Handler.usePcTime = true;
      this.MyS3Handler.OnProgress += new EventHandlerEx<int>(this.S3Handler_OnProgress);
    }

    private void S3Handler_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    internal override void ShowHandlerWindow()
    {
      this.GarantS3HandlerLoaded();
      this.MyS3Handler.ShowS3_HandlerMainWindow();
    }

    public override void Dispose()
    {
      if (this.MyS3Handler == null)
        return;
      this.MyS3Handler.GMM_Dispose();
      this.MyS3Handler.OnProgress -= new EventHandlerEx<int>(this.S3Handler_OnProgress);
      this.MyS3Handler = (S3_HandlerFunctions) null;
    }

    public override bool IsDevicesModified() => true;

    public override int UndoCount => this.MyS3Handler.UndoCount;

    public override bool Undo() => this.MyS3Handler.Undo();

    public override bool Print(string options) => this.MyS3Handler.Print(options);

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      this.GarantS3HandlerLoaded();
      if (this.MyS3Handler == null)
        return (List<GlobalDeviceId>) null;
      GlobalDeviceId globalDeviceIds = this.MyS3Handler.GetGlobalDeviceIds();
      if (globalDeviceIds == null)
        return base.GetGlobalDeviceIdList();
      return new List<GlobalDeviceId>() { globalDeviceIds };
    }

    public override bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      this.GarantS3HandlerLoaded();
      this.MyDeviceManager.MyBus.IsDeviceModified();
      if (!this.MyS3Handler.ReadConnectedDevice())
        return false;
      UpdatedDeviceIdentification = this.MyS3Handler.GetGlobalDeviceIds();
      return UpdatedDeviceIdentification != null;
    }

    public bool GetDeviceIdentification(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      UpdatedDeviceIdentification = this.MyS3Handler.GetGlobalDeviceIds();
      return UpdatedDeviceIdentification != null;
    }

    public override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      this.MyDeviceManager.ParameterType = ConfigurationType;
      return this.MyS3Handler.GetConfigurationParameters(SubDevice);
    }

    public override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ConfigParameterList,
      int SubDevice)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.SetConfigParamList = ConfigParameterList;
      if (!this.MyS3Handler.SetConfigurationParameter(ConfigParameterList, SubDevice) || ZR_ClassLibMessages.GetLastError() != 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Configuration error");
        if (ZR_ClassLibMessages.GetLastError() != 0)
        {
          stringBuilder.AppendLine("Error: " + ZR_ClassLibMessages.GetLastError().ToString());
          ZR_ClassLibMessages.LastErrorInfo lastErrorInfo = ZR_ClassLibMessages.GetLastErrorInfo();
          if (!string.IsNullOrEmpty(lastErrorInfo.LastErrorDescription))
          {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(lastErrorInfo.LastErrorDescription);
          }
        }
        ZR_ClassLibMessages.ClearErrors();
        throw new Exception(stringBuilder.ToString());
      }
      return true;
    }

    public override bool WriteChangedConfigurationParametersToDevice()
    {
      return this.MyS3Handler.WriteChangesToConnectedDevice();
    }

    public bool AreDataAvailable()
    {
      this.GarantS3HandlerLoaded();
      return this.MyS3Handler.NewConfigurationDataAvailable;
    }

    public override string LoadType() => this.MyS3Handler.OpenTypeByWindow();

    public override bool ChangeType(
      SortedList<OverrideID, ConfigurationParameter> additionalConfigurationParameters)
    {
      bool flag = false;
      try
      {
        bool[] OverwriteSelection = new bool[21]
        {
          true,
          false,
          true,
          false,
          false,
          false,
          true,
          false,
          true,
          false,
          false,
          false,
          false,
          false,
          true,
          true,
          true,
          true,
          false,
          false,
          false
        };
        flag = this.MyS3Handler.IsTypeOverwritePossible(OverwriteSelection);
        if (flag)
          flag = this.MyS3Handler.OverwriteWorkFromType(OverwriteSelection);
        if (flag && additionalConfigurationParameters != null)
          flag = this.MyS3Handler.SetConfigurationParameterNoClone(additionalConfigurationParameters);
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Exception, ex.ToString());
      }
      finally
      {
      }
      return flag;
    }

    public override bool TypeMode
    {
      get => this.typeMode;
      set
      {
        this.MyS3Handler.useBaseTypeByConfig = !value;
        this.typeMode = value;
      }
    }

    public override DateTime? SaveMeter()
    {
      this.MyS3Handler.SaveDevice();
      return new DateTime?();
    }
  }
}
