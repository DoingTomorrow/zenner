// Decompiled with JetBrains decompiler
// Type: RestSharp.RestResponse`1
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

#nullable disable
namespace RestSharp
{
  public class RestResponse<T> : RestResponseBase, IRestResponse<T>, IRestResponse
  {
    public T Data { get; set; }

    public static explicit operator RestResponse<T>(RestResponse response)
    {
      RestResponse<T> restResponse = new RestResponse<T>();
      restResponse.ContentEncoding = response.ContentEncoding;
      restResponse.ContentLength = response.ContentLength;
      restResponse.ContentType = response.ContentType;
      restResponse.Cookies = response.Cookies;
      restResponse.ErrorMessage = response.ErrorMessage;
      restResponse.Headers = response.Headers;
      restResponse.RawBytes = response.RawBytes;
      restResponse.ResponseStatus = response.ResponseStatus;
      restResponse.ResponseUri = response.ResponseUri;
      restResponse.Server = response.Server;
      restResponse.StatusCode = response.StatusCode;
      restResponse.StatusDescription = response.StatusDescription;
      restResponse.Request = response.Request;
      return restResponse;
    }
  }
}
