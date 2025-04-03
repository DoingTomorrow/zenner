// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MBusList
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MBusList : DeviceList
  {
    public static Logger MBusScannerLogger = LogManager.GetLogger("MBusScanner");

    public MBusList(DeviceCollectorFunctions BusRef)
    {
      this.MyBus = BusRef;
      this.bus = new ArrayList();
      this.FaultyDevices = new List<MBusDevice>();
    }

    internal override bool AddDevice(DeviceTypes NewType, bool select)
    {
      BusDevice e;
      switch (NewType)
      {
        case DeviceTypes.MBus:
          e = (BusDevice) new MBusDevice(this.MyBus);
          break;
        case DeviceTypes.ZR_Serie1:
          e = (BusDevice) new Serie1MBus(this.MyBus);
          break;
        case DeviceTypes.ZR_Serie2:
          e = (BusDevice) new Serie2MBus(this.MyBus);
          break;
        case DeviceTypes.ZR_EHCA:
          e = (BusDevice) new EHCA_MBus(this.MyBus);
          break;
        case DeviceTypes.ZR_RDM:
          e = (BusDevice) new RDM_Bus(this.MyBus);
          break;
        case DeviceTypes.Minol_Device:
          e = (BusDevice) new MinolDevice(this.MyBus);
          break;
        case DeviceTypes.ZR_Serie3:
          e = (BusDevice) new Serie3MBus(this.MyBus);
          break;
        case DeviceTypes.EDC:
          e = (BusDevice) this.MyBus.EDCHandler;
          break;
        case DeviceTypes.PDC:
          e = (BusDevice) this.MyBus.PDCHandler;
          break;
        case DeviceTypes.TemperatureSensor:
        case DeviceTypes.HumiditySensor:
          e = new BusDevice(this.MyBus);
          e.DeviceType = NewType;
          break;
        default:
          return false;
      }
      if (e is MBusDevice)
        e.DeviceType = NewType;
      this.bus.Add((object) e);
      this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, e);
      this.WorkBusAddresses();
      if (select)
        this.SelectedDevice = (BusDevice) this.bus[this.bus.Count - 1];
      return true;
    }

    internal override bool AddDevice(object NewDevice, bool select)
    {
      if (!(NewDevice is MBusDevice mbusDevice))
        return false;
      if (mbusDevice.Info.Manufacturer == "ZR_" || mbusDevice.Info.Manufacturer == "ZRM")
      {
        if (mbusDevice.Info.Medium != (byte) 8)
        {
          if (mbusDevice.Info.Version >= (byte) 128)
          {
            Serie2MBus e = new Serie2MBus(mbusDevice);
            this.bus.Add((object) e);
            this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) e);
          }
          else
          {
            Serie1MBus e = new Serie1MBus(mbusDevice);
            this.bus.Add((object) e);
            this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) e);
          }
        }
        else
        {
          EHCA_MBus e = new EHCA_MBus(mbusDevice);
          this.bus.Add((object) e);
          this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) e);
        }
      }
      else if (mbusDevice.Info.Manufacturer == "ZRI")
      {
        bool flag = mbusDevice.DeviceType == DeviceTypes.EDC;
        Serie3MBus e = new Serie3MBus(mbusDevice);
        if (flag)
          e.DeviceType = DeviceTypes.EDC;
        this.bus.Add((object) e);
        this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) e);
      }
      else
      {
        this.bus.Add(NewDevice);
        this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) mbusDevice);
      }
      this.WorkBusAddresses();
      if (select)
        this.SelectedDevice = (BusDevice) this.bus[this.bus.Count - 1];
      return true;
    }

    internal override bool DeleteSelectedDevice()
    {
      if (this.SelectedDevice == null)
        return false;
      byte aField1 = this.SelectedDevice.Info.A_Field;
      string meterNumber1 = this.SelectedDevice.Info.MeterNumber;
      int index;
      for (index = 0; index < this.bus.Count; ++index)
      {
        if (this.SelectedDevice == this.bus[index])
        {
          this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) null);
          break;
        }
        byte aField2 = ((BusDevice) this.bus[index]).Info.A_Field;
        string meterNumber2 = ((BusDevice) this.bus[index]).Info.MeterNumber;
        if ((int) aField1 == (int) aField2 && meterNumber1 == meterNumber2)
        {
          this.bus.RemoveAt(index);
          this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) null);
          break;
        }
      }
      this.WorkBusAddresses();
      if (this.bus.Count > 0)
      {
        if (index < this.bus.Count)
          this.SelectedDevice = (BusDevice) this.bus[index];
        else
          this.SelectedDevice = (BusDevice) this.bus[this.bus.Count - 1];
      }
      else
        this.SelectedDevice = (BusDevice) null;
      return true;
    }

    internal override bool SelectDeviceByPrimaryAddress(int Address)
    {
      foreach (MBusDevice bu in this.bus)
      {
        if (bu.PrimaryAddressOk && (int) bu.PrimaryDeviceAddress == Address)
        {
          this.SelectedDevice = (BusDevice) bu;
          return true;
        }
      }
      return false;
    }

    internal override bool SetPhysicalDeviceBySerialNumber(string SerialNumber)
    {
      if (this.bus == null || this.SelectedDevice == null || this.SelectedDevice.Info == null)
        return false;
      foreach (BusDevice bu in this.bus)
      {
        if (bu.Info != null && bu.Info.MeterNumber == SerialNumber)
        {
          this.SelectedDevice = bu;
          return true;
        }
      }
      if (!(this.SelectedDevice is Serie3MBus))
        return false;
      Serie3MBus serie3Mbus = new Serie3MBus(this.SelectedDevice.MyBus);
      serie3Mbus.Info = new DeviceInfo(this.SelectedDevice.Info);
      serie3Mbus.Info.MeterNumber = SerialNumber;
      serie3Mbus.MaxWriteBlockSize = ((Serie2MBus) this.SelectedDevice).MaxWriteBlockSize;
      this.bus.Add((object) serie3Mbus);
      this.SelectedDevice = (BusDevice) serie3Mbus;
      return true;
    }

    internal override bool SearchSingleDeviceByPrimaryAddress(int SearchAddress)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (!this.IsNewMBusDevicePossible() || SearchAddress < 0 || SearchAddress > 251 && SearchAddress != 254)
        return false;
      MBusDevice NewDevice = new MBusDevice(this.MyBus);
      NewDevice.PrimaryAddressKnown = true;
      NewDevice.PrimaryAddressOk = true;
      NewDevice.PrimaryDeviceAddress = (byte) SearchAddress;
      if (!NewDevice.REQ_UD2())
      {
        ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
        int num;
        switch (lastError)
        {
          case ZR_ClassLibMessages.LastErrors.NoError:
          case ZR_ClassLibMessages.LastErrors.Timeout:
          case ZR_ClassLibMessages.LastErrors.ComOpenError:
            num = 1;
            break;
          default:
            num = lastError == ZR_ClassLibMessages.LastErrors.CommunicationError ? 1 : 0;
            break;
        }
        if (num == 0)
        {
          NewDevice.DeviceType = DeviceTypes.MultipleDevices;
          NewDevice.PrimaryAddressOk = false;
          NewDevice.PrimaryAddressKnown = false;
          for (int index = 0; index < this.FaultyDevices.Count; ++index)
          {
            if (this.FaultyDevices[index].Info != null && (int) this.FaultyDevices[index].PrimaryDeviceAddress == (int) NewDevice.PrimaryDeviceAddress)
              return false;
          }
          this.AddFaultyDevice(NewDevice);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "MultipleDevices found.");
        }
        return false;
      }
      if (this.MyBus.MyBusMode == BusMode.MBusPointToPoint)
      {
        NewDevice.PrimaryDeviceAddress = NewDevice.Info.A_Field;
      }
      else
      {
        for (int index = 0; index < this.bus.Count; ++index)
        {
          if (((BusDevice) this.bus[index]).Info != null && (int) ((BusDevice) this.bus[index]).Info.ManufacturerCode == (int) NewDevice.Info.ManufacturerCode && (int) ((BusDevice) this.bus[index]).Info.Medium == (int) NewDevice.Info.Medium && (int) ((BusDevice) this.bus[index]).Info.Version == (int) NewDevice.Info.Version && ((BusDevice) this.bus[index]).Info.MeterNumber == NewDevice.Info.MeterNumber)
          {
            Application.DoEvents();
            return true;
          }
        }
      }
      this.AddDevice((object) NewDevice, true);
      return true;
    }

    internal override bool SearchSingleDeviceBySerialNumber(string SearchSerialNumber)
    {
      uint SerialNumberOut;
      return MBusDevice.StringToMBusSerialNumber(SearchSerialNumber, out SerialNumberOut) && this.SearchSingleDeviceBySerialNumber(SerialNumberOut);
    }

    public override bool SearchSingleDeviceBySerialNumber(uint BCD_SerialNumber)
    {
      if (!this.IsNewMBusDevicePossible())
        return false;
      this.MyBus.SendMessage(BCD_SerialNumber.ToString("X08"), this.bus.Count, GMM_EventArgs.MessageType.ScanAddressMessage);
      MBusDevice NewDevice = new MBusDevice(this.MyBus);
      NewDevice.PrimaryAddressKnown = false;
      NewDevice.PrimaryAddressOk = false;
      NewDevice.PrimaryDeviceAddress = (byte) 0;
      if (!this.MyBus.FastSecondaryAddressing)
        NewDevice.DeselectDevice();
      if (NewDevice.SelectDeviceOnBus(BCD_SerialNumber, ushort.MaxValue, byte.MaxValue, byte.MaxValue))
      {
        if (!NewDevice.REQ_UD2())
        {
          ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
          bool flag1 = BCD_SerialNumber.ToString("X08").StartsWith("F");
          bool flag2 = BCD_SerialNumber.ToString("X08").EndsWith("F");
          if (!flag1 && !flag2)
            MBusList.MBusScannerLogger.Fatal("Invalid device detected! Reason: " + lastError.ToString());
          else
            MBusList.MBusScannerLogger.Error("REQ_UD2 failed! Reason: " + lastError.ToString());
          ZR_ClassLibMessages.ClearErrors();
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
          return false;
        }
        for (int index = 0; index < this.bus.Count; ++index)
        {
          if (((BusDevice) this.bus[index]).Info != null && (int) ((BusDevice) this.bus[index]).Info.ManufacturerCode == (int) NewDevice.Info.ManufacturerCode && (int) ((BusDevice) this.bus[index]).Info.Medium == (int) NewDevice.Info.Medium && (int) ((BusDevice) this.bus[index]).Info.Version == (int) NewDevice.Info.Version && ((BusDevice) this.bus[index]).Info.MeterNumber == NewDevice.Info.MeterNumber)
            return true;
        }
        NewDevice.PrimaryDeviceAddress = NewDevice.Info.A_Field;
        NewDevice.PrimaryAddressKnown = true;
        this.AddDevice((object) NewDevice, true);
        return true;
      }
      MBusList.MBusScannerLogger.Debug("M-Bus device can not selected!");
      return false;
    }

    internal override bool ScanFromAddress(int ScanAddress)
    {
      bool flag = false;
      this.MyBus.BreakRequest = false;
      this.MyBus.BusState.StartGlobalFunctionTask(BusStatusClass.GlobalFunctionTasks.ScanPrimary);
      while (!this.MyBus.BreakRequest)
      {
        if (ScanAddress > 251)
        {
          this.MyBus.SendMessage(new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage));
          break;
        }
        if (this.MyBus.MyBusMode != BusMode.MBusPointToPoint || this.bus.Count <= 0)
        {
          if (this.MyBus.MyBusMode != 0)
          {
            GMM_EventArgs e = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
            e.ProgressPercentage = ScanAddress * 100 / 251;
            e.EventMessage = string.Format(DeviceCollectorFunctions.SerialBusMessage.GetString("PrimaryAddressSearch"), (object) ScanAddress, (object) 251, (object) this.bus.Count);
            this.MyBus.SendMessage(e);
            this.MyBus.SendProgress((object) this, e.ProgressPercentage);
            this.MyBus.SendProgressMessage((object) this, string.Format("Scan: {0} - {1}", (object) ScanAddress, (object) 251));
          }
          if (this.SearchSingleDeviceByPrimaryAddress(ScanAddress++) || ZR_ClassLibMessages.GetLastError() != ZR_ClassLibMessages.LastErrors.CommunicationError && ZR_ClassLibMessages.GetLastError() != ZR_ClassLibMessages.LastErrors.ComOpenError)
            Application.DoEvents();
          else
            goto label_10;
        }
        else
          break;
      }
      flag = true;
