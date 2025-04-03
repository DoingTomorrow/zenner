// Decompiled with JetBrains decompiler
// Type: AsyncCom.AsyncFunctions
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using MinoConnect;
using NLog;
using StartupLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  public class AsyncFunctions : 
    IAsyncFunctions,
    I_ZR_Component,
    ILockable,
    ICancelable,
    IWindow,
    IReadoutConfig
  {
    private static NLog.Logger logger = LogManager.GetLogger(nameof (AsyncFunctions));
    internal bool IsPluginObject = false;
    internal AsyncFunctionsBase MyComType;
    internal AsyncComConnectionType ConnectionTypeSelected = AsyncComConnectionType.COM;
    internal ResourceManager MyRes = new ResourceManager("AsyncCom.AsyncComRes", typeof (AsyncFunctions).Assembly);
    public EventLogger Logger = new EventLogger();
    internal MeterVPN MyMeterVPN = new MeterVPN();
    internal ZR_AsyncCom ComWindow;
    internal const int AnswerOffsetTime_default = 0;
    internal const int TransTime_GlobalOffset_default = 0;
    internal const int RecTime_GlobalOffset_default = 0;
    internal const int RecTime_BeforFirstByte_default = 400;
    internal const double RecTime_OffsetPerByte_default = 0.0;
    internal const int RecTime_OffsetPerBlock_default = 50;
    internal const int RecTransTime_default = 10;
    internal const int BreakIntervalTime_default = 10000;
    internal const int TransTime_BreakTime_default = 700;
    internal const int TransTime_AfterBreak_default = 50;
    internal const int TransTime_AfterOpen_default = 200;
    internal const int WaitBeforeRepeatTime_default = 200;
    private string ComPortUsed = "COM1";
    private int BaudrateUsed = 0;
    private string ParityUsed = "even";
    internal bool HardwareHandshake = false;
    internal bool ComIsOpen = false;
    internal WakeupSystem Wakeup = WakeupSystem.None;
    internal string transceiverDeviceInfo = "";
    internal bool UseMinoConnect = false;
    internal int MinoConnectAutoPowerOffTime = 1000;
    internal MinoConnectState.BaseStateEnum MinoConnectBaseState = MinoConnectState.BaseStateEnum.off;
    internal bool IgnoreReceiveErrorsOnTransmitTime = false;
    internal bool TestEcho = true;
    internal bool EchoOn = false;
    public bool EchoTestIsActive = false;
    public bool ErrorMessageBox = false;
    internal bool MBusFrameTestWindowOn = false;
    internal bool IrDa = false;
    public bool IrDaDaveTailSide = false;
    internal int MainThreadId;
    internal double ByteTime;
    internal int RecTime_BeforFirstByte;
    internal double RecTime_OffsetPerByte;
    internal int RecTime_OffsetPerBlock;
    internal int RecTime_GlobalOffset;
    internal int TransTime_GlobalOffset;
    internal int RecTransTime;
    internal int TransTime_BreakTime;
    internal int TransTime_AfterBreak;
    internal int TransTime_AfterOpen;
    public int WakeupIntervalTime;
    public int WaitBeforeRepeatTime;
    internal int AnswerOffsetTime;
    private Version ComponentVersion = new Version();
    public DateTime LastWakeupRefreshTime = DateTime.MinValue;
    internal DateTime LastTransmitEndTime = DateTime.MinValue;
    private DateTime EarliestTransmitTimeAbsolut = DateTime.MinValue;
    private static volatile string ownerOfLock;
    private bool readoutConfigByBusFile = false;
    internal StringBuilder LineBuffer = new StringBuilder(210);
    private ConfigList ConfigList;

    public event EventHandler<GMM_EventArgs> OnAsyncComMessage;

    public event System.EventHandler ConnectionLost;

    public event System.EventHandler BatterieLow;

    public bool WakeupTemporaryOff { get; set; }

    public ZR_ClassLibrary.TransceiverDevice Transceiver { get; set; }

    public bool ReadoutConfigByBusFile
    {
      get => this.readoutConfigByBusFile;
      set
      {
        this.readoutConfigByBusFile = value;
        if (!this.readoutConfigByBusFile)
          return;
        this.DisableConfigList();
      }
    }

    public bool BreakRequest { get; set; }

    public long InputBufferLength => this.MyComType.InputBufferLength;

    public int MinoConnectIrDaPulseLength { get; set; }

    internal int Baudrate
    {
      get => this.BaudrateUsed;
      set
      {
        if (value == this.BaudrateUsed)
          return;
        this.BaudrateUsed = value;
        this.ByteTime = 1000.0 / (double) this.Baudrate * 11.0;
        if (this.ComIsOpen && (!(this.MyComType is AsyncSerial) || !(((AsyncSerial) this.MyComType).MySerialPort is MinoConnectSerialPort)))
          this.Close();
      }
    }

    public DateTime FirstCalculatedEarliestTransmitTime { get; set; }

    internal DateTime EarliestTransmitTime
    {
      set
      {
        if (!(value > this.EarliestTransmitTimeAbsolut))
          return;
        this.EarliestTransmitTimeAbsolut = value;
      }
      get => this.EarliestTransmitTimeAbsolut;
    }

    internal string Parity
    {
      get => this.ParityUsed;
      set
      {
        if (!(value != this.ParityUsed))
          return;
        this.ParityUsed = value;
        if (this.ComIsOpen && (!(this.MyComType is AsyncSerial) || !(((AsyncSerial) this.MyComType).MySerialPort is MinoConnectSerialPort)))
          this.Close();
      }
    }

    internal string ComPort
    {
      get => this.ComPortUsed;
      set
      {
        if (!(value != this.ComPortUsed))
          return;
        this.ComPortUsed = value;
        if (this.ComIsOpen)
          this.Close();
        if (this.ConfigList != null && this.ConfigList.Port != this.ComPortUsed)
          this.ConfigList.Port = this.ComPortUsed;
      }
    }

    public AsyncFunctions() => this.BaseConstructor(true);

    public AsyncFunctions(bool noComWindow) => this.BaseConstructor(noComWindow);

    private void BaseConstructor(bool noComWindow)
    {
      this.BreakRequest = false;
      this.Transceiver = ZR_ClassLibrary.TransceiverDevice.None;
      this.MainThreadId = Thread.CurrentThread.ManagedThreadId;
      this.Baudrate = 2400;
      this.SetDefaultTiming(this.Baudrate);
      if (noComWindow)
      {
        this.ComWindow = (ZR_AsyncCom) null;
        this.ComponentVersion = (Version) null;
      }
      else
        this.ComWindow = new ZR_AsyncCom(this);
    }

    public bool IsLocked => !string.IsNullOrEmpty(AsyncFunctions.ownerOfLock);

    public void Lock(string owner) => AsyncFunctions.ownerOfLock = owner;

    public void Unlock() => AsyncFunctions.ownerOfLock = string.Empty;

    public string Owner => AsyncFunctions.ownerOfLock;

    public static bool IsSettingsEqual(
      SortedList<AsyncComSettings, object> settings,
      AsyncComSettings key,
      string value)
    {
      return settings != null && settings.ContainsKey(key) && settings[key] != null && settings[key].ToString() == value;
    }

    internal void AsyncComMessageBox(string Message)
    {
      int num = (int) GMM_MessageBox.ShowMessage("AsyncCom", Message, true);
    }

    public void ShowErrorMessageBox(bool on) => this.ErrorMessageBox = on;

    public void SetAnswerOffsetTime(int NewAnswerOffsetTime)
    {
      this.AnswerOffsetTime = NewAnswerOffsetTime;
    }

    public void ComWriteLoggerEvent(EventLogger.LoggerEvent Event)
    {
      this.Logger.WriteLoggerEvent(Event);
    }

    public void ComWriteLoggerData(EventLogger.LoggerEvent Event, ref ByteField data)
    {
      this.Logger.WriteLoggerData(Event, ref data);
    }

    public string ShowComWindow(string ComponentList)
    {
      if (this.ComWindow == null)
        this.ComWindow = new ZR_AsyncCom(this);
      else
        this.ComWindow.SetComState();
      this.ComWindow.InitStartMenu(ComponentList);
      int num = (int) this.ComWindow.ShowDialog();
      return this.ComWindow.StartComponentName;
    }

    public void ShowComWindow()
    {
      if (this.ComWindow == null)
        this.ComWindow = new ZR_AsyncCom(this);
      else
        this.ComWindow.SetComState();
      int num = (int) this.ComWindow.ShowDialog();
    }

    public bool ShowComWindowChanged()
    {
      if (this.ComWindow == null)
        this.ComWindow = new ZR_AsyncCom(this);
      else
        this.ComWindow.SetComState();
      return this.ComWindow.ShowDialog() == DialogResult.OK;
    }

    public string SingleParameter(CommParameter Parameter, string ParameterValue)
    {
      string str1 = "";
      bool flag = ParameterValue != null && ParameterValue.Length != 0;
      try
      {
        switch (Parameter)
        {
          case CommParameter.Type:
            str1 = this.ConnectionTypeSelected.ToString();
            if (flag && (this.MyComType == null || ParameterValue != str1))
            {
              switch (ParameterValue)
              {
                case "AsynchronSeriell":
                  this.SetType(AsyncComConnectionType.COM);
                  break;
                case "AsynchronIP":
                  this.SetType(AsyncComConnectionType.Remote);
                  break;
                default:
                  this.SetType((AsyncComConnectionType) Enum.Parse(typeof (AsyncComConnectionType), ParameterValue, false));
                  break;
              }
              break;
            }
            break;
          case CommParameter.Baudrate:
            str1 = this.Baudrate.ToString();
            if (flag)
            {
              this.Baudrate = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.COMserver:
            str1 = this.MyMeterVPN.SelectedCOMserver;
            if (flag)
            {
              this.MyMeterVPN.SelectedCOMserver = ParameterValue;
              break;
            }
            break;
          case CommParameter.Port:
            str1 = this.ComPort.ToString();
            if (flag)
            {
              this.ComPort = !char.IsDigit(ParameterValue[0]) ? ParameterValue : "COM" + ParameterValue;
              break;
            }
            break;
          case CommParameter.Parity:
            str1 = this.Parity;
            if (flag)
            {
              this.Parity = ParameterValue;
              break;
            }
            break;
          case CommParameter.UseBreak:
            str1 = this.Wakeup != WakeupSystem.Break ? false.ToString() : true.ToString();
            if (flag)
            {
              this.Wakeup = !bool.Parse(ParameterValue) ? WakeupSystem.None : WakeupSystem.Break;
              break;
            }
            break;
          case CommParameter.EchoOn:
            str1 = this.EchoOn.ToString();
            if (flag)
            {
              this.EchoOn = bool.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.TestEcho:
            str1 = this.TestEcho.ToString();
            if (flag)
            {
              this.TestEcho = bool.Parse(ParameterValue);
              if (this.ComWindow != null)
                this.ComWindow.ShowEcho();
              break;
            }
            break;
          case CommParameter.RecTime_BeforFirstByte:
            str1 = this.RecTime_BeforFirstByte.ToString();
            if (flag)
            {
              this.RecTime_BeforFirstByte = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.RecTime_OffsetPerByte:
            str1 = this.RecTime_OffsetPerByte.ToString((IFormatProvider) FixedFormates.TheFormates);
            if (flag)
            {
              this.RecTime_OffsetPerByte = double.Parse(ParameterValue, (IFormatProvider) FixedFormates.TheFormates);
              break;
            }
            break;
          case CommParameter.RecTime_GlobalOffset:
            str1 = this.RecTime_GlobalOffset.ToString();
            if (flag)
            {
              this.RecTime_GlobalOffset = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.TransTime_GlobalOffset:
            str1 = this.TransTime_GlobalOffset.ToString();
            if (flag)
            {
              this.TransTime_GlobalOffset = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.RecTransTime:
            str1 = this.RecTransTime.ToString();
            if (flag)
            {
              this.RecTransTime = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.TransTime_BreakTime:
            str1 = this.TransTime_BreakTime.ToString();
            if (flag)
            {
              this.TransTime_BreakTime = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.TransTime_AfterOpen:
            str1 = this.TransTime_AfterOpen.ToString();
            if (flag)
            {
              this.TransTime_AfterOpen = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.TransTime_AfterBreak:
            str1 = this.TransTime_AfterBreak.ToString();
            if (flag)
            {
              this.TransTime_AfterBreak = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.WaitBeforeRepeatTime:
            str1 = this.WaitBeforeRepeatTime.ToString();
            if (flag)
            {
              this.WaitBeforeRepeatTime = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.BreakIntervalTime:
            str1 = this.WakeupIntervalTime.ToString();
            if (flag)
            {
              this.WakeupIntervalTime = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.MinoConnectTestFor:
            str1 = this.Transceiver != ZR_ClassLibrary.TransceiverDevice.MinoConnect ? false.ToString() : true.ToString();
            if (flag)
            {
              this.Transceiver = !bool.Parse(ParameterValue) ? ZR_ClassLibrary.TransceiverDevice.None : ZR_ClassLibrary.TransceiverDevice.MinoConnect;
              break;
            }
            break;
          case CommParameter.MinoConnectPowerOffTime:
            str1 = this.MinoConnectAutoPowerOffTime.ToString();
            if (flag)
            {
              this.MinoConnectAutoPowerOffTime = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.Wakeup:
            str1 = this.Wakeup.ToString();
            if (flag)
            {
              this.Wakeup = (WakeupSystem) Enum.Parse(typeof (WakeupSystem), ParameterValue, false);
              break;
            }
            break;
          case CommParameter.TransceiverDevice:
            str1 = this.Transceiver.ToString();
            if (flag)
            {
              this.Transceiver = (ZR_ClassLibrary.TransceiverDevice) Enum.Parse(typeof (ZR_ClassLibrary.TransceiverDevice), ParameterValue, true);
              break;
            }
            break;
          case CommParameter.ForceMinoConnectState:
            str1 = "";
            if (flag)
            {
              this.MinoConnectBaseState = MinoConnectState.GetBaseStateFromPlugState((MinoConnectState.MinoConnectPlugState) Enum.Parse(typeof (MinoConnectState.MinoConnectPlugState), ParameterValue, false));
              break;
            }
            break;
          case CommParameter.IrDaSelection:
            str1 = !this.IrDa ? IrDaSelection.None.ToString() : (!this.IrDaDaveTailSide ? IrDaSelection.RoundSide.ToString() : IrDaSelection.DoveTailSide.ToString());
            if (flag)
            {
              switch ((IrDaSelection) Enum.Parse(typeof (IrDaSelection), ParameterValue, false))
              {
                case IrDaSelection.DoveTailSide:
                  this.IrDa = true;
                  this.IrDaDaveTailSide = true;
                  break;
                case IrDaSelection.RoundSide:
                  this.IrDa = true;
                  this.IrDaDaveTailSide = false;
                  break;
                default:
                  this.IrDa = false;
                  this.IrDaDaveTailSide = false;
                  break;
              }
              break;
            }
            break;
          case CommParameter.HardwareHandshake:
            str1 = this.HardwareHandshake.ToString();
            if (flag)
            {
              if (bool.Parse(ParameterValue) != this.HardwareHandshake)
              {
                this.Close();
                this.HardwareHandshake = bool.Parse(ParameterValue);
              }
              break;
            }
            break;
          case CommParameter.MinoConnectIsUSB:
            str1 = "";
            break;
          case CommParameter.MinoConnectIrDaPulseTime:
            str1 = this.MinoConnectIrDaPulseLength.ToString();
            if (flag)
            {
              this.MinoConnectIrDaPulseLength = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.RecTime_OffsetPerBlock:
            str1 = this.RecTime_OffsetPerBlock.ToString();
            if (flag)
            {
              this.RecTime_OffsetPerBlock = int.Parse(ParameterValue);
              break;
            }
            break;
          case CommParameter.MinoConnectBaseState:
            str1 = this.MinoConnectBaseState.ToString();
            if (flag)
            {
              this.MinoConnectBaseState = (MinoConnectState.BaseStateEnum) Enum.Parse(typeof (MinoConnectState.BaseStateEnum), ParameterValue, false);
              break;
            }
            break;
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        AsyncFunctions.logger.Error(ex, message);
        string str2 = "** Parameter value error **\r\n" + "\r\nParameter name: " + Parameter.ToString() + "\r\nParameter value: " + ParameterValue;
        this.AsyncComMessageBox(message + " " + str2);
      }
      return str1;
    }

    public string SingleParameter(string ParameterName, string ParameterValue)
    {
      return !new List<string>((IEnumerable<string>) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (CommParameter))).Contains(ParameterName) ? string.Empty : this.SingleParameter((CommParameter) Enum.Parse(typeof (CommParameter), ParameterName, true), ParameterValue);
    }

    public void GetCommParameter(ref ArrayList ParameterList)
    {
      ParameterList.Clear();
      for (int Parameter = 0; Parameter != 29; ++Parameter)
      {
        CommParameter commParameter = (CommParameter) Parameter;
        string str1 = commParameter.ToString();
        string str2 = str1;
        commParameter = CommParameter.UseBreak;
        string str3 = commParameter.ToString();
        if (!(str2 == str3))
        {
          string str4 = str1;
          commParameter = CommParameter.MinoConnectTestFor;
          string str5 = commParameter.ToString();
          if (!(str4 == str5))
          {
            string str6 = str1;
            commParameter = CommParameter.MinoConnectIsUSB;
            string str7 = commParameter.ToString();
            if (!(str6 == str7))
            {
              string str8 = this.SingleParameter((CommParameter) Parameter, string.Empty);
              ParameterList.Add((object) str1);
              ParameterList.Add((object) str8);
            }
          }
        }
      }
    }

    public SortedList<AsyncComSettings, object> GetAsyncComSettings()
    {
      ArrayList ParameterList = new ArrayList();
      this.GetCommParameter(ref ParameterList);
      if (ParameterList == null || ParameterList.Count % 2 != 0)
        return (SortedList<AsyncComSettings, object>) null;
      SortedList<AsyncComSettings, object> asyncComSettings = new SortedList<AsyncComSettings, object>();
      for (int index = 0; index < ParameterList.Count; index += 2)
      {
        if (Enum.IsDefined(typeof (AsyncComSettings), ParameterList[index]))
        {
          AsyncComSettings key = (AsyncComSettings) Enum.Parse(typeof (AsyncComSettings), ParameterList[index].ToString(), true);
          object obj = ParameterList[index + 1];
          asyncComSettings.Add(key, obj);
        }
      }
      return asyncComSettings;
    }

    public SortedList<string, string> GetAsyncComSettingsList()
    {
      SortedList<string, string> asyncComSettingsList = new SortedList<string, string>();
      ArrayList ParameterList = new ArrayList();
      this.GetCommParameter(ref ParameterList);
      if (ParameterList == null || ParameterList.Count % 2 != 0)
        return (SortedList<string, string>) null;
      for (int index = 0; index < ParameterList.Count; index += 2)
      {
        if (Enum.IsDefined(typeof (AsyncComSettings), ParameterList[index]))
        {
          string key = ParameterList[index].ToString();
          string str = ParameterList[index + 1].ToString();
          asyncComSettingsList.Add(key, str);
        }
      }
      return asyncComSettingsList;
    }

    public string GetAsyncComSettingsAsString()
    {
      ArrayList ParameterList = new ArrayList();
      this.GetCommParameter(ref ParameterList);
      if (ParameterList == null || ParameterList.Count % 2 != 0)
        throw new Exception("Illegal AsyncCom settings");
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < ParameterList.Count; index += 2)
      {
        if (Enum.IsDefined(typeof (AsyncComSettings), ParameterList[index]))
        {
          string str1 = ParameterList[index].ToString();
          string str2 = ParameterList[index + 1].ToString();
          if (stringBuilder.Length > 0)
            stringBuilder.Append(";");
          stringBuilder.Append(str1 + ";" + str2);
        }
      }
      return stringBuilder.ToString();
    }

    public bool SetAsyncComSettings(string asyncComSettings)
    {
      if (string.IsNullOrEmpty(asyncComSettings))
        return false;
      string[] strArray = asyncComSettings.Split(';');
      if (strArray.Length == 0)
        return false;
      SortedList<string, string> asyncComSettings1 = new SortedList<string, string>();
      for (int index = 0; index + 1 < strArray.Length; index += 2)
      {
        if (!asyncComSettings1.ContainsKey(strArray[index]))
          asyncComSettings1.Add(strArray[index], strArray[index + 1]);
      }
      return this.SetAsyncComSettings(asyncComSettings1);
    }

    public bool SetAsyncComSettings(
      SortedList<AsyncComSettings, object> asyncComSettings)
    {
      if (asyncComSettings == null)
        return false;
      SortedList<string, string> asyncComSettings1 = new SortedList<string, string>();
      foreach (KeyValuePair<AsyncComSettings, object> asyncComSetting in asyncComSettings)
        asyncComSettings1.Add(asyncComSetting.Key.ToString(), asyncComSetting.Value.ToString());
      return this.SetAsyncComSettings(asyncComSettings1);
    }

    public bool SetAsyncComSettings(SortedList<string, string> asyncComSettings)
    {
      if (asyncComSettings == null || asyncComSettings.Count == 0)
        return true;
      ArrayList ParameterList = new ArrayList();
      for (int index = 0; index < asyncComSettings.Count; ++index)
      {
        string key = asyncComSettings.Keys[index];
        ParameterList.Add((object) key);
        ParameterList.Add((object) asyncComSettings.Values[index]);
      }
      return this.SetCommParameter(ParameterList);
    }

    public bool SetCommParameter(ArrayList ParameterList)
    {
      if (ParameterList.Count == 0)
        return true;
      if (AsyncFunctions.logger.IsInfoEnabled)
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < ParameterList.Count; index += 2)
        {
          stringBuilder.Append(ParameterList[index].ToString());
          stringBuilder.Append(" = ");
          stringBuilder.Append(ParameterList[index + 1].ToString());
          stringBuilder.Append(ZR_Constants.SystemNewLine);
        }
        AsyncFunctions.logger.Info("Set AsyncComSettings: " + stringBuilder.ToString());
      }
      bool flag1 = false;
      bool flag2 = false;
      CommParameter commParameter;
      for (int index = 0; index < ParameterList.Count; index += 2)
      {
        string parameter = (string) ParameterList[index];
        string str1 = parameter;
        commParameter = CommParameter.Wakeup;
        string str2 = commParameter.ToString();
        if (str1 == str2)
          flag1 = true;
        string str3 = parameter;
        commParameter = CommParameter.TransceiverDevice;
        string str4 = commParameter.ToString();
        if (str3 == str4)
          flag2 = true;
      }
      ArrayList ParameterList1 = new ArrayList();
      this.GetCommParameter(ref ParameterList1);
      bool flag3 = false;
      for (int index = 0; index < ParameterList.Count; index += 2)
      {
        string parameter1 = (string) ParameterList[index];
        string parameter2 = (string) ParameterList[index + 1];
        int num1 = ParameterList1.IndexOf((object) parameter1);
        if (num1 <= -1 || num1 + 1 >= ParameterList1.Count || !(ParameterList1[num1 + 1].ToString() == parameter2))
        {
          int num2;
          if (flag1)
          {
            string str5 = parameter1;
            commParameter = CommParameter.UseBreak;
            string str6 = commParameter.ToString();
            num2 = str5 == str6 ? 1 : 0;
          }
          else
            num2 = 0;
          if (num2 == 0)
          {
            int num3;
            if (flag2)
            {
              string str7 = parameter1;
              commParameter = CommParameter.MinoConnectTestFor;
              string str8 = commParameter.ToString();
              num3 = str7 == str8 ? 1 : 0;
            }
            else
              num3 = 0;
            if (num3 == 0)
            {
              flag3 = true;
              this.SingleParameter(parameter1, parameter2);
            }
          }
        }
      }
      if (this.ComWindow != null && this.ComWindow.Focused)
        this.ComWindow.SetComState();
      if (this.MyComType is AsyncSerial && flag3 && this.IsOpen)
        this.ChangeDriverSettings();
      return true;
    }

    public bool SetCommParameter(ArrayList ParameterList, bool ComWindowRefresh)
    {
      bool flag = true;
      int count = ParameterList.Count;
      if (count % 2 == 1)
        --count;
      for (int index = 0; index < count; index += 2)
      {
        if (this.SingleParameter((string) ParameterList[index], (string) ParameterList[index + 1]).Length == 0)
          flag = false;
      }
      if (this.ComWindow != null & ComWindowRefresh)
        this.ComWindow.SetComState();
      return flag;
    }

    public bool ChangeParameterAtList(
      ArrayList ParameterList,
      string ParameterName,
      string NewParameter)
    {
      for (int index = 0; index < ParameterList.Count; index += 2)
      {
        if (ParameterList[index].ToString() == ParameterName)
        {
          ParameterList[index + 1] = (object) NewParameter;
          return true;
        }
      }
      return false;
    }

    public string GetParameterFromList(ArrayList ParameterList, string ParameterName)
    {
      for (int index = 0; index < ParameterList.Count; index += 2)
      {
        if (ParameterList[index].ToString() == ParameterName)
          return ParameterList[index + 1].ToString();
      }
      return "";
    }

    public string CreateParameterString(ArrayList ParameterList)
    {
      StringBuilder stringBuilder = new StringBuilder(300);
      for (int index = 0; index < ParameterList.Count; index += 2)
      {
        if (index != 0)
          stringBuilder.Append(';');
        stringBuilder.Append(ParameterList[index].ToString());
        stringBuilder.Append(';');
        stringBuilder.Append(ParameterList[index + 1].ToString());
      }
      return stringBuilder.ToString();
    }

    public ArrayList CreateParameterList(string ParameterString)
    {
      ArrayList parameterList = new ArrayList();
      char[] separator = new char[2]{ ',', ';' };
      foreach (object obj in ParameterString.Split(separator, 100))
        parameterList.Add((object) obj.ToString());
      return parameterList;
    }

    public bool ChangeDriverSettings()
    {
      if (this.MyComType == null || !(this.MyComType is AsyncSerial) || ((AsyncSerial) this.MyComType).MySerialPort == null)
        return false;
      AsyncFunctions.logger.Trace("Call ChangeDriverSettings");
      return ((AsyncSerial) this.MyComType).MySerialPort.ChangeDriverSettings();
    }

    public bool IsOpen => this.ComIsOpen;

    public bool Open()
    {
      if (this.IsPluginObject)
      {
        try
        {
          if (PlugInLoader.IsPluginLoaded("CommunicationPort"))
          {
            object obj = PlugInLoader.GetPlugIn("CommunicationPort").GetPluginInfo().Interface;
            obj.GetType().GetMethod("Close").Invoke(obj, (object[]) null);
          }
        }
        catch
        {
        }
      }
      if (this.ComIsOpen)
        return true;
      if (this.MyComType == null)
        this.SetType(this.ConnectionTypeSelected);
      this.IgnoreReceiveErrorsOnTransmitTime = false;
      this.HardwareHandshake = false;
      if (this.MyComType is AsyncSerial)
      {
        if (this.Transceiver == ZR_ClassLibrary.TransceiverDevice.MinoIR)
          this.IgnoreReceiveErrorsOnTransmitTime = true;
        if (this.Transceiver == ZR_ClassLibrary.TransceiverDevice.MinoConnect)
          this.HardwareHandshake = true;
      }
      try
      {
        if (!this.MyComType.Open())
        {
          if (this.Transceiver != ZR_ClassLibrary.TransceiverDevice.MinoHead)
            return false;
          int num = 3;
          bool flag = false;
          for (; num > 0; --num)
          {
            this.MyComType.Close();
            if (this.MyComType.Open())
            {
              flag = true;
              break;
            }
            if (!ZR_ClassLibrary.Util.Wait(800L, "while reopen connection to the MinoHead device.", (ICancelable) this, AsyncFunctions.logger))
              return false;
          }
          if (!flag)
            return false;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "ComOpen error:" + ZR_Constants.SystemNewLine + ex.Message);
        this.ComIsOpen = false;
        return false;
      }
      if (!this.EchoOn && this.TestEcho)
        this.EchoOn = this.IsEchoActiv();
      this.EarliestTransmitTime = SystemValues.DateTimeNow.AddMilliseconds((double) this.WaitBeforeRepeatTime);
      return true;
    }

    private bool IsEchoActiv()
    {
      AsyncFunctions.logger.Debug("Testing for echo");
      ByteField DataBlock1 = new ByteField(6);
      DataBlock1.Add(85);
      DataBlock1.Add(170);
      DataBlock1.Add(90);
      DataBlock1.Add(165);
      DataBlock1.Add(15);
      DataBlock1.Add(240);
      this.TransmitBlock(ref DataBlock1);
      ByteField DataBlock2 = new ByteField(6);
      if (!this.ReceiveBlock(ref DataBlock2, 6, true))
        return false;
      for (int index = 0; index < 6; ++index)
      {
        if ((int) DataBlock2.Data[index] != (int) DataBlock1.Data[index])
          return false;
      }
      return true;
    }

    public void WaitToEarliestTransmitTime()
    {
      if (this.EarliestTransmitTime > SystemValues.DateTimeNow)
      {
        Application.DoEvents();
        if (!(this.EarliestTransmitTime > SystemValues.DateTimeNow))
          return;
        this.ComWriteLoggerEvent(EventLogger.LoggerEvent.ComWaitTransmitTimeS);
        double totalMilliseconds;
        do
        {
          Application.DoEvents();
          totalMilliseconds = this.EarliestTransmitTime.Subtract(SystemValues.DateTimeNow).TotalMilliseconds;
          if (totalMilliseconds >= 0.0)
          {
            if (totalMilliseconds < 100.0)
              goto label_5;
          }
          else
            goto label_10;
        }
        while (ZR_ClassLibrary.Util.Wait(100L, nameof (WaitToEarliestTransmitTime), (ICancelable) this, AsyncFunctions.logger));
        goto label_8;
label_5:
        if (!ZR_ClassLibrary.Util.Wait((long) (int) totalMilliseconds, nameof (WaitToEarliestTransmitTime), (ICancelable) this, AsyncFunctions.logger))
          return;
        goto label_10;
label_8:
        return;
label_10:
        this.ComWriteLoggerEvent(EventLogger.LoggerEvent.ComWaitTransmitTimeE);
      }
      else if (!ZR_ClassLibrary.Util.Wait(0L, nameof (WaitToEarliestTransmitTime), (ICancelable) this, AsyncFunctions.logger))
        ;
    }

    public void ResetEarliestTransmitTime()
    {
      this.EarliestTransmitTimeAbsolut = SystemValues.DateTimeNow.AddMilliseconds((double) this.RecTransTime);
    }

    public void ResetLastTransmitEndTime() => this.MyComType.ResetLastTransmitEndTime();

    public void ClearWakeup()
    {
      this.WakeupTemporaryOff = false;
      this.LastWakeupRefreshTime = DateTime.MinValue;
    }

    public void TriggerWakeup() => this.LastWakeupRefreshTime = SystemValues.DateTimeNow;

    public bool SetHandshakeState(HandshakeStates HandshakeState)
    {
      return this.MyComType.SetHandshakeState(HandshakeState);
    }

    public bool Close()
    {
      bool flag = true;
      if (this.MyComType != null)
        flag = this.MyComType.Close();
      this.ComIsOpen = false;
      return flag;
    }

    public bool SetBreak() => this.MyComType.SetBreak();

    public bool ClearBreak() => this.MyComType.ClearBreak();

    public void ClearCom() => this.MyComType.ClearCom();

    public void TestComState() => this.MyComType.TestComState();

    public bool CallTransceiverFunction(TransceiverDeviceFunction function)
    {
      return this.CallTransceiverFunction(function, (object) null);
    }

    public bool CallTransceiverFunction(TransceiverDeviceFunction function, object param1)
    {
      return this.CallTransceiverFunction(function, param1, (object) null);
    }

    public bool CallTransceiverFunction(
      TransceiverDeviceFunction function,
      object param1,
      object param2)
    {
      return this.Open() && this.MyComType != null && this.MyComType.CallTransceiverDeviceFunction(function, param1, param2);
    }

    public bool SetComTimeouts() => true;

    public bool TransmitString(string DataString)
    {
      if (AsyncFunctions.logger.IsTraceEnabled)
        AsyncFunctions.logger.Trace("Send ASCII data: {0}", DataString);
      return this.MyComType.TransmitString(DataString);
    }

    public bool TransmitBlock(string DataString)
    {
      if (AsyncFunctions.logger.IsTraceEnabled)
        AsyncFunctions.logger.Trace("Send ASCII data: {0}", DataString);
      return this.MyComType.TransmitBlock(DataString);
    }

    public bool TransmitBlock(byte[] buffer)
    {
      ByteField DataBlock = new ByteField(buffer);
      return this.TransmitBlock(ref DataBlock);
    }

    public bool TransmitBlock(ref ByteField DataBlock)
    {
      if (!this.ComIsOpen && !this.Open())
        return false;
      if (AsyncFunctions.logger.IsTraceEnabled)
      {
        AsyncFunctions.logger.Trace("Transmit block: Size: {0}", DataBlock.Count);
        foreach (string message in ParameterService.GetMemoryInfo(DataBlock, 0))
          AsyncFunctions.logger.Trace(message);
      }
      return this.MyComType.TransmitBlock(ref DataBlock);
    }

    public bool SendBlock(ref ByteField DataBlock)
    {
      if (!this.ComIsOpen && !this.Open())
        return false;
      if (AsyncFunctions.logger.IsTraceEnabled)
      {
        AsyncFunctions.logger.Trace("Send data block: Size: {0}", DataBlock.Count);
        foreach (string message in ParameterService.GetMemoryInfo(DataBlock, 0))
          AsyncFunctions.logger.Trace(message);
      }
      return this.MyComType.SendBlock(ref DataBlock);
    }

    public void PureTransmit(byte[] byteList) => this.MyComType.PureTransmit(byteList);

    public bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first)
    {
      bool block = this.MyComType.ReceiveBlock(ref DataBlock, MinByteNb, first);
      if (AsyncFunctions.logger.IsTraceEnabled && DataBlock.Count > 0)
        AsyncFunctions.logger.Trace<int, string>("Received ({0}): {1}", DataBlock.Count, ZR_ClassLibrary.Util.ByteArrayToHexString(DataBlock.Data, 0, DataBlock.Count));
      return block;
    }

    public bool ReceiveBlock(ref ByteField DataBlock)
    {
      bool block = this.MyComType.ReceiveBlock(ref DataBlock);
      if (AsyncFunctions.logger.IsTraceEnabled && DataBlock.Count > 0)
        AsyncFunctions.logger.Trace<int, string>("Received ({0}): {1}", DataBlock.Count, ZR_ClassLibrary.Util.ByteArrayToHexString(DataBlock.Data, 0, DataBlock.Count));
      return block;
    }

    public bool GetCurrentInputBuffer(out byte[] buffer)
    {
      return this.MyComType.GetCurrentInputBuffer(out buffer);
    }

    public bool TryReceiveBlock(out byte[] buffer)
    {
      bool block = this.MyComType.TryReceiveBlock(out buffer);
      if (AsyncFunctions.logger.IsTraceEnabled & block && buffer != null)
        AsyncFunctions.logger.Trace<int, string>("Received ({0}): {1}", buffer.Length, ZR_ClassLibrary.Util.ByteArrayToHexString(buffer));
      return block;
    }

    public bool TryReceiveBlock(out byte[] buffer, int numberOfBytesToReceive)
    {
      bool block = this.MyComType.TryReceiveBlock(out buffer, numberOfBytesToReceive);
      if (AsyncFunctions.logger.IsTraceEnabled & block && buffer != null)
        AsyncFunctions.logger.Trace<int, string>("Received ({0}): {1}", buffer.Length, ZR_ClassLibrary.Util.ByteArrayToHexString(buffer));
      return block;
    }

    public bool ReceiveString(out string DataString)
    {
      bool flag = this.MyComType.ReceiveString(out DataString);
      if (AsyncFunctions.logger.IsTraceEnabled)
        AsyncFunctions.logger.Trace("Received ASCII data: {0}", DataString);
      return flag;
    }

    public bool ReceiveLine(out string ReceivedData)
    {
      bool line = this.MyComType.ReceiveLine(out ReceivedData);
      if (AsyncFunctions.logger.IsTraceEnabled)
        AsyncFunctions.logger.Trace("Received ASCII data: {0}", ReceivedData);
      return line;
    }

    public bool ReceiveCRLF_Line(out string ReceivedData)
    {
      bool crlfLine = this.MyComType.ReceiveCRLF_Line(out ReceivedData);
      if (AsyncFunctions.logger.IsTraceEnabled)
        AsyncFunctions.logger.Trace("Received ASCII data: {0}", ReceivedData);
      return crlfLine;
    }

    public bool ReceiveLine(out string ReceivedData, char[] EndCharacters, bool GetEmpty_CRLF_Line)
    {
      bool line = this.MyComType.ReceiveLine(out ReceivedData, EndCharacters, GetEmpty_CRLF_Line);
      if (AsyncFunctions.logger.IsTraceEnabled)
        AsyncFunctions.logger.Trace("Received ASCII data: {0}", ReceivedData);
      return line;
    }

    public bool ReceiveBlockToChar(ref ByteField DataBlock, byte EndChar)
    {
      bool blockToChar = this.MyComType.ReceiveBlockToChar(ref DataBlock, EndChar);
      if (AsyncFunctions.logger.IsTraceEnabled)
        AsyncFunctions.logger.Trace<int, string>("Received ({0}): {1}", DataBlock.Count, ZR_ClassLibrary.Util.ByteArrayToHexString(DataBlock.Data, 0, DataBlock.Count));
      return blockToChar;
    }

    public bool GetComPortIds(out string strComPortIds, bool ForceRefresh)
    {
      strComPortIds = string.Empty;
      List<string> stringList = new List<string>();
      StringBuilder stringBuilder = new StringBuilder();
      switch (this.ConnectionTypeSelected)
      {
        case AsyncComConnectionType.Remote:
        case AsyncComConnectionType.Remote_VPN:
          this.MyMeterVPN.Update((AsyncIP) this.MyComType);
          if (!this.MyMeterVPN.COMservers.ContainsKey((object) this.MyMeterVPN.SelectedCOMserver))
            return false;
          COMserver coMserver = (COMserver) this.MyMeterVPN.COMservers[(object) this.MyMeterVPN.SelectedCOMserver];
          if (coMserver == null)
            return false;
          if (ForceRefresh)
            coMserver.Update((AsyncIP) this.MyComType);
          IDictionaryEnumerator enumerator = coMserver.RemoteComs.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              DictionaryEntry current = (DictionaryEntry) enumerator.Current;
              stringList.Add("COM" + current.Key.ToString());
            }
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
      }
      stringList.Sort();
      stringBuilder.Length = 0;
      for (int index = 0; index < stringList.Count; ++index)
      {
        if (index > 0)
          stringBuilder.Append(",");
        stringBuilder.Append(stringList[index].ToString());
      }
      strComPortIds = stringBuilder.ToString();
      return true;
    }

    internal void SendAsyncComMessage(GMM_EventArgs MessageEventArgs)
    {
      if (this.OnAsyncComMessage == null)
        return;
      this.OnAsyncComMessage((object) this, MessageEventArgs);
    }

    public bool SendMinoConnectCommand(string cmd)
    {
      if (this.MyComType == null)
        throw new ArgumentNullException("MyComType can not be null!");
      if (!this.MyComType.Open())
        return false;
      if (!(this.MyComType is AsyncSerial comType))
        throw new ArgumentException("Wrong AsyncCom settings! It's supported only serial port.");
      if (!(comType.MySerialPort is MinoConnectSerialPort serialPort))
        throw new ArgumentException("Wrong AsyncCom trancseiver settings! MinoConnect is not chosen.");
      serialPort.WriteCommand(cmd);
      return true;
    }

    internal bool SetType(AsyncComConnectionType NewType)
    {
      if (this.MyComType != null)
        this.MyComType.Close();
      this.ConnectionTypeSelected = NewType;
      switch (NewType)
      {
        case AsyncComConnectionType.COM:
          this.MyComType = (AsyncFunctionsBase) new AsyncSerial(this);
          this.MyComType.ConnectionLost += new System.EventHandler(this.MyComType_ConnectionLost);
          this.MyComType.BatterieLow += new System.EventHandler(this.MyComType_BatterieLow);
          return true;
        case AsyncComConnectionType.Remote:
        case AsyncComConnectionType.Remote_VPN:
          this.MyComType = (AsyncFunctionsBase) new AsyncIP(this);
          return true;
        default:
          return false;
      }
    }

    private void MyComType_ConnectionLost(object sender, EventArgs e)
    {
      if (this.ConnectionLost == null)
        return;
      this.ConnectionLost(sender, e);
    }

    private void MyComType_BatterieLow(object sender, EventArgs e)
    {
      if (this.BatterieLow == null)
        return;
      this.BatterieLow(sender, e);
    }

    public void GMM_Dispose()
    {
      this.Close();
      if (this.ComWindow == null)
        return;
      this.ComWindow.Dispose();
    }

    public string GetTranceiverDeviceInfo()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.transceiverDeviceInfo);
      if (this.ComIsOpen)
      {
        if (this.Transceiver == ZR_ClassLibrary.TransceiverDevice.MinoConnect)
          stringBuilder.Append(ZR_Constants.SystemNewLine);
      }
      else
        stringBuilder.Append("Com is closed");
      return stringBuilder.ToString();
    }

    public bool UpdateTransceiverFirmware(string pathToFirmware)
    {
      if (!File.Exists(pathToFirmware))
        throw new FileNotFoundException("File does not exist! Path: " + pathToFirmware);
      if (!this.Open())
        return false;
      if (this.Transceiver != ZR_ClassLibrary.TransceiverDevice.MinoConnect)
        throw new ArgumentException(this.Transceiver.ToString() + " is not supported for firmware update!");
      if (!(this.MyComType.GetChannel() is SerialPort channel))
        return false;
      if (!this.CallTransceiverFunction(TransceiverDeviceFunction.DisableMinoConnectPolling))
        throw new Exception("Can not disable MinoConnect polling!");
      FlashMinoConnect flashMinoConnect = new FlashMinoConnect(channel);
      flashMinoConnect.ProgressChanged += new EventHandler<MinoConnect.ProgressChangedEventArgs>(this.Flasher_ProgressChanged);
      try
      {
        if (!flashMinoConnect.Upgrade(pathToFirmware))
          throw new Exception(flashMinoConnect.LastError);
      }
      finally
      {
        flashMinoConnect.ProgressChanged -= new EventHandler<MinoConnect.ProgressChangedEventArgs>(this.Flasher_ProgressChanged);
        flashMinoConnect.Dispose();
        this.RaiseProgressEvent(0);
        this.Close();
      }
      return true;
    }

    private void Flasher_ProgressChanged(object sender, MinoConnect.ProgressChangedEventArgs e)
    {
      this.RaiseProgressEvent(e.ProgressPercentage);
    }

    public void RaiseProgressEvent(int progressPercentage)
    {
      this.RaiseProgressEvent(progressPercentage, string.Empty);
    }

    public void RaiseProgressEvent(int progressPercentage, string status)
    {
      if (this.OnAsyncComMessage == null)
        return;
      this.OnAsyncComMessage((object) this, new GMM_EventArgs(status)
      {
        TheMessageType = GMM_EventArgs.MessageType.MessageAndProgressPercentage,
        ProgressPercentage = progressPercentage
      });
    }

    internal void SetDefaultTiming(int Baudrate)
    {
      this.AnswerOffsetTime = 0;
      this.TransTime_GlobalOffset = 0;
      this.RecTime_GlobalOffset = 0;
      this.RecTime_BeforFirstByte = this.TimeBevorFirstByteDefaultFromBaudrate(Baudrate);
      this.RecTime_OffsetPerByte = 0.0;
      this.RecTime_OffsetPerBlock = 50;
      this.RecTransTime = 10;
      this.WakeupIntervalTime = 10000;
      this.TransTime_BreakTime = 700;
      this.TransTime_AfterBreak = 50;
      this.TransTime_AfterOpen = 200;
      this.WaitBeforeRepeatTime = 200;
    }

    internal int TimeBevorFirstByteDefaultFromBaudrate(int Baudrate)
    {
      int num1 = (int) (1000.0 / (double) Baudrate * 330.0);
      int num2 = 263;
      if (num2 < 0)
        num2 = 0;
      return num1 + num2;
    }

    internal void RefreshComPorts()
    {
      if (this.ConnectionTypeSelected != AsyncComConnectionType.Remote_VPN)
        return;
      if (this.MyComType is AsyncIP)
      {
        this.MyMeterVPN.COMservers.Clear();
        this.MyMeterVPN.Update((AsyncIP) this.MyComType);
      }
      else
      {
        this.MyMeterVPN.COMservers.Clear();
        this.MyMeterVPN.Update(new AsyncIP(this));
      }
    }

    public Dictionary<string, string> LoadAvailableCOMservers()
    {
      if (this.MyMeterVPN.COMservers == null || this.MyMeterVPN.COMservers.Count == 0)
        this.RefreshComPorts();
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      for (int index = 0; index < this.MyMeterVPN.COMservers.Count; ++index)
        dictionary.Add(this.MyMeterVPN.COMservers.GetKey(index).ToString(), ((COMserver) this.MyMeterVPN.COMservers.GetByIndex(index)).Name);
      return dictionary;
    }

    public object ShowWindow(object parameters)
    {
      switch (parameters)
      {
        case null:
          this.ShowComWindow();
          return (object) this.GetAsyncComSettings();
        case string _:
          this.SetAsyncComSettings(parameters.ToString());
          goto default;
        case SortedList<AsyncComSettings, object> _:
          this.SetAsyncComSettings(parameters as SortedList<AsyncComSettings, object>);
          goto default;
        case SortedList<string, string> _:
          this.SetAsyncComSettings(parameters as SortedList<string, string>);
          goto default;
        default:
          goto case null;
      }
    }

    public void SetReadoutConfiguration(ConfigList configList)
    {
      if (this.ReadoutConfigByBusFile)
      {
        this.DisableConfigList();
      }
      else
      {
        if (configList == null)
          throw new ArgumentNullException(nameof (configList));
        if (this.ConfigList == null)
        {
          this.ConfigList = configList;
          this.SetAsyncComSettings(configList.GetSortedList());
          this.ConfigList.PropertyChanged += new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
          this.ConfigList.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ConfigList_CollectionChanged);
        }
        else if (this.ConfigList != configList)
          throw new ArgumentException("this.configList != configList");
      }
    }

    private void ConfigList_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.configValuesChanged();
    }

    private void ConfigList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.configValuesChanged();
    }

    private void DisableConfigList()
    {
      if (this.ConfigList == null)
        return;
      this.ConfigList.PropertyChanged -= new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
      this.ConfigList.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.ConfigList_CollectionChanged);
      this.ConfigList = (ConfigList) null;
    }

    private void configValuesChanged()
    {
      SortedList<string, string> asyncComSettings = new SortedList<string, string>();
      foreach (KeyValuePair<string, string> sorted in this.ConfigList.GetSortedList())
      {
        CommParameter result;
        if (Enum.TryParse<CommParameter>(sorted.Key, out result) && this.SingleParameter(result, (string) null) != sorted.Value)
          asyncComSettings.Add(sorted.Key, sorted.Value);
      }
      if (asyncComSettings.Count <= 0)
        return;
      this.SetAsyncComSettings(asyncComSettings);
    }

    public ConfigList GetReadoutConfiguration() => this.ConfigList;
  }
}
