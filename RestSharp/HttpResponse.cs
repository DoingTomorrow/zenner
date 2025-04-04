// Decompiled with JetBrains decompiler
// Type: RestSharp.HttpResponse
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace RestSharp
{
  public class HttpResponse : IHttpResponse
  {
    private string _content;
    private ResponseStatus _responseStatus;

    public HttpResponse()
    {
      this.Headers = (IList<HttpHeader>) new List<HttpHeader>();
      this.Cookies = (IList<HttpCookie>) new List<HttpCookie>();
    }

    public string ContentType { get; set; }

    public long ContentLength { get; set; }

    public string ContentEncoding { get; set; }

    public string Content
    {
      get
      {
        if (this._content == null)
          this._content = this.RawBytes.AsString();
        return this._content;
      }
    }

    public HttpStatusCode StatusCode { get; set; }

    public string StatusDescription { get; set; }

    public byte[] RawBytes { get; set; }

    public Uri ResponseUri { get; set; }

    public string Server { get; set; }

    public IList<HttpHeader> Headers { get; private set; }

    public IList<HttpCookie> Cookies { get; private set; }

    public ResponseStatus ResponseStatus
    {
      get => this._responseStatus;
      set => this._responseStatus = value;
    }

    public string ErrorMessage { get; set; }

    public Exception ErrorException { get; set; }
  }
}
