// Decompiled with JetBrains decompiler
// Type: RestSharp.IRestClient
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
  public interface IRestClient
  {
    CookieContainer CookieContainer { get; set; }

    string UserAgent { get; set; }

    int Timeout { get; set; }

    bool UseSynchronizationContext { get; set; }

    IAuthenticator Authenticator { get; set; }

    string BaseUrl { get; set; }

    IList<Parameter> DefaultParameters { get; }

    RestRequestAsyncHandle ExecuteAsync(
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback);

    RestRequestAsyncHandle ExecuteAsync<T>(
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback);

    X509CertificateCollection ClientCertificates { get; set; }

    IRestResponse Execute(IRestRequest request);

    IRestResponse<T> Execute<T>(IRestRequest request) where T : new();

    IWebProxy Proxy { get; set; }

    Uri BuildUri(IRestRequest request);

    RestRequestAsyncHandle ExecuteAsyncGet(
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback,
      string httpMethod);

    RestRequestAsyncHandle ExecuteAsyncPost(
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback,
      string httpMethod);

    RestRequestAsyncHandle ExecuteAsyncGet<T>(
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
      string httpMethod);

    RestRequestAsyncHandle ExecuteAsyncPost<T>(
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
      string httpMethod);

    IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod);

    IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod);

    IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new();

    IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new();
  }
}
