// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.AsyncFunctionsEx
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using AsyncCom;
using DeviceCollector;
using StartupLib;
using System;
using System.Collections.Generic;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  internal class AsyncFunctionsEx : IPort, IReadoutConfig
  {
    private DeviceCollectorFunctions deviceCollector;
    private AsyncFunctions asyncCom;

    public AsyncFunctionsEx(IDeviceCollector deviceCollector)
    {
      this.deviceCollector = deviceCollector as DeviceCollectorFunctions;
      this.asyncCom = deviceCollector.AsyncCom as AsyncFunctions;
    }

    public bool IsOpen => this.asyncCom.IsOpen;

    public void Open() => this.deviceCollector.ComOpen();

    public void Close() => this.deviceCollector.ComClose();

    public void Dispose() => this.deviceCollector.GMM_Dispose();

    public void ForceWakeup() => this.asyncCom.ClearWakeup();

    public bool DiscardInBuffer()
    {
      this.asyncCom.ClearCom();
      return true;
    }

    public void Write(byte[] request) => this.asyncCom.TransmitBlock(request);

    public byte[] ReadHeader(int count)
    {
      ByteField DataBlock = new ByteField(count);
      if (this.asyncCom.ReceiveBlock(ref DataBlock, count, true))
        return DataBlock.GetByteArray();
      ZR_ClassLibMessages.LastErrorInfo errorAndClearError = ZR_ClassLibMessages.GetLastErrorAndClearError();
      if (errorAndClearError.LastError == ZR_ClassLibMessages.LastErrors.Timeout)
        throw new TimeoutException(errorAndClearError.LastErrorDescription);
      throw new Exception(errorAndClearError.LastErrorDescription);
    }

    public byte[] ReadEnd(int count)
    {
      ByteField DataBlock = new ByteField(count);
      if (this.asyncCom.ReceiveBlock(ref DataBlock, count, false))
        return DataBlock.GetByteArray();
      ZR_ClassLibMessages.LastErrorInfo errorAndClearError = ZR_ClassLibMessages.GetLastErrorAndClearError();
      if (errorAndClearError.LastError == ZR_ClassLibMessages.LastErrors.Timeout)
        throw new TimeoutException(errorAndClearError.LastErrorDescription);
      throw new Exception(errorAndClearError.LastErrorDescription);
    }

    public byte[] ReadExisting() => throw new NotImplementedException();

    public event EventHandler<byte[]> OnRequest;

    public event EventHandler<byte[]> OnResponse;

    public void SetReadoutConfiguration(ConfigList configList)
    {
      throw new NotImplementedException();
    }

    public ConfigList GetReadoutConfiguration()
    {
      ConfigList readoutConfiguration = this.deviceCollector.GetReadoutConfiguration();
      if (readoutConfiguration == null)
      {
        SortedList<string, string> collectorSettingsList = this.deviceCollector.GetDeviceCollectorSettingsList();
        foreach (KeyValuePair<string, string> asyncComSetting in this.deviceCollector.GetAsyncComSettings())
        {
          if (!collectorSettingsList.ContainsKey(asyncComSetting.Key))
            collectorSettingsList.Add(asyncComSetting.Key, asyncComSetting.Value);
        }
        readoutConfiguration = PlugInLoader.ConfigListStatic ?? new ConfigList(collectorSettingsList);
        readoutConfiguration.Reset(collectorSettingsList);
        if (readoutConfiguration.ConnectionProfileID == 0)
          readoutConfiguration.ConnectionProfileID = 275;
      }
      return readoutConfiguration;
    }
  }
}
