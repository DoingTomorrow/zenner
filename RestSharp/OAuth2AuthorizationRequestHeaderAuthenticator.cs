// Decompiled with JetBrains decompiler
// Type: RestSharp.OAuth2AuthorizationRequestHeaderAuthenticator
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Linq;

#nullable disable
namespace RestSharp
{
  public class OAuth2AuthorizationRequestHeaderAuthenticator : OAuth2Authenticator
  {
    private readonly string _authorizationValue;

    public OAuth2AuthorizationRequestHeaderAuthenticator(string accessToken)
      : base(accessToken)
    {
      this._authorizationValue = "OAuth " + accessToken;
    }

    public override void Authenticate(IRestClient client, IRestRequest request)
    {
      if (request.Parameters.Any<Parameter>((Func<Parameter, bool>) (p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase))))
        return;
      request.AddParameter("Authorization", (object) this._authorizationValue, ParameterType.HttpHeader);
    }
  }
}
