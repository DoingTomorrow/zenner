// Decompiled with JetBrains decompiler
// Type: RestSharp.IHttp
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace RestSharp
{
  public interface IHttp
  {
    CookieContainer CookieContainer { get; set; }

    ICredentials Credentials { get; set; }

    string UserAgent { get; set; }

    int Timeout { get; set; }

    bool FollowRedirects { get; set; }

    X509CertificateCollection ClientCertificates { get; set; }

    int? MaxRedirects { get; set; }

    IList<HttpHeader> Headers { get; }

    IList<HttpParameter> Parameters { get; }

    IList<HttpFile> Files { get; }

    IList<HttpCookie> Cookies { get; }

    string RequestBody { get; set; }

    string RequestContentType { get; set; }

    Uri Url { get; set; }

    HttpWebRequest DeleteAsync(Action<HttpResponse> action);

    HttpWebRequest GetAsync(Action<HttpResponse> action);

    HttpWebRequest HeadAsync(Action<HttpResponse> action);

    HttpWebRequest OptionsAsync(Action<HttpResponse> action);

    HttpWebRequest PostAsync(Action<HttpResponse> action);

    HttpWebRequest PutAsync(Action<HttpResponse> action);

    HttpWebRequest PatchAsync(Action<HttpResponse> action);

    HttpWebRequest AsPostAsync(Action<HttpResponse> action, string httpMethod);

    HttpWebRequest AsGetAsync(Action<HttpResponse> action, string httpMethod);

    HttpResponse Delete();

    HttpResponse Get();

    HttpResponse Head();

    HttpResponse Options();

    HttpResponse Post();

    HttpResponse Put();

    HttpResponse Patch();

    HttpResponse AsPost(string httpMethod);

    HttpResponse AsGet(string httpMethod);

    IWebProxy Proxy { get; set; }
  }
}
