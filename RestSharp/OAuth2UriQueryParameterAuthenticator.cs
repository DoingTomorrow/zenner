// Decompiled with JetBrains decompiler
// Type: RestSharp.OAuth2UriQueryParameterAuthenticator
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

#nullable disable
namespace RestSharp
{
  public class OAuth2UriQueryParameterAuthenticator(string accessToken) : OAuth2Authenticator(accessToken)
  {
    public override void Authenticate(IRestClient client, IRestRequest request)
    {
      request.AddParameter("oauth_token", (object) this.AccessToken, ParameterType.GetOrPost);
    }
  }
}
