// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ObexWebResponse
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.IO;
using System.Net;

#nullable disable
namespace InTheHand.Net
{
  public class ObexWebResponse : WebResponse
  {
    private MemoryStream responseStream;
    private WebHeaderCollection responseHeaders;
    private ObexStatusCode statusCode;

    internal ObexWebResponse(MemoryStream stream, WebHeaderCollection headers, ObexStatusCode code)
    {
      this.responseStream = stream;
      this.responseHeaders = headers;
      this.statusCode = code;
    }

    public override WebHeaderCollection Headers => this.responseHeaders;

    public override long ContentLength
    {
      get
      {
        string responseHeader = this.responseHeaders["LENGTH"];
        return responseHeader != null && responseHeader != string.Empty ? long.Parse(responseHeader) : 0L;
      }
      set
      {
      }
    }

    public override string ContentType
    {
      get => this.responseHeaders["TYPE"];
      set
      {
      }
    }

    public ObexStatusCode StatusCode => this.statusCode;

    public override Stream GetResponseStream() => (Stream) this.responseStream;

    public override void Close()
    {
      if (this.responseStream == null)
        return;
      this.responseStream.Close();
    }

    [Obsolete]
    public void WriteFile(string fileName)
    {
      FileStream fileStream = System.IO.File.Create(fileName);
      byte[] buffer = new byte[1024];
      int count;
      do
      {
        count = this.responseStream.Read(buffer, 0, buffer.Length);
        fileStream.Write(buffer, 0, count);
      }
      while (count > 0);
      this.responseStream.Close();
      fileStream.Close();
    }
  }
}
