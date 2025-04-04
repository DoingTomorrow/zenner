// Decompiled with JetBrains decompiler
// Type: RestSharp.OAuth2Authenticator
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

#nullable disable
namespace RestSharp
{
  public abstract class OAuth2Authenticator : IAuthenticator
  {
    private readonly string _accessToken;

    public OAuth2Authenticator(string accessToken) => this._accessToken = accessToken;

    public string AccessToken => this._accessToken;

    public abstract void Authenticate(IRestClient client, IRestRequest request);
  }
}