label_10:
      this.MyBus.BusState.StartGlobalFunctionTask(BusStatusClass.GlobalFunctionTasks.Off);
      return flag;
    }

    internal override bool ScanFromSerialNumber(string StartSerialNumber)
    {
      this.MyBus.BreakRequest = false;
      bool flag = true;
      DateTime dateTimeNow1 = SystemValues.DateTimeNow;
      if (string.IsNullOrEmpty(StartSerialNumber))
        StartSerialNumber = "0fffffff";
      else if (StartSerialNumber.ToLower().StartsWith("f"))
        flag = false;
      int maxRequestRepeat = this.MyBus.MaxRequestRepeat;
      this.MyBus.BusState.StartGlobalFunctionTask(BusStatusClass.GlobalFunctionTasks.ScanSecundary);
      string ParameterValue = this.MyBus.MyCom.SingleParameter(CommParameter.TestEcho, "");
      try
      {
        if (!this.MyBus.MyCom.Open())
          return false;
        this.MyBus.MyCom.SingleParameter(CommParameter.TestEcho, "False");
        MBusList.MBusScannerLogger.Info("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        MBusList.MBusScannerLogger.Info<string, int>("M-Bus scanner was started! (StartSerialNumber: {0}, MaxRepeat {1})", StartSerialNumber, maxRequestRepeat);
        MBusList.MBusScannerLogger.Info("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        MBusList.MBusScannerLogger.Info(" ");
        uint SerialNumberOut;
        if (!MBusDevice.StringToMBusSerialNumber(StartSerialNumber, out SerialNumberOut))
          return false;
        int num = 0;
        Queue<uint> uintQueue = new Queue<uint>();
        while (!this.MyBus.BreakRequest && (this.MyBus.MyBusMode != BusMode.MBusPointToPoint || this.bus.Count <= 0))
        {
          bool collision = false;
          DateTime dateTimeNow2 = SystemValues.DateTimeNow;
          while (!this.MyBus.BreakRequest)
          {
            if (flag)
            {
              string str = SerialNumberOut.ToString("X8");
              int progress = ((int) str[0] - 48) * 90 / 9;
              if (str[1] != 'F')
                progress += ((int) str[1] - 48) * 10 / 9;
              this.MyBus.SendProgress((object) this, progress);
              this.MyBus.SendProgressMessage((object) this, "Scan: " + str);
            }
            else
            {
              string str = SerialNumberOut.ToString("X8");
              int progress = ((int) str[7] - 48) * 90 / 9;
              if (str[6] != 'F')
                progress += ((int) str[6] - 48) * 10 / 9;
              this.MyBus.SendProgress((object) this, progress);
              this.MyBus.SendProgressMessage((object) this, "Scan: " + str);
            }
            MBusList.MBusScannerLogger.Info("START SECUNDARY ADDRESS SCANN-----------------------------------> {0}", SerialNumberOut.ToString("X08"));
            dateTimeNow2 = SystemValues.DateTimeNow;
            if (!this.SearchSingleDeviceBySerialNumber(SerialNumberOut))
            {
              ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
              if (lastError == ZR_ClassLibMessages.LastErrors.IllegalData || lastError == ZR_ClassLibMessages.LastErrors.FramingError)
              {
                collision = true;
                ++num;
                MBusList.MBusScannerLogger.Error("Collision detected!");
                break;
              }
              if (lastError == ZR_ClassLibMessages.LastErrors.CommunicationError)
              {
                while (!this.MyBus.BreakRequest)
                {
                  MBusList.MBusScannerLogger.Fatal("Connection was unexpected closed! Try reconnect.");
                  this.MyBus.ComClose();
                  if (this.MyBus.ComOpen())
                    break;
                }
              }
              else
              {
                uintQueue.Enqueue(SerialNumberOut);
                break;
              }
            }
            else
            {
              Application.DoEvents();
              break;
            }
          }
          MBusList.MBusScannerLogger.Info("END SECUNDARY ADDRESS SCANN-------------------------------------> {0}", ZR_ClassLibrary.Util.ElapsedToString(SystemValues.DateTimeNow - dateTimeNow2));
          MBusList.MBusScannerLogger.Info(" ");
          MBusList.MBusScannerLogger.Info("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
          MBusList.MBusScannerLogger.Info(" ");
          if (flag)
          {
            if (!this.ScanWalkerSetNextAddressFirstToLast(ref SerialNumberOut, collision))
              break;
          }
          else if (!this.ScanWalkerSetNextAddressLastToFirst(ref SerialNumberOut, collision))
            break;
        }
        if (MBusList.MBusScannerLogger.IsTraceEnabled)
        {
          MBusList.MBusScannerLogger.Trace("Devices not found:");
          while (uintQueue.Count > 0)
            MBusList.MBusScannerLogger.Trace(uintQueue.Dequeue().ToString("X08"));
        }
        string str1 = string.Format("Elapsed time: {0}, Founds: {1}, Collisions: {2}", (object) ZR_ClassLibrary.Util.ElapsedToString(SystemValues.DateTimeNow.Subtract(dateTimeNow1)), (object) this.bus.Count, (object) num);
        if (this.MyBus.BreakRequest)
          MBusList.MBusScannerLogger.Info("M-Bus scanner was canceled! ({0})", str1);
        else
          MBusList.MBusScannerLogger.Info("M-Bus scanner was finished! ({0})", str1);
        this.MyBus.SendProgress((object) this, 100);
        ZR_ClassLibMessages.ClearErrors();
        return true;
      }
      finally
      {
        this.MyBus.MyCom.SingleParameter(CommParameter.TestEcho, ParameterValue);
        this.MyBus.BusState.StartGlobalFunctionTask(BusStatusClass.GlobalFunctionTasks.Off);
      }
    }

    internal override bool WorkBusAddresses()
    {
      this.MBusConverterAvailable = false;
      if (this.bus.Count == 1)
      {
        if (this.bus[0] is Receiver)
          return false;
        if (this.bus[0] is BusDevice)
        {
          DeviceTypes deviceType = ((BusDevice) this.bus[0]).DeviceType;
          if (deviceType == DeviceTypes.HumiditySensor || deviceType == DeviceTypes.TemperatureSensor)
            return false;
        }
      }
      this.PrimaryAddressingOk = true;
      this.AllAddressesOk = true;
      for (int index1 = 0; index1 < this.bus.Count; ++index1)
      {
        if (this.MyBus.MyBusMode != BusMode.WaveFlowRadio && this.MyBus.MyBusMode != BusMode.MinomatV2)
        {
          if (!((MBusDevice) this.bus[index1]).PrimaryAddressKnown)
          {
            this.PrimaryAddressingOk = false;
            break;
          }
          ((MBusDevice) this.bus[index1]).PrimaryAddressOk = true;
          for (int index2 = 0; index2 < this.bus.Count; ++index2)
          {
            if (index1 != index2 && (int) ((MBusDevice) this.bus[index1]).PrimaryDeviceAddress == (int) ((MBusDevice) this.bus[index2]).PrimaryDeviceAddress)
            {
              ((MBusDevice) this.bus[index1]).PrimaryAddressOk = false;
              this.AllAddressesOk = false;
              break;
            }
          }
          if (((MBusDevice) this.bus[index1]).PrimaryAddressOk && ((MBusDevice) this.bus[index1]).PrimaryDeviceAddress == (byte) 251)
            this.MBusConverterAvailable = true;
        }
      }
      if (!this.PrimaryAddressingOk)
      {
        this.AllAddressesOk = false;
        for (int index = 0; index < this.bus.Count; ++index)
          ((MBusDevice) this.bus[index]).PrimaryAddressOk = false;
      }
      return this.AllAddressesOk;
    }

    internal override bool OrganizeBus(int StartAddress)
    {
      this.MyBus.DeviceIsModified = true;
      this.WorkBusAddresses();
      this.MyBus.BreakRequest = false;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        if (this.MyBus.BreakRequest)
          return false;
        this.MyBus.SendMessage("Check device: ", index + 1, GMM_EventArgs.MessageType.StandardMessage);
        if (!((MBusDevice) this.bus[index]).REQ_UD2())
        {
          int num = (int) MessageBox.Show("Device check error!");
          return false;
        }
      }
      this.WorkBusAddresses();
      if (!this.PrimaryAddressingOk)
      {
        int num = (int) MessageBox.Show("Addressing not ok");
        return false;
      }
      int num1 = StartAddress;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        if (this.MyBus.BreakRequest)
          return false;
        if (!((MBusDevice) this.bus[index]).PrimaryAddressOk)
        {
          this.MyBus.SendMessage("Address device: ", index + 1, GMM_EventArgs.MessageType.StandardMessage);
          num1 = this.GetNextFreeAddress(num1);
          if (num1 < 0 || !((MBusDevice) this.bus[index]).SetPrimaryAddress(num1))
            return false;
          this.WorkBusAddresses();
        }
      }
      if (!this.PrimaryAddressingOk || !this.AllAddressesOk)
      {
        int num2 = (int) MessageBox.Show("Full Addressing not ok");
        return false;
      }
      for (int index1 = 0; index1 < StartAddress; ++index1)
      {
        for (int index2 = 0; index2 < this.bus.Count; ++index2)
        {
          if ((int) ((MBusDevice) this.bus[index2]).PrimaryDeviceAddress == index1)
          {
            this.MyBus.SendMessage("Shift device: ", index2 + 1, GMM_EventArgs.MessageType.StandardMessage);
            num1 = this.GetNextFreeAddress(num1);
            if (num1 < 0 || !((MBusDevice) this.bus[index2]).SetPrimaryAddress(num1))
              return false;
          }
        }
      }
      int index3;
      do
      {
        num1 = this.GetNextFreeAddress(num1);
        if (num1 >= 0)
        {
          int num3 = 0;
          index3 = 0;
          for (int index4 = 0; index4 < this.bus.Count; ++index4)
          {
            if ((int) ((MBusDevice) this.bus[index4]).PrimaryDeviceAddress > num3)
            {
              num3 = (int) ((MBusDevice) this.bus[index4]).PrimaryDeviceAddress;
              index3 = index4;
            }
          }
          if (num1 >= num3)
            goto label_39;
        }
        else
          goto label_29;
      }
      while (((MBusDevice) this.bus[index3]).SetPrimaryAddress(num1));
      goto label_37;
label_29:
      return false;
label_37:
      return false;
label_39:
      return true;
    }

    internal override bool SetPrimaryAddressOnBusWithoutShift(int NewAddress)
    {
      this.WorkBusAddresses();
      bool flag = ((MBusDevice) this.SelectedDevice).SetPrimaryAddress(NewAddress);
      this.WorkBusAddresses();
      return flag;
    }

    internal override bool SetPrimaryAddressOnBus(int NewAddress)
    {
      this.WorkBusAddresses();
      for (int index1 = 0; index1 < this.bus.Count; ++index1)
      {
        if (this.bus[index1] != this.SelectedDevice && (int) ((MBusDevice) this.bus[index1]).PrimaryDeviceAddress == NewAddress)
        {
          for (int index2 = index1 + 1; index2 < this.bus.Count; ++index2)
          {
            if (this.bus[index2] != this.SelectedDevice && (int) ((MBusDevice) this.bus[index2]).PrimaryDeviceAddress == NewAddress)
              goto label_10;
          }
          int nextFreeAddress = this.GetNextFreeAddress(NewAddress);
          if (nextFreeAddress < 0 || !((MBusDevice) this.bus[index1]).SetPrimaryAddress(nextFreeAddress))
            return false;
          break;
        }
      }
label_10:
      bool flag = ((MBusDevice) this.SelectedDevice).SetPrimaryAddress(NewAddress);
      this.WorkBusAddresses();
      return flag;
    }

    private bool ScanWalkerSetNextAddressFirstToLast(ref uint address, bool collision)
    {
      string str = address.ToString("X8");
      int length = str.IndexOf('F');
      if (length < 0)
      {
        if (Convert.ToByte(str[7].ToString()) < (byte) 9)
        {
          ++address;
          return true;
        }
        collision = false;
        length = 7;
      }
      if (collision && length < 8)
      {
        string s = (str.Substring(0, length) + "0").PadRight(8, 'F');
        address = uint.Parse(s, NumberStyles.HexNumber);
        return true;
      }
      byte num1 = Convert.ToByte(str[length - 1].ToString());
      if (num1 < (byte) 9)
      {
        string s = (str.Substring(0, length - 1) + ((int) num1 + 1).ToString()).PadRight(8, 'F');
        address = uint.Parse(s, NumberStyles.HexNumber);
        return true;
      }
      while (length > 1)
      {
        --length;
        byte num2 = Convert.ToByte(str[length - 1].ToString());
        if (num2 < (byte) 9)
        {
          string s = (str.Substring(0, length - 1) + ((int) num2 + 1).ToString()).PadRight(8, 'F');
          address = uint.Parse(s, NumberStyles.HexNumber);
          return true;
        }
      }
      return false;
    }

    private bool ScanWalkerSetNextAddressLastToFirst(ref uint address, bool collision)
    {
      uint address1 = uint.Parse(ZR_ClassLibrary.Util.ReverseString(address.ToString("X8")), NumberStyles.AllowHexSpecifier);
      if (!this.ScanWalkerSetNextAddressFirstToLast(ref address1, collision))
        return false;
      string s = ZR_ClassLibrary.Util.ReverseString(address1.ToString("X8"));
      address = uint.Parse(s, NumberStyles.AllowHexSpecifier);
      return true;
    }

    private int GetNextFreeAddress(int FromAddress)
    {
      int nextFreeAddress = FromAddress;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        if ((int) ((MBusDevice) this.bus[index]).PrimaryDeviceAddress == nextFreeAddress)
        {
          ++nextFreeAddress;
          index = -1;
          if (nextFreeAddress == 250)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Addressing overrun!");
            return -1;
          }
        }
      }
      return nextFreeAddress;
    }

    internal virtual bool IsNewMBusDevicePossible()
    {
      if (this.bus.Count < 1 && this.MyBus.MyBusMode == BusMode.MBusPointToPoint || this.MyBus.MyBusMode == BusMode.MBus)
        return true;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "No further device possible!");
      return false;
    }
  }
}
