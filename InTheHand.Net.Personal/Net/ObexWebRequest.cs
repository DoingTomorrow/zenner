// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexWebRequest
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

#nullable disable
namespace InTheHand.Net
{
  public class ObexWebRequest : WebRequest
  {
    private MemoryStream requestStream = new MemoryStream();
    private bool connected;
    private Socket s;
    private Stream ns;
    private Stream m_alreadyConnectedObexStream;
    private BluetoothPublicFactory _btFactory;
    private ushort remoteMaxPacket = 1024;
    private int connectionId;
    private WebHeaderCollection headers = new WebHeaderCollection();
    private ObexMethod method = ObexMethod.Put;
    private int timeout = 50000;
    private Uri uri;

    static ObexWebRequest()
    {
      PlatformVerification.ThrowException();
      ObexWebRequestCreate creator = new ObexWebRequestCreate();
      WebRequest.RegisterPrefix("obex", (IWebRequestCreate) creator);
      WebRequest.RegisterPrefix("obex-push", (IWebRequestCreate) creator);
      WebRequest.RegisterPrefix("obex-ftp", (IWebRequestCreate) creator);
      WebRequest.RegisterPrefix("obex-sync", (IWebRequestCreate) creator);
    }

    internal ObexWebRequest(Uri requestUri, BluetoothPublicFactory factory)
      : this(requestUri)
    {
      this._btFactory = factory;
    }

    public ObexWebRequest(Uri requestUri)
    {
      if (requestUri == (Uri) null)
        throw new ArgumentNullException(nameof (requestUri));
      this.uri = requestUri.Scheme.StartsWith("obex", StringComparison.OrdinalIgnoreCase) ? requestUri : throw new UriFormatException("Scheme type not supported by ObexWebRequest");
    }

    public ObexWebRequest(Uri requestUri, Stream stream)
      : this(requestUri)
    {
      if (requestUri == (Uri) null)
        throw new ArgumentNullException(nameof (requestUri));
      if (requestUri.Host.Length != 0)
        throw new ArgumentException("Uri must have no host part when passing in the connection stream.");
      if (stream == null)
        throw new ArgumentNullException(nameof (stream));
      if (!stream.CanRead || !stream.CanWrite)
        throw new ArgumentException("Stream must be open for reading and writing.");
      this.m_alreadyConnectedObexStream = stream;
    }

    internal static Uri CreateUrl(string scheme, BluetoothAddress target, string path)
    {
      if (string.IsNullOrEmpty(scheme))
        throw new ArgumentNullException(nameof (scheme));
      if (path == null)
        throw new ArgumentNullException(nameof (path));
      return new Uri(new Uri(scheme + "://" + target.ToString("N")), path);
    }

    internal ObexWebRequest(
      string scheme,
      BluetoothAddress target,
      string path,
      BluetoothPublicFactory factory)
      : this(ObexWebRequest.CreateUrl(scheme, target, path), factory)
    {
    }

    public ObexWebRequest(string scheme, BluetoothAddress target, string path)
      : this(ObexWebRequest.CreateUrl(scheme, target, path))
    {
    }

    public ObexWebRequest(BluetoothAddress target, string path)
      : this("obex", target, path)
    {
    }

    public override WebHeaderCollection Headers
    {
      get => this.headers;
      set => this.headers = value;
    }

