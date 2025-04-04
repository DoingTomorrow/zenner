// Decompiled with JetBrains decompiler
// Type: RestSharp.RestRequestAsyncHandle
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System.Net;

#nullable disable
namespace RestSharp
{
  public class RestRequestAsyncHandle
  {
    public HttpWebRequest WebRequest;

    public RestRequestAsyncHandle()
    {
    }

    public RestRequestAsyncHandle(HttpWebRequest webRequest) => this.WebRequest = webRequest;

    public void Abort()
    {
      if (this.WebRequest == null)
        return;
      this.WebRequest.Abort();
    }
  }
}
