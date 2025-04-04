// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth1Authenticator
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Authenticators.OAuth;
using RestSharp.Authenticators.OAuth.Extensions;
using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace RestSharp.Authenticators
{
  public class OAuth1Authenticator : IAuthenticator
  {
    public virtual string Realm { get; set; }

    public virtual OAuthParameterHandling ParameterHandling { get; set; }

    public virtual OAuthSignatureMethod SignatureMethod { get; set; }

    public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }

    internal virtual OAuthType Type { get; set; }

    internal virtual string ConsumerKey { get; set; }

    internal virtual string ConsumerSecret { get; set; }

    internal virtual string Token { get; set; }

    internal virtual string TokenSecret { get; set; }

    internal virtual string Verifier { get; set; }

    internal virtual string Version { get; set; }

    internal virtual string CallbackUrl { get; set; }

    internal virtual string SessionHandle { get; set; }

    internal virtual string ClientUsername { get; set; }

    internal virtual string ClientPassword { get; set; }

    public static OAuth1Authenticator ForRequestToken(string consumerKey, string consumerSecret)
    {
      return new OAuth1Authenticator()
      {
        ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
        SignatureMethod = OAuthSignatureMethod.HmacSha1,
        SignatureTreatment = OAuthSignatureTreatment.Escaped,
        ConsumerKey = consumerKey,
        ConsumerSecret = consumerSecret,
        Type = OAuthType.RequestToken
      };
    }

    public static OAuth1Authenticator ForRequestToken(
      string consumerKey,
      string consumerSecret,
      string callbackUrl)
    {
      OAuth1Authenticator oauth1Authenticator = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
      oauth1Authenticator.CallbackUrl = callbackUrl;
      return oauth1Authenticator;
    }

    public static OAuth1Authenticator ForAccessToken(
      string consumerKey,
      string consumerSecret,
      string token,
      string tokenSecret)
    {
      return new OAuth1Authenticator()
      {
        ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
        SignatureMethod = OAuthSignatureMethod.HmacSha1,
        SignatureTreatment = OAuthSignatureTreatment.Escaped,
        ConsumerKey = consumerKey,
        ConsumerSecret = consumerSecret,
        Token = token,
        TokenSecret = tokenSecret,
        Type = OAuthType.AccessToken
      };
    }

    public static OAuth1Authenticator ForAccessToken(
      string consumerKey,
      string consumerSecret,
      string token,
      string tokenSecret,
      string verifier)
    {
      OAuth1Authenticator oauth1Authenticator = OAuth1Authenticator.ForAccessToken(consumerKey, consumerSecret, token, tokenSecret);
      oauth1Authenticator.Verifier = verifier;
      return oauth1Authenticator;
    }

    public static OAuth1Authenticator ForAccessTokenRefresh(
      string consumerKey,
      string consumerSecret,
      string token,
      string tokenSecret,
      string sessionHandle)
    {
      OAuth1Authenticator oauth1Authenticator = OAuth1Authenticator.ForAccessToken(consumerKey, consumerSecret, token, tokenSecret);
      oauth1Authenticator.SessionHandle = sessionHandle;
      return oauth1Authenticator;
    }

    public static OAuth1Authenticator ForAccessTokenRefresh(
      string consumerKey,
      string consumerSecret,
      string token,
      string tokenSecret,
      string verifier,
      string sessionHandle)
    {
      OAuth1Authenticator oauth1Authenticator = OAuth1Authenticator.ForAccessToken(consumerKey, consumerSecret, token, tokenSecret);
      oauth1Authenticator.SessionHandle = sessionHandle;
      oauth1Authenticator.Verifier = verifier;
      return oauth1Authenticator;
    }

    public static OAuth1Authenticator ForClientAuthentication(
      string consumerKey,
      string consumerSecret,
      string username,
      string password)
    {
      return new OAuth1Authenticator()
      {
        ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
        SignatureMethod = OAuthSignatureMethod.HmacSha1,
        SignatureTreatment = OAuthSignatureTreatment.Escaped,
        ConsumerKey = consumerKey,
        ConsumerSecret = consumerSecret,
        ClientUsername = username,
        ClientPassword = password,
        Type = OAuthType.ClientAuthentication
      };
    }

    public static OAuth1Authenticator ForProtectedResource(
      string consumerKey,
      string consumerSecret,
      string accessToken,
      string accessTokenSecret)
    {
      return new OAuth1Authenticator()
      {
        Type = OAuthType.ProtectedResource,
        ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
        SignatureMethod = OAuthSignatureMethod.HmacSha1,
        SignatureTreatment = OAuthSignatureTreatment.Escaped,
        ConsumerKey = consumerKey,
        ConsumerSecret = consumerSecret,
        Token = accessToken,
        TokenSecret = accessTokenSecret
      };
    }

    public void Authenticate(IRestClient client, IRestRequest request)
    {
      OAuthWorkflow workflow = new OAuthWorkflow()
      {
        ConsumerKey = this.ConsumerKey,
        ConsumerSecret = this.ConsumerSecret,
        ParameterHandling = this.ParameterHandling,
        SignatureMethod = this.SignatureMethod,
        SignatureTreatment = this.SignatureTreatment,
        Verifier = this.Verifier,
        Version = this.Version,
        CallbackUrl = this.CallbackUrl,
        SessionHandle = this.SessionHandle,
        Token = this.Token,
        TokenSecret = this.TokenSecret,
        ClientUsername = this.ClientUsername,
        ClientPassword = this.ClientPassword
      };
      this.AddOAuthData(client, request, workflow);
    }

    private void AddOAuthData(IRestClient client, IRestRequest request, OAuthWorkflow workflow)
    {
      string url = client.BuildUri(request).ToString();
      int length = url.IndexOf('?');
      if (length != -1)
        url = url.Substring(0, length);
      string upperInvariant = request.Method.ToString().ToUpperInvariant();
      WebParameterCollection parameterCollection = new WebParameterCollection();
      foreach (Parameter parameter in client.DefaultParameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.GetOrPost)))
        parameterCollection.Add(new WebPair(parameter.Name, parameter.Value.ToString()));
      foreach (Parameter parameter in request.Parameters.Where<Parameter>((Func<Parameter, bool>) (p => p.Type == ParameterType.GetOrPost)))
        parameterCollection.Add(new WebPair(parameter.Name, parameter.Value.ToString()));
      OAuthWebQueryInfo oauthWebQueryInfo;
      switch (this.Type)
      {
        case OAuthType.RequestToken:
          workflow.RequestTokenUrl = url;
          oauthWebQueryInfo = workflow.BuildRequestTokenInfo(upperInvariant, parameterCollection);
          break;
        case OAuthType.AccessToken:
          workflow.AccessTokenUrl = url;
          oauthWebQueryInfo = workflow.BuildAccessTokenInfo(upperInvariant, parameterCollection);
          break;
        case OAuthType.ProtectedResource:
          oauthWebQueryInfo = workflow.BuildProtectedResourceInfo(upperInvariant, parameterCollection, url);
          break;
        case OAuthType.ClientAuthentication:
          workflow.AccessTokenUrl = url;
          oauthWebQueryInfo = workflow.BuildClientAuthAccessTokenInfo(upperInvariant, parameterCollection);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      switch (this.ParameterHandling)
      {
        case OAuthParameterHandling.HttpAuthorizationHeader:
          parameterCollection.Add("oauth_signature", oauthWebQueryInfo.Signature);
          request.AddHeader("Authorization", this.GetAuthorizationHeader((WebPairCollection) parameterCollection));
          break;
        case OAuthParameterHandling.UrlOrPostParameters:
          parameterCollection.Add("oauth_signature", oauthWebQueryInfo.Signature);
          using (IEnumerator<WebPair> enumerator = parameterCollection.Where<WebPair>((Func<WebPair, bool>) (parameter => !parameter.Name.IsNullOrBlank() && parameter.Name.StartsWith("oauth_"))).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              WebPair current = enumerator.Current;
              request.AddParameter(current.Name, (object) HttpUtility.UrlDecode(current.Value));
            }
            break;
          }
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private string GetAuthorizationHeader(WebPairCollection parameters)
    {
      StringBuilder stringBuilder = new StringBuilder("OAuth ");
      if (!this.Realm.IsNullOrBlank())
        stringBuilder.Append("realm=\"{0}\",".FormatWith((object) OAuthTools.UrlEncodeRelaxed(this.Realm)));
      parameters.Sort((Comparison<WebPair>) ((l, r) => l.Name.CompareTo(r.Name)));
      int num = 0;
      List<WebPair> list = parameters.Where<WebPair>((Func<WebPair, bool>) (parameter =>
      {
        if (parameter.Name.IsNullOrBlank() || parameter.Value.IsNullOrBlank())
          return false;
        return parameter.Name.StartsWith("oauth_") || parameter.Name.StartsWith("x_auth_");
      })).ToList<WebPair>();
      foreach (WebPair webPair in list)
      {
        ++num;
        string format = num < list.Count ? "{0}=\"{1}\"," : "{0}=\"{1}\"";
        stringBuilder.Append(format.FormatWith((object) webPair.Name, (object) webPair.Value));
      }
      return stringBuilder.ToString();
    }
  }
}
