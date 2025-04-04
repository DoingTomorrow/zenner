// Decompiled with JetBrains decompiler
// Type: RestSharp.Http
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace RestSharp
{
  public class Http : IHttp, IHttpFactory
  {
    private const string _lineBreak = "\r\n";
    private const string FormBoundary = "-----------------------------28947758029299";
    private Http.TimeOutState _timeoutState;
    private static readonly Encoding _defaultEncoding = Encoding.UTF8;
    private readonly IDictionary<string, Action<HttpWebRequest, string>> _restrictedHeaderActions;

    public HttpWebRequest DeleteAsync(Action<HttpResponse> action)
    {
      return this.GetStyleMethodInternalAsync("DELETE", action);
    }

    public HttpWebRequest GetAsync(Action<HttpResponse> action)
    {
      return this.GetStyleMethodInternalAsync("GET", action);
    }

    public HttpWebRequest HeadAsync(Action<HttpResponse> action)
    {
      return this.GetStyleMethodInternalAsync("HEAD", action);
    }

    public HttpWebRequest OptionsAsync(Action<HttpResponse> action)
    {
      return this.GetStyleMethodInternalAsync("OPTIONS", action);
    }

    public HttpWebRequest PostAsync(Action<HttpResponse> action)
    {
      return this.PutPostInternalAsync("POST", action);
    }

    public HttpWebRequest PutAsync(Action<HttpResponse> action)
    {
      return this.PutPostInternalAsync("PUT", action);
    }

    public HttpWebRequest PatchAsync(Action<HttpResponse> action)
    {
      return this.PutPostInternalAsync("PATCH", action);
    }

    public HttpWebRequest AsPostAsync(Action<HttpResponse> action, string httpMethod)
    {
      return this.PutPostInternalAsync(httpMethod.ToUpperInvariant(), action);
    }

    public HttpWebRequest AsGetAsync(Action<HttpResponse> action, string httpMethod)
    {
      return this.GetStyleMethodInternalAsync(httpMethod.ToUpperInvariant(), action);
    }

    private HttpWebRequest GetStyleMethodInternalAsync(string method, Action<HttpResponse> callback)
    {
      HttpWebRequest state = (HttpWebRequest) null;
      try
      {
        Uri url = this.Url;
        state = this.ConfigureAsyncWebRequest(method, url);
        this._timeoutState = new Http.TimeOutState()
        {
          Request = state
        };
        this.SetTimeout(state.BeginGetResponse((AsyncCallback) (result => this.ResponseCallback(result, callback)), (object) state), this._timeoutState);
      }
      catch (Exception ex)
      {
        Http.ExecuteCallback(new HttpResponse()
        {
          ErrorMessage = ex.Message,
          ErrorException = ex,
          ResponseStatus = ResponseStatus.Error
        }, callback);
      }
      return state;
    }

    private HttpWebRequest PutPostInternalAsync(string method, Action<HttpResponse> callback)
    {
      HttpWebRequest webRequest = (HttpWebRequest) null;
      try
      {
        webRequest = this.ConfigureAsyncWebRequest(method, this.Url);
        this.PreparePostBody(webRequest);
        this.WriteRequestBodyAsync(webRequest, callback);
      }
      catch (Exception ex)
      {
        Http.ExecuteCallback(new HttpResponse()
        {
          ErrorMessage = ex.Message,
          ErrorException = ex,
          ResponseStatus = ResponseStatus.Error
        }, callback);
      }
      return webRequest;
    }

    private void WriteRequestBodyAsync(HttpWebRequest webRequest, Action<HttpResponse> callback)
    {
      this._timeoutState = new Http.TimeOutState()
      {
        Request = webRequest
      };
      IAsyncResult asyncResult;
      if (this.HasBody || this.HasFiles)
      {
        webRequest.ContentLength = this.CalculateContentLength();
        asyncResult = webRequest.BeginGetRequestStream((AsyncCallback) (result => this.RequestStreamCallback(result, callback)), (object) webRequest);
      }
      else
        asyncResult = webRequest.BeginGetResponse((AsyncCallback) (r => this.ResponseCallback(r, callback)), (object) webRequest);
      this.SetTimeout(asyncResult, this._timeoutState);
    }

    private long CalculateContentLength()
    {
      if (!this.HasFiles)
        return (long) Http._defaultEncoding.GetByteCount(this.RequestBody);
      long num = 0;
      foreach (HttpFile file in (IEnumerable<HttpFile>) this.Files)
      {
        num += (long) Http._defaultEncoding.GetByteCount(Http.GetMultipartFileHeader(file));
        num += file.ContentLength;
        num += (long) Http._defaultEncoding.GetByteCount("\r\n");
      }
      foreach (HttpParameter parameter in (IEnumerable<HttpParameter>) this.Parameters)
        num += (long) Http._defaultEncoding.GetByteCount(Http.GetMultipartFormData(parameter));
      return num + (long) Http._defaultEncoding.GetByteCount(Http.GetMultipartFooter());
    }

    private void RequestStreamCallback(IAsyncResult result, Action<HttpResponse> callback)
    {
      HttpWebRequest asyncState = (HttpWebRequest) result.AsyncState;
      if (this._timeoutState.TimedOut)
      {
        Http.ExecuteCallback(new HttpResponse()
        {
          ResponseStatus = ResponseStatus.TimedOut
        }, callback);
      }
      else
      {
        try
        {
          using (Stream requestStream = asyncState.EndGetRequestStream(result))
          {
            if (this.HasFiles)
              this.WriteMultipartFormData(requestStream);
            else
              Http.WriteStringTo(requestStream, this.RequestBody);
          }
        }
        catch (Exception ex)
        {
          if (ex is WebException && ((WebException) ex).Status == WebExceptionStatus.RequestCanceled)
          {
            Http.ExecuteCallback(new HttpResponse()
            {
              ResponseStatus = ResponseStatus.TimedOut
            }, callback);
            return;
          }
          Http.ExecuteCallback(new HttpResponse()
          {
            ErrorMessage = ex.Message,
            ErrorException = ex,
            ResponseStatus = ResponseStatus.Error
          }, callback);
          return;
        }
        asyncState.BeginGetResponse((AsyncCallback) (r => this.ResponseCallback(r, callback)), (object) asyncState);
      }
    }

    private void SetTimeout(IAsyncResult asyncResult, Http.TimeOutState timeOutState)
    {
      if (this.Timeout == 0)
        return;
      ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, new WaitOrTimerCallback(Http.TimeoutCallback), (object) timeOutState, this.Timeout, true);
    }

    private static void TimeoutCallback(object state, bool timedOut)
    {
      if (!timedOut || !(state is Http.TimeOutState timeOutState))
        return;
      lock (timeOutState)
        timeOutState.TimedOut = true;
      if (timeOutState.Request == null)
        return;
      timeOutState.Request.Abort();
    }

    private static void GetRawResponseAsync(IAsyncResult result, Action<HttpWebResponse> callback)
    {
      new HttpResponse().ResponseStatus = ResponseStatus.None;
      HttpWebResponse httpWebResponse;
      try
      {
        httpWebResponse = ((WebRequest) result.AsyncState).EndGetResponse(result) as HttpWebResponse;
      }
      catch (WebException ex)
      {
        if (ex.Status == WebExceptionStatus.RequestCanceled)
          throw ex;
        httpWebResponse = ex.Response is HttpWebResponse ? ex.Response as HttpWebResponse : throw ex;
      }
      callback(httpWebResponse);
      httpWebResponse.Close();
    }

    private void ResponseCallback(IAsyncResult result, Action<HttpResponse> callback)
    {
      HttpResponse response = new HttpResponse()
      {
        ResponseStatus = ResponseStatus.None
      };
      try
      {
        if (this._timeoutState.TimedOut)
        {
          response.ResponseStatus = ResponseStatus.TimedOut;
          Http.ExecuteCallback(response, callback);
        }
        else
          Http.GetRawResponseAsync(result, (Action<HttpWebResponse>) (webResponse =>
          {
            Http.ExtractResponseData(response, webResponse);
            Http.ExecuteCallback(response, callback);
          }));
      }
      catch (Exception ex)
      {
        if (ex is WebException && ((WebException) ex).Status == WebExceptionStatus.RequestCanceled)
        {
          response.ResponseStatus = ResponseStatus.Aborted;
          Http.ExecuteCallback(response, callback);
        }
        else
        {
          response.ErrorMessage = ex.Message;
          response.ErrorException = ex;
          response.ResponseStatus = ResponseStatus.Error;
          Http.ExecuteCallback(response, callback);
        }
      }
    }

    private static void ExecuteCallback(HttpResponse response, Action<HttpResponse> callback)
    {
      callback(response);
    }

    private void AddAsyncHeaderActions()
    {
    }

    private HttpWebRequest ConfigureAsyncWebRequest(string method, Uri url)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(url);
      webRequest.UseDefaultCredentials = false;
      this.AppendHeaders(webRequest);
      this.AppendCookies(webRequest);
      webRequest.Method = method;
      if (!this.HasFiles)
        webRequest.ContentLength = 0L;
      if (this.Credentials != null)
        webRequest.Credentials = this.Credentials;
      if (this.UserAgent.HasValue())
        webRequest.UserAgent = this.UserAgent;
      if (this.ClientCertificates != null)
        webRequest.ClientCertificates = this.ClientCertificates;
      webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
      ServicePointManager.Expect100Continue = false;
      if (this.Timeout != 0)
        webRequest.Timeout = this.Timeout;
      if (this.Proxy != null)
        webRequest.Proxy = this.Proxy;
      if (this.FollowRedirects && this.MaxRedirects.HasValue)
        webRequest.MaximumAutomaticRedirections = this.MaxRedirects.Value;
      webRequest.AllowAutoRedirect = this.FollowRedirects;
      return webRequest;
    }

    public IHttp Create() => (IHttp) new Http();

    protected bool HasParameters => this.Parameters.Any<HttpParameter>();

    protected bool HasCookies => this.Cookies.Any<HttpCookie>();

    protected bool HasBody => !string.IsNullOrEmpty(this.RequestBody);

    protected bool HasFiles => this.Files.Any<HttpFile>();

    public string UserAgent { get; set; }

    public int Timeout { get; set; }

    public ICredentials Credentials { get; set; }

    public CookieContainer CookieContainer { get; set; }

    public IList<HttpFile> Files { get; private set; }

    public bool FollowRedirects { get; set; }

    public X509CertificateCollection ClientCertificates { get; set; }

    public int? MaxRedirects { get; set; }

    public IList<HttpHeader> Headers { get; private set; }

    public IList<HttpParameter> Parameters { get; private set; }

    public IList<HttpCookie> Cookies { get; private set; }

    public string RequestBody { get; set; }

    public string RequestContentType { get; set; }

    public Uri Url { get; set; }

    public IWebProxy Proxy { get; set; }

    public Http()
    {
      this.Headers = (IList<HttpHeader>) new List<HttpHeader>();
      this.Files = (IList<HttpFile>) new List<HttpFile>();
      this.Parameters = (IList<HttpParameter>) new List<HttpParameter>();
      this.Cookies = (IList<HttpCookie>) new List<HttpCookie>();
      this._restrictedHeaderActions = (IDictionary<string, Action<HttpWebRequest, string>>) new Dictionary<string, Action<HttpWebRequest, string>>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.AddSharedHeaderActions();
      this.AddSyncHeaderActions();
    }

    private void AddSharedHeaderActions()
    {
      this._restrictedHeaderActions.Add("Accept", (Action<HttpWebRequest, string>) ((r, v) => r.Accept = v));
      this._restrictedHeaderActions.Add("Content-Type", (Action<HttpWebRequest, string>) ((r, v) => r.ContentType = v));
      this._restrictedHeaderActions.Add("Date", (Action<HttpWebRequest, string>) ((r, v) =>
      {
        DateTime result;
        if (!DateTime.TryParse(v, out result))
          return;
        r.Date = result;
      }));
      this._restrictedHeaderActions.Add("Host", (Action<HttpWebRequest, string>) ((r, v) => r.Host = v));
      this._restrictedHeaderActions.Add("Range", (Action<HttpWebRequest, string>) ((r, v) => this.AddRange(r, v)));
    }

    private static string GetMultipartFormContentType()
    {
      return string.Format("multipart/form-data; boundary={0}", (object) "-----------------------------28947758029299");
    }

    private static string GetMultipartFileHeader(HttpFile file)
    {
      return string.Format("--{0}{4}Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"{4}Content-Type: {3}{4}{4}", (object) "-----------------------------28947758029299", (object) file.Name, (object) file.FileName, (object) (file.ContentType ?? "application/octet-stream"), (object) "\r\n");
    }

    private static string GetMultipartFormData(HttpParameter param)
    {
      return string.Format("--{0}{3}Content-Disposition: form-data; name=\"{1}\"{3}{3}{2}{3}", (object) "-----------------------------28947758029299", (object) param.Name, (object) param.Value, (object) "\r\n");
    }

    private static string GetMultipartFooter()
    {
      return string.Format("--{0}--{1}", (object) "-----------------------------28947758029299", (object) "\r\n");
    }

    private void AppendHeaders(HttpWebRequest webRequest)
    {
      foreach (HttpHeader header in (IEnumerable<HttpHeader>) this.Headers)
      {
        if (this._restrictedHeaderActions.ContainsKey(header.Name))
          this._restrictedHeaderActions[header.Name](webRequest, header.Value);
        else
          webRequest.Headers.Add(header.Name, header.Value);
      }
    }

    private void AppendCookies(HttpWebRequest webRequest)
    {
      webRequest.CookieContainer = this.CookieContainer ?? new CookieContainer();
      foreach (HttpCookie cookie1 in (IEnumerable<HttpCookie>) this.Cookies)
      {
        Cookie cookie2 = new Cookie()
        {
          Name = cookie1.Name,
          Value = cookie1.Value,
          Domain = webRequest.RequestUri.Host
        };
        webRequest.CookieContainer.Add(cookie2);
      }
    }

    private string EncodeParameters()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (HttpParameter parameter in (IEnumerable<HttpParameter>) this.Parameters)
      {
        if (stringBuilder.Length > 1)
          stringBuilder.Append("&");
        stringBuilder.AppendFormat("{0}={1}", (object) parameter.Name.UrlEncode(), (object) parameter.Value.UrlEncode());
      }
      return stringBuilder.ToString();
    }

    private void PreparePostBody(HttpWebRequest webRequest)
    {
      if (this.HasFiles)
        webRequest.ContentType = Http.GetMultipartFormContentType();
      else if (this.HasParameters)
      {
        webRequest.ContentType = "application/x-www-form-urlencoded";
        this.RequestBody = this.EncodeParameters();
      }
      else
      {
        if (!this.HasBody)
          return;
        webRequest.ContentType = this.RequestContentType;
      }
    }

    private static void WriteStringTo(Stream stream, string toWrite)
    {
      byte[] bytes = Http._defaultEncoding.GetBytes(toWrite);
      stream.Write(bytes, 0, bytes.Length);
    }

    private void WriteMultipartFormData(Stream requestStream)
    {
      foreach (HttpParameter parameter in (IEnumerable<HttpParameter>) this.Parameters)
        Http.WriteStringTo(requestStream, Http.GetMultipartFormData(parameter));
      foreach (HttpFile file in (IEnumerable<HttpFile>) this.Files)
      {
        Http.WriteStringTo(requestStream, Http.GetMultipartFileHeader(file));
        file.Writer(requestStream);
        Http.WriteStringTo(requestStream, "\r\n");
      }
      Http.WriteStringTo(requestStream, Http.GetMultipartFooter());
    }

    private static void ExtractResponseData(HttpResponse response, HttpWebResponse webResponse)
    {
      using (webResponse)
      {
        response.ContentEncoding = webResponse.ContentEncoding;
        response.Server = webResponse.Server;
        response.ContentType = webResponse.ContentType;
        response.ContentLength = webResponse.ContentLength;
        response.RawBytes = webResponse.GetResponseStream().ReadAsBytes();
        response.StatusCode = webResponse.StatusCode;
        response.StatusDescription = webResponse.StatusDescription;
        response.ResponseUri = webResponse.ResponseUri;
        response.ResponseStatus = ResponseStatus.Completed;
        if (webResponse.Cookies != null)
        {
          foreach (Cookie cookie in webResponse.Cookies)
            response.Cookies.Add(new HttpCookie()
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
        }
        foreach (string allKey in webResponse.Headers.AllKeys)
        {
          string header = webResponse.Headers[allKey];
          response.Headers.Add(new HttpHeader()
          {
            Name = allKey,
            Value = header
          });
        }
        webResponse.Close();
      }
    }

    private void AddRange(HttpWebRequest r, string range)
    {
      Match match = Regex.Match(range, "=(\\d+)-(\\d+)$");
      if (!match.Success)
        return;
      int int32_1 = Convert.ToInt32(match.Groups[1].Value);
      int int32_2 = Convert.ToInt32(match.Groups[2].Value);
      r.AddRange(int32_1, int32_2);
    }

    public HttpResponse Post() => this.PostPutInternal("POST");

    public HttpResponse Put() => this.PostPutInternal("PUT");

    public HttpResponse Get() => this.GetStyleMethodInternal("GET");

    public HttpResponse Head() => this.GetStyleMethodInternal("HEAD");

    public HttpResponse Options() => this.GetStyleMethodInternal("OPTIONS");

    public HttpResponse Delete() => this.GetStyleMethodInternal("DELETE");

    public HttpResponse Patch() => this.PostPutInternal("PATCH");

    public HttpResponse AsGet(string httpMethod)
    {
      return this.GetStyleMethodInternal(httpMethod.ToUpperInvariant());
    }

    public HttpResponse AsPost(string httpMethod)
    {
      return this.PostPutInternal(httpMethod.ToUpperInvariant());
    }

    private HttpResponse GetStyleMethodInternal(string method)
    {
      return this.GetResponse(this.ConfigureWebRequest(method, this.Url));
    }

    private HttpResponse PostPutInternal(string method)
    {
      HttpWebRequest httpWebRequest = this.ConfigureWebRequest(method, this.Url);
      this.PreparePostData(httpWebRequest);
      this.WriteRequestBody(httpWebRequest);
      return this.GetResponse(httpWebRequest);
    }

    private void AddSyncHeaderActions()
    {
      this._restrictedHeaderActions.Add("Connection", (Action<HttpWebRequest, string>) ((r, v) => r.Connection = v));
      this._restrictedHeaderActions.Add("Content-Length", (Action<HttpWebRequest, string>) ((r, v) => r.ContentLength = Convert.ToInt64(v)));
      this._restrictedHeaderActions.Add("Expect", (Action<HttpWebRequest, string>) ((r, v) => r.Expect = v));
      this._restrictedHeaderActions.Add("If-Modified-Since", (Action<HttpWebRequest, string>) ((r, v) => r.IfModifiedSince = Convert.ToDateTime(v)));
      this._restrictedHeaderActions.Add("Referer", (Action<HttpWebRequest, string>) ((r, v) => r.Referer = v));
      this._restrictedHeaderActions.Add("Transfer-Encoding", (Action<HttpWebRequest, string>) ((r, v) =>
      {
        r.TransferEncoding = v;
        r.SendChunked = true;
      }));
      this._restrictedHeaderActions.Add("User-Agent", (Action<HttpWebRequest, string>) ((r, v) => r.UserAgent = v));
    }

    private HttpResponse GetResponse(HttpWebRequest request)
    {
      HttpResponse response = new HttpResponse();
      response.ResponseStatus = ResponseStatus.None;
      try
      {
        HttpWebResponse rawResponse = Http.GetRawResponse(request);
        Http.ExtractResponseData(response, rawResponse);
      }
      catch (Exception ex)
      {
        response.ErrorMessage = ex.Message;
        response.ErrorException = ex;
        response.ResponseStatus = ResponseStatus.Error;
      }
      return response;
    }

    private static HttpWebResponse GetRawResponse(HttpWebRequest request)
    {
      try
      {
        return (HttpWebResponse) request.GetResponse();
      }
      catch (WebException ex)
      {
        if (ex.Response is HttpWebResponse)
          return ex.Response as HttpWebResponse;
        throw;
      }
    }

    private void PreparePostData(HttpWebRequest webRequest)
    {
      if (this.HasFiles)
      {
        webRequest.ContentType = Http.GetMultipartFormContentType();
        using (Stream requestStream = webRequest.GetRequestStream())
          this.WriteMultipartFormData(requestStream);
      }
      this.PreparePostBody(webRequest);
    }

    private void WriteRequestBody(HttpWebRequest webRequest)
    {
      if (!this.HasBody)
        return;
      byte[] bytes = Http._defaultEncoding.GetBytes(this.RequestBody);
      webRequest.ContentLength = (long) bytes.Length;
      using (Stream requestStream = webRequest.GetRequestStream())
        requestStream.Write(bytes, 0, bytes.Length);
    }

    private HttpWebRequest ConfigureWebRequest(string method, Uri url)
    {
      HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(url);
      webRequest.UseDefaultCredentials = false;
      ServicePointManager.Expect100Continue = false;
      this.AppendHeaders(webRequest);
      this.AppendCookies(webRequest);
      webRequest.Method = method;
      if (!this.HasFiles)
        webRequest.ContentLength = 0L;
      webRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
      if (this.ClientCertificates != null)
        webRequest.ClientCertificates = this.ClientCertificates;
      if (this.UserAgent.HasValue())
        webRequest.UserAgent = this.UserAgent;
      if (this.Timeout != 0)
        webRequest.Timeout = this.Timeout;
      if (this.Credentials != null)
        webRequest.Credentials = this.Credentials;
      if (this.Proxy != null)
        webRequest.Proxy = this.Proxy;
      webRequest.AllowAutoRedirect = this.FollowRedirects;
      if (this.FollowRedirects && this.MaxRedirects.HasValue)
        webRequest.MaximumAutomaticRedirections = this.MaxRedirects.Value;
      return webRequest;
    }

    private class TimeOutState
    {
      public bool TimedOut { get; set; }

      public HttpWebRequest Request { get; set; }
    }
  }
}
