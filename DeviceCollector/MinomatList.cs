// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MinomatList
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using GmmDbLib;
using NLog;
using StartupLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MinomatList : DeviceList
  {
    public Minomat _minomat;
    private static Logger logger = LogManager.GetLogger(nameof (MinomatList));
    private bool _readoutDataValid = false;
    private byte _lastRequestedDataType = 0;
    private DateTime _lastStartDate = SystemValues.DateTimeNow;
    private DateTime _lastStopDate = SystemValues.DateTimeNow;
    private bool _minomatConnectionIsOpen;
    private bool _listIsValid;
    private string _minomatSerial;

    public bool IsConnected => this._minomatConnectionIsOpen;

    public MinomatList(DeviceCollectorFunctions BusRef)
    {
      this.MyBus = BusRef;
      this.bus = new ArrayList();
      this.FaultyDevices = new List<MBusDevice>();
      this._minomatConnectionIsOpen = false;
    }

    public bool ConnectToMinomat()
    {
      this.MyBus.BreakRequest = false;
      if (string.IsNullOrEmpty(this.MyBus.DaKonId) && this._minomatSerial == null)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "DaKonId is missing!");
        return false;
      }
      if (this.MyBus.DaKonId != this._minomatSerial)
      {
        this._minomatSerial = this.MyBus.DaKonId;
        this._minomat = (Minomat) new MinomatV2(this.MyBus);
        this._minomat.minomatSerial = this._minomatSerial;
        this._minomat.minomatPassword = "3414";
        this._minomatConnectionIsOpen = false;
      }
      this.MyBus.SendProgressMessage((object) this, "Wake up ...");
      if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoConnect && this.MyBus.MyCom.IsOpen && this._minomat.Connect())
        return true;
      if (!this.MyBus.MyCom.Open())
        return false;
      if (this.MyBus.MyCom.Transceiver != TransceiverDevice.MinoConnect)
        this.MyBus.MyCom.CallTransceiverFunction(TransceiverDeviceFunction.TransparentModeOn);
      this._minomatConnectionIsOpen = this._minomat.Connect();
      if (!this._minomatConnectionIsOpen)
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, Ot.Gtm(Tg.CommunicationLogic, "FailedConnectToMinoat", "Could not connect to Minomat"));
      return this._minomatConnectionIsOpen;
    }

    public void DisconnectFromMinomat()
    {
      this._minomatConnectionIsOpen = false;
      this._readoutDataValid = false;
    }

    private bool GetMinomatRTC(out DateTime systemTime)
    {
      systemTime = new DateTime();
      return this.ConnectToMinomat() && this._minomat.GetDateTime(out systemTime);
    }

    internal bool SetMinomatRTC(DateTime dateTime)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      if (!this.ConnectToMinomat())
        return false;
      this._minomat.SetDateTime(dateTime);
      return true;
    }

    private bool GetMinomatStatus(out object status)
    {
      status = (object) null;
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      return this.ConnectToMinomat() && this._minomat.GetSystemStatus(out status);
    }

    private bool GetMinomatConfiguration(out object config)
    {
      config = (object) null;
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      return this.ConnectToMinomat() && this._minomat.GetConfiguration(out config);
    }

    public bool ReadMinomat()
    {
      if (this._readoutDataValid && this._minomatSerial == this.MyBus.DaKonId && this._lastStartDate == this.MyBus.ReadFromTime && this._lastStopDate == this.MyBus.ReadToTime && this._lastRequestedDataType == (byte) 3)
        return true;
      DateTime readFromTime = this.MyBus.ReadFromTime;
      DateTime dateTime = this.MyBus.ReadToTime;
      byte requestedDataType = 3;
      if (dateTime > SystemValues.DateTimeNow + TimeSpan.FromDays(31.0))
        dateTime = SystemValues.DateTimeNow + TimeSpan.FromDays(31.0);
      if (readFromTime > dateTime)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Illegal reading time span");
        return false;
      }
      MinomatList.logger.Debug<DateTime, DateTime>("Read values from {0} until {1}", readFromTime, dateTime);
      int months = dateTime.Month - readFromTime.Month + (dateTime.Year - readFromTime.Year) * 12;
      int month1 = dateTime.Month;
      DateTime dateTimeNow = SystemValues.DateTimeNow;
      int month2 = dateTimeNow.Month;
      int num1 = month1 - month2;
      int year1 = dateTime.Year;
      dateTimeNow = SystemValues.DateTimeNow;
      int year2 = dateTimeNow.Year;
      int num2 = (year1 - year2) * 12;
      int monthOffset = num1 + num2;
      if (monthOffset > 0)
        monthOffset = 0;
      if (months > 18)
        months = 18;
      if (months == 0)
        months = 1;
      MinomatList.logger.Debug("months: " + months.ToString() + " monthOffset: " + monthOffset.ToString());
      this._readoutDataValid = this.ReadMinomat(requestedDataType, months, monthOffset);
      if (this._readoutDataValid)
      {
        ZR_ClassLibMessages.ClearErrors();
        this._lastStartDate = this.MyBus.ReadFromTime;
        this._lastStopDate = this.MyBus.ReadToTime;
        this._lastRequestedDataType = requestedDataType;
      }
      return this._readoutDataValid;
    }

    private bool ReadMinomat(byte requestedDataType, int months, int monthOffset)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      if (!this.ConnectToMinomat())
        return false;
      byte slaveIndex = 0;
      Dictionary<string, Minomat.MinomatDeviceValue> processedMinomatDevices;
      Dictionary<string, byte> hkveSerialNumbersAsStrings;
      if (!this._minomat.Read(requestedDataType, slaveIndex, months, monthOffset, out processedMinomatDevices, out hkveSerialNumbersAsStrings))
        return false;
      if (hkveSerialNumbersAsStrings == null)
        return true;
      foreach (KeyValuePair<string, byte> keyValuePair in hkveSerialNumbersAsStrings)
      {
        Minomat.MinomatDeviceValue minomatDeviceValue;
        if (processedMinomatDevices.TryGetValue(keyValuePair.Key, out minomatDeviceValue))
        {
          MinomatDevice minomatDevice1 = (MinomatDevice) null;
          for (int index = 0; index < this.MyBus.MyDeviceList.bus.Count; ++index)
          {
            if (((BusDevice) this.MyBus.MyDeviceList.bus[index]).Info.MeterNumber == keyValuePair.Key)
            {
              minomatDevice1 = (MinomatDevice) this.MyBus.MyDeviceList.bus[index];
              break;
            }
          }
          if (minomatDevice1 == null)
          {
            MinomatDevice minomatDevice2 = new MinomatDevice(this.MyBus);
            minomatDevice2.PrimaryDeviceAddress = keyValuePair.Value;
            minomatDevice2.PrimaryAddressOk = true;
            minomatDevice2.PrimaryAddressKnown = true;
            minomatDevice2.Info.MeterNumber = keyValuePair.Key;
            if (minomatDeviceValue.configValues.ContainsKey(OverrideID.DiagnosticString) && minomatDeviceValue.configValues[OverrideID.DiagnosticString].ParameterValue != null)
            {
              string str = minomatDeviceValue.configValues[OverrideID.DiagnosticString].ParameterValue.ToString();
              if (!string.IsNullOrEmpty(str))
              {
                int num1 = str.IndexOf("Status=");
                if (num1 >= 0)
                {
                  int startIndex = num1 + 7;
                  if (startIndex + 2 <= str.Length)
                  {
                    string strValue = str.Substring(startIndex, 2);
                    byte num2 = 0;
                    if (ZR_ClassLibrary.Util.TryParseToByte(strValue, out num2))
                      minomatDevice2.Info.Status = num2;
                  }
                }
              }
            }
            minomatDevice2.readoutValues = minomatDeviceValue.readoutValues;
            minomatDevice2.configValues = minomatDeviceValue.configValues;
            this.bus.Add((object) minomatDevice2);
          }
          else if (minomatDeviceValue.readoutValues.Count > minomatDevice1.readoutValues.Count || minomatDeviceValue.configValues.Count > minomatDevice1.configValues.Count)
            MinomatList.logger.Debug("Error! ReadMinomat -> Got more readoutvalues than expected!");
        }
      }
      return true;
    }

    public bool SystemInit()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      return this.ConnectToMinomat() && this._minomat.SystemInit();
    }

    public bool StartHKVEReceptionWindow()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      return this.ConnectToMinomat() && this._minomat.StartHKVEReceptionWindow();
    }

    internal bool StopHKVEReceptionWindow()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      return this.ConnectToMinomat() && this._minomat.StopReception();
    }

    public bool RegisterHKVE(List<MinomatDevice> deviceList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      return this.ConnectToMinomat() && this._minomat.RegisterHKVE(deviceList);
    }

    public bool DeRegisterHKVE(List<MinomatDevice> deviceList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      return this.ConnectToMinomat() && this._minomat.DeRegisterHKVE(deviceList);
    }

    internal bool SetConfiguration(MinomatV2.Configuration configuration)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return false;
      }
      return this.ConnectToMinomat() && this._minomat.SetConfiguration(configuration);
    }

    internal MinomatV2.Configuration GetConfiguration()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return (MinomatV2.Configuration) null;
      }
      object config;
      return !this.ConnectToMinomat() || !this._minomat.GetConfiguration(out config) ? (MinomatV2.Configuration) null : (MinomatV2.Configuration) config;
    }

    internal MinomatV2.SystemStatus GetSystemStatus()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MinomatV2))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatV2!");
        return (MinomatV2.SystemStatus) null;
      }
      object systemStatus;
      return !this.ConnectToMinomat() || !this._minomat.GetSystemStatus(out systemStatus) ? (MinomatV2.SystemStatus) null : (MinomatV2.SystemStatus) systemStatus;
    }

    internal void SendStatusMessage(string message, int id, GMM_EventArgs.MessageType TheEvent)
    {
      this.MyBus.SendMessage(message, id, TheEvent);
    }

    internal bool GetAllRegisteredDevices(
      out List<MinomatDevice> devices,
      byte startAddress,
      byte endAddress)
    {
      devices = (List<MinomatDevice>) null;
      if (this._listIsValid)
        return true;
      return this.ConnectToMinomat() && this._minomat.GetAllRegisteredDevices(out devices, startAddress, endAddress);
    }

    internal override bool AddDevice(DeviceTypes NewType, bool select)
    {
      if (NewType != DeviceTypes.MinomatDevice)
        throw new NotImplementedException();
      object obj = (object) new MinomatDevice(this.MyBus);
      bool flag = false;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        if (((BusDevice) this.bus[index]).Info.MeterNumber == ((BusDevice) obj).Info.MeterNumber)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.bus.Add(obj);
      if (select)
        this.SelectedDevice = (BusDevice) this.bus[this.bus.Count - 1];
      return true;
    }

    internal override bool AddDevice(object NewDevice, bool select)
    {
      bool flag = false;
      for (int index = 0; index < this.bus.Count; ++index)
      {
        if (((BusDevice) this.bus[index]).Info.MeterNumber == ((BusDevice) NewDevice).Info.MeterNumber)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.bus.Add(NewDevice);
      if (select)
        this.SelectedDevice = (BusDevice) this.bus[this.bus.Count - 1];
      return true;
    }

    internal override bool ScanFromAddress(int ScanAddress) => this.ReadMinomat();

    internal override bool SearchSingleDeviceByPrimaryAddress(int SearchAddress)
    {
      if (SearchAddress < 0 || SearchAddress > 250 && SearchAddress != 254)
        return false;
      bool flag = false;
      this.MyBus.BreakRequest = false;
      List<MinomatDevice> devices;
      if (this.GetAllRegisteredDevices(out devices, (byte) SearchAddress, (byte) SearchAddress))
      {
        for (int index = 0; index < devices.Count; ++index)
          this.AddDevice((object) devices[index], true);
        flag = true;
      }
      return flag;
    }

    internal override bool SearchSingleDeviceBySerialNumber(string SearchSerialNumber)
    {
      long serialOfHKVE = 0;
      bool flag1 = true;
      bool flag2 = true;
      if (flag1)
      {
        if (SearchSerialNumber.Length < 8)
        {
          int num = 8 - SearchSerialNumber.Length;
          for (int index = 0; index < num; ++index)
            SearchSerialNumber = "0" + SearchSerialNumber;
        }
      }
      else if (SearchSerialNumber.Length < 8)
        return false;
      if (flag2)
      {
        int[] numArray = new int[8]
        {
          28,
          24,
          20,
          16,
          12,
          8,
          4,
          0
        };
        for (int index = 0; index < 8; ++index)
        {
          if (SearchSerialNumber[index] < '0' || SearchSerialNumber[index] > '9')
            return false;
          long num = (long) ((int) SearchSerialNumber[index] - 48);
          serialOfHKVE |= num << numArray[index];
        }
      }
      else
        serialOfHKVE = long.Parse(SearchSerialNumber);
      string answer;
      if (this._minomat.FindHKVE((ulong) serialOfHKVE, out answer))
      {
        int num1 = (int) MessageBox.Show(answer);
      }
      else
      {
        int num2 = (int) MessageBox.Show("error");
      }
      return true;
    }

    internal override void DeleteBusList()
    {
      this._listIsValid = false;
      this._readoutDataValid = false;
      base.DeleteBusList();
    }

    internal override bool DeleteSelectedDevice()
    {
      if (!(this.SelectedDevice is MinomatDevice selectedDevice))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled, "No device is selected!");
        return false;
      }
      if (!this.ConnectToMinomat())
        return false;
      bool flag = this._minomat.DeRegisterHKVE(selectedDevice);
      this.bus.Remove((object) this.SelectedDevice);
      this.SelectedDevice = (BusDevice) null;
      return flag;
    }

    internal override bool GetDeviceCollectorInfo(out object InfoObject)
    {
      InfoObject = (object) new Minomat.MinomatInfo();
      return this.GetMinomatConfiguration(out ((Minomat.MinomatInfo) InfoObject).configuration) && this.GetMinomatStatus(out ((Minomat.MinomatInfo) InfoObject).systemStatus) && this.GetMinomatRTC(out ((Minomat.MinomatInfo) InfoObject).systemTime);
    }

    public enum MinomatReadMode
    {
      EventData,
      MonthlyData,
      DailyData,
    }
  }
}
