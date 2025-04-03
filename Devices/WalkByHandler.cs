// Decompiled with JetBrains decompiler
// Type: Devices.WalkByHandler
// Assembly: Devices, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 793FC2DA-FF88-4FD5-BDE9-C00C0310F1EC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Devices.dll

using DeviceCollector;
using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace Devices
{
  internal sealed class WalkByHandler(DeviceManager MyDeviceManager) : BaseDevice(MyDeviceManager)
  {
    private static Logger logger = LogManager.GetLogger(nameof (WalkByHandler));

    public GlobalDeviceId SelectedDevice { get; private set; }

    public override event EventHandlerEx<int> OnProgress;

    public override event System.EventHandler ConnectionLost;

    public override object GetHandler() => (object) this.MyDeviceManager.MyBus.RadioReader;

    public override bool Open()
    {
      this.MyDeviceManager.MyBus.RadioReader.Open();
      return true;
    }

    public override bool Close()
    {
      this.MyDeviceManager.MyBus.RadioReader.Close();
      return true;
    }

    internal override void ShowHandlerWindow() => this.MyDeviceManager.MyBus.ShowBusWindow();

    public override void Dispose() => this.MyDeviceManager.MyBus.Dispose();

    public override List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      List<GlobalDeviceId> globalDeviceIdList = new List<GlobalDeviceId>();
      if (this.MyDeviceManager.MyBus.RadioReader.ReceivedData != null)
      {
        foreach (KeyValuePair<long, RadioDataSet> keyValuePair in this.MyDeviceManager.MyBus.RadioReader.ReceivedData)
        {
          GlobalDeviceId globalDeviceId = new GlobalDeviceId();
          globalDeviceId.Serialnumber = keyValuePair.Key.ToString();
          if (keyValuePair.Value.LastRadioPacket != null)
          {
            globalDeviceId.DeviceTypeName = keyValuePair.Value.LastRadioPacket.DeviceType.ToString();
            globalDeviceId.MeterType = ValueIdent.ConvertToMeterType(keyValuePair.Value.LastRadioPacket.DeviceType);
          }
          globalDeviceIdList.Add(globalDeviceId);
        }
      }
      return globalDeviceIdList;
    }

    public override bool SelectDevice(GlobalDeviceId device)
    {
      this.SelectedDevice = device;
      return true;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList,
      int subDeviceIndex)
    {
      if (this.SelectedDevice == null)
        return false;
      long int64 = Convert.ToInt64(this.SelectedDevice.Serialnumber);
      if (this.MyDeviceManager.MyBus.RadioReader.ReceivedData == null || !this.MyDeviceManager.MyBus.RadioReader.ReceivedData.ContainsKey(int64))
        return false;
      valueList = this.MyDeviceManager.MyBus.RadioReader.ReceivedData[int64].Data;
      return true;
    }

    public override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> valueList)
    {
      return this.GetValues(ref valueList, 0);
    }

    public override bool BeginSearchDevices() => this.ReadAll((List<long>) null);

    public override bool Read(StructureTreeNode structureTreeNode, List<long> filter) => false;

    public override bool ReadAll(List<long> filter)
    {
      if (this.MyDeviceManager != null && this.MyDeviceManager.MyBus != null)
        this.MyDeviceManager.BreakRequest = false;
      try
      {
        this.MyDeviceManager.MyBus.RadioReader.OnPacketReceived += new EventHandler<RadioPacket>(this.RadioReader_OnPacketReceived);
        this.MyDeviceManager.MyBus.RadioReader.OnProgress += new EventHandlerEx<int>(this.RadioReader_OnProgress);
        this.MyDeviceManager.MyBus.RadioReader.ConnectionLost += new System.EventHandler(this.RadioReader_ConnectionLost);
        this.MyDeviceManager.MyBus.RadioReader.Read();
        return true;
      }
      finally
      {
        this.MyDeviceManager.MyBus.RadioReader.OnPacketReceived -= new EventHandler<RadioPacket>(this.RadioReader_OnPacketReceived);
        this.MyDeviceManager.MyBus.RadioReader.OnProgress -= new EventHandlerEx<int>(this.RadioReader_OnProgress);
        this.MyDeviceManager.MyBus.RadioReader.ConnectionLost -= new System.EventHandler(this.RadioReader_ConnectionLost);
      }
    }

    private void RadioReader_OnProgress(object sender, int e)
    {
      if (this.OnProgress == null)
        return;
      this.OnProgress(sender, e);
    }

    private void RadioReader_ConnectionLost(object sender, EventArgs e)
    {
      if (this.ConnectionLost == null)
        return;
      this.ConnectionLost(sender, e);
    }

    private void RadioReader_OnPacketReceived(object sender, RadioPacket e)
    {
      if (!this.MyDeviceManager.IsValueIdentSetReceivedEventEnabled)
        return;
      ValueIdentSet e1 = new ValueIdentSet();
      e1.Manufacturer = e.Manufacturer;
      e1.Version = e.Version;
      e1.SerialNumber = e.FunkId.ToString();
      e1.AvailableValues = e.GetValues();
      e1.Buffer = e.Buffer;
      switch (e)
      {
        case RadioPacketWirelessMBus _:
          RadioPacketWirelessMBus packetWirelessMbus = e as RadioPacketWirelessMBus;
          e1.DeviceType = packetWirelessMbus.MediumString;
          e1.ZDF = packetWirelessMbus.ZDF;
          if (packetWirelessMbus.FunkIdSecundary.HasValue)
          {
            e1.Manufacturer = packetWirelessMbus.ManufacturerSecundary;
            e1.Version = packetWirelessMbus.VersionNumberSecundary.ToString();
            e1.SerialNumber = packetWirelessMbus.FunkIdSecundary.ToString();
            e1.DeviceType = MBusDevice.GetMediaString(packetWirelessMbus.MediumSecundary.Value);
            break;
          }
          break;
        case RadioPacketRadio2 _:
          e1.DeviceType = e.DeviceType.ToString();
          e1.ZDF = "SID;" + e1.SerialNumber + ";MAN;" + e1.Manufacturer + ";MED;" + e1.DeviceType;
          break;
        case RadioPacketRadio3 _:
          RadioPacketRadio3 radioPacketRadio3 = e as RadioPacketRadio3;
          e1.DeviceType = e.DeviceType.ToString();
          ValueIdentSet valueIdentSet = e1;
          byte? scenarioNr = radioPacketRadio3.ScenarioNr;
          int? nullable = scenarioNr.HasValue ? new int?((int) scenarioNr.GetValueOrDefault()) : new int?();
          valueIdentSet.Scenario = nullable;
          e1.ZDF = "SID;" + e1.SerialNumber + ";MAN;" + e1.Manufacturer + ";MED;" + e1.DeviceType;
          break;
        default:
          e1.DeviceType = e.DeviceType.ToString();
          break;
      }
      this.MyDeviceManager.OnValueIdentSetReceived(sender, e1);
    }
  }
}
