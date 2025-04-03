// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MinomatV2
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MinomatV2 : Minomat
  {
    private static Logger logger = LogManager.GetLogger(nameof (MinomatV2));
    private DeviceCollectorFunctions MyBus;
    private MinomatV2.CCommunication _communication;
    private MinomatV2.CCommandArray MyCCommandArray = new MinomatV2.CCommandArray();
    private static byte[] FRAME_PREFIX = new byte[5]
    {
      (byte) 85,
      (byte) 85,
      (byte) 85,
      (byte) 85,
      (byte) 15
    };
    private const byte NUMBER_OF_DEVICES_CONNECTED_TO_DAKON_SLAVE = 200;
    private const byte NUMBER_OF_SLAVES_CONNECTED_TO_DAKON_MASTER = 20;
    private const int M5P_F_KFACTOR = 1000;
    private const int PERS_FIELDSTRENGTHDATA = 1;
    private const int PERS_INTERMEDIATEDATA = 2;
    private const int PERS_KEYDATEDATA = 4;
    private const int PERS_MONTHLYDATA = 8;

    public MinomatV2(DeviceCollectorFunctions SerialBus)
    {
      this.MyBus = SerialBus;
      this._communication = (MinomatV2.CCommunication) new MinomatV2.CSerialCommunication(this.MyBus);
      this.MyCCommandArray = new MinomatV2.CCommandArray();
    }

    internal override bool GetDateTime(out DateTime dateTime)
    {
      dateTime = new DateTime();
      MinomatV2.logger.Debug("GetDateTime() called");
      MinomatV2.CCommandGetTime ccommandGetTime = new MinomatV2.CCommandGetTime(this._minomatSerial, this._minomatPassword);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) ccommandGetTime);
      if (this._communication.sendCommands(ref this.MyCCommandArray) && ccommandGetTime.hasReceivedResponse())
      {
        Minomat.CCommandTime time = ccommandGetTime.getTime();
        Minomat.CCommandDate date = ccommandGetTime.getDate();
        dateTime = new DateTime((int) date.getYear() + 2000, (int) date.getMonth(), (int) date.getDay(), (int) time.getHours(), (int) time.getMinutes(), (int) time.getSeconds());
        MinomatV2.logger.Debug(string.Format("GetDateTime() successfull: dateTime is {0}", (object) dateTime.ToString()));
        return true;
      }
      MinomatV2.logger.Error("GetDateTime() failed");
      return false;
    }

    internal override bool SetDateTime(DateTime dateTime)
    {
      MinomatV2.logger.Debug("Try to set the device clock: " + dateTime.ToShortDateString() + " " + dateTime.ToLongTimeString());
      MinomatV2.CCommandSetTime ccommandSetTime = new MinomatV2.CCommandSetTime(this._minomatSerial, this._minomatPassword, new Minomat.CCommandTime((byte) dateTime.Hour, (byte) dateTime.Minute, (byte) dateTime.Second), new Minomat.CCommandDate((byte) dateTime.Day, (byte) dateTime.Month, byte.Parse(dateTime.ToString("yy"))));
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) ccommandSetTime);
      if (!this._communication.sendCommands(ref this.MyCCommandArray, false))
      {
        string str = "Failed set the device clock!";
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        MinomatV2.logger.Error(str);
        return false;
      }
      MinomatV2.logger.Debug("The device clock was successfully updated!");
      return true;
    }

    internal override bool SetConfiguration(MinomatV2.Configuration configuration)
    {
      MinomatV2.logger.Debug<MinomatV2.Configuration>("SetConfiguration: {0}", configuration);
      MinomatV2.CCommandSetConfiguration setConfiguration = new MinomatV2.CCommandSetConfiguration(this._minomatSerial, this._minomatPassword, configuration);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) setConfiguration);
      if (!this._communication.sendCommands(ref this.MyCCommandArray, false))
      {
        string str = "Failed to set configuration!";
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        MinomatV2.logger.Error(str);
        return false;
      }
      MinomatV2.logger.Debug("The device configuration was successfully updated!");
      return true;
    }

    internal override bool StopReception()
    {
      MinomatV2.logger.Debug("StopReception() called");
      MinomatV2.CCommandStopReception ccommandStopReception = new MinomatV2.CCommandStopReception(this._minomatSerial, this._minomatPassword);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) ccommandStopReception);
      if (this._communication.sendCommands(ref this.MyCCommandArray))
      {
        MinomatV2.logger.Debug("StopReception() successfull");
        return true;
      }
      MinomatV2.logger.Error("StopReception() failed");
      return false;
    }

    internal override bool Read(
      byte dataType,
      byte slaveIndex,
      int months,
      int monthOffset,
      out Dictionary<string, Minomat.MinomatDeviceValue> processedMinomatDevices,
      out Dictionary<string, byte> hkveSerialNumbersAsStrings)
    {
      hkveSerialNumbersAsStrings = new Dictionary<string, byte>();
      processedMinomatDevices = new Dictionary<string, Minomat.MinomatDeviceValue>();
      byte startAddress = 0;
      List<MinomatDevice> deviceList = new List<MinomatDevice>();
      this.MyBus.SendProgressMessage((object) this, "Read list of registered devices...");
      if (!this.GetAllRegisteredDevices(out deviceList, startAddress, (byte) 200))
        return false;
      if (deviceList.Count == 0)
        return true;
      byte primaryDeviceAddress1 = deviceList[0].PrimaryDeviceAddress;
      byte primaryDeviceAddress2 = deviceList[deviceList.Count - 1].PrimaryDeviceAddress;
      if (MinomatV2.logger.IsTraceEnabled)
        MinomatV2.logger.Trace(string.Format("Read Minomat: startAddress->{0}, endAddress->{1}, months->{2}, monthOffset->{3}", (object) primaryDeviceAddress1, (object) primaryDeviceAddress2, (object) months, (object) monthOffset));
      MinomatV2.CCommandArray CommandArray = new MinomatV2.CCommandArray();
      MinomatV2.CCommandArray ccommandArray = new MinomatV2.CCommandArray();
      MinomatV2.CCommandArray commArray1 = new MinomatV2.CCommandArray();
      MinomatV2.CCommandArray commArray2 = new MinomatV2.CCommandArray();
      byte startBlock1 = (byte) Util.RoundDown((int) primaryDeviceAddress1, 4);
      byte endBlock1 = (byte) Util.RoundUp((int) primaryDeviceAddress2, 4);
      if (((int) dataType & 2) == 2 || ((int) dataType & 1) == 1)
        this.fillCommandArrayMonthlyDataDakon(ref commArray1, slaveIndex, startBlock1, endBlock1, months, monthOffset);
      byte startBlock2 = (byte) Util.RoundDown((int) primaryDeviceAddress1, 5);
      byte endBlock2 = (byte) Util.RoundUp((int) primaryDeviceAddress2, 5);
      if (((int) dataType & 1) == 1)
        this.fillCommandArrayEventDataDakon(ref commArray2, slaveIndex, startBlock2, endBlock2);
      for (int index = 0; index < commArray1.CCommands.Count; ++index)
        CommandArray.CCommands.Add(commArray1.CCommands[index]);
      for (int index = 0; index < commArray2.CCommands.Count; ++index)
        CommandArray.CCommands.Add(commArray2.CCommands[index]);
      bool flag = this._communication.sendCommands(ref CommandArray);
      if (flag)
      {
        MinomatV2.logger.Debug("Received {0} responces", CommandArray.CCommands.Count);
        if (((int) dataType & 2) == 2 || ((int) dataType & 1) == 1)
          this.processMonthlyDataDakon(commArray1, startBlock1, endBlock1, months, monthOffset, processedMinomatDevices, hkveSerialNumbersAsStrings);
        if (((int) dataType & 1) == 1)
          this.processEventDataDakon(commArray2, processedMinomatDevices, hkveSerialNumbersAsStrings, startBlock2, endBlock2);
      }
      else
        MinomatV2.logger.Error("sendCommands failed.");
      return flag;
    }

    internal override bool GetAllRegisteredDevices(
      out List<MinomatDevice> deviceList,
      byte startAddress,
      byte endAddress)
    {
      deviceList = new List<MinomatDevice>();
      MinomatV2.logger.Debug("Get all registered devices...");
      this.MyCCommandArray.CCommands.Clear();
      for (byte index = startAddress; (int) index < (int) endAddress; ++index)
        this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) new MinomatV2.CCommandCheckHKVERegistration(this._minomatSerial, this._minomatPassword, index));
      if (this._communication.sendCommands(ref this.MyCCommandArray))
      {
        for (int index = 0; index < this.MyCCommandArray.CCommands.Count; ++index)
        {
          if (this.MyCCommandArray.CCommands[index].hasReceivedResponse() && !this.MyCCommandArray.CCommands[index].hasError())
          {
            ulong num = ((MinomatV2.CCommandCheckHKVERegistration) this.MyCCommandArray.CCommands[index]).getSerialNoAnswer() & (ulong) uint.MaxValue;
            if (num != 0UL)
            {
              ((MinomatV2.CCommandCheckHKVERegistration) this.MyCCommandArray.CCommands[index]).getIndex();
              MinomatDevice minomatDevice = new MinomatDevice(this.MyBus);
              minomatDevice.PrimaryDeviceAddress = ((MinomatV2.CCommandCheckHKVERegistration) this.MyCCommandArray.CCommands[index]).getIndex();
              minomatDevice.Info.A_Field = minomatDevice.PrimaryDeviceAddress;
              minomatDevice.PrimaryAddressOk = true;
              minomatDevice.PrimaryAddressKnown = true;
              minomatDevice.Info.MeterNumber = ParameterService.ConvertInt32ToHexString((int) num);
              deviceList.Add(minomatDevice);
              if (MinomatV2.logger.IsDebugEnabled)
                MinomatV2.logger.Debug(string.Format("ID: {0}\tAddress: {1}", (object) minomatDevice.Info.MeterNumber, (object) minomatDevice.PrimaryDeviceAddress));
            }
          }
        }
        return true;
      }
      MinomatV2.logger.Error("GetAllRegisteredDevices() failed");
      return false;
    }

    internal override bool GetSystemStatus(out object systemStatus)
    {
      MinomatV2.logger.Debug("GetSystemStatus() called");
      systemStatus = (object) null;
      MinomatV2.CCommandGetSystemStatus ccommandGetSystemStatus = new MinomatV2.CCommandGetSystemStatus(this._minomatSerial, this._minomatPassword);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) ccommandGetSystemStatus);
      if (this._communication.sendCommands(ref this.MyCCommandArray) && !ccommandGetSystemStatus.hasError())
      {
        systemStatus = (object) ccommandGetSystemStatus.getSystemStatus();
        MinomatV2.logger.Debug("GetSystemStatus() succeeded: " + systemStatus.ToString());
        return true;
      }
      MinomatV2.logger.Error("GetSystemStatus() failed");
      return false;
    }

    internal override bool FindHKVE(ulong serialOfHKVE, out string answer)
    {
      MinomatV2.logger.Debug(string.Format("FindHKVE() for SN{0} called", (object) serialOfHKVE.ToString()));
      MinomatV2.CCommandFindHKVE ccommandFindHkve = new MinomatV2.CCommandFindHKVE(this._minomatSerial, this._minomatPassword, serialOfHKVE);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) ccommandFindHkve);
      answer = MinomatV2.CCommandFindHKVE.Answer.NOT_FOUND.ToString();
      if (this._communication.sendCommands(ref this.MyCCommandArray) && !ccommandFindHkve.hasError() && ccommandFindHkve.getAnswer() != 0)
      {
        MinomatV2.logger.Debug("FindHKVE() succeeded: " + answer);
        answer = ccommandFindHkve.getAnswer().ToString();
        return true;
      }
      MinomatV2.logger.Error("FindHKVE() failed");
      return false;
    }

    internal override bool RegisterHKVE(List<MinomatDevice> deviceList)
    {
      bool flag = true;
      foreach (MinomatDevice device in deviceList)
      {
        if (!Util.IsInteger(device.Info.MeterNumber))
        {
          string str = "Can not register the device! The serial number " + device.Info.MeterNumber + " is not valid!";
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
          MinomatV2.logger.Error(str);
        }
        else
        {
          uint uint32 = ParameterService.ConvertHexStringToUInt32(device.Info.MeterNumber);
          if (uint32 == 0U)
          {
            string str = "Can not register the device! The serial number " + device.Info.MeterNumber + " is not valid!";
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
            MinomatV2.logger.Error(str);
          }
          else
          {
            MinomatV2.CCommand command = (MinomatV2.CCommand) new MinomatV2.CCommandRegisterHKVE(this._minomatSerial, this._minomatPassword, (ulong) uint32, MinomatV2.CCommandRegisterHKVE.RegistrationType.REGISTER);
            if (!this._communication.sendCommand(ref command, true) & command.hasError())
            {
              string str = "Failed register the serial number " + device.Info.MeterNumber + "!";
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
              MinomatV2.logger.Error(str);
              flag = false;
            }
            else
            {
              switch (((MinomatV2.CCommandRegisterHKVE) command).getAnswer())
              {
                case MinomatV2.CCommandRegisterHKVE.Answer.OK:
                  MinomatV2.logger.Debug("Register {0} succeeded.", device.Info.MeterNumber);
                  break;
                case MinomatV2.CCommandRegisterHKVE.Answer.ALREADY_REGISTERED:
                  if (!this.DeRegisterHKVE(device))
                    return false;
                  if (!this._communication.sendCommand(ref command, true) & command.hasError())
                  {
                    string str = "Failed register the serial number " + device.Info.MeterNumber + "!";
                    ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
                    MinomatV2.logger.Error(str);
                    flag = false;
                    continue;
                  }
                  if (((MinomatV2.CCommandRegisterHKVE) command).getAnswer() != MinomatV2.CCommandRegisterHKVE.Answer.OK)
                  {
                    string str = "Can not register the serial number: " + device.Info.MeterNumber + " The device is already registered!";
                    ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
                    MinomatV2.logger.Debug(str);
                    flag = false;
                    break;
                  }
                  break;
                default:
                  throw new NotImplementedException();
              }
            }
          }
        }
      }
      return flag;
    }

    internal override bool DeRegisterHKVE(List<MinomatDevice> deviceList)
    {
      bool flag = true;
      foreach (MinomatDevice device in deviceList)
      {
        uint uint32 = ParameterService.ConvertHexStringToUInt32(device.Info.MeterNumber);
        if (uint32 == 0U)
        {
          string str = "Can not deregister a device! The serial number " + device.Info.MeterNumber + " is not valid!";
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
          MinomatV2.logger.Error(str);
          flag = false;
        }
        else
        {
          MinomatV2.CCommand command = (MinomatV2.CCommand) new MinomatV2.CCommandRegisterHKVE(this._minomatSerial, this._minomatPassword, (ulong) uint32, MinomatV2.CCommandRegisterHKVE.RegistrationType.REMOVE);
          if (!this._communication.sendCommand(ref command, true) & command.hasError())
          {
            string str = "Failed deregister the serialnumber " + device.Info.MeterNumber + "!";
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
            MinomatV2.logger.Error(str);
            flag = false;
          }
          else
          {
            switch (((MinomatV2.CCommandRegisterHKVE) command).getAnswer())
            {
              case MinomatV2.CCommandRegisterHKVE.Answer.OK:
                if (MinomatV2.logger.IsDebugEnabled)
                {
                  MinomatV2.logger.Debug("Deregister {0} succeeded.", device.Info.MeterNumber);
                  goto case MinomatV2.CCommandRegisterHKVE.Answer.NOT_FOUND;
                }
                else
                  goto case MinomatV2.CCommandRegisterHKVE.Answer.NOT_FOUND;
              case MinomatV2.CCommandRegisterHKVE.Answer.NOT_FOUND:
                continue;
              default:
                throw new NotImplementedException();
            }
          }
        }
      }
      return flag;
    }

    internal override bool CheckHKVERegistration(byte index, out ulong serialNo)
    {
      MinomatV2.logger.Debug(string.Format("CheckHKVERegistration() for index{0} called", (object) index));
      serialNo = 0UL;
      MinomatV2.CCommandCheckHKVERegistration hkveRegistration = new MinomatV2.CCommandCheckHKVERegistration(this._minomatSerial, this._minomatPassword, index);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) hkveRegistration);
      if (this._communication.sendCommands(ref this.MyCCommandArray) && !hkveRegistration.hasError() && hkveRegistration.hasReceivedResponse())
      {
        MinomatV2.logger.Debug("CheckHKVERegistration() succeeded, serial is " + serialNo.ToString());
        serialNo = hkveRegistration.getSerialNoAnswer();
        return true;
      }
      MinomatV2.logger.Error("CheckHKVERegistration() failed");
      return false;
    }

    internal override bool GetConfiguration(out object config)
    {
      MinomatV2.logger.Debug("GetConfiguration() called");
      config = (object) null;
      MinomatV2.CCommandGetConfiguration getConfiguration = new MinomatV2.CCommandGetConfiguration(this._minomatSerial, this._minomatPassword);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) getConfiguration);
      if (this._communication.sendCommands(ref this.MyCCommandArray) && !getConfiguration.hasError() && getConfiguration.hasReceivedResponse())
      {
        config = (object) getConfiguration.getConfiguration();
        return true;
      }
      MinomatV2.logger.Error("GetConfiguration() failed");
      return false;
    }

    public int RecTime_BeforFirstByte
    {
      get
      {
        if (this.MyBus.MyCom == null)
          return 2000;
        SortedList<AsyncComSettings, object> asyncComSettings = this.MyBus.MyCom.GetAsyncComSettings();
        return !asyncComSettings.ContainsKey(AsyncComSettings.RecTime_BeforFirstByte) || string.IsNullOrEmpty(asyncComSettings[AsyncComSettings.RecTime_BeforFirstByte].ToString()) ? 2000 : Convert.ToInt32(asyncComSettings[AsyncComSettings.RecTime_BeforFirstByte]);
      }
      set
      {
        if (this.MyBus.MyCom == null)
          return;
        this.MyBus.MyCom.SingleParameter(CommParameter.RecTime_BeforFirstByte, value.ToString());
      }
    }

    internal override bool SystemInit()
    {
      MinomatV2.logger.Debug("SystemInit called");
      MinomatV2.CCommandSystemInit ccommandSystemInit = new MinomatV2.CCommandSystemInit(this._minomatSerial, this._minomatPassword, 51647890UL);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) ccommandSystemInit);
      bool flag = false;
      int timeBeforFirstByte = this.RecTime_BeforFirstByte;
      if (timeBeforFirstByte < 6000)
        this.RecTime_BeforFirstByte = 6000;
      try
      {
        flag = this._communication.sendCommands(ref this.MyCCommandArray, true);
      }
      finally
      {
        this.RecTime_BeforFirstByte = timeBeforFirstByte;
      }
      if (flag && ccommandSystemInit.hasReceivedResponse())
      {
        if (ccommandSystemInit.isInvalidDeleteCode())
        {
          string str = "SystemInit failed! The delete code is invalid.";
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
          MinomatV2.logger.Error(str);
          return false;
        }
        if (!ccommandSystemInit.hasError())
          return true;
        string str1 = "SystemInit failed! Unknown error.";
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str1);
        MinomatV2.logger.Error(str1);
        return false;
      }
      MinomatV2.logger.Error("SystemInit failed");
      return false;
    }

    internal override bool StartHKVEReceptionWindow()
    {
      MinomatV2.logger.Debug("StartHKVEReceptionWindow() called");
      MinomatV2.CCommandStartHKVEReceptionWindow hkveReceptionWindow = new MinomatV2.CCommandStartHKVEReceptionWindow(this._minomatSerial, this._minomatPassword);
      this.MyCCommandArray.CCommands.Clear();
      this.MyCCommandArray.CCommands.Add((MinomatV2.CCommand) hkveReceptionWindow);
      if (this._communication.sendCommands(ref this.MyCCommandArray) && !hkveReceptionWindow.hasError() && hkveReceptionWindow.hasReceivedResponse())
      {
        MinomatV2.logger.Debug("StartHKVEReceptionWindow() succeeded, no Error and hasReceivedResponse");
        return true;
      }
      MinomatV2.logger.Error("StartHKVEReceptionWindow() failed");
      return false;
    }

    internal override bool Connect()
    {
      return this._communication.Connect(this._minomatSerial, this._minomatPassword);
    }

    private bool fillCommandArrayMonthlyDataDakon(
      ref MinomatV2.CCommandArray commArray,
      byte slaveIndex,
      byte startBlock,
      byte endBlock,
      int months,
      int monthOffset)
    {
      MinomatV2.logger.Debug<byte, byte>("startBlock->{0} endBlock->{1}", startBlock, endBlock);
      byte num1 = startBlock;
      int num2 = months;
      int num3 = monthOffset;
      SortedList<byte, string> sortedList = new SortedList<byte, string>();
      for (; (int) num1 <= (int) endBlock; ++num1)
      {
        DateTime dateTimeNow = SystemValues.DateTimeNow;
        Minomat.CCommandDate date = dateTimeNow.Month + num3 != 0 ? new Minomat.CCommandDate((byte) dateTimeNow.Day, (byte) (dateTimeNow.Month + num3), (byte) (dateTimeNow.Year - 2000)) : new Minomat.CCommandDate((byte) dateTimeNow.Day, (byte) 12, (byte) (dateTimeNow.Year - 2000 - 1));
        for (int index1 = 0; index1 < num2; ++index1)
        {
          byte primaryAddress;
          byte index2;
          if (slaveIndex > (byte) 0)
          {
            if (num1 < (byte) 25)
            {
              primaryAddress = slaveIndex;
              index2 = num1;
            }
            else
            {
              primaryAddress = (byte) ((uint) slaveIndex + 20U);
              index2 = (byte) ((uint) num1 - 25U);
            }
          }
          else
          {
            primaryAddress = (byte) 0;
            index2 = num1;
          }
          MinomatV2.CCommandGetMonthlyData ccommandGetMonthlyData = new MinomatV2.CCommandGetMonthlyData(this._minomatSerial, this._minomatPassword, ref date, primaryAddress, index2);
          commArray.CCommands.Add((MinomatV2.CCommand) ccommandGetMonthlyData);
          if (date.getMonth() == (byte) 1)
          {
            date.setYear((byte) ((uint) date.getYear() - 1U));
            date.setMonth((byte) 12);
          }
          else
            date.setMonth((byte) ((uint) date.getMonth() - 1U));
        }
      }
      return true;
    }

    private bool fillCommandArrayEventDataDakon(
      ref MinomatV2.CCommandArray commArray,
      byte slaveIndex,
      byte startBlock,
      byte endBlock)
    {
      string[] strArray1 = new string[2];
      string[] strArray2 = new string[2];
      string[] strArray3 = new string[2];
      string[] strArray4 = new string[2];
      for (byte index = startBlock; (int) index <= (int) endBlock; ++index)
      {
        MinomatV2.CCommandGetEventData ccommandGetEventData = new MinomatV2.CCommandGetEventData(this._minomatSerial, this._minomatPassword, slaveIndex, index);
        commArray.CCommands.Add((MinomatV2.CCommand) ccommandGetEventData);
      }
      return true;
    }

    private bool processMonthlyDataDakon(
      MinomatV2.CCommandArray commArray,
      byte startBlock,
      byte endBlock,
      int months,
      int monthOffset,
      Dictionary<string, Minomat.MinomatDeviceValue> processedMinomatDevices,
      Dictionary<string, byte> deviceSerialNumbersAsStrings)
    {
      byte num1 = startBlock;
      int num2 = months;
      int num3 = monthOffset;
      int num4 = 0;
      for (; (int) num1 <= (int) endBlock; ++num1)
      {
        DateTime dateTimeNow1 = SystemValues.DateTimeNow;
        Minomat.CCommandDate ccommandDate = dateTimeNow1.Month + num3 != 0 ? new Minomat.CCommandDate((byte) 0, (byte) (dateTimeNow1.Month + num3), (byte) (dateTimeNow1.Year - 2000)) : new Minomat.CCommandDate((byte) 0, (byte) 12, (byte) (dateTimeNow1.Year - 2000 - 1));
        for (int index1 = 0; index1 < num2; ++index1)
        {
          MinomatV2.CCommandGetMonthlyData ccommand = (MinomatV2.CCommandGetMonthlyData) commArray.CCommands[num4++];
          for (int index2 = 0; index2 < 4; ++index2)
          {
            Minomat.MonthlyDataset dataset = ccommand.getDataset(index2);
            if (dataset.hkveSerialNo != 0UL)
            {
              string str = this.getSerialNoFromBCDValue(dataset.hkveSerialNo).ToString();
              byte num5 = (byte) (4 * (int) num1 + index2);
              string StringValue = num5.ToString();
              if (!deviceSerialNumbersAsStrings.ContainsKey(str))
              {
                deviceSerialNumbersAsStrings.Add(str, num5);
                processedMinomatDevices.Add(str, (Minomat.MinomatDeviceValue) null);
              }
              else if (MinomatV2.logger.IsDebugEnabled && (int) deviceSerialNumbersAsStrings[str] != (int) num5)
                MinomatV2.logger.Debug(string.Format("WARNING: serial {0} exists on multiple adresses ({1}, {2}) within minomat", (object) str, (object) deviceSerialNumbersAsStrings[str], (object) num5));
              Minomat.MinomatDeviceValue minomatDeviceValue;
              if (processedMinomatDevices[str] == null)
              {
                minomatDeviceValue = new Minomat.MinomatDeviceValue();
                processedMinomatDevices[str] = minomatDeviceValue;
              }
              else
                minomatDeviceValue = processedMinomatDevices[str];
              this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.SerialNumber, new ConfigurationParameter(OverrideID.SerialNumber, str, true), false);
              this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DaKonSerialNumber, new ConfigurationParameter(OverrideID.DaKonSerialNumber, this.getSerialNoFromBCDValue(this._minomatSerial).ToString(), true), false);
              this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DaKonRegisterNumber, new ConfigurationParameter(OverrideID.DaKonRegisterNumber, StringValue, true), false);
              int num6;
              if (((MinomatV2.CCommandGetMonthlyData) commArray.CCommands[index1]).isDatasetMissing(index2))
              {
                this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DiagnosticString, new ConfigurationParameter(OverrideID.DiagnosticString, string.Format("Status={0:x2}", (object) dataset.status), true), false);
                if (((int) dataset.status & 1) == 0)
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DeviceHasError, new ConfigurationParameter(OverrideID.DeviceHasError, "false", true), false);
                else
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DeviceHasError, new ConfigurationParameter(OverrideID.DeviceHasError, "true", true), false);
              }
              else
              {
                this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DiagnosticString, new ConfigurationParameter(OverrideID.DiagnosticString, string.Format("Status={0:x2} Factor={1:x2}", (object) dataset.status, (object) dataset.factor), true), false);
                if (((int) dataset.status & 32) == 32)
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.Manipulation, new ConfigurationParameter(OverrideID.Manipulation, "true", true), false);
                else
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.Manipulation, new ConfigurationParameter(OverrideID.Manipulation, "false", true), false);
                if (((int) dataset.status & 16) == 0)
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DeviceHasError, new ConfigurationParameter(OverrideID.DeviceHasError, "false", true), false);
                else
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DeviceHasError, new ConfigurationParameter(OverrideID.DeviceHasError, "true", true), false);
                if (str.IndexOf("8") == 0)
                {
                  if (dataset.factor == (ushort) 1000)
                    this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.HCA_Scale, new ConfigurationParameter(OverrideID.HCA_Scale, HCA_Scale.Uniform.ToString(), true), false);
                  else
                    this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.HCA_Scale, new ConfigurationParameter(OverrideID.HCA_Scale, HCA_Scale.Product.ToString(), true), false);
                  float num7 = (float) ((int) dataset.factor / 1000);
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.HCA_Factor_Weighting, new ConfigurationParameter(OverrideID.HCA_Factor_Weighting, num7.ToString((IFormatProvider) FixedFormates.TheFormates), true), false);
                  if (((int) dataset.status & 128) == 0)
                    this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.HCA_SensorMode, new ConfigurationParameter(OverrideID.HCA_SensorMode, HCA_SensorMode.Single.ToString(), true), false);
                  else
                    this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.HCA_SensorMode, new ConfigurationParameter(OverrideID.HCA_SensorMode, HCA_SensorMode.Double.ToString(), true), false);
                }
                else
                {
                  num6 = (int) dataset.factor >> 4;
                  num6.ToString((IFormatProvider) FixedFormates.TheFormates);
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.HCA_SensorMode, new ConfigurationParameter(OverrideID.HCA_SensorMode, HCA_SensorMode.Double.ToString(), true), false);
                  this.AddValueToList(ref minomatDeviceValue.configValues, OverrideID.DeviceUnit, new ConfigurationParameter(OverrideID.DeviceUnit, this.getPulseUnit((ushort) ((uint) dataset.factor & 15U)), true), false);
                }
              }
              string[] strArray1 = new string[2];
              string[] strArray2 = new string[2];
              string[] strArray3 = new string[2];
              string[] strArray4 = new string[2];
              string deviceSerialNumber1 = str;
              long fullMonthReading = (long) dataset.fullMonthReading;
              long halfMonthReading1 = (long) dataset.halfMonthReading;
              DateTime dateTimeNow2 = SystemValues.DateTimeNow;
              num6 = dateTimeNow2.Month;
              string readingName1 = "FULLMONTHREADING: " + num6.ToString();
              Minomat.CCommandDate date1 = dataset.date;
              Minomat.CCommandDate requestedMonth1 = ccommandDate;
              int num8 = ccommand.hasChecksumError(index2) ? 1 : 0;
              int num9 = ccommand.isDatasetMissing(index2) ? 1 : 0;
              ref string local1 = ref strArray1[0];
              ref string local2 = ref strArray2[0];
              ref string local3 = ref strArray3[0];
              ref string local4 = ref strArray4[0];
              this.validateDataEntryDakon(deviceSerialNumber1, (ulong) fullMonthReading, (ulong) halfMonthReading1, readingName1, date1, requestedMonth1, num8 != 0, num9 != 0, ref local1, ref local2, ref local3, ref local4);
              if (strArray3[0] == "NOT_AVAILABLE" && strArray2[0] == "NO_DATA" || dataset.halfMonthReading == 0UL)
              {
                strArray1[1] = strArray1[0];
                strArray2[1] = "NO_DATA";
                strArray3[1] = "NOT_AVAILABLE";
                strArray4[1] = "NO_DATA";
              }
              else
              {
                string deviceSerialNumber2 = str;
                long halfMonthReading2 = (long) dataset.halfMonthReading;
                dateTimeNow2 = SystemValues.DateTimeNow;
                num6 = dateTimeNow2.Month;
                string readingName2 = "HALFMONTHREADING: " + num6.ToString();
                Minomat.CCommandDate date2 = dataset.date;
                Minomat.CCommandDate requestedMonth2 = ccommandDate;
                int num10 = ccommand.hasChecksumError(index2) ? 1 : 0;
                int num11 = ccommand.isDatasetMissing(index2) ? 1 : 0;
                ref string local5 = ref strArray1[1];
                ref string local6 = ref strArray2[1];
                ref string local7 = ref strArray3[1];
                ref string local8 = ref strArray4[1];
                this.validateDataEntryDakon(deviceSerialNumber2, (ulong) halfMonthReading2, 0UL, readingName2, date2, requestedMonth2, num10 != 0, num11 != 0, ref local5, ref local6, ref local7, ref local8);
              }
              DateTime key = new DateTime();
              bool flag1;
              try
              {
                key = DateTime.Parse(strArray1[0]);
                flag1 = true;
              }
              catch
              {
                flag1 = false;
              }
              if (flag1)
              {
                List<Minomat.ProcessedData> values;
                if (minomatDeviceValue.readoutValues.ContainsKey(key))
                {
                  values = minomatDeviceValue.readoutValues[key];
                }
                else
                {
                  values = new List<Minomat.ProcessedData>();
                  minomatDeviceValue.readoutValues.Add(key, values);
                }
                this.AddValueToList(ref values, "ewFunkId", str, Minomat.DataType.MonthlyData, false);
                this.AddValueToList(ref values, "ewName", "FULLMONTHREADING", Minomat.DataType.MonthlyData, false);
                this.AddValueToList(ref values, "ewDakonSNr", StringValue, Minomat.DataType.MonthlyData, false);
                this.AddValueToList(ref values, "ewDatum", strArray1[0], Minomat.DataType.MonthlyData, false);
                this.AddValueToList(ref values, "ewWert", strArray2[0], Minomat.DataType.MonthlyData, false);
                this.AddValueToList(ref values, "ewStatus", strArray3[0], Minomat.DataType.MonthlyData, false);
                this.AddValueToList(ref values, "ewStatusDetail", strArray4[0], Minomat.DataType.MonthlyData, false);
                this.AddValueToList(ref values, "ewFieldForceSum", dataset.fieldForceSum.ToString((IFormatProvider) FixedFormates.TheFormates), Minomat.DataType.MonthlyData, false);
                this.AddValueToList(ref values, "ewNumberOfReceivedHKVEFrames", dataset.hkveProtocols.ToString((IFormatProvider) FixedFormates.TheFormates), Minomat.DataType.MonthlyData, false);
              }
              bool flag2;
              try
              {
                key = DateTime.Parse(strArray1[1]);
                flag2 = true;
              }
              catch
              {
                flag2 = false;
              }
              if (flag2)
              {
                List<Minomat.ProcessedData> values;
                if (minomatDeviceValue.readoutValues.ContainsKey(key))
                {
                  values = minomatDeviceValue.readoutValues[key];
                }
                else
                {
                  values = new List<Minomat.ProcessedData>();
                  minomatDeviceValue.readoutValues.Add(key, values);
                }
                this.AddValueToList(ref values, "ewFunkId", str, Minomat.DataType.HalfMonthlyData, false);
                this.AddValueToList(ref values, "ewName", "HALFMONTHREADING", Minomat.DataType.HalfMonthlyData, false);
                this.AddValueToList(ref values, "ewDakonSNr", StringValue, Minomat.DataType.HalfMonthlyData, false);
                this.AddValueToList(ref values, "ewDatum", strArray1[1], Minomat.DataType.HalfMonthlyData, false);
                this.AddValueToList(ref values, "ewWert", strArray2[1], Minomat.DataType.HalfMonthlyData, false);
                this.AddValueToList(ref values, "ewStatus", strArray3[1], Minomat.DataType.HalfMonthlyData, false);
                this.AddValueToList(ref values, "ewStatusDetail", strArray4[1], Minomat.DataType.HalfMonthlyData, false);
                this.AddValueToList(ref values, "ewFieldForceSum", dataset.fieldForceSum.ToString((IFormatProvider) FixedFormates.TheFormates), Minomat.DataType.HalfMonthlyData, false);
                this.AddValueToList(ref values, "ewNumberOfReceivedHKVEFrames", dataset.hkveProtocols.ToString((IFormatProvider) FixedFormates.TheFormates), Minomat.DataType.HalfMonthlyData, false);
              }
            }
          }
          if (ccommandDate.getMonth() == (byte) 1)
          {
            ccommandDate.setYear((byte) ((uint) ccommandDate.getYear() - 1U));
            ccommandDate.setMonth((byte) 12);
          }
          else
            ccommandDate.setMonth((byte) ((uint) ccommandDate.getMonth() - 1U));
        }
      }
      return true;
    }

    private void AddValueToList(
      ref List<Minomat.ProcessedData> values,
      string name,
      string value,
      Minomat.DataType dataType,
      bool overrideValue)
    {
      for (int index = 0; index < values.Count; ++index)
      {
        Minomat.ProcessedData processedData = values[index];
        if (processedData.ParameterName == name && processedData.DataType == dataType)
        {
          if (processedData.ParameterValue == value || !overrideValue)
            return;
          processedData.ParameterValue = value;
          return;
        }
      }
      values.Add(new Minomat.ProcessedData(name, value, dataType));
    }

    private void AddValueToList(
      ref SortedList<OverrideID, ConfigurationParameter> values,
      OverrideID name,
      ConfigurationParameter value,
      bool overrideValue)
    {
      if (values.ContainsKey(name))
      {
        if (!overrideValue)
          return;
        values[name] = value;
      }
      else
        values.Add(name, value);
    }

    private bool processEventDataDakon(
      MinomatV2.CCommandArray commArray,
      Dictionary<string, Minomat.MinomatDeviceValue> processedMinomatDevices,
      Dictionary<string, byte> hkveSerialNumbersAsStrings,
      byte startBlock,
      byte endBlock)
    {
      string[] strArray1 = new string[2];
      string[] strArray2 = new string[2];
      string[] strArray3 = new string[2];
      string[] strArray4 = new string[2];
      int num1 = 0;
      GMM_EventArgs e = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
      for (byte index1 = startBlock; (int) index1 <= (int) endBlock; ++index1)
      {
        int num2 = (int) index1 * 100 / (int) endBlock;
        if (num1 != num2)
        {
          num1 = num2;
          e.EventMessage = "Progress encode " + num2.ToString() + "%";
          e.ProgressPercentage = num2;
          this.MyBus.SendMessage(e);
        }
        if (this.MyBus.BreakRequest)
        {
          MinomatV2.logger.Info("Break requested.");
          return false;
        }
        MinomatV2.CCommandGetEventData ccommand = (MinomatV2.CCommandGetEventData) commArray.CCommands[(int) index1 - (int) startBlock];
        for (byte index2 = 0; index2 < (byte) 5; ++index2)
        {
          Minomat.EventDataset dataset = ccommand.getDataset((int) index2);
          if (dataset != null && !dataset.isEmpty())
          {
            byte num3 = (byte) ((uint) index1 * 5U + (uint) index2);
            string str = string.Empty;
            if (hkveSerialNumbersAsStrings.ContainsValue(num3))
            {
              foreach (KeyValuePair<string, byte> serialNumbersAsString in hkveSerialNumbersAsStrings)
              {
                if ((int) serialNumbersAsString.Value == (int) num3)
                {
                  str = serialNumbersAsString.Key;
                  break;
                }
              }
            }
            if (processedMinomatDevices.ContainsKey(str))
            {
              this.validateDataEntryDakon(str, dataset.eventReading1, 0UL, "ST1", dataset.date1, (Minomat.CCommandDate) null, ccommand.hasChecksumError((int) index2), ccommand.isDatasetMissing((int) index2), ref strArray1[0], ref strArray2[0], ref strArray3[0], ref strArray4[0]);
              this.validateDataEntryDakon(str, dataset.eventReading2, 0UL, "ST2", dataset.date2, (Minomat.CCommandDate) null, ccommand.hasChecksumError((int) index2), ccommand.isDatasetMissing((int) index2), ref strArray1[1], ref strArray2[1], ref strArray3[1], ref strArray4[1]);
              if (strArray3[0] == "READING_VALID")
              {
                Minomat.MinomatDeviceValue minomatDeviceValue;
                if (processedMinomatDevices[str] == null)
                {
                  minomatDeviceValue = new Minomat.MinomatDeviceValue();
                  processedMinomatDevices[str] = minomatDeviceValue;
                }
                else
                  minomatDeviceValue = processedMinomatDevices[str];
                DateTime key = new DateTime();
                bool flag;
                try
                {
                  key = DateTime.Parse(strArray1[0]);
                  flag = true;
                }
                catch
                {
                  flag = false;
                }
                if (flag)
                {
                  List<Minomat.ProcessedData> values;
                  if (minomatDeviceValue.readoutValues.ContainsKey(key))
                  {
                    values = minomatDeviceValue.readoutValues[key];
                  }
                  else
                  {
                    values = new List<Minomat.ProcessedData>();
                    minomatDeviceValue.readoutValues.Add(key, values);
                  }
                  this.AddValueToList(ref values, "ewFunkId", str, Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewName", "DUEDATEREADING", Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewDatum", strArray1[0], Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewWert", strArray2[0], Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewStatus", strArray3[0], Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewStatusDetail", strArray4[0], Minomat.DataType.EventData, false);
                }
              }
              if (strArray3[1] == "READING_VALID")
              {
                Minomat.MinomatDeviceValue minomatDeviceValue;
                if (processedMinomatDevices[str] == null)
                {
                  minomatDeviceValue = new Minomat.MinomatDeviceValue();
                  processedMinomatDevices[str] = minomatDeviceValue;
                }
                else
                  minomatDeviceValue = processedMinomatDevices[str];
                DateTime key = new DateTime();
                bool flag;
                try
                {
                  key = DateTime.Parse(strArray1[1]);
                  flag = true;
                }
                catch
                {
                  flag = false;
                }
                if (flag)
                {
                  List<Minomat.ProcessedData> values;
                  if (minomatDeviceValue.readoutValues.ContainsKey(key))
                  {
                    values = minomatDeviceValue.readoutValues[key];
                  }
                  else
                  {
                    values = new List<Minomat.ProcessedData>();
                    minomatDeviceValue.readoutValues.Add(key, values);
                  }
                  this.AddValueToList(ref values, "ewFunkId", str, Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewName", "DUEDATEREADING", Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewDatum", strArray1[1], Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewWert", strArray2[1], Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewStatus", strArray3[1], Minomat.DataType.EventData, false);
                  this.AddValueToList(ref values, "ewStatusDetail", strArray4[1], Minomat.DataType.EventData, false);
                }
                else if (MinomatV2.logger.IsTraceEnabled)
                  MinomatV2.logger.Trace("date is invalid");
              }
            }
            else if (MinomatV2.logger.IsTraceEnabled)
              MinomatV2.logger.Trace(string.Format("processedMinomatDevices did not contain serialnumber, skipping."));
          }
        }
      }
      return true;
    }

    private void writeDataEntryDakon(
      string deviceSerialNumber,
      string readingName,
      string valueDate,
      string valueValue,
      string valueState,
      string valueStateDetail,
      string dakonSNrAsString)
    {
      if (!MinomatV2.logger.IsTraceEnabled)
        return;
      MinomatV2.logger.Trace(string.Format("{0};{1};{2};{3};{4};{5}", (object) deviceSerialNumber, (object) readingName, (object) valueDate, (object) valueValue, (object) valueState, (object) valueStateDetail));
    }

    private ulong getSerialNoFromBCDValue(ulong serialNoBCD)
    {
      ulong serialNoFromBcdValue = 0;
      byte[] numArray = new byte[8]
      {
        (byte) 28,
        (byte) 24,
        (byte) 20,
        (byte) 16,
        (byte) 12,
        (byte) 8,
        (byte) 4,
        (byte) 0
      };
      for (int index = 0; index < 8; ++index)
        serialNoFromBcdValue = (ulong) (10L * (long) serialNoFromBcdValue + ((long) (serialNoBCD >> (int) numArray[index]) & 15L));
      return serialNoFromBcdValue;
    }

    private void validateDataEntryDakon(
      string deviceSerialNumber,
      ulong readingValue,
      ulong hmReadingValue,
      string readingName,
      Minomat.CCommandDate readingDate,
      Minomat.CCommandDate requestedMonth,
      bool hasChecksumError,
      bool isDatasetMissing,
      ref string valueDate,
      ref string valueValue,
      ref string valueState,
      ref string valueStateDetail)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      if (requestedMonth != null && readingDate.getDay() > (byte) 15)
        flag4 = true;
      valueState = string.Empty;
      valueStateDetail = string.Empty;
      bool flag5;
      if (isDatasetMissing || readingValue == 0UL && readingDate.getDay() == (byte) 0 && readingDate.getMonth() == (byte) 0 && readingDate.getYear() == (byte) 0)
      {
        valueState = "NOT_AVAILABLE";
        valueStateDetail = "NO_RECEPTION";
        if (requestedMonth != null)
          this.adjustMonthlyReadingDateToRequestedMonth(readingDate, requestedMonth, readingName);
        flag5 = this.isDateValid(readingDate);
      }
      else if (hasChecksumError)
      {
        valueState = "NOT_AVAILABLE";
        valueStateDetail = "CHECKSUM_ERROR";
        if (requestedMonth != null)
          this.adjustMonthlyReadingDateToRequestedMonth(readingDate, requestedMonth, readingName);
        flag5 = this.isDateValid(readingDate);
      }
      else if (deviceSerialNumber.IndexOf("8") == 0 && readingValue == (ulong) ushort.MaxValue || deviceSerialNumber.IndexOf("8") != 0 && readingValue == (ulong) uint.MaxValue)
      {
        flag1 = true;
        valueState = "NOT_AVAILABLE";
        valueStateDetail = "NO_MEASUREMENT";
        if (requestedMonth != null)
          this.adjustMonthlyReadingDateToRequestedMonth(readingDate, requestedMonth, readingName);
        flag5 = this.isDateValid(readingDate);
        flag3 = !flag5;
      }
      else
      {
        flag2 = true;
        valueState = "READING_VALID";
        if (requestedMonth != null)
          this.adjustMonthlyReadingDateToRequestedMonth(readingDate, requestedMonth, readingName);
        flag5 = this.isDateValid(readingDate);
        flag3 = !flag5;
      }
      valueDate = string.Empty;
      valueDate = !flag5 ? new DateTime(1899, 12, 30, 12, 30, 0).ToString() : new DateTime((int) readingDate.getYear() + 2000, (int) readingDate.getMonth(), (int) readingDate.getDay(), 12, 0, 0).ToString();
      valueValue = string.Empty;
      bool flag6 = false;
      bool flag7 = false;
      if (flag2)
      {
        if (deviceSerialNumber.IndexOf("8") == 0)
        {
          valueValue = ((double) readingValue / 4.0).ToString((IFormatProvider) FixedFormates.TheFormates);
          if (readingValue > (ulong) ushort.MaxValue)
            flag6 = true;
        }
        else
        {
          valueValue = MinomatV2.TranslateBcdToBin((long) readingValue).ToString((IFormatProvider) FixedFormates.TheFormates);
          bool flag8;
          try
          {
            int.Parse(valueValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
            flag8 = true;
          }
          catch
          {
            flag8 = false;
          }
          if (!flag8)
            flag6 = true;
        }
        if (((readingValue != 0UL ? 0 : (hmReadingValue > 0UL ? 1 : 0)) | (flag4 ? 1 : 0)) != 0)
          flag7 = true;
      }
      else
        valueValue = "NO_DATA";
      if (flag1 | flag2)
      {
        if (!(flag6 | flag7 | flag3))
          return;
        valueState = "NOT_VALID";
        if (flag6 | flag7)
        {
          if (flag6)
          {
            string str = string.Format("Ungueltiger Messwert: {0:x}", (object) readingValue);
            if (valueStateDetail.Length > 0)
              valueStateDetail += ",";
            valueStateDetail += str;
          }
          if (flag7)
          {
            string str = string.Format("Halbmonatstest fehlgeschlagen: {0:s}", (object) valueValue);
            if (valueStateDetail.Length > 0)
              valueStateDetail += ",";
            valueStateDetail += str;
          }
          valueValue = "NO_DATA";
        }
        if (flag3)
          string.Format("Ungueltiges Datum: {0,2:d}.{1,2:d}.{2,4:d}", (object) readingDate.getDay(), (object) readingDate.getMonth(), (object) ((int) readingDate.getYear() + 2000));
      }
      else
        valueValue = "NO_DATA";
    }

    private static long TranslateBcdToBin(long InValue)
    {
      long num = 1;
      long bin = 0;
      for (; InValue > 0L; InValue >>= 4)
      {
        bin += (InValue & 15L) * num;
        num *= 10L;
      }
      return bin;
    }

    private bool isDateValid(Minomat.CCommandDate date)
    {
      if (date.getMonth() < (byte) 1 || date.getMonth() > (byte) 12 || date.getDay() < (byte) 1 || date.getDay() > (byte) 31)
        return false;
      DateTime dateTime;
      try
      {
        dateTime = new DateTime((int) date.getYear() + 2000, (int) date.getMonth(), (int) date.getDay(), 12, 0, 0);
      }
      catch
      {
        return false;
      }
      return dateTime.Year == (int) date.getYear() + 2000 && dateTime.Month == (int) date.getMonth() && dateTime.Day == (int) date.getDay();
    }

    private void adjustMonthlyReadingDateToRequestedMonth(
      Minomat.CCommandDate readingDate,
      Minomat.CCommandDate requestedMonth,
      string readingName)
    {
      string str1 = "FULLMONTHREADING";
      string str2 = "HALFMONTHREADING";
      if (readingName.IndexOf(str1) == 0)
      {
        readingDate.setDay((byte) 1);
        if (requestedMonth.getMonth() == (byte) 12)
        {
          readingDate.setYear((byte) ((uint) requestedMonth.getYear() + 1U));
          readingDate.setMonth((byte) 1);
        }
        else
        {
          readingDate.setYear(requestedMonth.getYear());
          readingDate.setMonth((byte) ((uint) requestedMonth.getMonth() + 1U));
        }
      }
      else
      {
        if (readingName.IndexOf(str2) != 0)
          return;
        readingDate.setYear(requestedMonth.getYear());
        readingDate.setMonth(requestedMonth.getMonth());
        readingDate.setDay((byte) 15);
      }
    }

    private string getPulseUnit(ushort pulseUnit)
    {
      switch (pulseUnit)
      {
        case 0:
          return "?";
        case 1:
          return "l";
        case 2:
          return "m3";
        case 3:
          return "Wh";
        case 4:
          return "kWh";
        case 5:
          return "MWh";
        case 6:
          return "J";
        case 7:
          return "kJ";
        case 8:
          return "MJ";
        case 9:
          return "GJ";
        case 10:
          return "min";
        case 11:
          return "h";
        default:
          return "?";
      }
    }

    public sealed class SystemStatus
    {
      public ulong IrdaHandheldDisturbance;
      public ulong RfHandheldDisturbance;
      public ulong HkveReceivePowerConsumption;
      public ulong MasterReceivePowerConsumption;
      public ushort FirmwareVersion;
      public ulong IrdaRFCheckPowerConsumption;
      public ulong IrdaHandheldPowerConsumption;
      public ulong RfHandheldPowerConsumption;
      public byte MinimumTemperature;
      public byte MaximumTemperature;
      public ushort BatteryVoltage;
      public byte CommSubsystem;
      public byte CommSubsystemVariant;
      public string CommSubsystemDetails;
      public MinomatV2.SystemStatus.ErrStatus ErrorStatus = new MinomatV2.SystemStatus.ErrStatus();

      public string FirmwareVersionAsString
      {
        get
        {
          ushort firmwareVersion = this.FirmwareVersion;
          return string.Format("{0:#}.{1:##}", (object) (((int) firmwareVersion >> 12 & 15) * 10 + ((int) firmwareVersion >> 8 & 15)), (object) (((int) firmwareVersion >> 4 & 15) * 10 + ((int) firmwareVersion & 15)));
        }
      }

      public SystemStatus()
      {
        this.ErrorStatus.hasFLASHBurnError = true;
        this.ErrorStatus.hasUARTReceptionError = true;
        this.ErrorStatus.hasChecksumError = true;
        this.ErrorStatus.hasClockError = true;
        this.ErrorStatus.hasEmptyBattery = true;
        this.ErrorStatus.hasTemperatureError = true;
        this.ErrorStatus.hasHKVEStackOverflow = true;
        this.ErrorStatus.hasGSMReceiveBufferOverflow = true;
        this.ErrorStatus.hasIRDAReceiveBufferOverflow = true;
        this.ErrorStatus.hasRFReceiveBufferOverflow = true;
        this.ErrorStatus.hasCC1020CalibrationError = true;
        this.ErrorStatus.hasPowerFailure = true;
      }

      public struct ErrStatus
      {
        public bool hasFLASHBurnError;
        public bool hasUARTReceptionError;
        public bool hasChecksumError;
        public bool hasClockError;
        public bool hasEmptyBattery;
        public bool hasTemperatureError;
        public bool hasHKVEStackOverflow;
        public bool hasGSMReceiveBufferOverflow;
        public bool hasIRDAReceiveBufferOverflow;
        public bool hasRFReceiveBufferOverflow;
        public bool hasCC1020CalibrationError;
        public bool hasPowerFailure;
      }
    }

    public sealed class Configuration
    {
      public string SerialNo;
      public byte PrimaryAddress = 0;
      public MinomatV2.Configuration.HKVE hkve = new MinomatV2.Configuration.HKVE();
      public MinomatV2.Configuration.Master master = new MinomatV2.Configuration.Master();
      public byte HandTerminalCycleDuration = 30;
      public DateTime EventDay;
      public string UserPassword;

      public sealed class HKVE
      {
        public byte WindowStart = 6;
        public byte NoOfWindowsAfterEvent = 6;
        public byte WindowGapAfterEvent = 4;
        public byte NoOfWindowsDailyData = 1;
        public byte WindowGapDailyData = 4;
      }

      public sealed class Master
      {
        public byte WindowStartDailyData = 7;
        public byte WindowDurationDailyData = 0;
        public byte WindowStartEvent = 31;
        public byte WindowDurationEvent = 9;
      }
    }

    private class CCommandArray
    {
      public List<MinomatV2.CCommand> CCommands = new List<MinomatV2.CCommand>();
    }

    private class CCommandDataArray
    {
      public byte[] DataArray;

      public void SetSize(uint Size)
      {
        if (Size < 0U || Size >= 1000U)
          return;
        this.DataArray = new byte[(int) Size];
      }
    }

    private class ResponseStatus
    {
      internal bool hasFlashError = true;
      internal bool hasParameterError = true;
      internal bool hasRTCError = true;
      internal bool hasGSMConnection = true;
      internal bool hasIrDAHandheldConnection = true;
      internal bool hasRFHandheldConnection = true;
      internal bool hasOpenMasterWindow = true;
      internal bool hasOpenHKVEWindow = true;

      public ResponseStatus()
      {
        this.hasFlashError = false;
        this.hasParameterError = false;
        this.hasRTCError = false;
        this.hasGSMConnection = false;
        this.hasIrDAHandheldConnection = false;
        this.hasRFHandheldConnection = false;
        this.hasOpenMasterWindow = false;
        this.hasOpenHKVEWindow = false;
      }
    }

    private class CCommandGetTime : MinomatV2.CCommand
    {
      private const int NUMBER = 0;
      private const int REACTION_TIME = 100;
      private Minomat.CCommandTime m_time = new Minomat.CCommandTime((byte) 0, (byte) 0, (byte) 0);
      private Minomat.CCommandDate m_date = new Minomat.CCommandDate((byte) 0, (byte) 0, (byte) 0);
      private bool m_rtcError;

      public CCommandGetTime(ulong serialNo, ulong userPassword)
        : base(serialNo, userPassword, 0U, 100U)
      {
        this.m_rtcError = false;
      }

      public Minomat.CCommandTime getTime() => this.m_time;

      public Minomat.CCommandDate getDate() => this.m_date;

      public bool isClockDefect() => this.m_rtcError;

      protected CCommandGetTime(ref MinomatV2.CCommandDataArray encodedReq)
        : base(ref encodedReq)
      {
      }

      protected override uint getResponseContainerSize() => MinomatV2.CCommand.BLOCK_SIZE;

      protected override bool decodeResponseContainerData(ref MinomatV2.CCommandDataArray resp)
      {
        this.m_time = new Minomat.CCommandTime(resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE], resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 1], resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 2]);
        this.m_rtcError = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 3] > (byte) 0;
        this.m_date = new Minomat.CCommandDate((ushort) ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4] << 8 | (uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5]));
        return true;
      }
    }

    private class CCommandSetTime : MinomatV2.CCommand
    {
      private const int NUMBER = 1;
      private const int REACTION_TIME = 300;
      private Minomat.CCommandTime m_time = new Minomat.CCommandTime((byte) 0, (byte) 0, (byte) 0);
      private Minomat.CCommandDate m_date = new Minomat.CCommandDate((byte) 0, (byte) 0, (byte) 0);
      private bool m_useCurrentDate;

      public CCommandSetTime(ulong serialNo, ulong userPassword)
        : base(serialNo, userPassword, 1U, 300U)
      {
        this.m_useCurrentDate = true;
      }

      public CCommandSetTime(
        ulong serialNo,
        ulong userPassword,
        Minomat.CCommandTime time,
        Minomat.CCommandDate date)
        : base(serialNo, userPassword, 1U, 300U)
      {
        this.m_useCurrentDate = false;
        this.m_time = time;
        this.m_date = date;
      }

      protected override void encodeRequestContainerData(ref MinomatV2.CCommandDataArray req)
      {
        if (this.m_useCurrentDate)
        {
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE] = (byte) SystemValues.DateTimeNow.Hour;
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 1] = (byte) SystemValues.DateTimeNow.Minute;
          byte[] dataArray = req.DataArray;
          int index = (int) MinomatV2.CCommand.REQUEST_SIZE + 2;
          DateTime dateTimeNow1 = SystemValues.DateTimeNow;
          int second = (int) (byte) dateTimeNow1.Second;
          dataArray[index] = (byte) second;
          dateTimeNow1 = SystemValues.DateTimeNow;
          int day = (int) (byte) dateTimeNow1.Day;
          DateTime dateTimeNow2 = SystemValues.DateTimeNow;
          int month = (int) (byte) dateTimeNow2.Month;
          dateTimeNow2 = SystemValues.DateTimeNow;
          int year = (int) (byte) (dateTimeNow2.Year - 1970);
          Minomat.CCommandDate ccommandDate = new Minomat.CCommandDate((byte) day, (byte) month, (byte) year);
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4] = (byte) ((uint) ccommandDate.getEncodedDate() >> 8);
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] = (byte) ((uint) ccommandDate.getEncodedDate() & (uint) byte.MaxValue);
        }
        else
        {
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE] = this.m_time.getHours();
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 1] = this.m_time.getMinutes();
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 2] = this.m_time.getSeconds();
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4] = (byte) ((uint) this.m_date.getEncodedDate() >> 8);
          req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] = (byte) ((uint) this.m_date.getEncodedDate() & (uint) byte.MaxValue);
        }
        this.generateChecksum(ref req, MinomatV2.CCommand.REQUEST_SIZE);
      }

      protected override uint getResponseContainerSize() => MinomatV2.CCommand.BLOCK_SIZE;

      protected override uint getRequestContainerSize() => MinomatV2.CCommand.BLOCK_SIZE;
    }

    private class CCommandGetSystemStatus : MinomatV2.CCommand
    {
      private const int NUMBER = 16;
      private const int REACTION_TIME = 100;
      private MinomatV2.SystemStatus m_systemStatus = new MinomatV2.SystemStatus();

      public CCommandGetSystemStatus(ulong serialNo, ulong userPassword)
        : base(serialNo, userPassword, 16U, 100U)
      {
      }

      public string getFirmwareVersionAsString()
      {
        ushort firmwareVersion = this.getSystemStatus().FirmwareVersion;
        return string.Format("{0:#}.{1:##}", (object) (((int) firmwareVersion >> 12 & 15) * 10 + ((int) firmwareVersion >> 8 & 15)), (object) (((int) firmwareVersion >> 4 & 15) * 10 + ((int) firmwareVersion & 15)));
      }

      public MinomatV2.SystemStatus getSystemStatus() => this.m_systemStatus;

      protected CCommandGetSystemStatus(ref MinomatV2.CCommandDataArray encodedReq)
        : base(ref encodedReq)
      {
      }

      protected override uint getResponseContainerSize() => 7U * MinomatV2.CCommand.BLOCK_SIZE;

      protected override bool decodeResponseContainerData(ref MinomatV2.CCommandDataArray resp)
      {
        this.m_systemStatus.IrdaHandheldDisturbance = (ulong) ((int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 3]);
        this.m_systemStatus.ErrorStatus.hasFLASHBurnError = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] & 1U) > 0U;
        this.m_systemStatus.ErrorStatus.hasUARTReceptionError = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] & 2U) > 0U;
        this.m_systemStatus.ErrorStatus.hasChecksumError = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] & 4U) > 0U;
        this.m_systemStatus.ErrorStatus.hasClockError = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] & 8U) > 0U;
        this.m_systemStatus.ErrorStatus.hasEmptyBattery = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] & 16U) > 0U;
        this.m_systemStatus.ErrorStatus.hasTemperatureError = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] & 32U) > 0U;
        this.m_systemStatus.ErrorStatus.hasHKVEStackOverflow = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] & 64U) > 0U;
        this.m_systemStatus.ErrorStatus.hasGSMReceiveBufferOverflow = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 5] & 128U) > 0U;
        this.m_systemStatus.ErrorStatus.hasIRDAReceiveBufferOverflow = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4] & 1U) > 0U;
        this.m_systemStatus.ErrorStatus.hasRFReceiveBufferOverflow = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4] & 2U) > 0U;
        this.m_systemStatus.ErrorStatus.hasCC1020CalibrationError = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4] & 4U) > 0U;
        this.m_systemStatus.ErrorStatus.hasPowerFailure = ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4] & 8U) > 0U;
        this.m_systemStatus.RfHandheldDisturbance = (ulong) ((int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 3]);
        this.m_systemStatus.MinimumTemperature = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 4];
        this.m_systemStatus.MaximumTemperature = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 5];
        this.m_systemStatus.HkveReceivePowerConsumption = (ulong) ((int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 3]);
        this.m_systemStatus.BatteryVoltage = (ushort) ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 4] << 8 | (uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 5]);
        this.m_systemStatus.MasterReceivePowerConsumption = (ulong) ((int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3 + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3 + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3 + 3]);
        this.m_systemStatus.FirmwareVersion = (ushort) ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3 + 4] << 8 | (uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3 + 5]);
        this.m_systemStatus.IrdaRFCheckPowerConsumption = (ulong) ((int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4 + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4 + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4 + 3]);
        this.m_systemStatus.IrdaHandheldPowerConsumption = (ulong) ((int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 5] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 5 + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 5 + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 5 + 3]);
        this.m_systemStatus.RfHandheldPowerConsumption = (ulong) ((int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 6] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 6 + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 6 + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 6 + 3]);
        if ((uint) resp.DataArray[4] > 7U * MinomatV2.CCommand.BLOCK_SIZE + MinomatV2.CCommand.REQUEST_SIZE)
        {
          this.m_systemStatus.CommSubsystem = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 8];
          this.m_systemStatus.CommSubsystemVariant = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 8 + 1];
          uint num1 = 0;
          uint num2 = MinomatV2.CCommand.BLOCK_SIZE * 9U;
          uint num3 = 1;
          for (; num1 < 17U * MinomatV2.CCommand.BLOCK_SIZE && resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) num2] != (byte) 0; ++num2)
          {
            this.m_systemStatus.CommSubsystemDetails += resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) num2].ToString();
            if (num3 == 6U)
            {
              num2 += 2U;
              num3 = 1U;
            }
            else
              ++num3;
            ++num1;
          }
        }
        else
        {
          this.m_systemStatus.CommSubsystem = (byte) 0;
          this.m_systemStatus.CommSubsystemVariant = (byte) 0;
          this.m_systemStatus.CommSubsystemDetails = string.Empty;
        }
        return true;
      }

      protected override bool isResponseSizeValid(ref MinomatV2.CCommandDataArray resp)
      {
        return (int) resp.DataArray[4] == 25 * (int) MinomatV2.CCommand.BLOCK_SIZE + (int) MinomatV2.CCommand.REQUEST_SIZE || (int) resp.DataArray[4] == 7 * (int) MinomatV2.CCommand.BLOCK_SIZE + (int) MinomatV2.CCommand.REQUEST_SIZE;
      }
    }

    private class CCommandGetEventData : MinomatV2.CCommand
    {
      private const int NUMBER = 4;
      private const int REACTION_TIME = 300;
      private byte m_primaryAddress;
      private byte m_index;
      private const byte NO_OF_DATASETS = 5;
      internal bool[] checksumErrors = new bool[5];
      internal bool[] datasetMissing = new bool[5];
      private Minomat.EventDataset[] datasets = new Minomat.EventDataset[5];

      public CCommandGetEventData(
        ulong serialNo,
        ulong userPassword,
        byte primaryAddress,
        byte index)
        : base(serialNo, userPassword, 4U, 300U)
      {
        this.m_primaryAddress = primaryAddress;
        this.m_index = index;
      }

      public bool hasChecksumError(int index)
      {
        return index >= 0 && index < 5 && this.checksumErrors[index];
      }

      public bool isDatasetMissing(int index)
      {
        return index >= 0 && index < 5 && this.datasetMissing[index];
      }

      public Minomat.EventDataset getDataset(int index)
      {
        return index >= 0 && index < 5 ? this.datasets[index] : (Minomat.EventDataset) null;
      }

      protected CCommandGetEventData(ref MinomatV2.CCommandDataArray encodedReq)
        : base(ref encodedReq)
      {
      }

      protected override ulong getLongParam() => (ulong) ((int) this.m_primaryAddress << 16);

      protected override byte getByteParam() => this.m_index;

      protected override uint getResponseContainerSize()
      {
        return (uint) (5 * (int) MinomatV2.CCommand.BLOCK_SIZE * 2);
      }

      protected override bool decodeResponseContainerData(ref MinomatV2.CCommandDataArray resp)
      {
        uint index1 = 0;
        uint index2 = MinomatV2.CCommand.REQUEST_SIZE;
        for (; index1 < 5U; ++index1)
        {
          this.datasets[(int) index1] = new Minomat.EventDataset();
          this.datasets[(int) index1].date1 = new Minomat.CCommandDate((ushort) ((uint) resp.DataArray[(int) index2] << 8 | (uint) resp.DataArray[(int) index2 + 1]));
          uint index3 = index2 + 2U;
          this.datasets[(int) index1].eventReading1 = (ulong) ((int) resp.DataArray[(int) index3] << 24 | (int) resp.DataArray[(int) index3 + 1] << 16 | (int) resp.DataArray[(int) index3 + 2] << 8 | (int) resp.DataArray[(int) index3 + 3]);
          uint index4 = index3 + 6U;
          this.datasets[(int) index1].date2 = new Minomat.CCommandDate((ushort) ((uint) resp.DataArray[(int) index4] << 8 | (uint) resp.DataArray[(int) index4 + 1]));
          uint index5 = index4 + 2U;
          this.datasets[(int) index1].eventReading2 = (ulong) ((int) resp.DataArray[(int) index5] << 24 | (int) resp.DataArray[(int) index5 + 1] << 16 | (int) resp.DataArray[(int) index5 + 2] << 8 | (int) resp.DataArray[(int) index5 + 3]);
          index2 = index5 + 6U;
          this.datasetMissing[(int) index1] = this.datasets[(int) index1].isEmpty();
        }
        return true;
      }

      protected override bool decodeLongParam(ulong longParam)
      {
        uint index = 0;
        uint num = 1;
        while (index < 5U)
        {
          this.checksumErrors[(int) index] = (longParam & (ulong) num) > 0UL;
          ++index;
          num <<= 1;
        }
        return true;
      }
    }

    private class CCommandGetDailyData : MinomatV2.CCommand
    {
      private const int NUMBER = 3;
      private const int REACTION_TIME = 300;
      private Minomat.CCommandDate m_date;
      private byte m_primaryAddress;
      private byte m_index;
      private const byte NO_OF_DATASETS = 5;
      internal bool[] checksumErrors = new bool[5];
      internal bool[] datasetMissing = new bool[5];
      private Minomat.DailyDataset[] datasets = new Minomat.DailyDataset[5];

      public CCommandGetDailyData(
        ulong serialNo,
        ulong userPassword,
        ref Minomat.CCommandDate date,
        byte primaryAddress,
        byte index)
        : base(serialNo, userPassword, 3U, 300U)
      {
        this.m_date = new Minomat.CCommandDate(date.getDay(), date.getMonth(), date.getYear());
        this.m_primaryAddress = primaryAddress;
        this.m_index = index;
      }

      public bool hasChecksumError(int index)
      {
        return index >= 0 && index < 5 && this.checksumErrors[index];
      }

      public bool isDatasetMissing(int index)
      {
        return index >= 0 && index < 5 && this.datasetMissing[index];
      }

      public Minomat.DailyDataset getDataset(int index)
      {
        return index >= 0 && index < 5 ? this.datasets[index] : (Minomat.DailyDataset) null;
      }

      protected CCommandGetDailyData(ref MinomatV2.CCommandDataArray encodedReq)
        : base(ref encodedReq)
      {
      }

      protected override ulong getLongParam() => (ulong) ((int) this.m_primaryAddress << 16);

      protected override byte getByteParam() => this.m_index;

      protected override uint getResponseContainerSize()
      {
        return (uint) (5 * (int) MinomatV2.CCommand.BLOCK_SIZE * 2);
      }

      protected override bool decodeResponseContainerData(ref MinomatV2.CCommandDataArray resp)
      {
        uint index1 = 0;
        uint index2 = MinomatV2.CCommand.REQUEST_SIZE;
        for (; index1 < 5U; ++index1)
        {
          this.datasets[(int) index1] = new Minomat.DailyDataset();
          this.datasets[(int) index1].date = new Minomat.CCommandDate((ushort) ((uint) resp.DataArray[(int) index2] << 8 | (uint) resp.DataArray[(int) index2 + 1]));
          uint index3 = index2 + 2U;
          this.datasets[(int) index1].dailyReading = (ulong) ((int) resp.DataArray[(int) index3] << 24 | (int) resp.DataArray[(int) index3 + 1] << 16 | (int) resp.DataArray[(int) index3 + 2] << 8 | (int) resp.DataArray[(int) index3 + 3]);
          index2 = index3 + 6U;
        }
        return true;
      }

      protected override bool decodeLongParam(ulong longParam)
      {
        uint index = 0;
        uint num = 1;
        while (index < 5U)
        {
          this.checksumErrors[(int) index] = (longParam & (ulong) ushort.MaxValue & (ulong) num) > 0UL;
          this.datasetMissing[(int) index] = (longParam & 4294901760UL & (ulong) num) > 0UL;
          ++index;
          num <<= 1;
        }
        return true;
      }
    }

    private class CCommandGetMonthlyData : MinomatV2.CCommand
    {
      private const int NUMBER = 2;
      private const int REACTION_TIME = 300;
      private Minomat.CCommandDate m_date;
      private byte m_primaryAddress;
      private byte m_index;
      private const byte NO_OF_DATASETS = 4;
      internal bool[] checksumErrors = new bool[4];
      internal bool[] datasetMissing = new bool[4];
      private Minomat.MonthlyDataset[] datasets = new Minomat.MonthlyDataset[4];

      public CCommandGetMonthlyData(
        ulong serialNo,
        ulong userPassword,
        ref Minomat.CCommandDate date,
        byte primaryAddress,
        byte index)
        : base(serialNo, userPassword, 2U, 300U)
      {
        this.m_date = new Minomat.CCommandDate(date.getDay(), date.getMonth(), date.getYear());
        this.m_primaryAddress = primaryAddress;
        this.m_index = index;
      }

      public bool hasChecksumError(int index)
      {
        return index >= 0 && index < 4 && this.checksumErrors[index];
      }

      public bool isDatasetMissing(int index)
      {
        return index >= 0 && index < 4 && this.datasetMissing[index];
      }

      public Minomat.MonthlyDataset getDataset(int index)
      {
        return index >= 0 && index < 4 ? this.datasets[index] : (Minomat.MonthlyDataset) null;
      }

      protected CCommandGetMonthlyData(ref MinomatV2.CCommandDataArray encodedReq)
        : base(ref encodedReq)
      {
      }

      protected override ulong getLongParam()
      {
        return (ulong) ((int) this.m_primaryAddress << 16 | (int) this.m_date.getEncodedDate());
      }

      protected override byte getByteParam() => this.m_index;

      protected override uint getResponseContainerSize()
      {
        return (uint) (4 * (int) MinomatV2.CCommand.BLOCK_SIZE * 4);
      }

      protected override bool decodeResponseContainerData(ref MinomatV2.CCommandDataArray resp)
      {
        uint index1 = 0;
        uint index2 = MinomatV2.CCommand.REQUEST_SIZE;
        for (; index1 < 4U; ++index1)
        {
          this.datasets[(int) index1] = new Minomat.MonthlyDataset();
          this.datasets[(int) index1].hkveSerialNo = (ulong) ((int) resp.DataArray[(int) index2] << 24 | (int) resp.DataArray[(int) index2 + 1] << 16 | (int) resp.DataArray[(int) index2 + 2] << 8 | (int) resp.DataArray[(int) index2 + 3]);
          uint index3 = index2 + 4U;
          this.datasets[(int) index1].status = resp.DataArray[(int) index3];
          uint index4 = index3 + 4U;
          this.datasets[(int) index1].fieldForceSum = (ushort) ((uint) resp.DataArray[(int) index4] << 8 | (uint) resp.DataArray[(int) index4 + 1]);
          uint index5 = index4 + 2U;
          this.datasets[(int) index1].hkveProtocols = (ushort) ((uint) resp.DataArray[(int) index5] << 8 | (uint) resp.DataArray[(int) index5 + 1]);
          uint index6 = index5 + 2U;
          this.datasets[(int) index1].deviceType = resp.DataArray[(int) index6];
          uint index7 = index6 + 4U;
          this.datasets[(int) index1].fullMonthReading = (ulong) ((int) resp.DataArray[(int) index7] << 24 | (int) resp.DataArray[(int) index7 + 1] << 16 | (int) resp.DataArray[(int) index7 + 2] << 8 | (int) resp.DataArray[(int) index7 + 3]);
          uint index8 = index7 + 4U;
          this.datasets[(int) index1].factor = (ushort) ((uint) resp.DataArray[(int) index8] << 8 | (uint) resp.DataArray[(int) index8 + 1]);
          uint index9 = index8 + 4U;
          this.datasets[(int) index1].halfMonthReading = (ulong) ((int) resp.DataArray[(int) index9] << 24 | (int) resp.DataArray[(int) index9 + 1] << 16 | (int) resp.DataArray[(int) index9 + 2] << 8 | (int) resp.DataArray[(int) index9 + 3]);
          uint index10 = index9 + 4U;
          this.datasets[(int) index1].date = new Minomat.CCommandDate((ushort) ((uint) resp.DataArray[(int) index10] << 8 | (uint) resp.DataArray[(int) index10 + 1]));
          index2 = index10 + 4U;
        }
        return true;
      }

      protected override bool decodeLongParam(ulong longParam)
      {
        uint index = 0;
        uint num = 1;
        while (index < 4U)
        {
          this.checksumErrors[(int) index] = (longParam & (ulong) ushort.MaxValue & (ulong) num) > 0UL;
          this.datasetMissing[(int) index] = (longParam & 4294901760UL & (ulong) num) > 0UL;
          ++index;
          num <<= 1;
        }
        return true;
      }
    }

    private class CCommandFindHKVE : MinomatV2.CCommand
    {
      private const int NUMBER = 18;
      private const int REACTION_TIME = 100;
      private ulong m_serialNoParam;
      private MinomatV2.CCommandFindHKVE.Answer answer;

      public CCommandFindHKVE(ulong serialNo, ulong userPassword, ulong serialNoParam)
        : base(serialNo, userPassword, 18U, 100U)
      {
        this.m_serialNoParam = serialNoParam;
      }

      public MinomatV2.CCommandFindHKVE.Answer getAnswer() => this.answer;

      protected override ulong getLongParam() => this.m_serialNoParam;

      protected override bool decodeLongParam(ulong LongParam)
      {
        switch ((int) LongParam)
        {
          case 0:
            this.answer = MinomatV2.CCommandFindHKVE.Answer.NOT_FOUND;
            break;
          case 1:
            this.answer = MinomatV2.CCommandFindHKVE.Answer.REGISTERED;
            break;
          case 2:
            this.answer = MinomatV2.CCommandFindHKVE.Answer.SELF_REGISTERED;
            break;
          default:
            return false;
        }
        return true;
      }

      protected CCommandFindHKVE(ref MinomatV2.CCommandDataArray encodedReq)
        : base(ref encodedReq)
      {
      }

      public enum Answer
      {
        NOT_FOUND,
        REGISTERED,
        SELF_REGISTERED,
      }
    }

    private class CCommandRegisterHKVE : MinomatV2.CCommand
    {
      private const int NUMBER = 5;
      private const int REACTION_TIME = 100;
      private ulong m_serialNoParam;
      private MinomatV2.CCommandRegisterHKVE.Answer answer;
      private MinomatV2.CCommandRegisterHKVE.RegistrationType m_registrationType;

      public CCommandRegisterHKVE(
        ulong serialNo,
        ulong userPassword,
        ulong serialNoParam,
        MinomatV2.CCommandRegisterHKVE.RegistrationType registrationType)
        : base(serialNo, userPassword, 5U, 100U)
      {
        this.m_serialNoParam = serialNoParam;
        this.m_registrationType = registrationType;
      }

      public MinomatV2.CCommandRegisterHKVE.Answer getAnswer() => this.answer;

      protected override ulong getLongParam() => this.m_serialNoParam;

      protected override bool decodeLongParam(ulong LongParam)
      {
        switch ((int) LongParam)
        {
          case 0:
            this.answer = MinomatV2.CCommandRegisterHKVE.Answer.OK;
            break;
          case 1:
            this.answer = MinomatV2.CCommandRegisterHKVE.Answer.ALREADY_REGISTERED;
            break;
          case 2:
            this.answer = MinomatV2.CCommandRegisterHKVE.Answer.NOT_FOUND;
            break;
          default:
            return false;
        }
        return true;
      }

      protected override byte getByteParam() => (byte) this.m_registrationType;

      protected CCommandRegisterHKVE(ref MinomatV2.CCommandDataArray encodedReq)
        : base(ref encodedReq)
      {
      }

      public enum Answer
      {
        OK,
        ALREADY_REGISTERED,
        NOT_FOUND,
      }

      public enum RegistrationType
      {
        REMOVE,
        REGISTER,
        SELF_REGISTERED,
      }
    }

    private class CCommandCheckHKVERegistration : MinomatV2.CCommand
    {
      private const int NUMBER = 6;
      private const int REACTION_TIME = 100;
      private byte m_index;
      private ulong m_serialNoAnswer;

      public CCommandCheckHKVERegistration(ulong serialNo, ulong userPassword, byte index)
        : base(serialNo, userPassword, 6U, 100U)
      {
        this.m_index = index;
      }

      public ulong getSerialNoAnswer() => this.m_serialNoAnswer;

      public byte getIndex() => this.m_index;

      protected override byte getByteParam() => this.m_index;

      protected override bool decodeLongParam(ulong LongParam)
      {
        this.m_serialNoAnswer = LongParam;
        return true;
      }
    }

    private class CCommandSystemInit : MinomatV2.CCommand
    {
      private const int NUMBER = 11;
      private const int REACTION_TIME = 0;
      private ulong m_deleteCode;

      public CCommandSystemInit(ulong serialNo, ulong userPassword, ulong deleteCode)
        : base(serialNo, userPassword, 11U, 0U)
      {
        this.m_deleteCode = deleteCode;
      }

      public bool isInvalidDeleteCode() => this.getResponseStatus().hasParameterError;

      protected override ulong getLongParam() => this.m_deleteCode;
    }

    private class CCommandStartHKVEReceptionWindow(ulong serialNo, ulong userPassword) : 
      MinomatV2.CCommand(serialNo, userPassword, 20U, 100U)
    {
      private const int NUMBER = 20;
      private const int REACTION_TIME = 100;
    }

    private class CCommandStopReception(ulong serialNo, ulong userPassword) : MinomatV2.CCommand(serialNo, userPassword, 13U, 100U)
    {
      private const int NUMBER = 13;
      private const int REACTION_TIME = 100;
    }

    private class CCommandGetConfiguration : MinomatV2.CCommand
    {
      private const int NUMBER = 15;
      private const int REACTION_TIME = 100;
      private MinomatV2.Configuration m_configuration = new MinomatV2.Configuration();

      public CCommandGetConfiguration(ulong serialNo, ulong userPassword)
        : base(serialNo, userPassword, 15U, 100U)
      {
      }

      public MinomatV2.Configuration getConfiguration() => this.m_configuration;

      protected CCommandGetConfiguration(ref MinomatV2.CCommandDataArray encodedReq)
        : base(ref encodedReq)
      {
      }

      protected override uint getResponseContainerSize() => 5U * MinomatV2.CCommand.BLOCK_SIZE;

      protected override bool decodeResponseContainerData(ref MinomatV2.CCommandDataArray resp)
      {
        Util.ByteArrayToHexString(resp.DataArray);
        this.m_configuration.SerialNo = ((ulong) ((int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 3])).ToString("X8");
        this.m_configuration.PrimaryAddress = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4];
        this.m_configuration.hkve.WindowStart = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE];
        this.m_configuration.master.WindowStartDailyData = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 1];
        this.m_configuration.master.WindowDurationDailyData = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 2];
        this.m_configuration.master.WindowStartEvent = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 3];
        this.m_configuration.master.WindowDurationEvent = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 4];
        this.m_configuration.hkve.NoOfWindowsAfterEvent = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2];
        this.m_configuration.hkve.WindowGapAfterEvent = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 1];
        this.m_configuration.hkve.NoOfWindowsDailyData = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 2];
        this.m_configuration.hkve.WindowGapDailyData = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 3];
        this.m_configuration.HandTerminalCycleDuration = resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 4];
        Minomat.CCommandDate ccommandDate = new Minomat.CCommandDate((ushort) ((uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3] << 8 | (uint) resp.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3 + 1]));
        this.m_configuration.EventDay = new DateTime(ccommandDate.getYear() == (byte) 0 ? 2000 : (int) ccommandDate.getYear() + 2000, (int) ccommandDate.getMonth(), (int) ccommandDate.getDay());
        this.m_configuration.UserPassword = Encoding.ASCII.GetString(resp.DataArray, (int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4, 4);
        return true;
      }
    }

    private class CCommandSetConfiguration : MinomatV2.CCommand
    {
      private const int NUMBER = 12;
      private const int REACTION_TIME = 0;
      private MinomatV2.Configuration configuration;

      public CCommandSetConfiguration(
        ulong serialNo,
        ulong userPassword,
        MinomatV2.Configuration configuration)
        : base(serialNo, userPassword, 12U, 0U)
      {
        this.configuration = configuration;
      }

      protected override void encodeRequestContainerData(ref MinomatV2.CCommandDataArray req)
      {
        long bcdInt64 = Util.ConvertInt64ToBcdInt64(Convert.ToInt64(this.configuration.SerialNo));
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE] = (byte) (bcdInt64 >> 24);
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 1] = (byte) (bcdInt64 >> 16);
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 2] = (byte) (bcdInt64 >> 8);
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 3] = (byte) bcdInt64;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + 4] = this.configuration.PrimaryAddress;
        this.generateChecksum(ref req, MinomatV2.CCommand.REQUEST_SIZE);
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE] = this.configuration.hkve.WindowStart;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 1] = this.configuration.master.WindowStartDailyData;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 2] = this.configuration.master.WindowDurationDailyData;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 3] = this.configuration.master.WindowStartEvent;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE + 4] = this.configuration.master.WindowDurationEvent;
        this.generateChecksum(ref req, MinomatV2.CCommand.REQUEST_SIZE + MinomatV2.CCommand.BLOCK_SIZE);
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2] = this.configuration.hkve.NoOfWindowsAfterEvent;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 1] = this.configuration.hkve.WindowGapAfterEvent;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 2] = this.configuration.hkve.NoOfWindowsDailyData;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 3] = this.configuration.hkve.WindowGapDailyData;
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 2 + 4] = this.configuration.HandTerminalCycleDuration;
        this.generateChecksum(ref req, MinomatV2.CCommand.REQUEST_SIZE + MinomatV2.CCommand.BLOCK_SIZE * 2U);
        ushort encodedDate = new Minomat.CCommandDate((byte) this.configuration.EventDay.Day, (byte) this.configuration.EventDay.Month, (byte) (this.configuration.EventDay.Year - 2000)).getEncodedDate();
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3] = (byte) ((uint) encodedDate >> 8);
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 3 + 1] = (byte) encodedDate;
        this.generateChecksum(ref req, MinomatV2.CCommand.REQUEST_SIZE + MinomatV2.CCommand.BLOCK_SIZE * 3U);
        byte[] bytes = Encoding.ASCII.GetBytes(this.configuration.UserPassword);
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4] = bytes[0];
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4 + 1] = bytes[1];
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4 + 2] = bytes[2];
        req.DataArray[(int) MinomatV2.CCommand.REQUEST_SIZE + (int) MinomatV2.CCommand.BLOCK_SIZE * 4 + 3] = bytes[3];
        this.generateChecksum(ref req, MinomatV2.CCommand.REQUEST_SIZE + MinomatV2.CCommand.BLOCK_SIZE * 4U);
        Util.ByteArrayToHexString(req.DataArray);
      }

      protected override uint getRequestContainerSize() => 5U * MinomatV2.CCommand.BLOCK_SIZE;

      protected override uint getResponseContainerSize() => 5U * MinomatV2.CCommand.BLOCK_SIZE;
    }

    private class CCommunication
    {
      private static int SEND_REPEAT = 3;
      private bool m_isLastCommandSuccessful;
      private ulong m_connectSerialNo;
      private ulong m_connectUserPassword;
      internal DeviceCollectorFunctions MyBus;

      public CCommunication(DeviceCollectorFunctions SerialBus) => this.MyBus = SerialBus;

      public bool sendCommands(ref MinomatV2.CCommandArray CommandArray)
      {
        return this.sendCommands(ref CommandArray, true);
      }

      public bool sendCommands(ref MinomatV2.CCommandArray CommandArray, bool waitForAnswer)
      {
        bool flag = true;
        int num1 = 0;
        this.MyBus.SendMessage("Start sending commands.", 0, GMM_EventArgs.MessageType.StandardMessage);
        int num2 = 0;
        GMM_EventArgs e = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
        for (int index = 0; index < CommandArray.CCommands.Count; ++index)
        {
          ++num1;
          Type type = CommandArray.CCommands[index].GetType();
          this.MyBus.SendProgressMessage((object) this, "Perform '" + type.Name.Replace("CCommand", "") + "' command " + index.ToString() + "/" + CommandArray.CCommands.Count.ToString());
          int num3;
          int num4;
          if (type == typeof (MinomatV2.CCommandCheckHKVERegistration))
          {
            num3 = 33;
            num4 = 17;
          }
          else
          {
            num3 = 50;
            num4 = 50;
          }
          int progress = num1 * num4 / CommandArray.CCommands.Count + num3;
          if (num2 != progress)
          {
            num2 = progress;
            e.EventMessage = string.Format("Download {0}%", (object) progress);
            e.ProgressPercentage = progress;
            this.MyBus.SendMessage(e);
            this.MyBus.SendProgress((object) this, progress);
          }
          MinomatV2.CCommand ccommand = CommandArray.CCommands[index];
          int num5;
          if (!this.sendCommand(ref ccommand, waitForAnswer))
          {
            MinomatV2.logger.Error("sendCommands() stopped: Error in Communication");
            DeviceCollectorFunctions bus = this.MyBus;
            int MessageInt = num1;
            num5 = MessageInt + 1;
            bus.SendMessage("Error in Communication.", MessageInt, GMM_EventArgs.MessageType.MinomatErrorMessage);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "sendCommand error");
            return false;
          }
          if (this.MyBus.BreakRequest)
          {
            DeviceCollectorFunctions bus = this.MyBus;
            int MessageInt = num1;
            num5 = MessageInt + 1;
            bus.SendMessage("Break requested. ", MessageInt, GMM_EventArgs.MessageType.EndMessage);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled);
            return false;
          }
        }
        this.MyBus.SendMessage("Sending finished.", 0, GMM_EventArgs.MessageType.StandardMessage);
        return flag;
      }

      public bool Connect(ulong serialNo, ulong userPassword)
      {
        this.m_connectSerialNo = serialNo;
        this.m_connectUserPassword = userPassword;
        return this.RunConnect();
      }

      private bool RunConnect()
      {
        this.MyBus.SendProgressMessage((object) this, "Check wake up ...");
        if (this.CheckIfMinomatIsAlive())
          return true;
        ZR_ClassLibMessages.ClearErrors();
        MinomatV2.logger.Trace("Send a stop command before trying to connect.");
        MinomatV2.CCommand command1 = (MinomatV2.CCommand) new MinomatV2.CCommandStopReception(this.m_connectSerialNo, this.m_connectUserPassword);
        if (!this.sendCommand(ref command1, false))
          return false;
        MinomatV2.CCommand command2 = (MinomatV2.CCommand) new MinomatV2.CCommandGetTime(this.m_connectSerialNo, this.m_connectUserPassword);
        this.MyBus.SendProgressMessage((object) this, "Wake up ...");
        MinomatV2.logger.Trace("Start wake up...");
        GMM_EventArgs e = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
        DateTime dateTimeNow = SystemValues.DateTimeNow;
        try
        {
          MinomatV2.logger.Factory.SuspendLogging();
          MinomatV2.logger.Factory.ReconfigExistingLoggers();
          int num = 0;
          while ((SystemValues.DateTimeNow - dateTimeNow).TotalSeconds < 30.0)
          {
            if (!this.sendCommand(ref command2, false))
            {
              MinomatV2.logger.Error("Failed to send wake up sequence!");
              return false;
            }
            int progress = (int) ((SystemValues.DateTimeNow - dateTimeNow).TotalSeconds * 33.0) / 30;
            if (num != progress)
            {
              num = progress;
              e.EventMessage = "Wake-up progress " + progress.ToString() + "%";
              e.ProgressPercentage = progress;
              this.MyBus.SendMessage(e);
              this.MyBus.SendProgress((object) this, progress);
            }
            if (this.MyBus.BreakRequest)
            {
              MinomatV2.logger.Info("Break requested.");
              return false;
            }
          }
        }
        finally
        {
          MinomatV2.logger.Factory.ResumeLogging();
          MinomatV2.logger.Factory.ReconfigExistingLoggers();
        }
        MinomatV2.logger.Trace("End wake up");
        if (!Util.Wait(2000L, "after wake-up sequence", (ICancelable) this.MyBus, MinomatV2.logger))
          return false;
        this.MyBus.MyCom.ClearCom();
        if (this.CheckIfMinomatIsAlive())
        {
          ZR_ClassLibMessages.ClearErrors();
          return true;
        }
        MinomatV2.logger.Trace("Send a stop command.");
        if (!this.sendCommand(ref command1, false))
          return false;
        this.MyBus.SendMessage("Connection failed.", 0, GMM_EventArgs.MessageType.MinomatErrorMessage);
        return false;
      }

      private bool CheckIfMinomatIsAlive()
      {
        MinomatV2.CCommand command = (MinomatV2.CCommand) new MinomatV2.CCommandGetTime(this.m_connectSerialNo, this.m_connectUserPassword);
        int sendRepeat = MinomatV2.CCommunication.SEND_REPEAT;
        MinomatV2.CCommunication.SEND_REPEAT = 2;
        bool flag = this.sendCommand(ref command, true);
        MinomatV2.CCommunication.SEND_REPEAT = sendRepeat;
        MinomatV2.logger.Trace("Minomat is alive: {0}", flag);
        return flag;
      }

      internal bool sendCommand(ref MinomatV2.CCommand command, bool waitForAnswer)
      {
        MinomatV2.CCommandDataArray ccommandDataArray = new MinomatV2.CCommandDataArray();
        int sendRepeat = MinomatV2.CCommunication.SEND_REPEAT;
        try
        {
          do
          {
            uint num = command.encodeRequest(ref ccommandDataArray);
            this.m_isLastCommandSuccessful = this.internalSendCommand(ref ccommandDataArray);
            if (!waitForAnswer)
              return true;
            if (this.m_isLastCommandSuccessful)
            {
              ccommandDataArray.SetSize(num);
              this.m_isLastCommandSuccessful = (int) this.internalReceiveCommand(ref ccommandDataArray, num, command.getReactionTime()) == (int) num;
            }
          }
          while (!this.m_isLastCommandSuccessful && --sendRepeat > 0);
          try
          {
            command.decodeResponse(this.m_isLastCommandSuccessful, ref ccommandDataArray);
          }
          catch (Exception ex)
          {
            string str = "Error in MinomatV2: decodeResponse " + ex.Message;
            MinomatV2.logger.Error(ex, str);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FramingError, str);
            return false;
          }
        }
        catch (Exception ex)
        {
          string str = "Error in MinomatV2: sendCommands " + ex.Message;
          MinomatV2.logger.Error(ex, str);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
          return false;
        }
        return this.m_isLastCommandSuccessful;
      }

      protected virtual void internalClearBuffers() => this.MyBus.MyCom.ClearCom();

      protected virtual bool internalSendCommand(ref MinomatV2.CCommandDataArray commandData)
      {
        return false;
      }

      protected virtual uint internalReceiveCommand(
        ref MinomatV2.CCommandDataArray commandData,
        uint responseLength,
        uint reactionTime)
      {
        return 0;
      }
    }

    private class CSerialCommunication(DeviceCollectorFunctions SerialBus) : MinomatV2.CCommunication(SerialBus)
    {
      protected override bool internalSendCommand(ref MinomatV2.CCommandDataArray commandData)
      {
        ByteField DataBlock = new ByteField(commandData.DataArray.Length + MinomatV2.FRAME_PREFIX.Length);
        for (int index = 0; index < MinomatV2.FRAME_PREFIX.Length; ++index)
          DataBlock.Add(MinomatV2.FRAME_PREFIX[index]);
        for (int index = 0; index < commandData.DataArray.Length; ++index)
          DataBlock.Add(commandData.DataArray[index]);
        return this.MyBus.MyCom.SendBlock(ref DataBlock);
      }

      protected override uint internalReceiveCommand(
        ref MinomatV2.CCommandDataArray commandData,
        uint responseLength,
        uint reactionTime)
      {
        int length = MinomatV2.FRAME_PREFIX.Length;
        ByteField DataBlock = new ByteField((int) ((long) responseLength + (long) length));
        if (this.MyBus.MyCom.ReceiveBlock(ref DataBlock, (int) ((long) responseLength + (long) length), true))
        {
          int count = DataBlock.Count;
          int num1 = 0;
          for (int index = 0; index < count; ++index)
          {
            if (DataBlock.Data[index] == (byte) 85)
            {
              num1 = index;
              break;
            }
          }
          int index1 = num1 + length + 4;
          if (index1 >= DataBlock.Data.Length)
            return 0;
          uint num2 = (uint) DataBlock.Data[index1];
          if ((long) num2 + (long) length == (long) (DataBlock.Count - num1))
          {
            commandData.SetSize(responseLength);
            for (int index2 = 0; (long) index2 < (long) num2; ++index2)
              commandData.DataArray[index2] = DataBlock.Data[index2 + length + num1];
            return responseLength;
          }
        }
        return 0;
      }
    }

    private abstract class CCommand
    {
      protected static uint BLOCK_SIZE = 8;
      protected static uint REQUEST_SIZE = MinomatV2.CCommand.BLOCK_SIZE * 4U;
      private uint m_number;
      private uint m_reactionTime;
      private ulong m_serialNo;
      private ulong m_userPassword;
      private MinomatV2.CCommandDataArray m_encodedReq = new MinomatV2.CCommandDataArray();
      private bool m_hasReceivedResponse;
      private MinomatV2.ResponseStatus m_responseStatus = new MinomatV2.ResponseStatus();

      protected CCommand(ulong serialNo, ulong userPassword, uint number, uint reactionTime)
      {
        this.m_serialNo = serialNo;
        this.m_userPassword = userPassword;
        this.m_number = number;
        this.m_reactionTime = reactionTime;
        this.m_hasReceivedResponse = false;
      }

      protected CCommand(ref MinomatV2.CCommandDataArray encodedReq)
      {
        if (this.m_encodedReq == null)
          this.m_encodedReq = new MinomatV2.CCommandDataArray();
        this.m_encodedReq = encodedReq;
        if ((long) this.m_encodedReq.DataArray.Length < (long) MinomatV2.CCommand.REQUEST_SIZE)
          return;
        byte index = 8;
        byte num = 24;
        while (index < (byte) 12)
        {
          this.m_serialNo |= (ulong) this.m_encodedReq.DataArray[(int) index] << (int) num;
          ++index;
          num -= (byte) 8;
        }
      }

      protected void generateChecksum(ref MinomatV2.CCommandDataArray req, uint index)
      {
        ushort checksumReversed = CRC.calculateChecksumReversed(ref req.DataArray, 6U, index);
        req.DataArray[(int) index + 6 + 1] = (byte) ((uint) checksumReversed >> 8);
        req.DataArray[(int) index + 6] = (byte) ((uint) checksumReversed & (uint) byte.MaxValue);
      }

      private void decodeResponseStatus(ushort byteParam)
      {
        this.m_responseStatus.hasFlashError = ((int) byteParam & 1) == 1;
        this.m_responseStatus.hasParameterError = ((int) byteParam & 2) == 2;
        this.m_responseStatus.hasRTCError = ((int) byteParam & 4) == 4;
        this.m_responseStatus.hasGSMConnection = ((int) byteParam & 8) == 8;
        this.m_responseStatus.hasIrDAHandheldConnection = ((int) byteParam & 16) == 16;
        this.m_responseStatus.hasRFHandheldConnection = ((int) byteParam & 32) == 32;
        this.m_responseStatus.hasOpenMasterWindow = ((int) byteParam & 64) == 64;
        this.m_responseStatus.hasOpenHKVEWindow = ((int) byteParam & 128) == 128;
      }

      private static int getIntFromHexChar(char Value) => Convert.ToInt32(Value.ToString(), 16);

      public uint getNumber() => this.m_number;

      public uint getReactionTime() => this.m_reactionTime;

      public ulong getSerialNo() => this.m_serialNo;

      public ulong getUserpassword() => this.m_userPassword;

      public bool hasReceivedResponse() => this.m_hasReceivedResponse;

      public MinomatV2.ResponseStatus getResponseStatus() => this.m_responseStatus;

      public bool hasError()
      {
        return !this.hasReceivedResponse() || this.m_responseStatus.hasFlashError || this.m_responseStatus.hasParameterError || this.m_responseStatus.hasRTCError;
      }

      public uint encodeRequest(ref MinomatV2.CCommandDataArray req)
      {
        uint requestContainerSize = this.getRequestContainerSize();
        req.SetSize(MinomatV2.CCommand.REQUEST_SIZE + requestContainerSize);
        this.m_encodedReq.SetSize(0U);
        if (this.m_encodedReq.DataArray.Length == 0)
        {
          int num1 = 0;
          byte[] dataArray1 = req.DataArray;
          int index1 = num1;
          int num2 = index1 + 1;
          int num3 = (int) (byte) (this.m_userPassword >> 24);
          dataArray1[index1] = (byte) num3;
          byte[] dataArray2 = req.DataArray;
          int index2 = num2;
          int num4 = index2 + 1;
          int num5 = (int) (byte) (this.m_userPassword >> 16);
          dataArray2[index2] = (byte) num5;
          byte[] dataArray3 = req.DataArray;
          int index3 = num4;
          int num6 = index3 + 1;
          int num7 = (int) (byte) (this.m_userPassword >> 8);
          dataArray3[index3] = (byte) num7;
          byte[] dataArray4 = req.DataArray;
          int index4 = num6;
          int num8 = index4 + 1;
          int num9 = (int) (byte) (this.m_userPassword & (ulong) byte.MaxValue);
          dataArray4[index4] = (byte) num9;
          byte[] dataArray5 = req.DataArray;
          int index5 = num8;
          int num10 = index5 + 1;
          int length = (int) (byte) req.DataArray.Length;
          dataArray5[index5] = (byte) length;
          byte[] dataArray6 = req.DataArray;
          int index6 = num10;
          int num11 = index6 + 1;
          int number = (int) (byte) this.m_number;
          dataArray6[index6] = (byte) number;
          this.generateChecksum(ref req, 0U);
          int num12 = num11 + 2;
          byte[] dataArray7 = req.DataArray;
          int index7 = num12;
          int num13 = index7 + 1;
          int num14 = (int) (byte) (this.m_serialNo >> 24);
          dataArray7[index7] = (byte) num14;
          byte[] dataArray8 = req.DataArray;
          int index8 = num13;
          int num15 = index8 + 1;
          int num16 = (int) (byte) (this.m_serialNo >> 16);
          dataArray8[index8] = (byte) num16;
          byte[] dataArray9 = req.DataArray;
          int index9 = num15;
          int num17 = index9 + 1;
          int num18 = (int) (byte) (this.m_serialNo >> 8);
          dataArray9[index9] = (byte) num18;
          byte[] dataArray10 = req.DataArray;
          int index10 = num17;
          int num19 = index10 + 1;
          int num20 = (int) (byte) (this.m_serialNo & (ulong) byte.MaxValue);
          dataArray10[index10] = (byte) num20;
          byte[] dataArray11 = req.DataArray;
          int index11 = num19;
          int num21 = index11 + 1;
          dataArray11[index11] = (byte) 0;
          byte[] dataArray12 = req.DataArray;
          int index12 = num21;
          int num22 = index12 + 1;
          dataArray12[index12] = (byte) 0;
          this.generateChecksum(ref req, MinomatV2.CCommand.BLOCK_SIZE);
          int num23 = num22 + 2;
          for (int index13 = 0; index13 < 6; ++index13)
            req.DataArray[num23++] = (byte) 0;
          this.generateChecksum(ref req, MinomatV2.CCommand.BLOCK_SIZE * 2U);
          int num24 = num23 + 2;
          byte[] dataArray13 = req.DataArray;
          int index14 = num24;
          int num25 = index14 + 1;
          dataArray13[index14] = byte.MaxValue;
          ulong longParam = this.getLongParam();
          byte[] dataArray14 = req.DataArray;
          int index15 = num25;
          int num26 = index15 + 1;
          int num27 = (int) (byte) (longParam >> 24);
          dataArray14[index15] = (byte) num27;
          byte[] dataArray15 = req.DataArray;
          int index16 = num26;
          int num28 = index16 + 1;
          int num29 = (int) (byte) (longParam >> 16);
          dataArray15[index16] = (byte) num29;
          byte[] dataArray16 = req.DataArray;
          int index17 = num28;
          int num30 = index17 + 1;
          int num31 = (int) (byte) (longParam >> 8);
          dataArray16[index17] = (byte) num31;
          byte[] dataArray17 = req.DataArray;
          int index18 = num30;
          int num32 = index18 + 1;
          int num33 = (int) (byte) (longParam & (ulong) byte.MaxValue);
          dataArray17[index18] = (byte) num33;
          byte[] dataArray18 = req.DataArray;
          int index19 = num32;
          int num34 = index19 + 1;
          int byteParam = (int) this.getByteParam();
          dataArray18[index19] = (byte) byteParam;
          this.generateChecksum(ref req, MinomatV2.CCommand.BLOCK_SIZE * 3U);
          int num35 = num34 + 2;
          if (requestContainerSize > 0U)
            this.encodeRequestContainerData(ref req);
        }
        return MinomatV2.CCommand.REQUEST_SIZE + this.getResponseContainerSize();
      }

      public void decodeResponse(bool hasReceivedResponse, ref MinomatV2.CCommandDataArray resp)
      {
        if (!hasReceivedResponse || (long) resp.DataArray.Length != (long) (MinomatV2.CCommand.REQUEST_SIZE + this.getResponseContainerSize()))
          return;
        int num = (int) ((long) resp.DataArray.Length / (long) MinomatV2.CCommand.BLOCK_SIZE);
        for (uint index = 0; (long) index < (long) num; ++index)
        {
          if (!CRC.correctErrorReversed(ref resp.DataArray, MinomatV2.CCommand.BLOCK_SIZE, index * MinomatV2.CCommand.BLOCK_SIZE))
            return;
        }
        byte index1 = (byte) (MinomatV2.CCommand.BLOCK_SIZE * 3U);
        this.decodeResponseStatus((ushort) resp.DataArray[(int) index1 + 5]);
        bool flag = (long) resp.DataArray[0] == ((long) (this.m_userPassword >> 24) & (long) byte.MaxValue) && (long) resp.DataArray[1] == ((long) (this.m_userPassword >> 16) & (long) byte.MaxValue) && (long) resp.DataArray[2] == ((long) (this.m_userPassword >> 8) & (long) byte.MaxValue) && (long) resp.DataArray[3] == ((long) this.m_userPassword & (long) byte.MaxValue);
        if (!flag)
          flag = resp.DataArray[0] == (byte) 51 && resp.DataArray[1] == (byte) 52 && resp.DataArray[2] == (byte) 49 && resp.DataArray[3] == (byte) 52;
        if (!flag || !this.isResponseSizeValid(ref resp) || (int) resp.DataArray[5] != (int) this.m_number || (long) ((int) resp.DataArray[(int) MinomatV2.CCommand.BLOCK_SIZE] << 24 | (int) resp.DataArray[(int) MinomatV2.CCommand.BLOCK_SIZE + 1] << 16 | (int) resp.DataArray[(int) MinomatV2.CCommand.BLOCK_SIZE + 2] << 8 | (int) resp.DataArray[(int) MinomatV2.CCommand.BLOCK_SIZE + 3]) != (long) this.m_serialNo || resp.DataArray[(int) index1] != byte.MaxValue || !this.decodeLongParam((ulong) ((int) resp.DataArray[(int) index1 + 1] << 24 | (int) resp.DataArray[(int) index1 + 2] << 16 | (int) resp.DataArray[(int) index1 + 3] << 8 | (int) resp.DataArray[(int) index1 + 4])) || !this.decodeResponseContainerData(ref resp))
          return;
        this.m_hasReceivedResponse = true;
      }

      protected virtual ulong getLongParam() => 0;

      protected virtual bool decodeLongParam(ulong LongParam) => true;

      protected virtual byte getByteParam() => 0;

      protected virtual uint getRequestContainerSize() => 0;

      protected virtual void encodeRequestContainerData(ref MinomatV2.CCommandDataArray resp)
      {
      }

      protected virtual uint getResponseContainerSize() => 0;

      protected virtual bool decodeResponseContainerData(ref MinomatV2.CCommandDataArray resp)
      {
        return true;
      }

      protected virtual bool isResponseSizeValid(ref MinomatV2.CCommandDataArray resp)
      {
        return (int) resp.DataArray[4] == resp.DataArray.Length;
      }
    }
  }
}