    public override string Method
    {
      get
      {
        switch (this.method)
        {
          case ObexMethod.Put:
            return "PUT";
          case ObexMethod.Get:
            return "GET";
          default:
            return "";
        }
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value));
        switch (value.ToUpper())
        {
          case "PUT":
            this.method = ObexMethod.Put;
            break;
          case "GET":
            this.method = ObexMethod.Get;
            break;
          default:
            throw new InvalidOperationException("Method not supported");
        }
      }
    }

    private ObexStatusCode Connect()
    {
      if (!this.connected)
      {
        if (this.ns == null)
        {
          try
          {
            if (this.uri.Host.Length == 0)
            {
              this.ns = this.m_alreadyConnectedObexStream;
            }
            else
            {
              BluetoothAddress result1;
              if (BluetoothAddress.TryParse(this.uri.Host, out result1))
              {
                BluetoothClient bluetoothClient = this._btFactory != null ? this._btFactory.CreateBluetoothClient() : new BluetoothClient();
                Guid service;
                switch (this.uri.Scheme)
                {
                  case "obex-ftp":
                    service = BluetoothService.ObexFileTransfer;
                    break;
                  case "obex-sync":
                    service = BluetoothService.IrMCSyncCommand;
                    break;
                  case "obex-pbap":
                    service = BluetoothService.PhonebookAccessPse;
                    break;
                  default:
                    service = BluetoothService.ObexObjectPush;
                    break;
                }
                BluetoothEndPoint remoteEP = new BluetoothEndPoint(result1, service);
                bluetoothClient.Connect(remoteEP);
                this.ns = (Stream) bluetoothClient.GetStream();
              }
              else
              {
                IrDAAddress result2;
                if (IrDAAddress.TryParse(this.uri.Host, out result2))
                {
                  this.s = new Socket(AddressFamily.Irda, SocketType.Stream, ProtocolType.IP);
                  this.s.Connect((EndPoint) new IrDAEndPoint(result2, "OBEX"));
                }
                else
                {
                  this.s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                  IPAddress address;
                  try
                  {
                    address = IPAddress.Parse(this.uri.Host);
                  }
                  catch
                  {
                    address = Dns.Resolve(this.uri.Host).AddressList[0];
                  }
                  this.s.Connect((EndPoint) new IPEndPoint(address, 650));
                }
              }
              if (this.ns == null)
                this.ns = (Stream) new NetworkStream(this.s, true);
              this.ns.ReadTimeout = this.timeout;
              this.ns.WriteTimeout = this.timeout;
            }
            return this.Connect_Obex();
          }
          finally
          {
            if (this.s != null && !this.s.Connected)
              this.s = (Socket) null;
          }
        }
      }
      return ~(ObexStatusCode.UnsupportedMediaType | ObexStatusCode.MultipleChoices | ObexStatusCode.Final);
    }

    private ObexStatusCode Connect_Obex()
    {
      byte[] buffer;
      if (this.uri.Scheme == "obex-ftp")
        buffer = new byte[26]
        {
          (byte) 128,
          (byte) 0,
          (byte) 26,
          (byte) 16,
          (byte) 0,
          (byte) 32,
          (byte) 0,
          (byte) 70,
          (byte) 0,
          (byte) 19,
          (byte) 249,
          (byte) 236,
          (byte) 123,
          (byte) 196,
          (byte) 149,
          (byte) 60,
          (byte) 17,
          (byte) 210,
          (byte) 152,
          (byte) 78,
          (byte) 82,
          (byte) 84,
          (byte) 0,
          (byte) 220,
          (byte) 158,
          (byte) 9
        };
      else
        buffer = new byte[7]
        {
          (byte) 128,
          (byte) 0,
          (byte) 7,
          (byte) 16,
          (byte) 0,
          (byte) 32,
          (byte) 0
        };
      this.ns.Write(buffer, 0, buffer.Length);
      byte[] numArray1 = new byte[3];
      ObexWebRequest.StreamReadBlockMust(this.ns, numArray1, 0, 3);
      if (numArray1[0] == (byte) 160)
      {
        short size = (short) ((int) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(numArray1, 1)) - 3);
        byte[] numArray2 = new byte[3 + (int) size];
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 0, 3);
        ObexWebRequest.StreamReadBlockMust(this.ns, numArray2, 3, (int) size);
        ObexParser.ParseHeaders(numArray2, true, ref this.remoteMaxPacket, (Stream) null, this.headers);
        if (this.headers["CONNECTIONID"] != null)
          this.connectionId = int.Parse(this.headers["CONNECTIONID"]);
      }
      return (ObexStatusCode) numArray1[0];
    }

    public override string ContentType
    {
      get => this.headers["TYPE"];
      set => this.headers["TYPE"] = value;
    }

    public override long ContentLength
    {
      get
      {
        string header = this.headers["LENGTH"];
        return header == null || header == string.Empty ? 0L : long.Parse(header);
      }
      set => this.headers["LENGTH"] = value.ToString();
    }

    public override IWebProxy Proxy
    {
      get => throw new NotSupportedException();
      set => throw new NotSupportedException();
    }

    public override int Timeout
    {
      get => this.timeout;
      set
      {
        if (value < -1)
          throw new ArgumentOutOfRangeException(nameof (value));
        if (value == -1)
          this.timeout = 0;
        else
          this.timeout = value;
      }
    }

    public override Uri RequestUri => this.uri;

    private ObexStatusCode DoPut()
    {
      ObexStatusCode status = ~(ObexStatusCode.UnsupportedMediaType | ObexStatusCode.MultipleChoices | ObexStatusCode.Final);
      byte[] buffer1 = new byte[(int) this.remoteMaxPacket];
      string stringToUnescape = this.uri.PathAndQuery;
      if (!this.uri.UserEscaped)
        stringToUnescape = Uri.UnescapeDataString(stringToUnescape);
      string s = stringToUnescape.TrimStart('/');
      int num1 = (s.Length + 1) * 2;
      int index1 = 3;
      buffer1[0] = (byte) 2;
      if (this.connectionId != 0)
      {
        buffer1[index1] = (byte) 203;
        BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.connectionId)).CopyTo((Array) buffer1, index1 + 1);
        index1 += 5;
      }
      buffer1[index1] = (byte) 1;
      BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) (num1 + 3))).CopyTo((Array) buffer1, index1 + 1);
      Encoding.BigEndianUnicode.GetBytes(s).CopyTo((Array) buffer1, index1 + 3);
      int index2 = index1 + (3 + num1);
      string header = this.headers["TYPE"];
      if (header != null && header != "")
      {
        int num2 = header.Length + 1;
        buffer1[index2] = (byte) 66;
        BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) (num2 + 3))).CopyTo((Array) buffer1, index2 + 1);
        Encoding.ASCII.GetBytes(header).CopyTo((Array) buffer1, index2 + 3);
        index2 += 3 + num2;
      }
      if (this.ContentLength != 0L)
      {
        buffer1[index2] = (byte) 195;
        BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Convert.ToInt32(this.ContentLength))).CopyTo((Array) buffer1, index2 + 1);
        index2 += 5;
      }
      BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) index2)).CopyTo((Array) buffer1, 1);
      this.ns.Write(buffer1, 0, index2);
      if (this.CheckResponse(ref status, false))
      {
        byte[] buffer2 = this.requestStream.GetBuffer();
        int num3 = this.ContentLength <= 0L ? buffer2.Length : (int) this.ContentLength;
        MemoryStream memoryStream = new MemoryStream(buffer2);
        while (num3 > 0)
        {
          int count;
          if (num3 <= (int) this.remoteMaxPacket - 6)
          {
            count = num3;
            num3 = 0;
            buffer1[0] = (byte) 130;
            buffer1[3] = (byte) 73;
          }
          else
          {
            count = (int) this.remoteMaxPacket - 6;
            num3 -= count;
            buffer1[0] = (byte) 2;
            buffer1[3] = (byte) 72;
          }
          int num4 = memoryStream.Read(buffer1, 6, count);
          BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) (num4 + 3))).CopyTo((Array) buffer1, 4);
          BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) (num4 + 6))).CopyTo((Array) buffer1, 1);
          this.ns.Write(buffer1, 0, num4 + 6);
          if (!this.CheckResponse(ref status, false))
            return status;
        }
      }
      return status;
    }

    private ObexStatusCode DoGet(MemoryStream ms, WebHeaderCollection headers)
    {
      byte[] numArray = new byte[(int) this.remoteMaxPacket];
      numArray[0] = (byte) 131;
      int index = 3;
      if (this.connectionId != 0)
      {
        numArray[index] = (byte) 203;
        BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.connectionId)).CopyTo((Array) numArray, index + 1);
        index += 5;
      }
      string s = this.uri.PathAndQuery.TrimStart('/');
      if (s.Length > 0)
      {
        int num = s.Length * 2 + 2;
        numArray[index] = (byte) 1;
        BitConverter.GetBytes((int) IPAddress.HostToNetworkOrder((short) (num + 3))).CopyTo((Array) numArray, index + 1);
        Encoding.BigEndianUnicode.GetBytes(s).CopyTo((Array) numArray, index + 3);
        index += num + 3;
      }
      string header = this.headers["TYPE"];
      if (header != null)
      {
        numArray[index] = (byte) 66;
        BitConverter.GetBytes((int) IPAddress.HostToNetworkOrder((short) (header.Length + 1 + 3))).CopyTo((Array) numArray, index + 1);
        Encoding.ASCII.GetBytes(header).CopyTo((Array) numArray, index + 3);
        index += header.Length + 4;
      }
      BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) index)).CopyTo((Array) numArray, 1);
      ObexStatusCode obexStatusCode;
      do
      {
        this.ns.Write(numArray, 0, index);
        ObexWebRequest.StreamReadBlockMust(this.ns, numArray, 0, 3);
        int offset = 3;
        obexStatusCode = (ObexStatusCode) numArray[0];
        short hostOrder = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(numArray, 1));
        ObexWebRequest.StreamReadBlockMust(this.ns, numArray, offset, (int) hostOrder - offset);
        ObexParser.ParseHeaders(numArray, false, ref this.remoteMaxPacket, (Stream) ms, headers);
        numArray[0] = (byte) 131;
        BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short) 3)).CopyTo((Array) numArray, 1);
        index = 3;
      }
      while (obexStatusCode == (ObexStatusCode.Final | ObexStatusCode.Continue));
      return obexStatusCode == (ObexStatusCode.Final | ObexStatusCode.OK) ? obexStatusCode : throw new WebException("GET failed with code: " + (object) obexStatusCode, WebExceptionStatus.ProtocolError);
    }

    private bool CheckResponse(ref ObexStatusCode status, bool isConnectResponse)
    {
      if (isConnectResponse)
        throw new ArgumentException("CheckResponse does not know how to parse the connect response");
      byte[] buffer1 = new byte[3];
      ObexWebRequest.StreamReadBlockMust(this.ns, buffer1, 0, buffer1.Length);
      status = (ObexStatusCode) buffer1[0];
      ObexStatusCode obexStatusCode = status;
      if ((uint) obexStatusCode <= 32U)
      {
        if (obexStatusCode != ObexStatusCode.Continue && obexStatusCode != ObexStatusCode.OK)
          goto label_9;
      }
      else if (obexStatusCode != (ObexStatusCode.Final | ObexStatusCode.Continue) && obexStatusCode != (ObexStatusCode.Final | ObexStatusCode.OK))
        goto label_9;
      short size = (short) ((int) IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer1, 1)) - 3);
      if (size > (short) 0)
      {
        byte[] buffer2 = new byte[(int) size];
        ObexWebRequest.StreamReadBlockMust(this.ns, buffer2, 0, (int) size);
        if (size == (short) 5)
        {
          int num = (int) buffer2[0];
        }
      }
      return true;
