// Decompiled with JetBrains decompiler
// Type: RestSharp.IRestResponse
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections.Generic;
using System.Net;

#nullable disable
namespace RestSharp
{
  public interface IRestResponse
  {
    IRestRequest Request { get; set; }

    string ContentType { get; set; }

    long ContentLength { get; set; }

    string ContentEncoding { get; set; }

    string Content { get; set; }

    HttpStatusCode StatusCode { get; set; }

    string StatusDescription { get; set; }

    byte[] RawBytes { get; set; }

    Uri ResponseUri { get; set; }

    string Server { get; set; }

    IList<RestResponseCookie> Cookies { get; }

    IList<Parameter> Headers { get; }

    ResponseStatus ResponseStatus { get; set; }

    string ErrorMessage { get; set; }

    Exception ErrorException { get; set; }
  }
}
