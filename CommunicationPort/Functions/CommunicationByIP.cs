// Decompiled with JetBrains decompiler
// Type: CommunicationPort.Functions.CommunicationByIP
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.MeterVPNServer;
using ZR_ClassLibrary;

#nullable disable
namespace CommunicationPort.Functions
{
  internal class CommunicationByIP : CommunicationBase, IDisposable
  {
    private static Logger logger = LogManager.GetLogger(nameof (CommunicationByIP));
    private const int METER_VPN_DATA_PORT = 2000;
    private TcpClient client;
    private NetworkStream stream;

    public override bool IsOpen => this.client != null && this.client.Connected;

    public override int BytesToRead => this.client == null ? 0 : this.client.Available;

    internal CommunicationByIP(ICommunicationFunctions comPortFunctions)
      : base(comPortFunctions)
    {
    }

    public override void Open()
    {
      if (this.IsOpen)
        return;
      int port = 2000;
      if (this.configList.Type == "Remote_VPN")
      {
        string comServerNameOrIP = this.configList.COMserver;
        if (string.IsNullOrEmpty(comServerNameOrIP))
          throw new Exception("COMServer parameter is not set!");
        string hostname;
        if (Util.IsIP(comServerNameOrIP))
          hostname = this.configList.COMserver;
        else
          hostname = (new List<COMserver>((IEnumerable<COMserver>) (MeterVPN.ReadListOfCOMserver() ?? throw new Exception("Can not access to the MeterVPN server!"))).Find((Predicate<COMserver>) (x => x.Cert.ToUpper() == comServerNameOrIP.ToUpper())) ?? throw new Exception("COMserver name '" + comServerNameOrIP + "' is not available on the MeterVPN server!")).IP;
        this.client = new TcpClient();
        this.client.NoDelay = true;
        this.client.Connect(hostname, port);
        this.SetNextTimeAfterOpen();
        this.stream = this.client.GetStream();
        if (this.client.Client.Available > 0)
        {
          byte[] numArray = new byte[this.client.Client.Available];
          this.client.Client.Receive(numArray);
          if (!Encoding.ASCII.GetString(numArray, 0, numArray.Length).StartsWith("Port already in use"))
            ;
        }
        CommunicationByIP.logger.Trace("Port open");
      }
      else
      {
        if (this.configList.Type == "Remote")
          throw new NotImplementedException(this.configList.Type);
        throw new Exception("Invalid Type for CommunicationByIP! " + this.configList.Type);
      }
    }

    public override void Close()
    {
      CommunicationByIP.logger.Trace("Close called...");
      if (this.client != null)
        this.client.Close();
      if (this.stream == null)
        return;
      this.stream.Close();
    }

    public override void Write(byte[] buffer) => this.Write(buffer, 0, buffer.Length);

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.Open();
      this.DiscardInBuffer();
      this.WaitToNextTransmitTime();
      DateTime endTimepoint = this.CalculateEndTimepoint(count);
      this.stream.Write(buffer, offset, count);
      this.WaitOf(endTimepoint);
      if (!CommunicationByIP.logger.IsTraceEnabled)
        return;
      CommunicationByIP.logger.Trace("Write: " + Util.ByteArrayToHexString(buffer, offset, count));
    }

    public override void WriteWithoutDiscardInputBuffer(byte[] data)
    {
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      int readHeaderTimeout = this.GetReadHeaderTimeout(count);
      byte[] src = this.ReadData(count, readHeaderTimeout);
      Buffer.BlockCopy((Array) src, 0, (Array) buffer, offset, src.Length);
      return src.Length;
    }

    public override byte[] ReadHeader(int count)
    {
      int readHeaderTimeout = this.GetReadHeaderTimeout(count);
      return this.ReadData(count, readHeaderTimeout);
    }

    public override byte[] ReadEnd(int count)
    {
      int readEndTimeout = this.GetReadEndTimeout(count);
      return this.ReadData(count, readEndTimeout);
    }

    public byte[] ReadData(int count, int timeout_ms)
    {
      this.stream.ReadTimeout = timeout_ms;
      byte[] numArray1 = new byte[count];
      int length = 0;
      try
      {
        while (length < count)
          length += this.stream.Read(numArray1, length, numArray1.Length - length);
      }
      catch (IOException ex)
      {
        if (!(ex.InnerException is SocketException innerException) || innerException.SocketErrorCode != SocketError.TimedOut)
          throw ex;
        byte[] numArray2 = length > 0 ? new byte[length] : throw new TimeoutException("Timeout " + timeout_ms.ToString() + "ms", (Exception) ex);
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, length);
        throw new MissingBytesTimeoutException("Timeout " + timeout_ms.ToString() + "ms. Missed " + (count - length).ToString() + " byte(s)", count, numArray2);
      }
      finally
      {
        this.SetNextTimeAfterRead();
      }
      if (CommunicationByIP.logger.IsTraceEnabled)
        CommunicationByIP.logger.Trace("Read: " + Util.ByteArrayToHexString(numArray1));
      return numArray1;
    }

    public override void DiscardCurrentInBuffer()
    {
    }

    public override bool DiscardInBuffer()
    {
      bool flag = false;
      if (this.stream != null && this.stream.CanRead && this.stream.DataAvailable)
      {
        List<byte> byteList = new List<byte>();
        while (this.stream.DataAvailable)
          byteList.Add((byte) this.stream.ReadByte());
        CommunicationByIP.logger.Error("DiscardInBuffer: " + Util.ByteArrayToHexString(byteList.ToArray()));
      }
      return flag;
    }

    public override void Dispose() => base.Dispose();
  }
}
