// Decompiled with JetBrains decompiler
// Type: RestSharp.RestClient
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Deserializers;
using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

#nullable disable
namespace RestSharp
{
  public class RestClient : IRestClient
  {
    private static readonly Version version = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;
    public IHttpFactory HttpFactory = (IHttpFactory) new SimpleFactory<Http>();
    private string _baseUrl;

    public virtual RestRequestAsyncHandle ExecuteAsync(
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      string name = System.Enum.GetName(typeof (Method), (object) request.Method);
      switch (request.Method)
      {
        case Method.POST:
        case Method.PUT:
        case Method.PATCH:
          return this.ExecuteAsync(request, callback, name, new Func<IHttp, Action<HttpResponse>, string, HttpWebRequest>(RestClient.DoAsPostAsync));
        default:
          return this.ExecuteAsync(request, callback, name, new Func<IHttp, Action<HttpResponse>, string, HttpWebRequest>(RestClient.DoAsGetAsync));
      }
    }

    public virtual RestRequestAsyncHandle ExecuteAsyncGet(
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback,
      string httpMethod)
    {
      return this.ExecuteAsync(request, callback, httpMethod, new Func<IHttp, Action<HttpResponse>, string, HttpWebRequest>(RestClient.DoAsPostAsync));
    }

    public virtual RestRequestAsyncHandle ExecuteAsyncPost(
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback,
      string httpMethod)
    {
      request.Method = Method.POST;
      return this.ExecuteAsync(request, callback, httpMethod, new Func<IHttp, Action<HttpResponse>, string, HttpWebRequest>(RestClient.DoAsGetAsync));
    }

    private RestRequestAsyncHandle ExecuteAsync(
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback,
      string httpMethod,
      Func<IHttp, Action<HttpResponse>, string, HttpWebRequest> getWebRequest)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RestClient.\u003C\u003Ec__DisplayClass3 cDisplayClass3_1 = new RestClient.\u003C\u003Ec__DisplayClass3();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass3_1.request = request;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass3_1.callback = callback;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass3_1.\u003C\u003E4__this = this;
      IHttp http = this.HttpFactory.Create();
      // ISSUE: reference to a compiler-generated field
      this.AuthenticateIfNeeded(this, cDisplayClass3_1.request);
      this.AddDefaultParameter("Accept", (object) string.Join(", ", this.AcceptTypes.ToArray<string>()), ParameterType.HttpHeader);
      // ISSUE: reference to a compiler-generated field
      this.ConfigureHttp(cDisplayClass3_1.request, http);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass3_1.asyncHandle = new RestRequestAsyncHandle();
      // ISSUE: reference to a compiler-generated method
      Action<HttpResponse> action = new Action<HttpResponse>(cDisplayClass3_1.\u003CExecuteAsync\u003Eb__0);
      if (this.UseSynchronizationContext && SynchronizationContext.Current != null)
      {
        SynchronizationContext ctx = SynchronizationContext.Current;
        Action<HttpResponse> cb = action;
        action = (Action<HttpResponse>) (resp =>
        {
          // ISSUE: variable of a compiler-generated type
          RestClient.\u003C\u003Ec__DisplayClass3 cDisplayClass3 = cDisplayClass3_1;
          HttpResponse resp1 = resp;
          ctx.Post((SendOrPostCallback) (s => cb(resp1)), (object) null);
        });
      }
      // ISSUE: reference to a compiler-generated field
      cDisplayClass3_1.asyncHandle.WebRequest = getWebRequest(http, action, httpMethod);
      // ISSUE: reference to a compiler-generated field
      return cDisplayClass3_1.asyncHandle;
    }

    private static HttpWebRequest DoAsGetAsync(
      IHttp http,
      Action<HttpResponse> response_cb,
      string method)
    {
      return http.AsGetAsync(response_cb, method);
    }

    private static HttpWebRequest DoAsPostAsync(
      IHttp http,
      Action<HttpResponse> response_cb,
      string method)
    {
      return http.AsPostAsync(response_cb, method);
    }

    private void ProcessResponse(
      IRestRequest request,
      HttpResponse httpResponse,
      RestRequestAsyncHandle asyncHandle,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      RestResponse restResponse = this.ConvertToRestResponse(request, httpResponse);
      callback((IRestResponse) restResponse, asyncHandle);
    }

