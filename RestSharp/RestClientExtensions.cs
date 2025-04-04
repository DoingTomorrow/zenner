// Decompiled with JetBrains decompiler
// Type: RestSharp.RestClientExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;

#nullable disable
namespace RestSharp
{
  public static class RestClientExtensions
  {
    public static RestRequestAsyncHandle ExecuteAsync(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse> callback)
    {
      return client.ExecuteAsync(request, (Action<IRestResponse, RestRequestAsyncHandle>) ((response, handle) => callback(response)));
    }

    public static RestRequestAsyncHandle ExecuteAsync<T>(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse<T>> callback)
      where T : new()
    {
      return client.ExecuteAsync<T>(request, (Action<IRestResponse<T>, RestRequestAsyncHandle>) ((response, asyncHandle) => callback(response)));
    }

    public static RestRequestAsyncHandle GetAsync<T>(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
      where T : new()
    {
      request.Method = Method.GET;
      return client.ExecuteAsync<T>(request, callback);
    }

    public static RestRequestAsyncHandle PostAsync<T>(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
      where T : new()
    {
      request.Method = Method.POST;
      return client.ExecuteAsync<T>(request, callback);
    }

    public static RestRequestAsyncHandle PutAsync<T>(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
      where T : new()
    {
      request.Method = Method.PUT;
      return client.ExecuteAsync<T>(request, callback);
    }

    public static RestRequestAsyncHandle HeadAsync<T>(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
      where T : new()
    {
      request.Method = Method.HEAD;
      return client.ExecuteAsync<T>(request, callback);
    }

    public static RestRequestAsyncHandle OptionsAsync<T>(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
      where T : new()
    {
      request.Method = Method.OPTIONS;
      return client.ExecuteAsync<T>(request, callback);
    }

    public static RestRequestAsyncHandle PatchAsync<T>(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
      where T : new()
    {
      request.Method = Method.PATCH;
      return client.ExecuteAsync<T>(request, callback);
    }

    public static RestRequestAsyncHandle DeleteAsync<T>(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
      where T : new()
    {
      request.Method = Method.DELETE;
      return client.ExecuteAsync<T>(request, callback);
    }

    public static RestRequestAsyncHandle GetAsync(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      request.Method = Method.GET;
      return client.ExecuteAsync(request, callback);
    }

    public static RestRequestAsyncHandle PostAsync(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      request.Method = Method.POST;
      return client.ExecuteAsync(request, callback);
    }

    public static RestRequestAsyncHandle PutAsync(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      request.Method = Method.PUT;
      return client.ExecuteAsync(request, callback);
    }

    public static RestRequestAsyncHandle HeadAsync(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      request.Method = Method.HEAD;
      return client.ExecuteAsync(request, callback);
    }

    public static RestRequestAsyncHandle OptionsAsync(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      request.Method = Method.OPTIONS;
      return client.ExecuteAsync(request, callback);
    }

    public static RestRequestAsyncHandle PatchAsync(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      request.Method = Method.PATCH;
      return client.ExecuteAsync(request, callback);
    }

    public static RestRequestAsyncHandle DeleteAsync(
      this IRestClient client,
      IRestRequest request,
      Action<IRestResponse, RestRequestAsyncHandle> callback)
    {
      request.Method = Method.DELETE;
      return client.ExecuteAsync(request, callback);
    }

    public static IRestResponse<T> Get<T>(this IRestClient client, IRestRequest request) where T : new()
    {
      request.Method = Method.GET;
      return client.Execute<T>(request);
    }

    public static IRestResponse<T> Post<T>(this IRestClient client, IRestRequest request) where T : new()
    {
      request.Method = Method.POST;
      return client.Execute<T>(request);
    }

    public static IRestResponse<T> Put<T>(this IRestClient client, IRestRequest request) where T : new()
    {
      request.Method = Method.PUT;
      return client.Execute<T>(request);
    }

    public static IRestResponse<T> Head<T>(this IRestClient client, IRestRequest request) where T : new()
    {
      request.Method = Method.HEAD;
      return client.Execute<T>(request);
    }

    public static IRestResponse<T> Options<T>(this IRestClient client, IRestRequest request) where T : new()
    {
      request.Method = Method.OPTIONS;
      return client.Execute<T>(request);
    }

    public static IRestResponse<T> Patch<T>(this IRestClient client, IRestRequest request) where T : new()
    {
      request.Method = Method.PATCH;
      return client.Execute<T>(request);
    }

    public static IRestResponse<T> Delete<T>(this IRestClient client, IRestRequest request) where T : new()
    {
      request.Method = Method.DELETE;
      return client.Execute<T>(request);
    }

    public static IRestResponse Get(this IRestClient client, IRestRequest request)
    {
      request.Method = Method.GET;
      return client.Execute(request);
    }

    public static IRestResponse Post(this IRestClient client, IRestRequest request)
    {
      request.Method = Method.POST;
      return client.Execute(request);
    }

    public static IRestResponse Put(this IRestClient client, IRestRequest request)
    {
      request.Method = Method.PUT;
      return client.Execute(request);
    }

    public static IRestResponse Head(this IRestClient client, IRestRequest request)
    {
      request.Method = Method.HEAD;
      return client.Execute(request);
    }

    public static IRestResponse Options(this IRestClient client, IRestRequest request)
    {
      request.Method = Method.OPTIONS;
      return client.Execute(request);
    }

    public static IRestResponse Patch(this IRestClient client, IRestRequest request)
    {
      request.Method = Method.PATCH;
      return client.Execute(request);
    }

    public static IRestResponse Delete(this IRestClient client, IRestRequest request)
    {
      request.Method = Method.DELETE;
      return client.Execute(request);
    }

    public static void AddDefaultParameter(this IRestClient restClient, Parameter p)
    {
      if (p.Type == ParameterType.RequestBody)
        throw new NotSupportedException("Cannot set request body from default headers. Use Request.AddBody() instead.");
      restClient.DefaultParameters.Add(p);
    }

    public static void AddDefaultParameter(this IRestClient restClient, string name, object value)
    {
      restClient.AddDefaultParameter(new Parameter()
      {
        Name = name,
        Value = value,
        Type = ParameterType.GetOrPost
      });
    }

    public static void AddDefaultParameter(
      this IRestClient restClient,
      string name,
      object value,
      ParameterType type)
    {
      restClient.AddDefaultParameter(new Parameter()
      {
        Name = name,
        Value = value,
        Type = type
      });
    }

    public static void AddDefaultHeader(this IRestClient restClient, string name, string value)
    {
      restClient.AddDefaultParameter(name, (object) value, ParameterType.HttpHeader);
    }

    public static void AddDefaultUrlSegment(this IRestClient restClient, string name, string value)
    {
      restClient.AddDefaultParameter(name, (object) value, ParameterType.UrlSegment);
    }

    public static RestResponse<object> ExecuteDynamic(this IRestClient client, IRestRequest request)
    {
      IRestResponse restResponse1 = client.Execute(request);
      RestResponse<object> restResponse2 = (RestResponse<object>) restResponse1;
      object obj = SimpleJson.DeserializeObject(restResponse1.Content);
      restResponse2.Data = obj;
      return restResponse2;
    }
  }
}
