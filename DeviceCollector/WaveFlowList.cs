// Decompiled with JetBrains decompiler
// Type: DeviceCollector.WaveFlowList
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System.Collections;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  internal class WaveFlowList : DeviceList
  {
    public WaveFlowList(DeviceCollectorFunctions BusRef)
    {
      this.MyBus = BusRef;
      this.bus = new ArrayList();
      this.FaultyDevices = new List<MBusDevice>();
    }

    internal override bool DeleteSelectedDevice()
    {
      if (this.SelectedDevice == null)
        return false;
      int index;
      for (index = 0; index < this.bus.Count; ++index)
      {
        if (this.bus[index] == this.SelectedDevice)
        {
          this.bus.RemoveAt(index);
          break;
        }
      }
      if (this.bus.Count > 0)
      {
        if (index < this.bus.Count)
          this.SelectedDevice = (BusDevice) this.bus[index];
        else
          this.SelectedDevice = (BusDevice) this.bus[this.bus.Count - 1];
      }
      else
        this.SelectedDevice = (BusDevice) null;
      return false;
    }

    internal override bool AddDevice(DeviceTypes NewType, bool select)
    {
      if (NewType != DeviceTypes.WaveFlowDevice)
        return false;
      this.bus.Add((object) new WaveFlowDevice(this.MyBus));
      if (select)
        this.SelectedDevice = (BusDevice) this.bus[this.bus.Count - 1];
      return true;
    }

    internal override bool AddDevice(object NewDevice, bool select)
    {
      WaveFlowDevice waveFlowDevice = NewDevice as WaveFlowDevice;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        if (waveFlowDevice.Info.MeterNumber == ((BusDevice) this.bus[index]).Info.MeterNumber)
          return true;
      }
      this.bus.Add((object) waveFlowDevice);
      this.WorkBusAddresses();
      if (select)
        this.SelectedDevice = (BusDevice) this.bus[this.bus.Count - 1];
      return true;
    }

    internal override bool SearchSingleDeviceBySerialNumber(string SearchSerialNumber)
    {
      WaveFlowDevice NewDevice = new WaveFlowDevice(this.MyBus);
      NewDevice.Info.MeterNumber = SearchSerialNumber;
      if (!NewDevice.ReadParameters())
        return false;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        if (((BusDevice) this.bus[index]).Info != null && (int) ((BusDevice) this.bus[index]).Info.ManufacturerCode == (int) NewDevice.Info.ManufacturerCode && (int) ((BusDevice) this.bus[index]).Info.Medium == (int) NewDevice.Info.Medium && ((BusDevice) this.bus[index]).Info.MeterNumber == NewDevice.Info.MeterNumber)
          return true;
      }
      this.AddDevice((object) NewDevice, true);
      return true;
    }
  }
}
