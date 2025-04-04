// Decompiled with JetBrains decompiler
// Type: RestSharp.RestResponseBase
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
  public abstract class RestResponseBase
  {
    private string _content;
    private ResponseStatus _responseStatus;

    public RestResponseBase()
    {
      this.Headers = (IList<Parameter>) new List<Parameter>();
      this.Cookies = (IList<RestResponseCookie>) new List<RestResponseCookie>();
    }

    public IRestRequest Request { get; set; }

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
      set => this._content = value;
    }

    public HttpStatusCode StatusCode { get; set; }

    public string StatusDescription { get; set; }

    public byte[] RawBytes { get; set; }

    public Uri ResponseUri { get; set; }

    public string Server { get; set; }

    public IList<RestResponseCookie> Cookies { get; protected internal set; }

    public IList<Parameter> Headers { get; protected internal set; }

    public ResponseStatus ResponseStatus
    {
      get => this._responseStatus;
      set => this._responseStatus = value;
    }

    public string ErrorMessage { get; set; }

    public Exception ErrorException { get; set; }
  }
}
