// Decompiled with JetBrains decompiler
// Type: MBusLib.MBus
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class MBus : IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (MBus));
    private const int WAIT_AFTER_COLLISION_MS = 1000;
    public const int DEVICE_MODEL_ID_GENERIC_MBUS = 53;
    public const byte PRIMARY_ADDRESS_OF_SELECTED_DEVICE = 253;
    public const byte PRIMARY_ADDRESS_OF_BROADCAST = 254;
    public const byte PRIMARY_ADDRESS_OF_BROADCAST_NO_ANSWER = 255;

    public MBusRepeater Repeater { get; private set; }

    public event EventHandler<RSP_UD> OnDeviceFound;

    public MBus(IPort port)
    {
      this.Repeater = port != null ? new MBusRepeater(port) : throw new ArgumentNullException();
    }

    public void Dispose()
    {
      if (this.Repeater == null)
        return;
      this.Repeater.Dispose();
      this.Repeater = (MBusRepeater) null;
    }

    public async Task SND_NKE_Async(
      ProgressHandler progress,
      CancellationToken token,
      byte address)
    {
      await Task.Run((Action) (() => this.SND_NKE(progress, token, address)), token);
    }

    public void SND_NKE(ProgressHandler progress, CancellationToken token, byte address)
    {
      token.ThrowIfCancellationRequested();
      this.Repeater.Port.Write(new MBusFrame()
      {
        Type = FrameType.ShortFrame,
        Control = C_Field.SND_NKE,
        Address = address
      }.ToByteArray());
      string message = "SND_NKE (addr: " + address.ToString() + ")";
      progress?.Report(message);
      MBus.logger.Debug(message);
      if (address == byte.MaxValue)
        return;
      this.WaitAndDiscardInBuffer(300);
    }

    public async Task ApplicationResetAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte address)
    {
      await Task.Run((Action) (() => this.ApplicationReset(progress, token, address)), token);
    }

    public void ApplicationReset(ProgressHandler progress, CancellationToken token, byte address)
    {
      token.ThrowIfCancellationRequested();
      this.Repeater.Port.Write(new MBusFrame()
      {
        Control = C_Field.SND_UD_73h,
        Address = address,
        ControlInfo = CI_Field.ApplicationReset
      }.ToByteArray());
      string message = "Application Reset (addr: " + address.ToString() + ")";
      progress?.Report(message);
      MBus.logger.Debug(message);
      if (address == byte.MaxValue)
        return;
      this.WaitAndDiscardInBuffer(300);
    }

    public async Task ChangePrimaryAddressAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte newAddress,
      byte oldAddress = 254)
    {
      await Task.Run((Action) (() => this.ChangePrimaryAddress(progress, token, newAddress, oldAddress)), token);
    }

    public void ChangePrimaryAddress(
      ProgressHandler progress,
      CancellationToken token,
      byte newAddress,
      byte oldAddress = 254)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (newAddress == (byte) 251 || newAddress == byte.MaxValue)
        throw new ArgumentException("Invalid primary address! Value: " + newAddress.ToString());
      token.ThrowIfCancellationRequested();
      VariableDataBlock variableDataBlock = new VariableDataBlock();
      variableDataBlock.SetDIF((byte) 1);
      variableDataBlock.SetVIF((byte) 122);
      variableDataBlock.SetData(BitConverter.GetBytes((short) newAddress));
      MBusFrame frame = new MBusFrame();
      frame.Control = C_Field.SND_UD_53h;
      frame.ControlInfo = CI_Field.DataSendMode1;
      frame.UserData = variableDataBlock.ToByteArray();
      frame.Address = oldAddress;
      progress.Split(new double[2]{ 10.0, 90.0 });
      progress.Report("Change primary address from " + frame.Address.ToString() + " to " + newAddress.ToString());
      MBusFrame resultFrame = this.Repeater.GetResultFrame(frame, progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response by change the primary address! Expected: ACK, but receive: " + resultFrame.Type.ToString());
    }

    public async Task SelectDeviceOnBusAsync(
      ProgressHandler progress,
      CancellationToken token,
      Wildcard wildcard,
      bool waitOfTimeout)
    {
      await Task.Run((Action) (() => this.SelectDeviceOnBus(progress, token, wildcard, waitOfTimeout)), token);
    }

    public void SelectDeviceOnBus(
      ProgressHandler progress,
      CancellationToken token,
      Wildcard wildcard,
      bool waitOfTimeout)
    {
      this.SelectDeviceOnBus(progress, token, wildcard.ID_BCD, wildcard.Manufacturer, wildcard.Version, wildcard.Medium, waitOfTimeout);
    }

    public void SelectDeviceOnBus(
      ProgressHandler progress,
      CancellationToken token,
      uint id_bcd,
      ushort manufacturer,
      byte version,
      byte medium,
      bool waitOfTimeout)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      token.ThrowIfCancellationRequested();
      MBusFrame mbusFrame = new MBusFrame();
      mbusFrame.Control = C_Field.SND_UD_73h;
      mbusFrame.Address = (byte) 253;
      mbusFrame.ControlInfo = CI_Field.SelectionOfDevice;
      List<byte> byteList = new List<byte>(8);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(id_bcd));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(manufacturer));
      byteList.Add(version);
      byteList.Add(medium);
      mbusFrame.UserData = byteList.ToArray();
      string message = string.Format("Select Slave (ID:{0:X8}, MAN:{1:X4}, VER:{2:X2}, MED:{3:X2})", (object) id_bcd, (object) manufacturer, (object) version, (object) medium);
      progress.Report(message);
      MBus.logger.Debug(message);
      this.Repeater.Port.Write(mbusFrame.ToByteArray());
      byte[] buffer1 = this.Repeater.Port.ReadHeader(1);
      if (buffer1[0] != (byte) 229)
      {
        this.WaitAndDiscardInBuffer(1000);
        throw new InvalidFrameException("Invalid M-Bus response. Expected ACK 0xE5, actual 0x" + buffer1[0].ToString("X2"), buffer1);
      }
      if (!waitOfTimeout)
        return;
      try
      {
        byte[] buffer2 = this.Repeater.Port.ReadHeader(1);
        this.WaitAndDiscardInBuffer(1000);
        throw new InvalidFrameException("Collision detected by selection of a slave.", buffer2);
      }
      catch (TimeoutException ex)
      {
      }
    }

    public async Task<RSP_UD> REQ_UD2Async(
      ProgressHandler progress,
      CancellationToken token,
      byte address,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      RSP_UD rspUd = await Task.Run<RSP_UD>((Func<RSP_UD>) (() => this.REQ_UD2(progress, token, address, useREQ_UD2_5B, isMultiTelegrammEnabled)), token);
      return rspUd;
    }

    public RSP_UD REQ_UD2(
      ProgressHandler progress,
      CancellationToken token,
      byte address,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      if (this.Repeater == null)
        throw new ObjectDisposedException("Repeater");
      token.ThrowIfCancellationRequested();
      bool flag = useREQ_UD2_5B;
      RSP_UD rspUd = (RSP_UD) null;
      int num = 0;
      VariableDataStructure lastRecord;
      do
      {
        if (progress != null)
        {
          progress.Split(new double[3]{ 10.0, 20.0, 70.0 });
          string message = "REQ_UD2 (addr: " + address.ToString() + ") " + (num > 0 ? num.ToString() : string.Empty);
          progress.Report(message);
          MBus.logger.Debug(message);
        }
        MBusFrame resultFrame = this.Repeater.GetResultFrame(new MBusFrame()
        {
          Type = FrameType.ShortFrame,
          Address = address,
          Control = flag ? C_Field.REQ_UD2_5Bh : C_Field.REQ_UD2_7Bh
        }, progress, token);
        if (resultFrame.Type != FrameType.LongFrame)
          throw new InvalidFrameException("Invalid M-Bus frame! Expected: LongFrame, Actual: " + resultFrame.Type.ToString(), resultFrame.ToByteArray());
        if (rspUd == null)
          rspUd = new RSP_UD();
        rspUd.Add(resultFrame);
        flag = !flag;
        ++num;
        lastRecord = rspUd.GetLastRecord();
      }
      while (isMultiTelegrammEnabled && lastRecord.HasMoreRecordsInNextTelegram);
      if (progress != null)
      {
        if (rspUd.Frames.Count > 1)
          progress.Report("RSP_UD " + rspUd.Frames.Count.ToString() + " telegram(s)");
        else
          progress.Report("RSP_UD");
      }
      return rspUd;
    }

    public async Task ScanSecundaryAsync(
      ProgressHandler progress,
      CancellationToken token,
      bool useREQ_UD2_5B,
      string scanStartSerialnumber,
      bool isMultiTelegrammEnabled)
    {
      await Task.Run((Action) (() => this.ScanSecundary(progress, token, useREQ_UD2_5B, scanStartSerialnumber, isMultiTelegrammEnabled)), token);
    }

    public void ScanSecundary(
      ProgressHandler progress,
      CancellationToken token,
      bool useREQ_UD2_5B,
      string scanStartSerialnumber,
      bool isMultiTelegrammEnabled)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      Wildcard wildcard = new Wildcard(4294967280U);
      uint result = 0;
      if (uint.TryParse(scanStartSerialnumber, NumberStyles.HexNumber, (IFormatProvider) null, out result))
        wildcard.ID_BCD = result;
      bool flag = wildcard.ID_BCD >> 28 == 0U;
      try
      {
        progress.Split(22);
        while (true)
        {
          token.ThrowIfCancellationRequested();
          bool collision = false;
          try
          {
            this.SelectDeviceOnBus(progress, token, wildcard, true);
            this.CheckDevice(progress, token, (byte) 253, useREQ_UD2_5B, isMultiTelegrammEnabled);
          }
          catch (ChecksumException ex)
          {
            MBus.logger.Error(ex.Message);
            progress.Report(ex.Message);
            collision = true;
          }
          catch (InvalidFrameException ex)
          {
            MBus.logger.Error(ex.Message);
            progress.Report(ex.Message);
            collision = true;
          }
          catch (Exception ex)
          {
            if (ex is TimeoutException)
              MBus.logger.Debug(ex.Message);
            else
              MBus.logger.Error(ex.Message);
            progress.Report(ex.Message);
          }
          if (collision)
          {
            progress.Split(21);
            int num = 10;
            while (num-- > 0)
            {
              try
              {
                this.WaitAndDiscardInBuffer(1000);
                this.SND_NKE(progress, token, (byte) 253);
                byte[] buffer = this.Repeater.Port.ReadHeader((int) byte.MaxValue);
                MBus.logger.Error("Garbage after collision: " + Util.ByteArrayToHexString((IEnumerable<byte>) buffer));
              }
              catch (TimeoutException ex)
              {
                goto label_21;
              }
              catch (Exception ex)
              {
                MBus.logger.Error(ex.Message);
              }
            }
            break;
          }
label_21:
          uint idBcd = wildcard.ID_BCD;
          if (flag)
          {
            if (!MBus.ScanWalkerSetNextAddressFirstToLast(ref idBcd, collision))
              goto label_27;
          }
          else if (!MBus.ScanWalkerSetNextAddressLastToFirst(ref idBcd, collision))
            goto label_24;
          wildcard.ID_BCD = idBcd;
        }
        throw new Exception("Input buffer contains a Stream of uninterrupted bytes.");
label_27:
        return;
label_24:;
      }
      finally
      {
        progress.Report("Scan done!");
      }
    }

    public async Task ScanPrimaryAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte scanStartAddress,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      await Task.Run((Action) (() => this.ScanPrimary(progress, token, scanStartAddress, useREQ_UD2_5B, isMultiTelegrammEnabled)), token);
    }

    public void ScanPrimary(
      ProgressHandler progress,
      CancellationToken token,
      byte scanStartAddress,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      progress.Split(250 - (int) scanStartAddress + 1);
      for (byte address = scanStartAddress; address <= (byte) 250; ++address)
        this.ScanSlaveViaAddress(progress, token, address, useREQ_UD2_5B, isMultiTelegrammEnabled);
      progress.Report("Scan done!");
    }

    public async Task ScanSlaveViaWildcardAsync(
      ProgressHandler progress,
      CancellationToken token,
      Wildcard wildcard,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      await Task.Run((Action) (() => this.ScanSlaveViaWildcard(progress, token, wildcard, useREQ_UD2_5B, isMultiTelegrammEnabled)), token);
    }

    public void ScanSlaveViaWildcard(
      ProgressHandler progress,
      CancellationToken token,
      Wildcard wildcard,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      try
      {
        progress.Split(4);
        token.ThrowIfCancellationRequested();
        try
        {
          this.SelectDeviceOnBus(progress, token, wildcard, true);
          this.CheckDevice(progress, token, (byte) 253, useREQ_UD2_5B, isMultiTelegrammEnabled);
        }
        catch (ChecksumException ex)
        {
          MBus.logger.Error(ex.Message);
          progress.Report(ex.Message);
          this.WaitAndDiscardInBuffer(1000);
        }
        catch (InvalidFrameException ex)
        {
          MBus.logger.Error(ex.Message);
          progress.Report(ex.Message);
          this.WaitAndDiscardInBuffer(1000);
        }
        catch (Exception ex)
        {
          if (ex is TimeoutException)
            MBus.logger.Debug(ex.Message);
          else
            MBus.logger.Error(ex.Message);
          progress.Report(ex.Message);
        }
      }
      finally
      {
        progress.Report("Scan done!");
      }
    }

    public async Task ScanSlaveViaAddressAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte address,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      await Task.Run((Action) (() => this.ScanSlaveViaAddress(progress, token, address, useREQ_UD2_5B, isMultiTelegrammEnabled)), token);
    }

    public void ScanSlaveViaAddress(
      ProgressHandler progress,
      CancellationToken token,
      byte address,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      token.ThrowIfCancellationRequested();
      try
      {
        this.CheckDevice(progress, token, address, useREQ_UD2_5B, isMultiTelegrammEnabled);
      }
      catch (ChecksumException ex)
      {
        MBus.logger.Error(ex.Message);
        progress.Report(ex.Message);
        this.WaitAndDiscardInBuffer(1000);
      }
      catch (InvalidFrameException ex)
      {
        MBus.logger.Error(ex.Message);
        progress.Report(ex.Message);
        this.WaitAndDiscardInBuffer(1000);
      }
      catch (Exception ex)
      {
        if (ex is TimeoutException)
          MBus.logger.Debug(ex.Message);
        else
          MBus.logger.Error(ex.Message);
        progress.Report(ex.Message);
      }
    }

    private void CheckDevice(
      ProgressHandler progress,
      CancellationToken token,
      byte address,
      bool useREQ_UD2_5B,
      bool isMultiTelegrammEnabled)
    {
      if (this.OnDeviceFound == null)
        return;
      RSP_UD e = this.REQ_UD2(progress, token, address, useREQ_UD2_5B, isMultiTelegrammEnabled);
      if (e == null)
        return;
      this.OnDeviceFound((object) this, e);
    }

    private static bool ScanWalkerSetNextAddressFirstToLast(ref uint address, bool collision)
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

    private static bool ScanWalkerSetNextAddressLastToFirst(ref uint address, bool collision)
    {
      uint address1 = uint.Parse(Util.ReverseString(address.ToString("X8")), NumberStyles.HexNumber);
      if (!MBus.ScanWalkerSetNextAddressFirstToLast(ref address1, collision))
        return false;
      string s = Util.ReverseString(address1.ToString("X8"));
      address = uint.Parse(s, NumberStyles.HexNumber);
      return true;
    }

    public async Task SetAddrViaIDAsync(
      ProgressHandler progress,
      CancellationToken token,
      Wildcard wildcard,
      byte newAddress)
    {
      progress.Split(2);
      await this.SelectDeviceOnBusAsync(progress, token, wildcard, false);
      await this.ChangePrimaryAddressAsync(progress, token, newAddress, (byte) 253);
    }

    public async Task<bool> SetBaudRateAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte address,
      Baud baud)
    {
      bool flag = await Task.Run<bool>((Func<bool>) (() => this.SetBaudRate(progress, token, address, baud)), token);
      return flag;
    }

    public bool SetBaudRate(
      ProgressHandler progress,
      CancellationToken token,
      byte address,
      Baud baud)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (this.Repeater == null)
        throw new ObjectDisposedException("Repeater");
      token.ThrowIfCancellationRequested();
      progress.Report("Set baud " + baud.ToString() + " (addr: " + address.ToString() + ")");
      this.Repeater.Port.Write(new MBusFrame()
      {
        Control = C_Field.SND_UD_73h,
        Address = address,
        ControlInfo = ((CI_Field) baud)
      }.ToByteArray());
      byte[] buffer = this.Repeater.Port.ReadHeader(1);
      if (buffer[0] != (byte) 229)
      {
        this.WaitAndDiscardInBuffer(1000);
        throw new InvalidFrameException("Failed set the new baud " + baud.ToString() + "!", buffer);
      }
      return true;
    }

    private void WaitAndDiscardInBuffer(int milliseconds)
    {
      int num = 5;
      do
      {
        Thread.Sleep(milliseconds);
        if (num <= 0)
          throw new Exception("Input buffer contains a Stream of uninterrupted bytes.");
        --num;
      }
      while (this.Repeater.Port.DiscardInBuffer());
    }
  }
}
