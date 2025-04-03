// Decompiled with JetBrains decompiler
// Type: AsyncCom.AsyncIP
// Assembly: AsyncCom, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: D6F4F79A-8F4B-4BF8-A607-52E7B777C135
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AsyncCom.dll

using AsyncCom.MeterVPNServer;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace AsyncCom
{
  public sealed class AsyncIP(AsyncFunctions MyParent) : AsyncFunctionsBase(MyParent)
  {
    private const int DATA_PORT = 2000;
    private const int CONTROL_PORT = 3000;
    private static Logger logger = LogManager.GetLogger(nameof (AsyncIP));
    private TcpClient controlTcpClient;
    private NetworkStream controlNetworkStream;
    private TcpClient dataTcpClient;
    private NetworkStream dataNetworkStream;
    private MeterVPNService MyMeterVPNServer;

    public override long InputBufferLength
    {
      get
      {
        try
        {
          return this.dataNetworkStream.Length;
        }
        catch (NotSupportedException ex)
        {
          return 0;
        }
      }
    }

    public override bool Open()
    {
      if (this.OpenDataConnection())
        return true;
      if (this.MyAsyncFunctions.ErrorMessageBox)
        this.MyAsyncFunctions.AsyncComMessageBox(this.GetResString("TCPOpenError") + "\r\n" + ZR_ClassLibMessages.GetLastErrorStringTranslated());
      return false;
    }

    private string GetResString(string name) => this.MyAsyncFunctions.MyRes.GetString(name);

    public override bool Close()
    {
      AsyncIP.logger.Trace("Close called...");
      this.MyAsyncFunctions.ComIsOpen = false;
      this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComClose);
      if (this.controlTcpClient != null)
        this.controlTcpClient.Close();
      if (this.controlNetworkStream != null)
        this.controlNetworkStream.Close();
      if (this.dataTcpClient != null)
        this.dataTcpClient.Close();
      if (this.dataNetworkStream != null)
        this.dataNetworkStream.Close();
      if (this.MyAsyncFunctions.ComWindow != null)
        this.MyAsyncFunctions.ComWindow.SetComOpenState();
      return true;
    }

    public override void ClearCom()
    {
      if (this.dataNetworkStream != null && this.dataNetworkStream.CanRead && this.dataNetworkStream.DataAvailable)
      {
        List<byte> byteList = new List<byte>();
        while (this.dataNetworkStream.DataAvailable && !this.MyAsyncFunctions.BreakRequest)
          byteList.Add((byte) this.dataNetworkStream.ReadByte());
        AsyncIP.logger.Error("DATA-channel has disposed following bytes: " + Util.ByteArrayToHexString(byteList.ToArray()));
      }
      if (this.controlNetworkStream == null || !this.controlNetworkStream.CanRead || !this.controlNetworkStream.DataAvailable)
        return;
      List<byte> byteList1 = new List<byte>();
      while (this.controlNetworkStream.DataAvailable && !this.MyAsyncFunctions.BreakRequest)
        byteList1.Add((byte) this.controlNetworkStream.ReadByte());
      AsyncIP.logger.Error("CONTROL-channel has disposed following bytes: " + Util.ByteArrayToHexString(byteList1.ToArray()));
    }

    private bool OpenControlConnection()
    {
      if (string.IsNullOrEmpty(this.MyAsyncFunctions.MyMeterVPN.SelectedCOMserver))
        return false;
      if (this.controlTcpClient != null && this.controlTcpClient.Client != null && this.controlTcpClient.Client.Connected)
      {
        if (this.controlNetworkStream == null)
        {
          this.controlNetworkStream = this.controlTcpClient.GetStream();
          this.controlNetworkStream.WriteTimeout = 500;
          this.controlNetworkStream.ReadTimeout = 500;
        }
        return true;
      }
      COMserver comServer = this.GetCOMServer();
      AsyncIP.logger.Info<string, int>("Try connect to the COMServer CONTROL ({0}:{1})", comServer.IPAddress, 3000);
      try
      {
        if (this.controlTcpClient != null)
          this.controlTcpClient.Close();
        if (this.controlNetworkStream != null)
          this.controlNetworkStream.Close();
        ZR_ClassLibMessages.ClearErrors();
        this.controlTcpClient = new TcpClient();
        this.controlTcpClient.NoDelay = true;
        this.controlNetworkStream = (NetworkStream) null;
        this.controlTcpClient.ReceiveTimeout = this.MyAsyncFunctions.RecTime_BeforFirstByte;
        try
        {
          this.controlTcpClient.Connect(comServer.IPAddress, 3000);
        }
        catch (Exception ex)
        {
          string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
          AsyncIP.logger.Error(ex, str);
          this.Close();
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
          return false;
        }
        if (!Util.Wait(3000L, "after create TCP/IP connection", (ICancelable) this.MyAsyncFunctions, AsyncIP.logger))
          return false;
        if (this.controlTcpClient.Client.Available > 0)
        {
          byte[] numArray = new byte[this.controlTcpClient.Client.Available];
          this.controlTcpClient.Client.Receive(numArray);
          string str1 = Encoding.ASCII.GetString(numArray, 0, numArray.Length);
          if (str1.StartsWith("Too many controller ports"))
          {
            string str2 = string.Format("{0}! Please try again later!", (object) str1);
            AsyncIP.logger.Error(str2);
            this.Close();
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, str2);
            return false;
          }
          AsyncIP.logger.Info("Message from COM Server(CONTROL) Buffer: {0}", Util.ByteArrayToHexString(numArray));
        }
        this.controlNetworkStream = this.controlTcpClient.GetStream();
        this.controlNetworkStream.WriteTimeout = 5000;
        this.controlNetworkStream.ReadTimeout = 5000;
        AsyncIP.logger.Info("Successfully connected!");
        return true;
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        AsyncIP.logger.Error(ex, message);
        this.Close();
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Failed estabilish the CONTROL connection with COMServer. " + message);
        return false;
      }
    }

    public override bool TransmitControlCommand(string cmd)
    {
      if (!this.OpenControlConnection())
        return false;
      AsyncIP.logger.Trace("Send control command: {0}", cmd);
      try
      {
        byte[] bytes = Encoding.ASCII.GetBytes(cmd + "\n\r");
        if (!this.controlNetworkStream.CanWrite)
          return false;
        this.controlNetworkStream.Write(bytes, 0, bytes.Length);
        this.controlNetworkStream.Flush();
        return true;
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        AsyncIP.logger.Error(ex, str);
        this.Close();
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        return false;
      }
    }

    public override bool ReceiveControlBlock(
      out string ReceivedData,
      string startTag,
      string endTag)
    {
      ReceivedData = string.Empty;
      if (!this.OpenControlConnection())
        return false;
      StringBuilder stringBuilder = new StringBuilder();
      int tickCount = Environment.TickCount;
      while (this.controlNetworkStream.CanRead)
      {
        if (Environment.TickCount > tickCount + this.controlNetworkStream.ReadTimeout)
        {
          AsyncIP.logger.Trace("Timeout. " + this.controlNetworkStream.ReadTimeout.ToString());
          break;
        }
        if (this.controlNetworkStream.DataAvailable)
        {
          byte[] numArray = new byte[this.controlTcpClient.ReceiveBufferSize];
          try
          {
            int count = this.controlNetworkStream.Read(numArray, 0, numArray.Length);
            string str = Encoding.ASCII.GetString(numArray, 0, count);
            stringBuilder.Append(str);
            if (str.IndexOf(startTag) >= 0 && str.IndexOf(endTag) > 0)
              break;
          }
          catch (Exception ex)
          {
            string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
            AsyncIP.logger.Error(ex, str);
            this.Close();
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
            return false;
          }
          tickCount = Environment.TickCount;
        }
        else if (!Util.Wait(100L, "while receive input buffer.", (ICancelable) this.MyAsyncFunctions, AsyncIP.logger))
          return false;
      }
      ReceivedData = stringBuilder.ToString();
      AsyncIP.logger.Trace<int, string>("Received CONTROL block. Size: {0}, Buffer: {1}", ReceivedData.Length, ReceivedData);
      return true;
    }

    private bool OpenDataConnection()
    {
      this.MyAsyncFunctions.ComIsOpen = false;
      COMserver comServer = this.GetCOMServer();
      if (comServer == null)
        return false;
      if (this.dataTcpClient != null && this.dataTcpClient.Client != null && this.dataTcpClient.Client.Connected)
      {
        string ipAddress = comServer.IPAddress;
        if (!(((IPEndPoint) this.dataTcpClient.Client.RemoteEndPoint).Address.ToString() != ipAddress))
        {
          if (this.dataNetworkStream == null)
          {
            this.dataNetworkStream = this.controlTcpClient.GetStream();
            this.dataNetworkStream.WriteTimeout = 5000;
            this.dataNetworkStream.ReadTimeout = 5000;
          }
          this.MyAsyncFunctions.ComIsOpen = true;
          return true;
        }
      }
      int port = 2000;
      if (this.MyAsyncFunctions.ComPort.ToUpper() == "COM2")
        port = 2003;
      AsyncIP.logger.Info<string, int>("Try connect to the COMServer DATA ({0}:{1})", comServer.IPAddress, port);
      try
      {
        if (this.dataTcpClient != null)
          this.dataTcpClient.Close();
        if (this.dataNetworkStream != null)
          this.dataNetworkStream.Close();
        ZR_ClassLibMessages.ClearErrors();
        this.dataTcpClient = new TcpClient();
        this.dataTcpClient.NoDelay = true;
        this.dataNetworkStream = (NetworkStream) null;
        int timeBeforFirstByte = this.MyAsyncFunctions.RecTime_BeforFirstByte;
        try
        {
          this.dataTcpClient.Connect(comServer.IPAddress, port);
        }
        catch (Exception ex)
        {
          string str = string.Format("Error: {0}", (object) ex.Message);
          AsyncIP.logger.Error(ex, str);
          this.Close();
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, str);
          return false;
        }
        if (!Util.Wait(3000L, "after create TCP/IP connection", (ICancelable) this.MyAsyncFunctions, AsyncIP.logger))
          return false;
        if (this.dataTcpClient.Client == null)
        {
          this.dataTcpClient.Close();
          return false;
        }
        if (this.dataTcpClient.Client.Available > 0)
        {
          byte[] numArray = new byte[this.dataTcpClient.Client.Available];
          int num = this.dataTcpClient.Client.Receive(numArray);
          string str1 = Encoding.ASCII.GetString(numArray, 0, numArray.Length);
          if (str1.StartsWith("Port already in use"))
          {
            if (comServer.Update(this) && comServer.RemoteComs.Count > 0)
            {
              string connectedTo = ((RemoteCom) comServer.RemoteComs[(object) 1]).ConnectedTo;
              string str2 = string.Format("{0} from: {1}", (object) str1, (object) connectedTo);
              AsyncIP.logger.Error(str2);
              try
              {
                string str3 = "?";
                foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                  if (address.AddressFamily == AddressFamily.InterNetwork && address.ToString().StartsWith("1."))
                  {
                    str3 = address.ToString();
                    break;
                  }
                }
                AsyncIP.logger.Trace("My OpenVPN IP: " + str3);
                if (str3 == connectedTo)
                {
                  AsyncIP.logger.Trace("I have last used this COM Server! Try resolve this problem...");
                  this.TransmitControlCommand("disconnect " + 2000.ToString());
                  if (this.ReceiveControlBlock(out string _, "disconnect", "->"))
                    str2 += " Please try again!";
                }
              }
              catch (Exception ex)
              {
                string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
                AsyncIP.logger.Error(ex, message);
              }
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, str2);
            }
            this.Close();
            return false;
          }
          AsyncIP.logger.Info<int, string>("Message from COM Server(DATA): Size:{0}, Buffer:{1}", num, Util.ByteArrayToHexString(numArray));
        }
        this.dataNetworkStream = this.dataTcpClient.GetStream();
        this.dataNetworkStream.WriteTimeout = 5000;
        this.dataNetworkStream.ReadTimeout = 5000;
        this.MyAsyncFunctions.ComIsOpen = true;
        AsyncIP.logger.Info("Successfully connected!");
        this.MyAsyncFunctions.SendAsyncComMessage(new GMM_EventArgs(GMM_EventArgs.MessageType.StatusChanged)
        {
          EventMessage = "Com opened"
        });
        return true;
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        AsyncIP.logger.Error(ex, message);
        this.Close();
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, "Failed estabilish the DATA connection with COMServer. " + message);
        return false;
      }
    }

    public override bool TransmitBlock(string DataString)
    {
      ByteField DataBlock = new ByteField(DataString.Length);
      for (int index = 0; index < DataString.Length; ++index)
        DataBlock.Add((byte) DataString[index]);
      return this.TransmitBlock(ref DataBlock);
    }

    public override bool TransmitBlock(ref ByteField DataBlock)
    {
      if (DataBlock == null || DataBlock.Count < 1)
        throw new ArgumentException(nameof (DataBlock));
      if (!this.OpenDataConnection())
        return false;
      this.MyAsyncFunctions.WaitToEarliestTransmitTime();
      this.ClearCom();
      this.MyAsyncFunctions.LastTransmitEndTime = SystemValues.DateTimeNow.AddMilliseconds(this.MyAsyncFunctions.ByteTime * (double) DataBlock.Count);
      this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComTransmitData, ref DataBlock);
      byte[] numArray = new byte[DataBlock.Count];
      Array.Copy((Array) DataBlock.Data, (Array) numArray, numArray.Length);
      try
      {
        if (!this.dataNetworkStream.CanWrite)
          return false;
        this.dataNetworkStream.Write(numArray, 0, numArray.Length);
        this.dataNetworkStream.Flush();
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        AsyncIP.logger.Error(ex, str);
        this.Close();
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        return false;
      }
      if (this.MyAsyncFunctions.EchoOn)
      {
        ByteField byteField = new ByteField(DataBlock.Count);
        bool flag = this.ReceiveBlock(ref byteField, DataBlock.Count, true);
        if (flag)
        {
          for (int index = 0; index < DataBlock.Count; ++index)
          {
            if ((int) DataBlock.Data[index] != (int) byteField.Data[index])
              flag = false;
          }
        }
        if (!flag)
        {
          this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComEchoError, ref byteField);
          return false;
        }
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComEchoOk);
      }
      return true;
    }

    public override bool ReceiveBlock(ref ByteField DataBlock)
    {
      DataBlock.Count = 0;
      if (!this.OpenDataConnection())
        return false;
      int timeBeforFirstByte = this.MyAsyncFunctions.RecTime_BeforFirstByte;
      List<byte> byteList = new List<byte>();
      int tickCount = Environment.TickCount;
      while (this.dataNetworkStream.CanRead && !this.MyAsyncFunctions.BreakRequest)
      {
        if (Environment.TickCount > tickCount + timeBeforFirstByte)
        {
          AsyncIP.logger.Trace("Timeout while receive DATA block!");
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout);
          break;
        }
        if (this.dataNetworkStream.DataAvailable)
        {
          try
          {
            byte num = (byte) this.dataNetworkStream.ReadByte();
            byteList.Add(num);
          }
          catch (ObjectDisposedException ex)
          {
            string str = "The connection was closed unexpectedly! Error: " + ex.Message;
            AsyncIP.logger.Error((Exception) ex, str);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
            return false;
          }
          tickCount = Environment.TickCount;
        }
        else if (!Util.Wait(200L, "while receive input buffer.", (ICancelable) this.MyAsyncFunctions, AsyncIP.logger))
          return false;
      }
      byte[] array = byteList.ToArray();
      DataBlock.Data = array;
      DataBlock.Count = array.Length;
      this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComReceiveData, ref DataBlock);
      this.MyAsyncFunctions.EarliestTransmitTime = SystemValues.DateTimeNow.AddMilliseconds((double) this.MyAsyncFunctions.RecTransTime);
      return DataBlock.Count != 0;
    }

    public override bool ReceiveBlock(ref ByteField DataBlock, int MinByteNb, bool first)
    {
      if (MinByteNb <= 0)
        throw new ArgumentException(nameof (MinByteNb));
      DataBlock.Count = 0;
      if (!this.OpenDataConnection())
        return false;
      int ActualTimeout;
      this.GetReceiveBlockTiming(MinByteNb, first, out DateTime _, out ActualTimeout);
      AsyncIP.logger.Trace<int, bool, int>("Calculated timing (MinByteNb: {0}, first: {1}, Timeout {2})", MinByteNb, first, ActualTimeout);
      List<byte> byteList = new List<byte>();
      int tickCount = Environment.TickCount;
      while (byteList.Count < MinByteNb && !this.MyAsyncFunctions.BreakRequest && this.dataNetworkStream.CanRead)
      {
        if (Environment.TickCount > tickCount + ActualTimeout)
        {
          AsyncIP.logger.Trace("Timeout while receive DATA block!");
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout);
          break;
        }
        if (this.dataNetworkStream.DataAvailable)
        {
          try
          {
            byte num = (byte) this.dataNetworkStream.ReadByte();
            byteList.Add(num);
          }
          catch (ObjectDisposedException ex)
          {
            string str = "The connection was closed unexpectedly! Error: " + ex.Message;
            AsyncIP.logger.Error((Exception) ex, str);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
            return false;
          }
          tickCount = Environment.TickCount;
        }
        else if (!Util.Wait(200L, "while receive input buffer.", (ICancelable) this.MyAsyncFunctions, AsyncIP.logger))
          return false;
      }
      byte[] array = byteList.ToArray();
      string hexString = Util.ByteArrayToHexString(array);
      DataBlock.Data = array;
      DataBlock.Count = array.Length;
      this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComReceiveData, ref DataBlock);
      this.MyAsyncFunctions.ResetEarliestTransmitTime();
      if (array.Length == MinByteNb)
      {
        ZR_ClassLibMessages.ClearErrors();
        return true;
      }
      if (array.Length != 0)
      {
        string str = string.Format("Received unexpected DATA block. Is it perhaps the collision? Size: {0}, Buffer: {1}", (object) array.Length, (object) hexString);
        AsyncIP.logger.Error(str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
      }
      return false;
    }

    public override bool GetCurrentInputBuffer(out byte[] buffer)
    {
      buffer = (byte[]) null;
      if (!this.OpenDataConnection())
        return false;
      int timeBeforFirstByte = this.MyAsyncFunctions.RecTime_BeforFirstByte;
      List<byte> byteList = new List<byte>();
      int tickCount = Environment.TickCount;
      while (this.dataNetworkStream.CanRead && !this.MyAsyncFunctions.BreakRequest && Environment.TickCount <= tickCount + timeBeforFirstByte)
      {
        if (this.dataNetworkStream.DataAvailable)
        {
          try
          {
            while (this.dataNetworkStream.DataAvailable && !this.MyAsyncFunctions.BreakRequest)
              byteList.Add((byte) this.dataNetworkStream.ReadByte());
          }
          catch (ObjectDisposedException ex)
          {
            string str = "The connection was closed unexpectedly! Error: " + ex.Message;
            AsyncIP.logger.Error((Exception) ex, str);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
            return false;
          }
          tickCount = Environment.TickCount;
        }
        else if (!Util.Wait(500L, "while receive input buffer.", (ICancelable) this.MyAsyncFunctions, AsyncIP.logger))
          return false;
      }
      buffer = byteList.ToArray();
      this.MyAsyncFunctions.EarliestTransmitTime = SystemValues.DateTimeNow.AddMilliseconds((double) this.MyAsyncFunctions.RecTransTime);
      return true;
    }

    public override bool SendBlock(ref ByteField DataBlock) => this.TransmitBlock(ref DataBlock);

    public override void PureTransmit(byte[] byteList) => throw new NotImplementedException();

    public override bool ReceiveLine(out string ReceivedData)
    {
      char[] EndCharacters = new char[2]{ '\r', '\n' };
      return this.ReceiveLine(out ReceivedData, EndCharacters, false);
    }

    public override bool ReceiveCRLF_Line(out string ReceivedData)
    {
      char[] EndCharacters = new char[2]{ '\r', '\n' };
      return this.ReceiveLine(out ReceivedData, EndCharacters, true);
    }

    public override bool ReceiveLine(
      out string ReceivedData,
      char[] EndCharacters,
      bool GetEmpty_CRLF_Line)
    {
      throw new NotImplementedException();
    }

    public override bool ReceiveBlockToChar(ref ByteField DataBlock, byte EndChar)
    {
      if (!this.MyAsyncFunctions.ComIsOpen)
        return false;
      ByteField byteField = new ByteField(DataBlock.Count);
      int count = DataBlock.Count;
      DataBlock.Count = 0;
      bool flag = false;
      int Timeout = this.MyAsyncFunctions.RecTime_BeforFirstByte + this.MyAsyncFunctions.AnswerOffsetTime;
      this.MyAsyncFunctions.Logger.GetTimeTicks();
      do
      {
        this.dataNetworkStream.ReadTimeout = Timeout;
        this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiverPoll);
        try
        {
          int num = this.ReadStream(DataBlock.Data, DataBlock.Count, 1, this.dataNetworkStream, Timeout);
          count -= num;
          DataBlock.Count += num;
          this.MyAsyncFunctions.Logger.WriteLoggerData(EventLogger.LoggerEvent.ComReceiveData, ref DataBlock);
          if ((int) DataBlock.Data[DataBlock.Count - 1] == (int) EndChar)
          {
            this.MyAsyncFunctions.EarliestTransmitTime = SystemValues.DateTimeNow.AddMilliseconds((double) this.MyAsyncFunctions.RecTransTime);
            return true;
          }
        }
        catch (Exception ex)
        {
          flag = true;
          this.WorkException(ex);
        }
      }
      while (count > 0 && !flag);
      return false;
    }

    private int ReadStream(
      byte[] buffer,
      int Position,
      int Count,
      NetworkStream Stream,
      int Timeout)
    {
      int num1 = 0;
      long timeTicks = this.MyAsyncFunctions.Logger.GetTimeTicks();
      while (num1 < Count)
      {
        if (this.MyAsyncFunctions.Logger.GetTimeDifferenc(this.MyAsyncFunctions.Logger.GetTimeTicks() - timeTicks) > (long) Timeout)
          return num1 > 0 ? num1 : throw new TimeoutException();
        if (Stream.DataAvailable)
        {
          int num2 = Stream.Read(buffer, Position, Count);
          Position += num2;
          Count -= num2;
          num1 += num2;
        }
      }
      return num1;
    }

    public bool GetCOMServersFromMeterVPN(out AsyncCom.MeterVPNServer.COMserver[] COMservers)
    {
      AsyncIP.logger.Trace("Call GetCOMServersFromMeterVPN");
      COMservers = (AsyncCom.MeterVPNServer.COMserver[]) null;
      try
      {
        AsyncIP.logger.Trace("Try create MeterVPNService");
        if (this.MyMeterVPNServer == null)
          this.MyMeterVPNServer = new MeterVPNService();
        if (this.MyAsyncFunctions.ConnectionTypeSelected == AsyncComConnectionType.Remote)
        {
          AsyncIP.logger.Trace("MyAsyncFunctions.ConnectionTypeSelected == AsyncComConnectionType.Remote");
          AsyncIP.LANVPN lanvpn = new AsyncIP.LANVPN();
          COMservers = lanvpn.GetLocalCOMservers();
        }
        else
        {
          using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
          {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("1.0.0.1"), 80);
            AsyncIP.logger.Trace("Load COMServer list from 1.0.0.1:80");
            if (!socket.BeginConnect((EndPoint) remoteEP, (AsyncCallback) null, (object) null).AsyncWaitHandle.WaitOne(this.MyAsyncFunctions.RecTime_BeforFirstByte, false))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, "Timeout in GetCOMServersFromMeterVPN (...)");
              return false;
            }
          }
          this.MyMeterVPNServer.Proxy = GlobalProxySelection.GetEmptyWebProxy();
          COMservers = this.MyMeterVPNServer.GetCOMservers("test");
          if (COMservers != null)
          {
            foreach (AsyncCom.MeterVPNServer.COMserver coMserver in COMservers)
              AsyncIP.logger.Trace("{0}, IP: {1}, Name: {2}, Online: {3}", new object[4]
              {
                (object) coMserver.Cert,
                (object) coMserver.IP,
                (object) coMserver.Name,
                (object) coMserver.Online
              });
          }
        }
        if (COMservers == null)
          return false;
      }
      catch (WebException ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        AsyncIP.logger.Error((Exception) ex, message);
        if (this.MyAsyncFunctions.ErrorMessageBox)
          this.MyAsyncFunctions.AsyncComMessageBox(this.GetResString("NetworkProxyError") + "\r\n" + ZR_ClassLibMessages.GetLastErrorStringTranslated());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Proxy error. " + message);
        COMservers = (AsyncCom.MeterVPNServer.COMserver[]) null;
        return false;
      }
      catch (Exception ex)
      {
        string str = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        AsyncIP.logger.Error(ex, str);
        if (this.MyAsyncFunctions.ErrorMessageBox)
          this.MyAsyncFunctions.AsyncComMessageBox(this.GetResString("TCPOpenError") + "\r\n" + ZR_ClassLibMessages.GetLastErrorStringTranslated());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        COMservers = (AsyncCom.MeterVPNServer.COMserver[]) null;
        return false;
      }
      return true;
    }

    public bool AddCOMserverToCustomer(string Cert, string Name, string Password)
    {
      try
      {
        if (this.MyMeterVPNServer == null)
          this.MyMeterVPNServer = new MeterVPNService();
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
          IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("1.0.0.1"), 80);
          if (!socket.BeginConnect((EndPoint) remoteEP, (AsyncCallback) null, (object) null).AsyncWaitHandle.WaitOne(this.MyAsyncFunctions.RecTime_BeforFirstByte, false))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, "Timeout in AddCOMserverToCustomer(...)");
            throw new ApplicationException("Failed to connect server.");
          }
        }
        return this.MyMeterVPNServer.AddCOMserverToCustomer(Cert, Name, Password) == "ok";
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        AsyncIP.logger.Error(ex, message);
        if (this.MyAsyncFunctions.ErrorMessageBox)
          this.MyAsyncFunctions.AsyncComMessageBox(this.GetResString("TCPOpenError") + "\r\n" + ZR_ClassLibMessages.GetLastErrorStringTranslated());
        return false;
      }
    }

    public bool DelCOMserverFromCustomer(string Cert)
    {
      try
      {
        if (this.MyMeterVPNServer == null)
          this.MyMeterVPNServer = new MeterVPNService();
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("1.0.0.1"), 80);
        if (!socket.BeginConnect((EndPoint) remoteEP, (AsyncCallback) null, (object) null).AsyncWaitHandle.WaitOne(this.MyAsyncFunctions.RecTime_BeforFirstByte, false))
        {
          socket.Close();
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, "Timeout in DelCOMserverFromCustomer(...).");
          throw new ApplicationException("Failed to connect server.");
        }
        socket.Close();
        return this.MyMeterVPNServer.DelCOMserverFromCustomer(Cert) == "ok";
      }
      catch
      {
        if (this.MyAsyncFunctions.ErrorMessageBox)
          this.MyAsyncFunctions.AsyncComMessageBox(this.GetResString("TCPOpenError") + "\r\n" + ZR_ClassLibMessages.GetLastErrorStringTranslated());
        return false;
      }
    }

    public bool ModCOMserver(string Cert, string Name)
    {
      try
      {
        if (this.MyMeterVPNServer == null)
          this.MyMeterVPNServer = new MeterVPNService();
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("1.0.0.1"), 80);
        if (!socket.BeginConnect((EndPoint) remoteEP, (AsyncCallback) null, (object) null).AsyncWaitHandle.WaitOne(this.MyAsyncFunctions.RecTime_BeforFirstByte, false))
        {
          socket.Close();
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, "Timeout in ModCOMserver(...).");
          throw new ApplicationException("Failed to connect server.");
        }
        socket.Close();
        return this.MyMeterVPNServer.ModCOMserver(Cert, Name) == "ok";
      }
      catch
      {
        if (this.MyAsyncFunctions.ErrorMessageBox)
          this.MyAsyncFunctions.AsyncComMessageBox(this.GetResString("TCPOpenError") + "\r\n" + ZR_ClassLibMessages.GetLastErrorStringTranslated());
        return false;
      }
    }

    private bool WorkException(Exception Ex)
    {
      switch (Ex)
      {
        case TimeoutException _:
          this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComReceiveTimeout);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, "Timeout.");
          return false;
        case IOException _:
          this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComIOException);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "IOException.");
          break;
        default:
          this.MyAsyncFunctions.Logger.WriteLoggerEvent(EventLogger.LoggerEvent.ComUnknownError);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Unknown error.");
          break;
      }
      this.MyAsyncFunctions.ComIsOpen = false;
      return false;
    }

    private COMserver GetCOMServer()
    {
      string selectedCoMserver = this.MyAsyncFunctions.MyMeterVPN.SelectedCOMserver;
      if (string.IsNullOrEmpty(selectedCoMserver))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, "Remote end point is not set!");
        return (COMserver) null;
      }
      if (Util.IsIP(selectedCoMserver))
        return new COMserver()
        {
          IPAddress = selectedCoMserver,
          Name = selectedCoMserver,
          Online = true,
          LastSeen = SystemValues.DateTimeNow.ToString()
        };
      if (!this.MyAsyncFunctions.MyMeterVPN.COMservers.ContainsKey((object) selectedCoMserver) && !this.MyAsyncFunctions.MyMeterVPN.Update(this))
        return (COMserver) null;
      if (this.MyAsyncFunctions.MyMeterVPN.COMservers.ContainsKey((object) selectedCoMserver))
        return (COMserver) this.MyAsyncFunctions.MyMeterVPN.COMservers[(object) selectedCoMserver];
      string str = selectedCoMserver + " is a unknown COM server!";
      AsyncIP.logger.Error(str);
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.ComOpenError, str);
      return (COMserver) null;
    }

    public override void ClearComErrors()
    {
    }

    public override bool ClearBreak() => false;

    public override bool SetBreak() => false;

    public override void TestComState()
    {
    }

    public override bool SetHandshakeState(HandshakeStates HandshakeState) => false;

    public override bool CallTransceiverDeviceFunction(
      TransceiverDeviceFunction function,
      object param1,
      object param2)
    {
      return true;
    }

    public override bool TryReceiveBlock(out byte[] buffer)
    {
      buffer = (byte[]) null;
      if (this.dataNetworkStream == null || !this.dataNetworkStream.CanRead || !this.dataNetworkStream.DataAvailable)
        return false;
      List<byte> byteList = new List<byte>();
      while (this.dataNetworkStream.DataAvailable)
      {
        byte[] numArray1 = new byte[1024];
        int length = this.dataNetworkStream.Read(numArray1, 0, numArray1.Length);
        if (length > 0)
        {
          byte[] numArray2 = new byte[length];
          Array.Copy((Array) numArray1, (Array) numArray2, numArray2.Length);
          byteList.AddRange((IEnumerable<byte>) numArray2);
          if (AsyncIP.logger.IsTraceEnabled)
            AsyncIP.logger.Trace("Data available: " + Util.ByteArrayToHexString(numArray2));
        }
        if (!Util.Wait(100L, nameof (TryReceiveBlock), (ICancelable) this.MyAsyncFunctions, AsyncIP.logger))
          return false;
      }
      buffer = byteList.ToArray();
      return true;
    }

    public override bool TryReceiveBlock(out byte[] buffer, int numberOfBytesToReceive)
    {
      throw new NotImplementedException();
    }

    public override object GetChannel() => (object) null;

    internal class LANVPN
    {
      private Socket UDPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      private string Target_IP = "224.0.0.1";
      private int Target_Port = 9000;
      private int Listen_Port = 9050;
      private SortedList MessageQueue = new SortedList();
      private SortedList MyCOMservers = new SortedList();
      private string LastCommand = "";

      internal AsyncCom.MeterVPNServer.COMserver[] GetLocalCOMservers()
      {
        try
        {
          this.InitMulticastSocket();
          this.Send("*", AsyncIP.LANVPN.COMserverCommands.GetIPConfig, "");
          Thread.Sleep(1000);
          this.ParseAvailableMessages();
        }
        catch
        {
          return new AsyncCom.MeterVPNServer.COMserver[0];
        }
        AsyncCom.MeterVPNServer.COMserver[] localCoMservers = new AsyncCom.MeterVPNServer.COMserver[this.MyCOMservers.Count];
        int index = 0;
        foreach (DictionaryEntry coMserver1 in this.MyCOMservers)
        {
          AsyncIP.LANVPN.COMserver coMserver2 = (AsyncIP.LANVPN.COMserver) coMserver1.Value;
          localCoMservers[index] = new AsyncCom.MeterVPNServer.COMserver();
          localCoMservers[index].Name = coMserver2.Name;
          localCoMservers[index].Cert = coMserver2.Name;
          localCoMservers[index].IP = coMserver2.IP;
          localCoMservers[index].LastSeen = SystemValues.DateTimeNow.ToString();
          localCoMservers[index].Traffic = "";
          localCoMservers[index].Online = true;
          ++index;
        }
        return localCoMservers;
      }

      private bool InitMulticastSocket()
      {
        try
        {
          IPEndPoint localEP = new IPEndPoint(IPAddress.Any, this.Listen_Port);
          this.UDPSocket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.Debug, 1);
          this.UDPSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
          this.UDPSocket.Bind((EndPoint) localEP);
          this.UDPSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);
          this.UDPSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, (object) new MulticastOption(IPAddress.Parse(this.Target_IP), IPAddress.Any));
          this.Receive();
        }
        catch
        {
          return false;
        }
        return true;
      }

      private void Send(
        string COMserver,
        AsyncIP.LANVPN.COMserverCommands Command,
        string Parameter)
      {
        Parameter += ",";
        byte[] bytes = Encoding.ASCII.GetBytes(COMserver + " " + Command.ToString() + " " + Parameter);
        this.LastCommand = Command.ToString();
        EndPoint remoteEP = (EndPoint) new IPEndPoint(IPAddress.Parse(this.Target_IP), this.Target_Port);
        this.UDPSocket.BeginSendTo(bytes, 0, bytes.Length, SocketFlags.None, remoteEP, new AsyncCallback(this.SendCallback), (object) this.UDPSocket);
      }

      private void SendCallback(IAsyncResult ar)
      {
        try
        {
          ((Socket) ar.AsyncState).EndSendTo(ar);
        }
        catch
        {
        }
      }

      private void Receive()
      {
        try
        {
          EndPoint remoteEP = (EndPoint) new IPEndPoint(IPAddress.Any, this.Listen_Port);
          AsyncIP.LANVPN.StateObject state = new AsyncIP.LANVPN.StateObject();
          state.workSocket = this.UDPSocket;
          this.UDPSocket.BeginReceiveFrom(state.buffer, 0, 1024, SocketFlags.None, ref remoteEP, new AsyncCallback(this.ReceiveCallback), (object) state);
        }
        catch
        {
        }
      }

      private void ReceiveCallback(IAsyncResult ar)
      {
        try
        {
          EndPoint endPoint = (EndPoint) new IPEndPoint(IPAddress.Any, this.Listen_Port);
          AsyncIP.LANVPN.StateObject asyncState = (AsyncIP.LANVPN.StateObject) ar.AsyncState;
          Socket workSocket = asyncState.workSocket;
          int from = workSocket.EndReceiveFrom(ar, ref endPoint);
          workSocket.BeginReceiveFrom(asyncState.buffer, 0, 1024, SocketFlags.None, ref endPoint, new AsyncCallback(this.ReceiveCallback), (object) asyncState);
          string[] strArray = Encoding.ASCII.GetString(asyncState.buffer, 0, from).Split(' ');
          if (strArray.Length < 3)
            return;
          this.MessageQueue.Add((object) strArray[0], (object) new AsyncIP.LANVPN.ReceivedMessage()
          {
            COMserver = strArray[0],
            Command = strArray[1],
            Parameters = strArray[2]
          });
        }
        catch
        {
        }
      }

      private void ParseAvailableMessages()
      {
        foreach (DictionaryEntry message in this.MessageQueue)
        {
          AsyncIP.LANVPN.ReceivedMessage receivedMessage = (AsyncIP.LANVPN.ReceivedMessage) message.Value;
          if (receivedMessage.Command == AsyncIP.LANVPN.COMserverCommands.GetIPConfig.ToString())
          {
            AsyncIP.LANVPN.COMserver coMserver = new AsyncIP.LANVPN.COMserver();
            coMserver.Name = receivedMessage.COMserver;
            string[] strArray = receivedMessage.Parameters.ToString().Split(',');
            coMserver.Version = strArray[0];
            for (int index = 1; index < strArray.Length; ++index)
            {
              if (strArray[index].Contains("addr"))
                coMserver.IP = strArray[index].Substring(strArray[index].IndexOf(':') + 1);
              if (strArray[index].Contains("Mask"))
                coMserver.Subnet = strArray[index].Substring(strArray[index].IndexOf(':') + 1);
              if (strArray[index].Contains("0.0.0.0") && coMserver.Gateway == "")
                coMserver.Gateway = strArray[index + 1];
              if (strArray[index].Contains("DHCP"))
                coMserver.IPConfig = strArray[index];
              if (strArray[index].Contains("STATIC"))
                coMserver.IPConfig = strArray[index];
              if (strArray[index].Contains("nameserver") && coMserver.DNS == "")
                coMserver.DNS = strArray[index + 1];
              if (strArray[index].Contains("http-proxy") && coMserver.DNS == "")
              {
                if (strArray[index][0] == ';')
                {
                  coMserver.ProxyType = "none";
                }
                else
                {
                  coMserver.ProxyIP = strArray[index + 1];
                  coMserver.ProxyPort = strArray[index + 2];
                  coMserver.ProxyType = strArray[index + 4];
                }
              }
            }
            this.MyCOMservers.Add((object) coMserver.Name, (object) coMserver);
          }
        }
        this.MessageQueue.Clear();
      }

      private class StateObject
      {
        public Socket workSocket = (Socket) null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[1024];
      }

      private class ReceivedMessage
      {
        public string COMserver;
        public string Command;
        public string Parameters;
      }

      private class COMserver
      {
        public string Name = "";
        public string Version = "";
        public string IPConfig = "";
        public string IP = "";
        public string Subnet = "";
        public string Gateway = "";
        public string DNS = "";
        public string ProxyType = "";
        public string ProxyIP = "";
        public string ProxyPort = "";
        public string ProxyUser = "";
        public string ProxyPass = "";
      }

      private enum COMserverCommands
      {
        GetIPConfig,
        SetIPConfig,
        GetVPNConfig,
        SetVPNConfig,
        Reboot,
      }
    }
  }
}