    public virtual RestRequestAsyncHandle ExecuteAsync<T>(
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
    {
      return this.ExecuteAsync(request, (Action<IRestResponse, RestRequestAsyncHandle>) ((response, asyncHandle) => this.DeserializeResponse<T>(request, callback, response, asyncHandle)));
    }

    public virtual RestRequestAsyncHandle ExecuteAsyncGet<T>(
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
      string httpMethod)
    {
      return this.ExecuteAsyncGet(request, (Action<IRestResponse, RestRequestAsyncHandle>) ((response, asyncHandle) => this.DeserializeResponse<T>(request, callback, response, asyncHandle)), httpMethod);
    }

    public virtual RestRequestAsyncHandle ExecuteAsyncPost<T>(
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
      string httpMethod)
    {
      return this.ExecuteAsyncPost(request, (Action<IRestResponse, RestRequestAsyncHandle>) ((response, asyncHandle) => this.DeserializeResponse<T>(request, callback, response, asyncHandle)), httpMethod);
    }

    private void DeserializeResponse<T>(
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback,
      IRestResponse response,
      RestRequestAsyncHandle asyncHandle)
    {
      IRestResponse<T> restResponse = (IRestResponse<T>) (response as RestResponse<T>);
      if (response.ResponseStatus != ResponseStatus.Aborted)
        restResponse = this.Deserialize<T>(request, response);
      callback(restResponse, asyncHandle);
    }

    public RestClient()
    {
      this.ContentHandlers = (IDictionary<string, IDeserializer>) new Dictionary<string, IDeserializer>();
      this.AcceptTypes = (IList<string>) new List<string>();
      this.DefaultParameters = (IList<Parameter>) new List<Parameter>();
      this.AddHandler("application/json", (IDeserializer) new JsonDeserializer());
      this.AddHandler("application/xml", (IDeserializer) new XmlDeserializer());
      this.AddHandler("text/json", (IDeserializer) new JsonDeserializer());
      this.AddHandler("text/x-json", (IDeserializer) new JsonDeserializer());
      this.AddHandler("text/javascript", (IDeserializer) new JsonDeserializer());
      this.AddHandler("text/xml", (IDeserializer) new XmlDeserializer());
      this.AddHandler("*", (IDeserializer) new XmlDeserializer());
      this.FollowRedirects = true;
    }

    public RestClient(string baseUrl)
      : this()
    {
      this.BaseUrl = baseUrl;
    }

    private IDictionary<string, IDeserializer> ContentHandlers { get; set; }

    private IList<string> AcceptTypes { get; set; }

    public IList<Parameter> DefaultParameters { get; private set; }

    public void AddHandler(string contentType, IDeserializer deserializer)
    {
      this.ContentHandlers[contentType] = deserializer;
      if (!(contentType != "*"))
        return;
      this.AcceptTypes.Add(contentType);
    }

    public void RemoveHandler(string contentType)
    {
      this.ContentHandlers.Remove(contentType);
      this.AcceptTypes.Remove(contentType);
    }

    public void ClearHandlers()
    {
      this.ContentHandlers.Clear();
      this.AcceptTypes.Clear();
    }

    private IDeserializer GetHandler(string contentType)
    {
      if (string.IsNullOrEmpty(contentType) && this.ContentHandlers.ContainsKey("*"))
        return this.ContentHandlers["*"];
      int length = contentType.IndexOf(';');
      if (length > -1)
        contentType = contentType.Substring(0, length);
      IDeserializer handler = (IDeserializer) null;
      if (this.ContentHandlers.ContainsKey(contentType))
        handler = this.ContentHandlers[contentType];
      else if (this.ContentHandlers.ContainsKey("*"))
        handler = this.ContentHandlers["*"];
      return handler;
    }

    public int? MaxRedirects { get; set; }

    public X509CertificateCollection ClientCertificates { get; set; }

    public bool FollowRedirects { get; set; }

    public CookieContainer CookieContainer { get; set; }

    public string UserAgent { get; set; }

    public int Timeout { get; set; }

    public bool UseSynchronizationContext { get; set; }

    public IAuthenticator Authenticator { get; set; }

    public virtual string BaseUrl
    {
      get => this._baseUrl;
      set
      {
        this._baseUrl = value;
        if (this._baseUrl == null || !this._baseUrl.EndsWith("/"))
          return;
        this._baseUrl = this._baseUrl.Substring(0, this._baseUrl.Length - 1);
      }
    }

    private void AuthenticateIfNeeded(RestClient client, IRestRequest request)
    {
      if (this.Authenticator == null)
        return;
      this.Authenticator.Authenticate((IRestClient) client, request);
    }

    public Uri BuildUri(IRestRequest request)
    {
      string uriString = request.Resource;
      foreach (Parameter parameter in request.Parameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.UrlSegment)))
        uriString = uriString.Replace("{" + parameter.Name + "}", parameter.Value.ToString().UrlEncode());
      if (!string.IsNullOrEmpty(uriString) && uriString.StartsWith("/"))
        uriString = uriString.Substring(1);
      if (!string.IsNullOrEmpty(this.BaseUrl))
        uriString = !string.IsNullOrEmpty(uriString) ? string.Format("{0}/{1}", (object) this.BaseUrl, (object) uriString) : this.BaseUrl;
      if (request.Method != Method.POST && request.Method != Method.PUT && request.Method != Method.PATCH && request.Parameters.Any<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.GetOrPost)))
      {
        if (uriString.EndsWith("/"))
          uriString = uriString.Substring(0, uriString.Length - 1);
        string str = this.EncodeParameters(request);
        uriString = string.Format("{0}?{1}", (object) uriString, (object) str);
      }
      return new Uri(uriString);
    }

    private string EncodeParameters(IRestRequest request)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Parameter parameter in request.Parameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.GetOrPost)))
      {
        if (stringBuilder.Length > 1)
          stringBuilder.Append("&");
        stringBuilder.AppendFormat("{0}={1}", (object) parameter.Name.UrlEncode(), (object) parameter.Value.ToString().UrlEncode());
      }
      return stringBuilder.ToString();
    }

    private void ConfigureHttp(IRestRequest request, IHttp http)
    {
      http.CookieContainer = this.CookieContainer;
      using (IEnumerator<Parameter> enumerator = this.DefaultParameters.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          Parameter p = enumerator.Current;
          if (!request.Parameters.Any<Parameter>((Func<Parameter, bool>) (p2 => p2.Name == p.Name && p2.Type == p.Type)))
            request.AddParameter(p);
        }
      }
      http.Url = this.BuildUri(request);
      string input = this.UserAgent ?? http.UserAgent;
      http.UserAgent = input.HasValue() ? input : "RestSharp " + RestClient.version.ToString();
      int num = request.Timeout > 0 ? request.Timeout : this.Timeout;
      if (num > 0)
        http.Timeout = num;
      http.FollowRedirects = this.FollowRedirects;
      if (this.ClientCertificates != null)
        http.ClientCertificates = this.ClientCertificates;
      http.MaxRedirects = this.MaxRedirects;
      if (request.Credentials != null)
        http.Credentials = request.Credentials;
      foreach (HttpHeader httpHeader in request.Parameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.HttpHeader)).Select<Parameter, HttpHeader>((Func<Parameter, HttpHeader>) (p => new HttpHeader()
      {
        Name = p.Name,
        Value = p.Value.ToString()
      })))
        http.Headers.Add(httpHeader);
      foreach (HttpCookie httpCookie in request.Parameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.Cookie)).Select<Parameter, HttpCookie>((Func<Parameter, HttpCookie>) (p => new HttpCookie()
      {
        Name = p.Name,
        Value = p.Value.ToString()
      })))
        http.Cookies.Add(httpCookie);
      foreach (HttpParameter httpParameter in request.Parameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.GetOrPost && p.Value != null)).Select<Parameter, HttpParameter>((Func<Parameter, HttpParameter>) (p => new HttpParameter()
      {
        Name = p.Name,
        Value = p.Value.ToString()
      })))
        http.Parameters.Add(httpParameter);
      foreach (FileParameter file in request.Files)
        http.Files.Add(new HttpFile()
        {
          Name = file.Name,
          ContentType = file.ContentType,
          Writer = file.Writer,
          FileName = file.FileName,
          ContentLength = file.ContentLength
        });
      Parameter parameter = request.Parameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.RequestBody)).FirstOrDefault<Parameter>();
      if (parameter == null)
        return;
      http.RequestBody = parameter.Value.ToString();
      http.RequestContentType = parameter.Name;
    }

    private RestResponse ConvertToRestResponse(IRestRequest request, HttpResponse httpResponse)
    {
      RestResponse restResponse = new RestResponse();
      restResponse.Content = httpResponse.Content;
      restResponse.ContentEncoding = httpResponse.ContentEncoding;
      restResponse.ContentLength = httpResponse.ContentLength;
      restResponse.ContentType = httpResponse.ContentType;
      restResponse.ErrorException = httpResponse.ErrorException;
      restResponse.ErrorMessage = httpResponse.ErrorMessage;
      restResponse.RawBytes = httpResponse.RawBytes;
      restResponse.ResponseStatus = httpResponse.ResponseStatus;
      restResponse.ResponseUri = httpResponse.ResponseUri;
      restResponse.Server = httpResponse.Server;
      restResponse.StatusCode = httpResponse.StatusCode;
      restResponse.StatusDescription = httpResponse.StatusDescription;
      restResponse.Request = request;
      foreach (HttpHeader header in (IEnumerable<HttpHeader>) httpResponse.Headers)
        restResponse.Headers.Add(new Parameter()
        {
          Name = header.Name,
          Value = (object) header.Value,
          Type = ParameterType.HttpHeader
        });
      foreach (HttpCookie cookie in (IEnumerable<HttpCookie>) httpResponse.Cookies)
        restResponse.Cookies.Add(new RestResponseCookie()
        {
          Comment = cookie.Comment,
          CommentUri = cookie.CommentUri,
          Discard = cookie.Discard,
          Domain = cookie.Domain,
          Expired = cookie.Expired,
          Expires = cookie.Expires,
          HttpOnly = cookie.HttpOnly,
          Name = cookie.Name,
          Path = cookie.Path,
          Port = cookie.Port,
          Secure = cookie.Secure,
          TimeStamp = cookie.TimeStamp,
          Value = cookie.Value,
          Version = cookie.Version
        });
      return restResponse;
    }

    private IRestResponse<T> Deserialize<T>(IRestRequest request, IRestResponse raw)
    {
      request.OnBeforeDeserialization(raw);
      IDeserializer handler = this.GetHandler(raw.ContentType);
      handler.RootElement = request.RootElement;
      handler.DateFormat = request.DateFormat;
      handler.Namespace = request.XmlNamespace;
      IRestResponse<T> restResponse = (IRestResponse<T>) new RestResponse<T>();
      try
      {
        restResponse = raw.toAsyncResponse<T>();
        restResponse.Data = handler.Deserialize<T>(raw);
        restResponse.Request = request;
      }
      catch (Exception ex)
      {
        restResponse.ResponseStatus = ResponseStatus.Error;
        restResponse.ErrorMessage = ex.Message;
        restResponse.ErrorException = ex;
      }
      return restResponse;
    }

    public IWebProxy Proxy { get; set; }

    public byte[] DownloadData(IRestRequest request) => this.Execute(request).RawBytes;

    public virtual IRestResponse Execute(IRestRequest request)
    {
      string name = System.Enum.GetName(typeof (Method), (object) request.Method);
      switch (request.Method)
      {
        case Method.POST:
        case Method.PUT:
        case Method.PATCH:
          return this.Execute(request, name, new Func<IHttp, string, HttpResponse>(RestClient.DoExecuteAsPost));
        default:
          return this.Execute(request, name, new Func<IHttp, string, HttpResponse>(RestClient.DoExecuteAsGet));
      }
    }

    public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
    {
      return this.Execute(request, httpMethod, new Func<IHttp, string, HttpResponse>(RestClient.DoExecuteAsGet));
    }

    public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
    {
      request.Method = Method.POST;
      return this.Execute(request, httpMethod, new Func<IHttp, string, HttpResponse>(RestClient.DoExecuteAsPost));
    }

    private IRestResponse Execute(
      IRestRequest request,
      string httpMethod,
      Func<IHttp, string, HttpResponse> getResponse)
    {
      this.AuthenticateIfNeeded(this, request);
      this.AddDefaultParameter("Accept", (object) string.Join(", ", this.AcceptTypes.ToArray<string>()), ParameterType.HttpHeader);
      IRestResponse restResponse = (IRestResponse) new RestResponse();
      try
      {
        IHttp http = this.HttpFactory.Create();
        this.ConfigureHttp(request, http);
        this.ConfigureProxy(http);
        restResponse = (IRestResponse) this.ConvertToRestResponse(request, getResponse(http, httpMethod));
        restResponse.Request = request;
        restResponse.Request.IncreaseNumAttempts();
      }
      catch (Exception ex)
      {
        restResponse.ResponseStatus = ResponseStatus.Error;
        restResponse.ErrorMessage = ex.Message;
        restResponse.ErrorException = ex;
      }
      return restResponse;
    }

    private static HttpResponse DoExecuteAsGet(IHttp http, string method) => http.AsGet(method);

    private static HttpResponse DoExecuteAsPost(IHttp http, string method) => http.AsPost(method);

    public virtual IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
    {
      return this.Deserialize<T>(request, this.Execute(request));
    }

    public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new()
    {
      return this.Deserialize<T>(request, this.ExecuteAsGet(request, httpMethod));
    }

    public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new()
    {
      return this.Deserialize<T>(request, this.ExecuteAsPost(request, httpMethod));
    }

    private void ConfigureProxy(IHttp http)
    {
      if (this.Proxy == null)
        return;
      http.Proxy = this.Proxy;
    }
  }
}
