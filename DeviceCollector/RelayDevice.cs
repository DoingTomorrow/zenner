// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RelayDevice
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  internal sealed class RelayDevice
  {
    private static Logger logger = LogManager.GetLogger(nameof (RelayDevice));
    private IAsyncFunctions channel;
    private DeviceCollectorFunctions theBus;
    private const int RETRY_LIMIT_LOGIN = 12;
    private const int RETRY_LIMIT_LOGOUT = 1;
    private const string DEFAULT_SERVICE_PASSWORD = "1767";
    private const string FILE_NAME = "ZENTRALE.BIN_";

    public event System.EventHandler ReadStorageCompleted;

    public RelayDevice(DeviceCollectorFunctions theBus)
    {
      this.MBusTelegrams = new MbusTelegramCollection();
      this.theBus = theBus;
      this.channel = theBus.MyCom;
      this.Password = "1767";
      SortedList<DeviceCollectorSettings, object> collectorSettings = theBus.GetDeviceCollectorSettings();
      if (collectorSettings == null || !collectorSettings.ContainsKey(DeviceCollectorSettings.Password))
        return;
      string str = collectorSettings[DeviceCollectorSettings.Password] as string;
      if (!string.IsNullOrEmpty(str))
        this.Password = str;
    }

    public string Password { get; private set; }

    public byte[] ActualPacketData { get; private set; }

    public byte[] ReceivedFile { get; set; }

    public MbusTelegramCollection MBusTelegrams { get; private set; }

    public MbusTelegramCollection Read()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MBus))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M-Bus!");
        return (MbusTelegramCollection) null;
      }
      this.theBus.BreakRequest = false;
      if (!this.channel.Open())
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Can not open connection to the Relay device!");
        return (MbusTelegramCollection) null;
      }
      if (!this.Login())
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Login to the Relay Central Unit device failed!");
        return (MbusTelegramCollection) null;
      }
      if (this.theBus.BreakRequest)
        return (MbusTelegramCollection) null;
      if (!this.ReadDataStorage())
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Can not read data stored by the Relay Central Unit!");
        return (MbusTelegramCollection) null;
      }
      return !this.SplitRelayTelegrams() ? (MbusTelegramCollection) null : this.MBusTelegrams;
    }

    private bool Login()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MBus))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M-Bus!");
        return false;
      }
      ByteField DataBlock1 = new ByteField();
      string message = nameof (Login);
      int num = 0;
      while (num < 12 && !this.theBus.BreakRequest)
      {
        this.theBus.SendMessage(new GMM_EventArgs(message));
        ByteField DataBlock2 = new ByteField(4);
        DataBlock2.Add(27);
        DataBlock2.Add(27);
        DataBlock2.Add(27);
        DataBlock2.Add(1);
        this.channel.TransmitBlock(ref DataBlock2);
        if (!Util.Wait(300L, nameof (Login), (ICancelable) this.theBus, RelayDevice.logger))
          return false;
        this.channel.ReceiveBlock(ref DataBlock1);
        if (!Util.Wait(1000L, nameof (Login), (ICancelable) this.theBus, RelayDevice.logger))
          return false;
        if (this.ContainsByteSequence(DataBlock1.Data, DataBlock1.Count, new byte[4]
        {
          (byte) 27,
          (byte) 27,
          (byte) 27,
          (byte) 240
        }))
        {
          this.theBus.SendMessage(new GMM_EventArgs("The login was successful!"));
          return true;
        }
        message += ".";
        this.theBus.SendMessage(new GMM_EventArgs(message));
        ByteField DataBlock3 = new ByteField(4);
        DataBlock3.Add(27);
        DataBlock3.Add(27);
        DataBlock3.Add(27);
        DataBlock3.Add(2);
        this.channel.TransmitBlock(ref DataBlock3);
        for (int index = 0; index < this.Password.Length; ++index)
        {
          ByteField DataBlock4 = new ByteField(1);
          DataBlock4.Add((byte) this.Password[index]);
          this.channel.TransmitBlock(ref DataBlock4);
          if (!Util.Wait(300L, "Send password", (ICancelable) this.theBus, RelayDevice.logger))
            return false;
          this.channel.ReceiveBlock(ref DataBlock1);
        }
        ByteField DataBlock5 = new ByteField(1);
        DataBlock5.Add(13);
        this.channel.TransmitBlock(ref DataBlock5);
        if (!Util.Wait(500L, "Receive password", (ICancelable) this.theBus, RelayDevice.logger))
          return false;
        this.channel.ReceiveBlock(ref DataBlock1);
        if (!Util.Wait(500L, "After password", (ICancelable) this.theBus, RelayDevice.logger))
          return false;
        if (this.ContainsByteSequence(DataBlock1.Data, DataBlock1.Count, new byte[4]
        {
          (byte) 27,
          (byte) 27,
          (byte) 27,
          (byte) 2
        }))
        {
          this.theBus.SendMessage(new GMM_EventArgs("The login was successful!"));
          return true;
        }
        if (this.ContainsByteSequence(DataBlock1.Data, DataBlock1.Count, new byte[4]
        {
          (byte) 27,
          (byte) 27,
          (byte) 27,
          (byte) 240
        }))
        {
          this.theBus.SendMessage(new GMM_EventArgs("The login was successful!"));
          return true;
        }
        ++num;
        if (num > 3)
        {
          this.theBus.SendMessage(new GMM_EventArgs("Login attempt was not successful. Try again (" + num.ToString() + " attempt)."));
        }
        else
        {
          message += ".";
          this.theBus.SendMessage(new GMM_EventArgs(message));
        }
        if (!Util.Wait(4000L, nameof (Login), (ICancelable) this.theBus, RelayDevice.logger))
          return false;
      }
      return false;
    }

    private bool Logout()
    {
      this.theBus.SendMessage(new GMM_EventArgs(nameof (Logout)));
      ByteField DataBlock1 = new ByteField(new byte[5]
      {
        (byte) 27,
        (byte) 27,
        (byte) 27,
        (byte) 3,
        (byte) 13
      });
      int num = 0;
      while (num < 1 && !this.theBus.BreakRequest)
      {
        this.channel.TransmitBlock(ref DataBlock1);
        ByteField DataBlock2 = new ByteField();
        try
        {
          if (!Util.Wait(400L, nameof (Logout), (ICancelable) this.theBus, RelayDevice.logger))
            return false;
          this.channel.ReceiveBlock(ref DataBlock2);
          if (this.ContainsByteSequence(DataBlock2.Data, DataBlock2.Count, new byte[4]
          {
            (byte) 27,
            (byte) 27,
            (byte) 27,
            (byte) 3
          }))
          {
            this.theBus.SendMessage(new GMM_EventArgs("The logout was successful!"));
            return true;
          }
        }
        catch (TimeoutException ex)
        {
        }
        ++num;
        this.theBus.SendMessage(new GMM_EventArgs("Logout attempt was not successful. Try again (" + num.ToString() + " attempt)."));
        if (!Util.Wait(1000L, nameof (Logout), (ICancelable) this.theBus, RelayDevice.logger))
          return false;
      }
      return false;
    }

    private bool ReadDataStorage()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MBus))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M-Bus!");
        return false;
      }
      ByteField DataBlock1 = new ByteField(new byte[4]
      {
        (byte) 27,
        (byte) 27,
        (byte) 27,
        (byte) 19
      });
      this.channel.TransmitBlock(ref DataBlock1);
      if (!Util.Wait(600L, nameof (ReadDataStorage), (ICancelable) this.theBus, RelayDevice.logger))
        return false;
      ByteField DataBlock2 = new ByteField();
      this.channel.ReceiveBlock(ref DataBlock2);
      if (this.ContainsByteSequence(DataBlock2.Data, DataBlock2.Count, new byte[4]
      {
        (byte) 27,
        (byte) 27,
        (byte) 27,
        (byte) 19
      }))
      {
        RelayDevice.logger.Debug("The device has changed to the Y-Modem mode successfully.");
        YModem ymodem = new YModem(this.channel);
        ymodem.PacketReceived += new System.EventHandler(this.Ymodem_PacketReceived);
        byte[] numArray = ymodem.Receive(true);
        if (numArray == null)
          return false;
        this.ReceivedFile = numArray;
        if (this.ReadStorageCompleted != null)
          this.ReadStorageCompleted((object) this, (EventArgs) null);
        ymodem.Receive(true);
        if (!Util.Wait(500L, "Read data", (ICancelable) this.theBus, RelayDevice.logger))
          return false;
        this.channel.ReceiveBlock(ref DataBlock2);
        if (this.ContainsByteSequence(DataBlock2.Data, DataBlock2.Count, new byte[4]
        {
          (byte) 27,
          (byte) 27,
          (byte) 27,
          (byte) 17
        }))
          RelayDevice.logger.Debug("The Y-Modem-mode has been successfully exited.");
        else
          RelayDevice.logger.Debug("The Y-Modem-mode has been failed to exited.");
        return true;
      }
      RelayDevice.logger.Debug("Can not change to Y-Modem-mode.");
      return false;
    }

    private bool SplitRelayTelegrams()
    {
      if (this.ReceivedFile == null)
        return false;
      this.MBusTelegrams.Clear();
      for (int sourceIndex = 10; sourceIndex + 4 < this.ReceivedFile.Length; ++sourceIndex)
      {
        bool flag1 = this.ReceivedFile[sourceIndex] == (byte) 104 && this.ReceivedFile[sourceIndex + 3] == (byte) 104;
        bool flag2 = (int) this.ReceivedFile[sourceIndex + 1] == (int) this.ReceivedFile[sourceIndex + 2] && this.ReceivedFile[sourceIndex + 1] > (byte) 0;
        if (flag1 && flag2)
        {
          string str = Encoding.ASCII.GetString(this.ReceivedFile, sourceIndex - 10, 10);
          DateTime timePoint = DateTime.Parse(string.Format("{0}/{1}/20{2} {3}:{4}:00", (object) str.Substring(0, 2), (object) str.Substring(2, 2), (object) str.Substring(4, 2), (object) str.Substring(6, 2), (object) str.Substring(8, 2)), (IFormatProvider) FixedFormates.TheFormates.DateTimeFormat);
          byte primaryAddress = this.ReceivedFile[sourceIndex + 5];
          byte[] numArray = new byte[(int) this.ReceivedFile[sourceIndex + 1] + 6];
          Array.Copy((Array) this.ReceivedFile, sourceIndex, (Array) numArray, 0, numArray.Length);
          long int64 = Util.ConvertBcdInt64ToInt64((long) numArray[7] + ((long) numArray[8] << 8) + ((long) numArray[9] << 16) + ((long) numArray[10] << 24));
          this.MBusTelegrams.Add(primaryAddress, int64, timePoint, numArray);
        }
      }
      return true;
    }

    private bool ContainsByteSequence(byte[] src, int length, byte[] part)
    {
      for (int index1 = 0; index1 < length; ++index1)
      {
        if ((int) src[index1] == (int) part[0] && length - index1 >= part.Length)
        {
          bool flag = true;
          int index2 = 0;
          while (index2 < part.Length)
          {
            if ((int) src[index1] != (int) part[index2])
            {
              flag = false;
              break;
            }
            ++index2;
            ++index1;
          }
          if (flag)
            return true;
        }
      }
      return false;
    }

    private void Ymodem_PacketReceived(object sender, EventArgs e)
    {
      if (!(sender is YModem ymodem))
        return;
      this.ActualPacketData = ymodem.ActualPacketData;
      if (ymodem.ExpectedPackets > 0L && ymodem.ExpectedPackets >= (long) ymodem.ActualPacketNumber)
      {
        int num = (int) ((long) (ymodem.ActualPacketNumber * 100) / ymodem.ExpectedPackets);
        GMM_EventArgs e1 = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
        e1.EventMessage = string.Format("Progress {0} %", (object) num);
        e1.ProgressPercentage = num;
        try
        {
          this.theBus.SendMessage(e1);
        }
        catch
        {
        }
      }
      if (!this.theBus.BreakRequest)
        return;
      ymodem.IsCanceled = true;
    }

    private void SaveFile(byte[] buffer)
    {
      using (FileStream output = new FileStream(Path.Combine(SystemValues.LoggDataPath, "ZENTRALE.BIN_" + SystemValues.DateTimeNow.Millisecond.ToString()), FileMode.Create, FileAccess.Write))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
          binaryWriter.Write(buffer);
      }
    }

    private byte[] LoadFile()
    {
      string[] files = Directory.GetFiles(SystemValues.LoggDataPath);
      string path1 = string.Empty;
      long num1 = 0;
      foreach (string path2 in files)
      {
        string fileName = Path.GetFileName(path2);
        if (fileName.StartsWith("ZENTRALE.BIN_"))
        {
          try
          {
            long num2 = long.Parse(fileName.Substring("ZENTRALE.BIN_".Length));
            if (num1 < num2)
            {
              num1 = num2;
              path1 = path2;
            }
          }
          catch
          {
          }
        }
      }
      if (!File.Exists(path1))
        return (byte[]) null;
      using (FileStream input = new FileStream(path1, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
          return binaryReader.ReadBytes((int) input.Length);
      }
    }
  }
}
