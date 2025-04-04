// Decompiled with JetBrains decompiler
// Type: RestSharp.Extensions.ResponseExtensions
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

#nullable disable
namespace RestSharp.Extensions
{
  public static class ResponseExtensions
  {
    public static IRestResponse<T> toAsyncResponse<T>(this IRestResponse response)
    {
      RestResponse<T> asyncResponse = new RestResponse<T>();
      asyncResponse.ContentEncoding = response.ContentEncoding;
      asyncResponse.ContentLength = response.ContentLength;
      asyncResponse.ContentType = response.ContentType;
      asyncResponse.Cookies = response.Cookies;
      asyncResponse.ErrorMessage = response.ErrorMessage;
      asyncResponse.Headers = response.Headers;
      asyncResponse.RawBytes = response.RawBytes;
      asyncResponse.ResponseStatus = response.ResponseStatus;
      asyncResponse.ResponseUri = response.ResponseUri;
      asyncResponse.Server = response.Server;
      asyncResponse.StatusCode = response.StatusCode;
      asyncResponse.StatusDescription = response.StatusDescription;
      return (IRestResponse<T>) asyncResponse;
    }
  }
}
