// Decompiled with JetBrains decompiler
// Type: RestSharp.IHttpResponse
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace RestSharp
{
  public interface IHttpResponse
  {
    string ContentType { get; set; }

    long ContentLength { get; set; }

    string ContentEncoding { get; set; }

    string Content { get; }

    HttpStatusCode StatusCode { get; set; }

    string StatusDescription { get; set; }

    byte[] RawBytes { get; set; }

    Uri ResponseUri { get; set; }

    string Server { get; set; }

    IList<HttpHeader> Headers { get; }

    IList<HttpCookie> Cookies { get; }

    ResponseStatus ResponseStatus { get; set; }

    string ErrorMessage { get; set; }

    Exception ErrorException { get; set; }
  }
}
