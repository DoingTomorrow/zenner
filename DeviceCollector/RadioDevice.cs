// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioDevice
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public sealed class RadioDevice : BusDevice
  {
    private RadioList walkByList;
    public RadioPacket Device;
    public List<DeviceInfo> DeviceInfoList;

    public RadioDataSet ReceivedRadioDataSet
    {
      get
      {
        if (this.walkByList == null || this.walkByList.ReceivedData == null)
          return (RadioDataSet) null;
        long key = Util.ToLong((object) this.Info.MeterNumber);
        return !this.walkByList.ReceivedData.ContainsKey(key) ? (RadioDataSet) null : this.walkByList.ReceivedData[key];
      }
    }

    public RadioDevice(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.walkByList = TheBus.MyDeviceList as RadioList;
    }

    public RadioDevice(DeviceCollectorFunctions TheBus, RadioPacket packet)
      : base(TheBus)
    {
      this.walkByList = TheBus.MyDeviceList as RadioList;
      this.Device = packet;
      this.DeviceType = packet.DeviceType;
      this.DeviceInfoList = new List<DeviceInfo>();
    }
  }
}
