// Decompiled with JetBrains decompiler
// Type: DeviceCollector.DeviceList
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public abstract class DeviceList
  {
    private static Logger logger = LogManager.GetLogger(nameof (DeviceList));
    public const string COLUMN_NAME_INDEX_NUMBER = "Nr.";
    public const string COLUMN_NAME_IS_SELECTED_ON_BUS = "IsSel";
    public const string COLUMN_NAME_SELECTED_REPEATS = "SelRep";
    public const string COLUMN_NAME_READ_REPEATS = "ReadRep";
    public const string COLUMN_NAME_DESELECT_REPEATS = "DeselRep";
    public const string COLUMN_NAME_SERIAL_NUMBER = "SerialNr.";
    protected bool PrimaryAddressingOk = false;
    protected bool AllAddressesOk = false;
    internal bool MBusConverterAvailable = false;
    private BusDevice SelectedDeviceInternal;
    public ArrayList bus;
    public List<MBusDevice> FaultyDevices;
    internal DeviceCollectorFunctions MyBus;

    public BusDevice SelectedDevice
    {
      set
      {
        this.SelectedDeviceInternal = value;
        this.MyBus.DeviceIsModified = true;
      }
      get => this.SelectedDeviceInternal;
    }

    internal int GetIndexOfSelectedDevice()
    {
      return this.bus == null || this.bus.Count == 0 ? -1 : this.bus.IndexOf((object) this.SelectedDeviceInternal);
    }

    internal virtual void DeleteBusList()
    {
      if (this.bus != null && this.bus.Count > 0)
        this.bus.Clear();
      if (this.FaultyDevices != null && this.FaultyDevices.Count > 0)
        this.FaultyDevices.Clear();
      this.SelectedDevice = (BusDevice) null;
    }

    public void AddFaultyDevice(MBusDevice NewDevice) => this.FaultyDevices.Add(NewDevice);

    internal void RemoveFaultyDevices(byte address)
    {
      if (this.FaultyDevices == null)
        return;
      MBusDevice mbusDevice = (MBusDevice) null;
      foreach (MBusDevice faultyDevice in this.FaultyDevices)
      {
        if ((int) faultyDevice.PrimaryDeviceAddress == (int) address)
        {
          mbusDevice = faultyDevice;
          break;
        }
      }
      if (mbusDevice == null)
        return;
      this.FaultyDevices.Remove(mbusDevice);
    }

    internal bool SelectDeviceByIndex(int index)
    {
      if (index >= this.bus.Count)
        return false;
      this.SelectedDevice = (BusDevice) this.bus[index];
      return true;
    }

    internal int GetSelectedIndex()
    {
      if (this.SelectedDevice == null)
        return -1;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        if (this.bus[index].Equals((object) this.SelectedDevice))
          return index;
      }
      return -1;
    }

    internal void GetBusTable(out DataTable BusTable, BusMode busMode)
    {
      bool flag = this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other;
      BusTable = new DataTable();
      BusTable.TableName = "BusInfo";
      if (this.bus.Count == 0 && this.FaultyDevices.Count == 0)
        return;
      BusTable.Columns.Add("Nr.", typeof (int));
      BusTable.Columns.Add("Manuf.", typeof (string));
      if (busMode != 0)
        BusTable.Columns.Add("SerialNr.", typeof (string));
      if (this.bus.Count != 0 && !(this.bus[0] is IdentDevice))
      {
        BusTable.Columns.Add("DeviceType", typeof (string));
        if (!flag && (this.MyBus.MyBusMode == BusMode.MBus || this.MyBus.MyBusMode != 0))
        {
          if (busMode != 0)
          {
            BusTable.Columns.Add("AddrOk", typeof (bool));
            BusTable.Columns.Add("Address", typeof (string));
            BusTable.Columns.Add("IsSel", typeof (bool));
          }
          BusTable.Columns.Add("SelRep", typeof (int));
          BusTable.Columns.Add("ReadRep", typeof (int));
          BusTable.Columns.Add("DeselRep", typeof (int));
        }
      }
      if (this.FaultyDevices.Count != 0)
      {
        if (BusTable.Columns["DeviceType"] == null)
          BusTable.Columns.Add("DeviceType", typeof (string));
        if (BusTable.Columns["AddrOk"] == null)
          BusTable.Columns.Add("AddrOk", typeof (bool));
        if (BusTable.Columns["Address"] == null)
          BusTable.Columns.Add("Address", typeof (string));
      }
      if (((this.MyBus.MyBusMode == BusMode.Radio2 || this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio4 || this.MyBus.MyBusMode == BusMode.MinomatRadioTest || this.MyBus.MyBusMode == BusMode.RadioMS ? 1 : (this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoConnect || this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoHead)
          BusTable.Columns.Add("RSSI", typeof (int));
        if (!flag)
        {
          BusTable.Columns.Add("DeviceError", typeof (bool));
          BusTable.Columns.Add("Manipulated", typeof (bool));
        }
        BusTable.Columns.Add("ReceivedPackets", typeof (int));
        BusTable.Columns.Add("LastSeen", typeof (string));
        BusTable.Columns.Add("IntervalSec", typeof (int));
        if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoConnect)
          BusTable.Columns.Add("MCT", typeof (uint));
      }
      BusTable.Columns.Add("DeviceInfoText", typeof (string));
      int num = 0;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        try
        {
          BusDevice bu1 = (BusDevice) this.bus[index];
          bu1.TableIndex = num++;
          DataRow row = BusTable.NewRow();
          bu1.TableDataRow = row;
          row["Nr."] = (object) (index + 1);
          if (BusTable.Columns.Contains("SerialNr."))
            row["SerialNr."] = (object) "???";
          row["Manuf."] = (object) "???";
          if (this.bus[index] is MBusDevice)
          {
            MBusDevice bu2 = (MBusDevice) this.bus[index];
            row["DeviceType"] = (object) bu2.DeviceType.ToString();
            if (BusTable.Columns.Contains("Address"))
              row["Address"] = !bu2.PrimaryAddressKnown ? (object) "???" : (object) bu2.PrimaryDeviceAddress.ToString("d03");
            if (bu2.Info != null)
            {
              row["Manuf."] = (object) bu2.Info.Manufacturer;
              if (BusTable.Columns.Contains("SerialNr."))
                row["SerialNr."] = (object) bu2.Info.MeterNumber;
            }
            if (BusTable.Columns.Contains("AddrOk"))
              row["AddrOk"] = (object) bu2.PrimaryAddressOk;
            if (BusTable.Columns.Contains("IsSel"))
              row["IsSel"] = (object) bu2.IsSelectedOnBus;
            row["DeviceInfoText"] = (object) bu2.DeviceInfoText;
          }
          else if (this.bus[index] is IdentDevice)
          {
            if (BusTable.Columns.Contains("SerialNr."))
              row["SerialNr."] = (object) ((BusDevice) this.bus[index]).Info.MeterNumber;
          }
          else if (this.MyBus.MyBusMode == BusMode.WaveFlowRadio)
          {
            row["DeviceType"] = (object) ((BusDevice) this.bus[index]).DeviceType.ToString();
            row["Manuf."] = (object) ((BusDevice) this.bus[index]).Info.Manufacturer;
            if (BusTable.Columns.Contains("SerialNr."))
              row["SerialNr."] = (object) ((BusDevice) this.bus[index]).Info.MeterNumber;
          }
          else if (this.bus[index] is MinomatDevice)
          {
            row["DeviceType"] = (object) ((BusDevice) this.bus[index]).DeviceType.ToString();
            row["Manuf."] = (object) ((BusDevice) this.bus[index]).Info.Manufacturer;
            if (BusTable.Columns.Contains("SerialNr."))
              row["SerialNr."] = (object) ((BusDevice) this.bus[index]).Info.MeterNumber;
          }
          else if (this.bus[index] is RadioDevice)
          {
            RadioDevice bu3 = this.bus[index] as RadioDevice;
            if (bu3.Device is RadioDevicePacket)
            {
              RadioDevicePacket device = bu3.Device as RadioDevicePacket;
              row["Manuf."] = (object) bu3.Info.Manufacturer;
              row["DeviceType"] = (object) bu3.Info.MediumString;
              row["SerialNr."] = (object) bu3.Device.FunkId;
              if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoConnect || this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoHead)
              {
                int? rssiDBm = bu3.Device.RSSI_dBm;
                row["RSSI"] = !rssiDBm.HasValue ? (object) 0 : (object) bu3.Device.RSSI_dBm;
              }
              if (!flag)
              {
                row["DeviceError"] = (object) device.IsDeviceError;
                row["Manipulated"] = (object) device.IsManipulated;
              }
              row["ReceivedPackets"] = (object) bu3.DeviceInfoList.Count;
              row["LastSeen"] = (object) bu3.Info.LastReadingDate.ToString("HH:mm:ss");
              if (bu3.DeviceInfoList.Count > 1)
              {
                DateTime lastReadingDate1 = bu3.DeviceInfoList[bu3.DeviceInfoList.Count - 2].LastReadingDate;
                DateTime lastReadingDate2 = bu3.DeviceInfoList[bu3.DeviceInfoList.Count - 1].LastReadingDate;
                row["IntervalSec"] = (object) (int) lastReadingDate2.Subtract(lastReadingDate1).TotalSeconds;
              }
              if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoConnect)
                row["MCT"] = (object) device.MCT;
            }
            else if (bu3.Device is RadioPacketMinomatV4)
            {
              RadioPacketMinomatV4 device = bu3.Device as RadioPacketMinomatV4;
              row["Manuf."] = (object) bu3.Info.Manufacturer;
              row["DeviceType"] = (object) bu3.Info.MediumString;
              row["SerialNr."] = (object) bu3.Device.FunkId;
              if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoConnect || this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoHead)
              {
                int? rssiDBm = bu3.Device.RSSI_dBm;
                row["RSSI"] = !rssiDBm.HasValue ? (object) 0 : (object) bu3.Device.RSSI_dBm;
              }
              row["ReceivedPackets"] = (object) bu3.DeviceInfoList.Count;
              row["LastSeen"] = (object) bu3.Info.LastReadingDate.ToString("HH:mm:ss");
              if (bu3.DeviceInfoList.Count > 1)
              {
                DateTime lastReadingDate3 = bu3.DeviceInfoList[bu3.DeviceInfoList.Count - 2].LastReadingDate;
                DateTime lastReadingDate4 = bu3.DeviceInfoList[bu3.DeviceInfoList.Count - 1].LastReadingDate;
                row["IntervalSec"] = (object) (int) lastReadingDate4.Subtract(lastReadingDate3).TotalSeconds;
              }
              if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoConnect)
                row["MCT"] = (object) device.MCT;
            }
          }
          BusTable.Rows.Add(row);
        }
        catch (Exception ex)
        {
          DeviceList.logger.Error(ex.Message);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal data on create device list! Error: " + ex.Message);
        }
      }
      try
      {
        for (int index = 0; index < this.FaultyDevices.Count; ++index)
        {
          BusDevice faultyDevice = (BusDevice) this.FaultyDevices[index];
          faultyDevice.TableIndex = num++;
          DataRow row = BusTable.NewRow();
          faultyDevice.TableDataRow = row;
          row["Nr."] = (object) 0;
          if (BusTable.Columns.Contains("SerialNr."))
            row["SerialNr."] = (object) "Collision";
          row["Manuf."] = (object) "-";
          row["DeviceType"] = (object) this.FaultyDevices[index].DeviceType.ToString();
          if (BusTable.Columns.Contains("Address"))
            row["Address"] = (object) this.FaultyDevices[index].PrimaryDeviceAddress.ToString("d03");
          if (BusTable.Columns.Contains("AddrOk"))
            row["AddrOk"] = (object) this.FaultyDevices[index].PrimaryAddressOk;
          BusTable.Rows.Add(row);
        }
      }
      catch
      {
      }
    }

    internal virtual string GetAllParameters()
    {
      StringBuilder stringBuilder = new StringBuilder(100000);
      for (int index = 0; index < this.bus.Count; ++index)
      {
        BusDevice bu = this.bus[index] as BusDevice;
        if (index > 0)
          stringBuilder.Append("<r>");
        if (bu.Info != null)
          stringBuilder.Append(bu.Info.GetZDFParameterString());
        else
          stringBuilder.Append("NotRead");
      }
      return stringBuilder.ToString();
    }

    internal virtual bool DeleteSelectedDevice() => throw new NotImplementedException();

    internal virtual bool AddDevice(DeviceTypes NewType, bool select)
    {
      throw new NotImplementedException();
    }

    internal virtual bool AddDevice(object NewDevice, bool select)
    {
      throw new NotImplementedException();
    }

    internal virtual bool ScanFromAddress(int ScanAddress) => throw new NotImplementedException();

    internal virtual bool ScanFromSerialNumber(string StartSerialNumber)
    {
      throw new NotImplementedException();
    }

    internal virtual bool SearchSingleDeviceByPrimaryAddress(int SearchAddress)
    {
      throw new NotImplementedException();
    }

    internal virtual bool SearchSingleDeviceBySerialNumber(string SearchSerialNumber)
    {
      throw new NotImplementedException();
    }

    public virtual bool SearchSingleDeviceBySerialNumber(uint BCD_SerialNumber)
    {
      throw new NotImplementedException();
    }

    internal virtual bool SelectDeviceByPrimaryAddress(int Address)
    {
      throw new NotImplementedException();
    }

    internal virtual bool SelectDeviceBySerialNumber(string SerialNumber)
    {
      if (this.bus == null)
        return false;
      foreach (BusDevice bu in this.bus)
      {
        if (bu.Info != null && bu.Info.MeterNumber == SerialNumber)
        {
          this.SelectedDevice = bu;
          return true;
        }
      }
      return false;
    }

    internal virtual bool SetPhysicalDeviceBySerialNumber(string SerialNumber)
    {
      throw new Exception("Function SetPhysicalDeviceBySerialNumber for this list type not available.");
    }

    internal virtual bool WorkBusAddresses() => throw new NotImplementedException();

    internal virtual bool OrganizeBus(int StartAddress) => throw new NotImplementedException();

    internal virtual bool SetPrimaryAddressOnBusWithoutShift(int NewAddress)
    {
      throw new NotImplementedException();
    }

    internal virtual bool SetPrimaryAddressOnBus(int NewAddress)
    {
      throw new NotImplementedException();
    }

    internal virtual bool GetDeviceCollectorInfo(out object InfoObject)
    {
      throw new NotImplementedException();
    }
  }
}
