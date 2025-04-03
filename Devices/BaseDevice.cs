// Decompiled with JetBrains decompiler
// Type: Devices.BaseDevice
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  public class BaseDevice : IDisposable
  {
    protected DeviceManager MyDeviceManager;
    private bool typeMode;

    public virtual event EventHandlerEx<int> OnProgress;

    public virtual event EventHandlerEx<string> OnProgressMessage;

    public virtual event System.EventHandler ConnectionLost;

    public BaseDevice(DeviceManager MyDeviceManager) => this.MyDeviceManager = MyDeviceManager;

    public virtual bool Open() => this.MyDeviceManager.MyBus.ComOpen();

    public virtual bool Close() => this.MyDeviceManager.MyBus.ComClose();

    public virtual bool Print(string options)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool Connect(ref GlobalDeviceId Device)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return (List<GlobalDeviceId>) null;
    }

    public virtual bool SelectDevice(GlobalDeviceId device)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool BeginSearchDevices()
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool IsDevicesModified() => false;

    public virtual bool Read(StructureTreeNode structureTreeNode, List<long> filter)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool ReadAll(List<long> filter)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      ValueList = (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      string serialnumber)
    {
      ValueList = (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool GetValues(
      int valueGroup,
      out SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      ValueList = (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDeviceIndex)
    {
      ValueList = (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool GetValues(
      string zdf,
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDeviceIndex)
    {
      ValueList = (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual string GetZdfValues() => string.Empty;

    public virtual UniqueIdentification GetUniqueIdentification() => (UniqueIdentification) null;

    public virtual int UndoCount => 0;

    public virtual bool Undo() => false;

    public virtual bool ExecuteMethod(
      OverrideID overrideID,
      bool isSetMethod,
      out object result,
      object param1,
      object param2,
      object param3,
      object param4)
    {
      result = (object) null;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool ReadConfigurationParameters(out GlobalDeviceId UpdatedDeviceIdentification)
    {
      UpdatedDeviceIdentification = (GlobalDeviceId) null;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual async Task<int> ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      ReadPartsSelection readPartsSelection)
    {
      await Task.Delay(1);
      throw new NotImplementedException("ReadDeviceAsymc");
    }

    public virtual async Task WriteDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await Task.Delay(1);
      throw new NotImplementedException(nameof (WriteDeviceAsync));
    }

    public SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType)
    {
      return this.GetConfigurationParameters(ConfigurationType, 0);
    }

    public virtual SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return (SortedList<OverrideID, ConfigurationParameter>) null;
    }

    public bool SetConfigurationParameters(OverrideID key, object value)
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.GetConfigurationParameters(ConfigurationParameter.ValueType.Complete);
      if (configurationParameters == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "Please use ReadConfigurationParameters function first!");
        return false;
      }
      SortedList<OverrideID, ConfigurationParameter> parameterList = new SortedList<OverrideID, ConfigurationParameter>();
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> keyValuePair in configurationParameters)
      {
        if (keyValuePair.Key == key)
        {
          keyValuePair.Value.ParameterValue = value;
          parameterList.Add(key, keyValuePair.Value);
          break;
        }
      }
      if (parameterList.Count != 0)
        return this.SetConfigurationParameters(parameterList);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "Unknown OverrideID!");
      return false;
    }

    public virtual bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList)
    {
      return this.SetConfigurationParameters(parameterList, 0);
    }

    public virtual bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> parameterList,
      int SubDevice)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual bool WriteChangedConfigurationParametersToDevice()
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return false;
    }

    public virtual string LoadType()
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return string.Empty;
    }

    public virtual bool ChangeType(
      SortedList<OverrideID, ConfigurationParameter> additionalConfigurationParameters)
    {
      return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
    }

    public virtual bool TypeMode
    {
      get => this.typeMode;
      set => this.typeMode = value;
    }

    public virtual void ClearDeviceList()
    {
      if (this.MyDeviceManager == null || this.MyDeviceManager.MyBus == null)
        return;
      this.MyDeviceManager.MyBus.DeleteBusInfo();
    }

    public virtual object GetHandler()
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented);
      return (object) null;
    }

    public virtual void Dispose()
    {
    }

    internal virtual void ShowHandlerWindow()
    {
    }

    public virtual DateTime? SaveMeter() => throw new NotImplementedException("Save meter backup");
  }
}
