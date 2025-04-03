// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RelayList
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using System.Collections;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  internal class RelayList : MBusList
  {
    private RelayDevice relayDevice;
    private static IAsyncFunctions asyncComOriginal;
    private RelayAsyncFunctions asyncComDummy;

    public RelayList(DeviceCollectorFunctions busDevice)
      : base(busDevice)
    {
      this.MyBus = busDevice;
      this.bus = new ArrayList();
      this.FaultyDevices = new List<MBusDevice>();
    }

    internal override bool ScanFromAddress(int scanAddress)
    {
      if (this.MyBus.MyCom is RelayAsyncFunctions)
        this.MyBus.MyCom = RelayList.asyncComOriginal;
      if (this.relayDevice == null)
        this.relayDevice = new RelayDevice(this.MyBus);
      MbusTelegramCollection telegramCollection = this.relayDevice.Read();
      if (telegramCollection == null)
        return false;
      RelayList.asyncComOriginal = this.MyBus.MyCom;
      this.asyncComDummy = new RelayAsyncFunctions();
      this.MyBus.MyCom = (IAsyncFunctions) this.asyncComDummy;
      foreach (MBusTelegramSet mbusTelegramSet in (List<MBusTelegramSet>) telegramCollection)
      {
        if (mbusTelegramSet.MBusTelegrams.Count != 0)
        {
          MbusTelegram mbusTelegram = mbusTelegramSet.MBusTelegrams[0];
          this.bus.Add((object) this.ReadMBusTelegram(mbusTelegramSet.PrimaryAddress, mbusTelegram));
        }
      }
      this.MyBus.MyCom = RelayList.asyncComOriginal;
      return true;
    }

    internal List<DeviceInfo> GetAllParametersOfSelectedDevice()
    {
      if (this.asyncComDummy == null || this.relayDevice == null || this.relayDevice.MBusTelegrams == null || this.relayDevice.MBusTelegrams.Count == 0)
        return (List<DeviceInfo>) null;
      DeviceInfo info = this.MyBus.GetSelectedDevice().Info;
      MBusTelegramSet telegramsOfMbusDevice = this.relayDevice.MBusTelegrams.GetTelegramsOfMBusDevice(info.A_Field, long.Parse(info.MeterNumber));
      if (telegramsOfMbusDevice == null || telegramsOfMbusDevice.MBusTelegrams == null || telegramsOfMbusDevice.MBusTelegrams.Count == 0)
        return (List<DeviceInfo>) null;
      try
      {
        this.MyBus.MyCom = (IAsyncFunctions) this.asyncComDummy;
        List<DeviceInfo> ofSelectedDevice = new List<DeviceInfo>(telegramsOfMbusDevice.MBusTelegrams.Count);
        foreach (MbusTelegram mbusTelegram in telegramsOfMbusDevice.MBusTelegrams)
        {
          MBusDevice mbusDevice = this.ReadMBusTelegram(telegramsOfMbusDevice.PrimaryAddress, mbusTelegram);
          if (mbusDevice != null && mbusDevice.Info != null)
            ofSelectedDevice.Add(mbusDevice.Info);
        }
        return ofSelectedDevice;
      }
      catch
      {
        return (List<DeviceInfo>) null;
      }
      finally
      {
        this.MyBus.MyCom = RelayList.asyncComOriginal;
      }
    }

    private MBusDevice ReadMBusTelegram(byte primaryAddress, MbusTelegram telegramToRead)
    {
      MBusDevice mbusDevice = new MBusDevice(this.MyBus);
      mbusDevice.PrimaryAddressKnown = true;
      mbusDevice.PrimaryAddressOk = true;
      mbusDevice.PrimaryDeviceAddress = primaryAddress;
      this.asyncComDummy.MbusTelegramToRead = telegramToRead;
      ByteField DataBlock = (ByteField) null;
      this.MyBus.MyCom.TransmitBlock(ref DataBlock);
      if (!mbusDevice.ReceiveHeader() || !mbusDevice.ReceiveLongframeEnd(telegramToRead.TimePoint))
        return (MBusDevice) null;
      mbusDevice.GenerateParameterList(true);
      mbusDevice.Info.ParameterOk = true;
      return mbusDevice;
    }
  }
}
