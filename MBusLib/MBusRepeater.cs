// Decompiled with JetBrains decompiler
// Type: MBusLib.MBusRepeater
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace MBusLib
{
  public sealed class MBusRepeater : IDisposable
  {
    private static Logger MBusRepeaterLogger = LogManager.GetLogger("MBus_Repeater");

    public static byte LastResponseMBusStatusByte { get; private set; }

    public IPort Port { get; private set; }

    public string AES_Key { get; set; }

    public MBusRepeater(IPort port) => this.Port = port;

    public void Dispose()
    {
      if (this.Port == null)
        return;
      this.Port.Dispose();
    }

    public async Task<MBusFrame> GetResultFrameAsync(
      MBusFrame frame,
      ProgressHandler progress,
      CancellationToken token)
    {
      MBusFrame resultFrameAsync = await Task.Run<MBusFrame>((Func<MBusFrame>) (() => this.GetResultFrame(frame, progress, token)), token);
      return resultFrameAsync;
    }

    public MBusFrame GetResultFrame(MBusFrame frame)
    {
      return this.GetResultFrame(frame, (ProgressHandler) null, CancellationToken.None);
    }

    public MBusFrame GetResultFrame(
      MBusFrame frame,
      ProgressHandler progress,
      CancellationToken token)
    {
      if (frame == null)
        throw new ArgumentNullException(nameof (frame));
      if (!this.Port.IsOpen)
      {
        if (progress != null)
        {
          progress.Split(new double[2]{ 10.0, 90.0 });
          progress.Report("Port open...");
        }
        this.Port.Open();
      }
      int millisecondsDelay = 100;
      int num1 = 3;
      ConfigList readoutConfiguration = this.Port.GetReadoutConfiguration();
      if (readoutConfiguration != null)
      {
        if (readoutConfiguration.WaitBeforeRepeatTime > 0)
          millisecondsDelay = readoutConfiguration.WaitBeforeRepeatTime;
        if (readoutConfiguration.MaxRequestRepeat > 0)
          num1 = readoutConfiguration.MaxRequestRepeat;
      }
      List<Exception> exceptionList = new List<Exception>();
      for (int index = 0; index < num1; ++index)
      {
        token.ThrowIfCancellationRequested();
        try
        {
          if (index > 0)
            Task.Delay(millisecondsDelay, token);
          byte[] byteArray = frame.ToByteArray();
          MBusRepeater.MBusRepeaterLogger.Debug("REQUEST: " + BitConverter.ToString(byteArray));
          this.Port.Write(byteArray);
          return this.ReadMBusFrame();
        }
        catch (TimeoutException ex)
        {
          exceptionList.Add((Exception) ex);
          this.Port.ForceWakeup();
        }
        catch (Exception ex)
        {
          exceptionList.Add(ex);
        }
        if (progress != null)
        {
          int num2 = 100 / (num1 - index);
          int num3 = 100 - num2;
          progress.Split(new double[2]
          {
            (double) num2,
            (double) num3
          });
          progress.Report(exceptionList[exceptionList.Count - 1].Message + " Retry: " + (index + 1).ToString());
        }
      }
      Exception lastException = exceptionList.Last<Exception>();
      if (exceptionList.All<Exception>((Func<Exception, bool>) (x => x.GetType() == lastException.GetType())))
        throw lastException;
      throw new AggregateException((IEnumerable<Exception>) exceptionList);
    }

    public void TransmitMBusFrame(MBusFrame frame)
    {
      if (frame == null)
        throw new ArgumentNullException(nameof (frame));
      if (!this.Port.IsOpen)
        this.Port.Open();
      int num1 = 100;
      int num2 = 3;
      ConfigList readoutConfiguration = this.Port.GetReadoutConfiguration();
      if (readoutConfiguration != null)
      {
        if (readoutConfiguration.WaitBeforeRepeatTime > 0)
          num1 = readoutConfiguration.WaitBeforeRepeatTime;
        if (readoutConfiguration.MaxRequestRepeat > 0)
          num2 = readoutConfiguration.MaxRequestRepeat;
      }
      List<Exception> exceptionList = new List<Exception>();
      byte[] byteArray = frame.ToByteArray();
      MBusRepeater.MBusRepeaterLogger.Debug("REQUEST: " + BitConverter.ToString(byteArray));
      this.Port.Write(byteArray);
    }

    public MBusFrame ReadMBusFrame()
    {
      byte[] numArray1 = this.Port.ReadHeader(1);
      if (numArray1[0] == (byte) 229)
        return MBusFrame.Parse(numArray1);
      if (numArray1[0] == (byte) 16)
      {
        byte[] src = this.Port.ReadEnd(4);
        byte[] numArray2 = new byte[5]
        {
          numArray1[0],
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0
        };
        Buffer.BlockCopy((Array) src, 0, (Array) numArray2, 1, src.Length);
        return (int) MBusUtil.CalculateChecksum(numArray2, 1, 2) == (int) numArray2[3] ? MBusFrame.Parse(numArray2) : throw new ChecksumException(numArray2);
      }
      byte[] second = this.Port.ReadEnd(3);
      if (second[2] != (byte) 104 || (int) second[0] != (int) second[1] || second[0] == (byte) 0)
      {
        byte[] array = ((IEnumerable<byte>) numArray1).Concat<byte>((IEnumerable<byte>) second).ToArray<byte>();
        throw new InvalidFrameException("Invalid M-Bus header. Expected 68 L L 68 , actual: " + Util.ByteArrayToHexString((IEnumerable<byte>) array), array);
      }
      byte[] src1 = this.Port.ReadEnd((int) second[0] + 2);
      byte[] numArray3 = new byte[4 + src1.Length];
      numArray3[0] = second[2];
      numArray3[1] = second[0];
      numArray3[2] = second[1];
      numArray3[3] = second[2];
      Buffer.BlockCopy((Array) src1, 0, (Array) numArray3, 4, src1.Length);
      MBusFrame frame = (int) MBusUtil.CalculateChecksum(numArray3, 4, numArray3.Length - 2) == (int) numArray3[numArray3.Length - 2] ? MBusFrame.Parse(numArray3) : throw new ChecksumException(numArray3);
      if (frame != null && frame.IsVariableDataStructure)
      {
        try
        {
          VariableDataStructure variableDataStructure = VariableDataStructure.Parse(frame);
          if (variableDataStructure != null && variableDataStructure.Header != null)
          {
            MBusRepeater.LastResponseMBusStatusByte = variableDataStructure.Header.Status;
            MBusRepeater.MBusRepeaterLogger.Info("MBusRepeater.LastResponseMBusStatusByte: " + MBusRepeater.LastResponseMBusStatusByte.ToString("X2") + "h");
          }
        }
        catch (Exception ex)
        {
          MBusRepeater.MBusRepeaterLogger.Error(ex.Message);
        }
      }
      return frame;
    }

    public byte[] GetResultData(
      MBusFrame frame,
      int NumberOfBytes,
      ProgressHandler progress,
      CancellationToken token)
    {
      if (frame == null)
        throw new ArgumentNullException(nameof (frame));
      if (!this.Port.IsOpen)
      {
        if (progress != null)
        {
          progress.Split(new double[2]{ 10.0, 90.0 });
          progress.Report("Port open...");
        }
        this.Port.Open();
      }
      int millisecondsDelay = 100;
      int num1 = 3;
      ConfigList readoutConfiguration = this.Port.GetReadoutConfiguration();
      if (readoutConfiguration != null)
      {
        if (readoutConfiguration.WaitBeforeRepeatTime > 0)
          millisecondsDelay = readoutConfiguration.WaitBeforeRepeatTime;
        if (readoutConfiguration.MaxRequestRepeat > 0)
          num1 = readoutConfiguration.MaxRequestRepeat;
      }
      List<Exception> exceptionList = new List<Exception>();
      for (int index = 0; index < num1; ++index)
      {
        token.ThrowIfCancellationRequested();
        try
        {
          if (index > 0)
            Task.Delay(millisecondsDelay, token);
          byte[] byteArray = frame.ToByteArray();
          MBusRepeater.MBusRepeaterLogger.Debug("REQUEST: " + BitConverter.ToString(byteArray));
          this.Port.Write(byteArray);
          byte[] resultData = this.Port.ReadHeader(NumberOfBytes);
          progress?.Report("OK");
          return resultData;
        }
        catch (TimeoutException ex)
        {
          exceptionList.Add((Exception) ex);
          this.Port.ForceWakeup();
        }
        catch (Exception ex)
        {
          exceptionList.Add(ex);
        }
        if (progress != null)
        {
          int num2 = 100 / (num1 - index);
          int num3 = 100 - num2;
          progress.Split(new double[2]
          {
            (double) num2,
            (double) num3
          });
          progress.Report(exceptionList[exceptionList.Count - 1].Message + " Retry: " + (index + 1).ToString());
        }
      }
      Exception lastException = exceptionList.Last<Exception>();
      if (exceptionList.All<Exception>((Func<Exception, bool>) (x => x.GetType() == lastException.GetType())))
        throw lastException;
      throw new AggregateException((IEnumerable<Exception>) exceptionList);
    }

    public async Task<MBusFrameCrypt> GetResultFrameAsync(
      MBusFrameCrypt frame,
      ProgressHandler progress,
      CancellationToken token)
    {
      MBusFrameCrypt resultFrameAsync = await Task.Run<MBusFrameCrypt>((Func<MBusFrameCrypt>) (() => this.GetResultFrame(frame, progress, token)), token);
      return resultFrameAsync;
    }

    public MBusFrameCrypt GetResultFrame(MBusFrameCrypt frame)
    {
      return this.GetResultFrame(frame, (ProgressHandler) null, CancellationToken.None);
    }

    public MBusFrameCrypt GetResultFrame(
      MBusFrameCrypt frame,
      ProgressHandler progress,
      CancellationToken token)
    {
      if (frame == null)
        throw new ArgumentNullException(nameof (frame));
      if (!this.Port.IsOpen)
      {
        if (progress != null)
        {
          progress.Split(new double[2]{ 10.0, 90.0 });
          progress.Report("Port open...");
        }
        this.Port.Open();
      }
      int millisecondsDelay = 500;
      int num1 = 3;
      ConfigList readoutConfiguration = this.Port.GetReadoutConfiguration();
      if (readoutConfiguration != null)
      {
        if (readoutConfiguration.WaitBeforeRepeatTime > 0)
          millisecondsDelay = readoutConfiguration.WaitBeforeRepeatTime;
        if (readoutConfiguration.MaxRequestRepeat > 0)
          num1 = readoutConfiguration.MaxRequestRepeat;
      }
      List<Exception> exceptionList = new List<Exception>();
      for (int index = 0; index < num1; ++index)
      {
        token.ThrowIfCancellationRequested();
        try
        {
          if (index > 0)
            Task.Delay(millisecondsDelay, token);
          byte[] byteArray = frame.ToByteArray();
          MBusRepeater.MBusRepeaterLogger.Debug("REQUEST: " + BitConverter.ToString(byteArray));
          this.Port.Write(byteArray);
          return this.ReadMBusFrameCrypt();
        }
        catch (TimeoutException ex)
        {
          exceptionList.Add((Exception) ex);
          this.Port.ForceWakeup();
        }
        catch (Exception ex)
        {
          exceptionList.Add(ex);
        }
        if (progress != null)
        {
          int num2 = 100 / (num1 - index);
          int num3 = 100 - num2;
          progress.Split(new double[2]
          {
            (double) num2,
            (double) num3
          });
          progress.Report(exceptionList[exceptionList.Count - 1].Message + " Retry: " + (index + 1).ToString());
        }
      }
      Exception lastException = exceptionList.Last<Exception>();
      if (exceptionList.All<Exception>((Func<Exception, bool>) (x => x.GetType() == lastException.GetType())))
        throw lastException;
      throw new AggregateException((IEnumerable<Exception>) exceptionList);
    }

    public void TransmitMBusFrame(MBusFrameCrypt frame)
    {
      if (frame == null)
        throw new ArgumentNullException(nameof (frame));
      if (!this.Port.IsOpen)
        this.Port.Open();
      int num1 = 100;
      int num2 = 3;
      ConfigList readoutConfiguration = this.Port.GetReadoutConfiguration();
      if (readoutConfiguration != null)
      {
        if (readoutConfiguration.WaitBeforeRepeatTime > 0)
          num1 = readoutConfiguration.WaitBeforeRepeatTime;
        if (readoutConfiguration.MaxRequestRepeat > 0)
          num2 = readoutConfiguration.MaxRequestRepeat;
      }
      List<Exception> exceptionList = new List<Exception>();
      byte[] byteArray = frame.ToByteArray();
      MBusRepeater.MBusRepeaterLogger.Debug("REQUEST: " + BitConverter.ToString(byteArray));
      this.Port.Write(byteArray);
    }

    public MBusFrameCrypt ReadMBusFrameCrypt(byte[] encryptKey = null)
    {
      byte[] numArray1 = this.Port.ReadHeader(1);
      if (numArray1[0] == (byte) 229)
        return MBusFrameCrypt.Parse(Direction.GatewayToDevice, new DateTime?(DateTime.Now), numArray1);
      if (numArray1[0] == (byte) 16)
      {
        byte[] src = this.Port.ReadEnd(4);
        byte[] numArray2 = new byte[5]
        {
          numArray1[0],
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0
        };
        Buffer.BlockCopy((Array) src, 0, (Array) numArray2, 1, src.Length);
        return (int) MBusUtil.CalculateChecksum(numArray2, 1, 2) == (int) numArray2[3] ? MBusFrameCrypt.Parse(Direction.GatewayToDevice, new DateTime?(DateTime.Now), numArray2) : throw new ChecksumException(numArray2);
      }
      byte[] second = this.Port.ReadEnd(3);
      if (second[2] != (byte) 104 || (int) second[0] != (int) second[1] || second[0] == (byte) 0)
      {
        byte[] array = ((IEnumerable<byte>) numArray1).Concat<byte>((IEnumerable<byte>) second).ToArray<byte>();
        throw new InvalidFrameException("Invalid M-Bus header. Expected 68 L L 68 , actual: " + Util.ByteArrayToHexString((IEnumerable<byte>) array), array);
      }
      byte[] src1 = this.Port.ReadEnd((int) second[0] + 2);
      byte[] numArray3 = new byte[4 + src1.Length];
      numArray3[0] = second[2];
      numArray3[1] = second[0];
      numArray3[2] = second[1];
      numArray3[3] = second[2];
      Buffer.BlockCopy((Array) src1, 0, (Array) numArray3, 4, src1.Length);
      if ((int) MBusUtil.CalculateChecksum(numArray3, 4, numArray3.Length - 2) != (int) numArray3[numArray3.Length - 2])
        throw new ChecksumException(numArray3);
      MBusFrameCrypt frame = MBusFrameCrypt.Parse(Direction.DeviceToGateway, new DateTime?(DateTime.Now), numArray3, encryptKey);
      if (frame != null && frame.IsVariableDataStructure)
      {
        try
        {
          VariableDataStructure variableDataStructure = VariableDataStructure.Parse(frame);
          if (variableDataStructure != null && variableDataStructure.Header != null)
          {
            MBusRepeater.LastResponseMBusStatusByte = variableDataStructure.Header.Status;
            MBusRepeater.MBusRepeaterLogger.Info("MBusRepeater.LastResponseMBusStatusByte: " + MBusRepeater.LastResponseMBusStatusByte.ToString("X2") + "h");
          }
        }
        catch (Exception ex)
        {
          MBusRepeater.MBusRepeaterLogger.Error(ex.Message);
        }
      }
      return frame;
    }

    public byte[] GetResultData(
      MBusFrameCrypt frame,
      int NumberOfBytes,
      ProgressHandler progress,
      CancellationToken token)
    {
      if (frame == null)
        throw new ArgumentNullException(nameof (frame));
      if (!this.Port.IsOpen)
      {
        if (progress != null)
        {
          progress.Split(new double[2]{ 10.0, 90.0 });
          progress.Report("Port open...");
        }
        this.Port.Open();
      }
      int millisecondsDelay = 100;
      int num1 = 3;
      ConfigList readoutConfiguration = this.Port.GetReadoutConfiguration();
      if (readoutConfiguration != null)
      {
        if (readoutConfiguration.WaitBeforeRepeatTime > 0)
          millisecondsDelay = readoutConfiguration.WaitBeforeRepeatTime;
        if (readoutConfiguration.MaxRequestRepeat > 0)
          num1 = readoutConfiguration.MaxRequestRepeat;
      }
      List<Exception> exceptionList = new List<Exception>();
      for (int index = 0; index < num1; ++index)
      {
        token.ThrowIfCancellationRequested();
        try
        {
          if (index > 0)
            Task.Delay(millisecondsDelay, token);
          byte[] byteArray = frame.ToByteArray();
          MBusRepeater.MBusRepeaterLogger.Debug("REQUEST: " + BitConverter.ToString(byteArray));
          this.Port.Write(byteArray);
          byte[] resultData = this.Port.ReadHeader(NumberOfBytes);
          progress?.Report("OK");
          return resultData;
        }
        catch (TimeoutException ex)
        {
          exceptionList.Add((Exception) ex);
          this.Port.ForceWakeup();
        }
        catch (Exception ex)
        {
          exceptionList.Add(ex);
        }
        if (progress != null)
        {
          int num2 = 100 / (num1 - index);
          int num3 = 100 - num2;
          progress.Split(new double[2]
          {
            (double) num2,
            (double) num3
          });
          progress.Report(exceptionList[exceptionList.Count - 1].Message + " Retry: " + (index + 1).ToString());
        }
      }
      Exception lastException = exceptionList.Last<Exception>();
      if (exceptionList.All<Exception>((Func<Exception, bool>) (x => x.GetType() == lastException.GetType())))
        throw lastException;
      throw new AggregateException((IEnumerable<Exception>) exceptionList);
    }
  }
}
