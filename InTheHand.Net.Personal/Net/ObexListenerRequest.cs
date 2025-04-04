// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexListenerRequest
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.IO;
using System.Net;

#nullable disable
namespace InTheHand.Net
{
  public class ObexListenerRequest
  {
    private byte[] input;
    private WebHeaderCollection headers;
    private EndPoint localEndPoint;
    private EndPoint remoteEndPoint;
    private Uri uri;

    internal ObexListenerRequest(
      byte[] input,
      WebHeaderCollection headers,
      EndPoint localEndPoint,
      EndPoint remoteEndPoint)
    {
      this.input = input;
      this.headers = headers;
      this.localEndPoint = localEndPoint;
      this.remoteEndPoint = remoteEndPoint;
    }

    public long ContentLength64
    {
      get
      {
        string header = this.headers["LENGTH"];
        return header != null && header != "" ? long.Parse(header) : -1L;
      }
    }

    public string ContentType => this.headers["TYPE"];

    public WebHeaderCollection Headers => this.headers;

    public EndPoint LocalEndPoint => this.localEndPoint;

    public string ObexMethod => "PUT";

    public Stream InputStream => (Stream) new MemoryStream(this.input);

    public Version ProtocolVersion => new Version(1, 0);

    public string RawUrl => this.Url.PathAndQuery;

    public EndPoint RemoteEndPoint => this.remoteEndPoint;

    public string UserHostAddress
    {
      get
      {
        if (this.localEndPoint is BluetoothEndPoint)
          return (this.localEndPoint as BluetoothEndPoint).Address.ToString("P");
        if (this.localEndPoint is IrDAEndPoint)
          return (this.localEndPoint as IrDAEndPoint).Address.ToString("P");
        if (!(this.localEndPoint is IPEndPoint))
          return "";
        IPEndPoint localEndPoint = this.localEndPoint as IPEndPoint;
        return localEndPoint.Address.ToString() + ":" + (object) localEndPoint.Port;
      }
    }

    public Uri Url
    {
      get
      {
        if (this.uri == (Uri) null)
          this.uri = new Uri("obex-push://" + (!(this.localEndPoint is BluetoothEndPoint) ? ((IrDAEndPoint) this.localEndPoint).Address.ToString() : ((BluetoothEndPoint) this.localEndPoint).Address.ToString()) + "/" + this.headers["NAME"]);
        return this.uri;
      }
    }

    public void WriteFile(string fileName)
    {
      FileStream fs = System.IO.File.Create(fileName);
      this.WriteFile((Stream) fs);
      fs.Close();
    }

    internal void WriteFile(Stream fs)
    {
      MemoryStream memoryStream = new MemoryStream(this.input);
      int contentLength64 = (int) this.ContentLength64;
      int num = 0;
      if (contentLength64 == -1)
      {
        int length = this.input.Length;
      }
      byte[] buffer = new byte[1024];
      int count;
      do
      {
        count = memoryStream.Read(buffer, 0, buffer.Length);
        fs.Write(buffer, 0, count);
        num += count;
      }
      while (count > 0);
      memoryStream.Close();
    }
  }
}