label_9:
      return false;
    }

    private void DisconnectIgnoreIOErrors()
    {
      if (this.ns == null)
        return;
      ObexStatusCode status = ~(ObexStatusCode.UnsupportedMediaType | ObexStatusCode.MultipleChoices | ObexStatusCode.Final);
      short num = 3;
      byte[] buffer = new byte[8];
      buffer[0] = (byte) 129;
      if (this.connectionId != 0)
      {
        buffer[3] = (byte) 203;
        BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.connectionId)).CopyTo((Array) buffer, 4);
        num += (short) 5;
      }
      BitConverter.GetBytes(IPAddress.HostToNetworkOrder(num)).CopyTo((Array) buffer, 1);
      this.ns.Write(buffer, 0, (int) num);
      try
      {
        this.CheckResponse(ref status, false);
      }
      catch (EndOfStreamException ex)
      {
      }
      catch (IOException ex)
      {
      }
      this.ns.Close();
    }

    public override Stream GetRequestStream() => (Stream) this.requestStream;

    public void ReadFile(string fileName)
    {
      FileStream fileStream = System.IO.File.OpenRead(fileName);
      long num = 0;
      byte[] buffer = new byte[1024];
      int count;
      do
      {
        count = fileStream.Read(buffer, 0, buffer.Length);
        num += (long) count;
        this.requestStream.Write(buffer, 0, count);
      }
      while (count > 0);
      fileStream.Close();
      this.requestStream.Close();
      this.ContentLength = num;
    }

    public override WebResponse GetResponse()
    {
      MemoryStream memoryStream = new MemoryStream();
      WebHeaderCollection headers = new WebHeaderCollection();
      ObexStatusCode code;
      try
      {
        code = this.Connect();
      }
      catch (Exception ex)
      {
        throw new WebException("Connect failed.", ex, WebExceptionStatus.ConnectFailure, (WebResponse) null);
      }
      try
      {
        switch (this.method)
        {
          case ObexMethod.Put:
          case ObexMethod.PutFinal:
            code = this.DoPut();
            break;
          case ObexMethod.Get:
            code = this.DoGet(memoryStream, headers);
            memoryStream.Seek(0L, SeekOrigin.Begin);
            break;
          default:
            throw new WebException("Unsupported Method.", (Exception) new InvalidOperationException(), WebExceptionStatus.ProtocolError, (WebResponse) null);
        }
        this.DisconnectIgnoreIOErrors();
      }
      catch (WebException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new WebException("Operation failed.", ex, WebExceptionStatus.UnknownError, (WebResponse) null);
      }
      finally
      {
        if (this.ns != null)
          this.ns.Close();
      }
      return (WebResponse) new ObexWebResponse(memoryStream, headers, code);
    }

    private static void StreamReadBlockMust(Stream stream, byte[] buffer, int offset, int size)
    {
      if (ObexWebRequest.StreamReadBlock(stream, buffer, offset, size) < size)
        throw new EndOfStreamException("Connection closed whilst reading an OBEX packet.");
    }

    private static int StreamReadBlock(Stream stream, byte[] buffer, int offset, int size)
    {
      int num1;
      int num2;
      for (num1 = 0; size - num1 > 0; num1 += num2)
      {
        num2 = stream.Read(buffer, offset + num1, size - num1);
        if (num2 == 0)
          break;
      }
      return num1;
    }

    public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
    {
      AsyncResult<WebResponse> state1 = new AsyncResult<WebResponse>(callback, state);
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.HackApmRunner_GetResponse), (object) state1);
      return (IAsyncResult) state1;
    }

    private void HackApmRunner_GetResponse(object state)
    {
      ((AsyncResult<WebResponse>) state).SetAsCompletedWithResultOf((Func<WebResponse>) (() => this.GetResponse()), false);
    }

    public override WebResponse EndGetResponse(IAsyncResult asyncResult)
    {
      return ((AsyncResult<WebResponse>) asyncResult).EndInvoke();
    }

    private static class SchemeNames
    {
      internal const string Prefix = "obex";
      internal const string Default = "obex";
      internal const string Push = "obex-push";
      internal const string Ftp = "obex-ftp";
      internal const string Sync = "obex-sync";
      internal const string Pbap = "obex-pbap";
    }
  }
}
