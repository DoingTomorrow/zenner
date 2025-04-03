// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioReader
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using CommunicationPort.Functions;
using CommunicationPort.UserInterface;
using NLog;
using PlugInLib;
using StartupLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public sealed class RadioReader
  {
    private static Logger logger = LogManager.GetLogger(nameof (RadioReader));
    private DateTime receiveStartTime;
    private CommunicationByMinoConnect micon;
    private DeviceCollectorFunctions deviceCollector;
    private readonly GMM_EventArgs WAIT_EVENT = new GMM_EventArgs(GMM_EventArgs.MessageType.Wait);

    private CommunicationByMinoConnect GetMicon()
    {
      if (this.micon != null)
        return this.micon;
      SortedList<string, string> sortedList = this.deviceCollector != null ? this.deviceCollector.GetAsyncComSettings() : throw new ArgumentNullException("deviceCollector");
      this.deviceCollector.ComClose();
      GmmPlugIn plugIn = PlugInLoader.GetPlugIn("CommunicationPort");
      if (plugIn != null)
      {
        CommunicationPortWindowFunctions portWindowFunctions = plugIn.GetPluginInfo().Interface as CommunicationPortWindowFunctions;
        portWindowFunctions.SetReadoutConfiguration(sortedList);
        this.micon = portWindowFunctions.portFunctions.GetCommunicationByMinoConnect();
      }
      else
      {
        CommunicationPortFunctions communicationPortFunctions = new CommunicationPortFunctions();
        communicationPortFunctions.SetReadoutConfiguration(sortedList);
        this.micon = communicationPortFunctions.GetCommunicationByMinoConnect();
      }
      return this.micon;
    }

    public event EventHandler<RadioPacket> OnPacketReceived;

    public event EventHandlerEx<int> OnProgress;

    public event System.EventHandler ConnectionLost;

    public bool IsBusy { get; private set; }

    public RadioReader(DeviceCollectorFunctions deviceCollector)
    {
      this.deviceCollector = deviceCollector;
      this.IsBusy = false;
    }

    public Dictionary<long, RadioDataSet> ReceivedData { get; private set; }

    public void Open() => this.GetMicon().Open();

    public void Close() => this.GetMicon().Close();

    public List<GlobalDeviceId> Read()
    {
      return this.deviceCollector.MyBusMode == BusMode.MinomatRadioTest ? this.Read(new int?(16)) : this.Read(new int?());
    }

    public List<GlobalDeviceId> Read(int? timeoutInSec)
    {
      if (this.IsBusy)
      {
        RadioReader.logger.Error("Abort an asynchronous method call! The WalkByReader is busy.");
        return (List<GlobalDeviceId>) null;
      }
      this.receiveStartTime = DateTime.Now;
      radioList = (RadioList) null;
      try
      {
        this.IsBusy = true;
        ZR_ClassLibMessages.ClearErrors();
        this.deviceCollector.BreakRequest = false;
        if (!(this.deviceCollector.GetDeviceListForBusMode() is RadioList radioList))
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Wrong DeviceList by radio!");
          return (List<GlobalDeviceId>) null;
        }
        radioList.ClearReceivedPackets();
        this.ReceivedData = radioList.ReceivedData;
        if (!this.StartRadio())
          return (List<GlobalDeviceId>) null;
        CommunicationByMinoConnect micon = this.GetMicon();
        micon.DiscardInBuffer();
        int e = 0;
        while (!this.deviceCollector.BreakRequest)
        {
          if (this.OnProgress != null)
          {
            if (radioList.ExpectedDevices != null && radioList.ExpectedDevices.Count > 0)
            {
              this.OnProgress((object) this, radioList.GetProgress());
            }
            else
            {
              if (e >= 100)
                e = 0;
              e += 5;
              this.OnProgress((object) this, e);
            }
          }
          if (timeoutInSec.HasValue)
          {
            if (DateTime.Now.Subtract(this.receiveStartTime).TotalSeconds > (double) timeoutInSec.Value)
              break;
          }
          RadioPacket radioPacket;
          try
          {
            radioPacket = this.TryReceiveRadioPacket(timeoutInSec);
          }
          catch (Exception ex)
          {
            string str = "Failed to receive a radio packet! Stop WalkBy. Error: " + ex.Message;
            RadioReader.logger.Error(ex, str);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, str);
            return radioList.GetGlobalDeviceIdList();
          }
          if (radioPacket == null)
          {
            if (this.deviceCollector.BreakRequest || !micon.IsOpen)
            {
              RadioReader.logger.Info("Stop radio");
              return radioList.GetGlobalDeviceIdList();
            }
          }
          else
          {
            if (radioList.ExpectedDevices != null && radioList.ExpectedDevices.Count > 0)
            {
              if (radioList.ExpectedDevices.Contains(radioPacket.FunkId))
              {
                if (this.OnPacketReceived != null)
                  this.OnPacketReceived((object) this, radioPacket);
                radioList.AddPacket(radioPacket);
              }
            }
            else if (this.OnPacketReceived != null)
              this.OnPacketReceived((object) this, radioPacket);
            else
              radioList.AddPacket(radioPacket);
            string str = "?";
            if (radioList.ReceivedData.ContainsKey(radioPacket.FunkId))
              str = radioList.ReceivedData[radioPacket.FunkId].PacketsCount.ToString();
            this.deviceCollector.SendMessage(new GMM_EventArgs(radioPacket.FunkId == 0L ? "MinoConnect test packet received (" + str + ")" : radioPacket.FunkId.ToString() + " (" + str + ")")
            {
              TheMessageType = GMM_EventArgs.MessageType.WalkByPacketReceived
            });
            if (radioList.HasAllExpectedDevicesFound)
            {
              RadioReader.logger.Info("All devices was successful found!");
              ZR_ClassLibMessages.ClearErrors();
              break;
            }
            Application.DoEvents();
            if (this.deviceCollector.BreakRequest)
            {
              RadioReader.logger.Info("Cancel...");
              return radioList.GetGlobalDeviceIdList();
            }
          }
        }
      }
      finally
      {
        try
        {
          this.StopRadio();
        }
        catch (Exception ex)
        {
          string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          RadioReader.logger.Error(ex, message);
        }
        this.deviceCollector.BreakRequest = false;
        this.IsBusy = false;
      }
      return radioList.GetGlobalDeviceIdList();
    }

    private RadioPacket TryReceiveRadioPacket(int? timeoutInSec)
    {
      Application.DoEvents();
      if (this.deviceCollector.BreakRequest)
        return (RadioPacket) null;
      ZR_ClassLibMessages.ClearErrors();
      bool hasRssi = this.deviceCollector.MyCom.Transceiver == TransceiverDevice.MinoConnect;
      RadioPacket radioPacket;
      byte[] numArray;
      if (this.deviceCollector.MyBusMode == BusMode.Radio2)
      {
        radioPacket = (RadioPacket) new RadioPacketRadio2();
        if (this.deviceCollector.MyCom.Transceiver != TransceiverDevice.MinoConnect)
          throw new NotImplementedException();
        numArray = this.TryReceiveRadioTelegramByMinoConnect(timeoutInSec);
      }
      else if (this.deviceCollector.MyBusMode == BusMode.Radio3 || this.deviceCollector.MyBusMode == BusMode.Radio4 || this.deviceCollector.MyBusMode == BusMode.Radio3_868_95_RUSSIA)
      {
        radioPacket = (RadioPacket) new RadioPacketRadio3();
        if (this.deviceCollector.MyCom.Transceiver != TransceiverDevice.MinoConnect)
          throw new NotImplementedException("Unknown transceiver! Value: " + this.deviceCollector.MyCom.Transceiver.ToString());
        numArray = this.TryReceiveRadioTelegramByMinoConnect(timeoutInSec);
      }
      else if (this.deviceCollector.MyBusMode == BusMode.wMBusC1A || this.deviceCollector.MyBusMode == BusMode.wMBusC1B || this.deviceCollector.MyBusMode == BusMode.wMBusS1 || this.deviceCollector.MyBusMode == BusMode.wMBusS1M || this.deviceCollector.MyBusMode == BusMode.wMBusS2 || this.deviceCollector.MyBusMode == BusMode.wMBusT1 || this.deviceCollector.MyBusMode == BusMode.wMBusT2_meter || this.deviceCollector.MyBusMode == BusMode.wMBusT2_other)
      {
        radioPacket = (RadioPacket) new RadioPacketWirelessMBus();
        if (this.deviceCollector.MyCom.Transceiver == TransceiverDevice.MinoConnect)
        {
          numArray = this.TryReceiveRadioTelegramByMinoConnect(timeoutInSec);
        }
        else
        {
          if (this.deviceCollector.MyCom.Transceiver != TransceiverDevice.None)
            throw new NotImplementedException();
          CommunicationByMinoConnect micon = this.GetMicon();
          do
          {
            numArray = micon.ReadExisting();
          }
          while (!this.deviceCollector.BreakRequest && numArray.Length == 0);
        }
      }
      else
      {
        if (this.deviceCollector.MyBusMode != BusMode.RadioMS && this.deviceCollector.MyBusMode != BusMode.MinomatRadioTest)
          throw new NotImplementedException();
        radioPacket = (RadioPacket) new RadioPacketMinomatV4();
        if (this.deviceCollector.MyCom.Transceiver != TransceiverDevice.MinoConnect)
          throw new NotImplementedException();
        numArray = this.TryReceiveRadioTelegramByMinoConnect(timeoutInSec);
      }
      if (this.deviceCollector.BreakRequest || numArray == null)
        return (RadioPacket) null;
      radioPacket.MyFunctions = this.deviceCollector;
      try
      {
        if (!radioPacket.Parse(numArray, SystemValues.DateTimeNow, hasRssi))
          return (RadioPacket) null;
      }
      catch (Exception ex)
      {
        RadioReader.logger.Error<string, string>("Failed to parse the radio packet! Error: {0}, Buffer: {1}", ex.Message, ZR_ClassLibrary.Util.ByteArrayToHexString(numArray));
        return (RadioPacket) null;
      }
      return radioPacket;
    }

    public RadioPacket ReceiveOnePacket(long funkId, int timeout)
    {
      if (this.IsBusy)
        return (RadioPacket) null;
      this.deviceCollector.BreakRequest = false;
      if (!(this.deviceCollector.GetDeviceListForBusMode() is RadioList deviceListForBusMode))
        return (RadioPacket) null;
      deviceListForBusMode.ClearExpectedDevices();
      if (!deviceListForBusMode.AddExpectedDevice(funkId))
        return (RadioPacket) null;
      if (this.deviceCollector.MyBusMode == BusMode.MinomatRadioTest)
        this.deviceCollector.DaKonId = funkId.ToString();
      return this.Read(new int?(timeout)) == null || this.ReceivedData == null || !this.ReceivedData.ContainsKey(funkId) ? (RadioPacket) null : this.ReceivedData[funkId].LastRadioPacket;
    }

    private bool StartRadio()
    {
      ZR_ClassLibMessages.ClearErrors();
      CommunicationByMinoConnect micon = this.GetMicon();
      if (this.deviceCollector.MyCom.Transceiver != TransceiverDevice.MinoConnect)
        micon.Close();
      micon.Open();
      micon.ConnectionLost += new System.EventHandler(this.micon_ConnectionLost);
      return this.StartRadio(this.deviceCollector.MyBusMode);
    }

    private void micon_ConnectionLost(object sender, EventArgs e)
    {
      if (this.ConnectionLost == null)
        return;
      this.ConnectionLost(sender, e);
    }

    private bool StartRadio(BusMode mode)
    {
      CommunicationByMinoConnect micon = this.GetMicon();
      switch (mode)
      {
        case BusMode.MinomatRadioTest:
          if (!UserManager.CheckPermission(UserRights.Rights.MinomatV4))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinomatRadioTest!");
          if (string.IsNullOrEmpty(this.deviceCollector.DaKonId))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Wrong NetworkID for MinomatRadioTest!");
          byte networkID;
          try
          {
            networkID = byte.Parse(this.deviceCollector.DaKonId);
          }
          catch
          {
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Wrong NetworkID for MinomatRadioTest!");
          }
          return micon.StartMinomatRadioTest(networkID);
        case BusMode.Radio2:
          if (!UserManager.CheckPermission(UserRights.Rights.WalkBy))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Radio2!");
          micon.StartRadio2();
          return true;
        case BusMode.Radio3:
          if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Radio3!");
          micon.StartRadio3();
          return true;
        case BusMode.Radio4:
          if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Radio4!");
          micon.StartRadio4();
          return true;
        case BusMode.wMBusS1:
          if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for WirelessMBus!");
          micon.Start_wMBusS1();
          return true;
        case BusMode.wMBusS1M:
          if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for WirelessMBus!");
          micon.Start_wMBusS1M();
          return true;
        case BusMode.wMBusS2:
          if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for WirelessMBus!");
          micon.Start_wMBusS2();
          return true;
        case BusMode.wMBusT1:
          if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for WirelessMBus!");
          micon.Start_wMBusT1();
          return true;
        case BusMode.wMBusT2_meter:
          if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for WirelessMBus!");
          micon.Start_wMBusT2_meter();
          return true;
        case BusMode.wMBusT2_other:
          if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for WirelessMBus!");
          micon.Start_wMBusT2_other();
          return true;
        case BusMode.wMBusC1A:
          if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for WirelessMBus!");
          micon.Start_wMBusC1A();
          return true;
        case BusMode.wMBusC1B:
          if (!UserManager.CheckPermission(UserRights.Rights.WirelessMBus))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for WirelessMBus!");
          micon.Start_wMBusC1B();
          return true;
        case BusMode.Radio3_868_95_RUSSIA:
          if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Radio3 (RUSSIA)!");
          micon.StartRadio3_868_95_RUSSIA();
          return true;
        case BusMode.RadioMS:
          return !UserManager.CheckPermission(UserRights.Rights.MinomatV4) ? ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for RadioMS!") : micon.Start_RadioMS();
        default:
          throw new ArgumentException("Invalid radio mode! Value: " + mode.ToString());
      }
    }

    private void StopRadio()
    {
      CommunicationByMinoConnect micon = this.GetMicon();
      micon.ConnectionLost -= new System.EventHandler(this.micon_ConnectionLost);
      try
      {
        if (micon.IsOpen)
        {
          micon.StopRadio();
          micon.DiscardCurrentInBuffer();
        }
      }
      catch (Exception ex)
      {
        RadioReader.logger.Trace(ex.Message);
      }
      micon.Close();
    }

    private bool CheckScgiHeader(byte[] buffer)
    {
      if (buffer[0] != (byte) 170)
      {
        if (RadioReader.logger.IsTraceEnabled)
          RadioReader.logger.Trace("Invalid SyncByte in SCGI header! Expected: 0xAA, Actual: 0x" + buffer[0].ToString("X2"));
        return false;
      }
      if (buffer[1] <= (byte) 0)
        return true;
      if (RadioReader.logger.IsTraceEnabled)
        RadioReader.logger.Trace("Invalid Version in SCGI header! Expected: 0x00, Actual: 0x" + buffer[1].ToString("X2"));
      return false;
    }

    private byte[] TryReceiveRadioTelegramByMinoConnect(int? timeoutInSec)
    {
      try
      {
        byte[] numArray1 = (byte[]) null;
        Application.DoEvents();
        CommunicationByMinoConnect micon = this.GetMicon();
        DateTime now;
        while (!this.deviceCollector.BreakRequest && micon.BytesToRead < 6)
        {
          if (!micon.IsOpen)
          {
            RadioReader.logger.Error("The serial port was closed!");
            return (byte[]) null;
          }
          if (numArray1 == null)
          {
            if (timeoutInSec.HasValue)
            {
              now = DateTime.Now;
              if (now.Subtract(this.receiveStartTime).TotalSeconds > (double) timeoutInSec.Value)
                return (byte[]) null;
            }
            if (!ZR_ClassLibrary.Util.Wait(700L, "while receive SCGI header of the radio packet", (ICancelable) this.deviceCollector))
              return (byte[]) null;
            this.deviceCollector.SendMessage(this.WAIT_EVENT);
          }
          else
            break;
        }
        Application.DoEvents();
        if (this.deviceCollector.BreakRequest)
          return (byte[]) null;
        byte[] numArray2 = micon.ReadHeader(6);
        if (numArray2 == null)
          return (byte[]) null;
        if (!this.CheckScgiHeader(numArray2))
        {
          RadioReader.logger.Error("Invalid SCGI header!");
          micon.DiscardInBuffer();
          return (byte[]) null;
        }
        Application.DoEvents();
        if (this.deviceCollector.BreakRequest)
          return (byte[]) null;
        int count = ((int) numArray2[2] & (int) sbyte.MaxValue) * 2 + 1;
        int num1 = 5;
        while (!this.deviceCollector.BreakRequest && micon.BytesToRead < count)
        {
          --num1;
          Application.DoEvents();
          if (this.deviceCollector.BreakRequest)
            return (byte[]) null;
          if (timeoutInSec.HasValue)
          {
            now = DateTime.Now;
            if (now.Subtract(this.receiveStartTime).TotalSeconds > (double) timeoutInSec.Value)
              return (byte[]) null;
          }
          if (!micon.IsOpen)
          {
            RadioReader.logger.Error("The serial port is closed!");
            return (byte[]) null;
          }
          if (!ZR_ClassLibrary.Util.Wait(500L, "while receive residue of SCGI frame", (ICancelable) this.deviceCollector))
            return (byte[]) null;
          if (num1 == 0)
          {
            micon.DiscardInBuffer();
            return (byte[]) null;
          }
        }
        byte[] b = micon.ReadHeader(count);
        if (b == null)
          return (byte[]) null;
        Application.DoEvents();
        if (this.deviceCollector.BreakRequest)
          return (byte[]) null;
        byte[] sourceArray = ZR_ClassLibrary.Util.Combine(numArray2, b);
        byte num2 = 0;
        for (int index = 1; index < sourceArray.Length - 1; ++index)
          num2 += sourceArray[index];
        if ((int) num2 != (int) sourceArray[sourceArray.Length - 1])
          return (byte[]) null;
        byte[] destinationArray = new byte[sourceArray.Length - 6 - 1];
        Array.Copy((Array) sourceArray, 6, (Array) destinationArray, 0, destinationArray.Length);
        return destinationArray;
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        RadioReader.logger.Error(ex, message);
        return (byte[]) null;
      }
    }
  }
}
