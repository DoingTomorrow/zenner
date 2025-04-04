// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.OAuthWorkflow
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using RestSharp.Authenticators.OAuth.Extensions;
using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

#nullable disable
namespace RestSharp.Authenticators.OAuth
{
  internal class OAuthWorkflow
  {
    public virtual string Version { get; set; }

    public virtual string ConsumerKey { get; set; }

    public virtual string ConsumerSecret { get; set; }

    public virtual string Token { get; set; }

    public virtual string TokenSecret { get; set; }

    public virtual string CallbackUrl { get; set; }

    public virtual string Verifier { get; set; }

    public virtual string SessionHandle { get; set; }

    public virtual OAuthSignatureMethod SignatureMethod { get; set; }

    public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }

    public virtual OAuthParameterHandling ParameterHandling { get; set; }

    public virtual string ClientUsername { get; set; }

    public virtual string ClientPassword { get; set; }

    public virtual string RequestTokenUrl { get; set; }

    public virtual string AccessTokenUrl { get; set; }

    public virtual string AuthorizationUrl { get; set; }

    public OAuthWebQueryInfo BuildRequestTokenInfo(string method)
    {
      return this.BuildRequestTokenInfo(method, (WebParameterCollection) null);
    }

    public virtual OAuthWebQueryInfo BuildRequestTokenInfo(
      string method,
      WebParameterCollection parameters)
    {
      this.ValidateTokenRequestState();
      if (parameters == null)
        parameters = new WebParameterCollection();
      string timestamp = OAuthTools.GetTimestamp();
      string nonce = OAuthTools.GetNonce();
      this.AddAuthParameters((ICollection<WebPair>) parameters, timestamp, nonce);
      string signature = OAuthTools.GetSignature(this.SignatureMethod, this.SignatureTreatment, OAuthTools.ConcatenateRequestElements(method, this.RequestTokenUrl, parameters), this.ConsumerSecret);
      return new OAuthWebQueryInfo()
      {
        WebMethod = method,
        ParameterHandling = this.ParameterHandling,
        ConsumerKey = this.ConsumerKey,
        SignatureMethod = this.SignatureMethod.ToRequestValue(),
        SignatureTreatment = this.SignatureTreatment,
        Signature = signature,
        Timestamp = timestamp,
        Nonce = nonce,
        Version = this.Version ?? "1.0",
        Callback = OAuthTools.UrlEncodeRelaxed(this.CallbackUrl ?? ""),
        TokenSecret = this.TokenSecret,
        ConsumerSecret = this.ConsumerSecret
      };
    }

    public virtual OAuthWebQueryInfo BuildAccessTokenInfo(string method)
    {
      return this.BuildAccessTokenInfo(method, (WebParameterCollection) null);
    }

    public virtual OAuthWebQueryInfo BuildAccessTokenInfo(
      string method,
      WebParameterCollection parameters)
    {
      this.ValidateAccessRequestState();
      if (parameters == null)
        parameters = new WebParameterCollection();
      Uri uri = new Uri(this.AccessTokenUrl);
      string timestamp = OAuthTools.GetTimestamp();
      string nonce = OAuthTools.GetNonce();
      this.AddAuthParameters((ICollection<WebPair>) parameters, timestamp, nonce);
      string signature = OAuthTools.GetSignature(this.SignatureMethod, this.SignatureTreatment, OAuthTools.ConcatenateRequestElements(method, uri.ToString(), parameters), this.ConsumerSecret, this.TokenSecret);
      return new OAuthWebQueryInfo()
      {
        WebMethod = method,
        ParameterHandling = this.ParameterHandling,
        ConsumerKey = this.ConsumerKey,
        Token = this.Token,
        SignatureMethod = this.SignatureMethod.ToRequestValue(),
        SignatureTreatment = this.SignatureTreatment,
        Signature = signature,
        Timestamp = timestamp,
        Nonce = nonce,
        Version = this.Version ?? "1.0",
        Verifier = this.Verifier,
        Callback = this.CallbackUrl,
        TokenSecret = this.TokenSecret,
        ConsumerSecret = this.ConsumerSecret
      };
    }

    public virtual OAuthWebQueryInfo BuildClientAuthAccessTokenInfo(
      string method,
      WebParameterCollection parameters)
    {
      this.ValidateClientAuthAccessRequestState();
      if (parameters == null)
        parameters = new WebParameterCollection();
      Uri uri = new Uri(this.AccessTokenUrl);
      string timestamp = OAuthTools.GetTimestamp();
      string nonce = OAuthTools.GetNonce();
      this.AddXAuthParameters((ICollection<WebPair>) parameters, timestamp, nonce);
      string signature = OAuthTools.GetSignature(this.SignatureMethod, this.SignatureTreatment, OAuthTools.ConcatenateRequestElements(method, uri.ToString(), parameters), this.ConsumerSecret);
      return new OAuthWebQueryInfo()
      {
        WebMethod = method,
        ParameterHandling = this.ParameterHandling,
        ClientMode = "client_auth",
        ClientUsername = this.ClientUsername,
        ClientPassword = this.ClientPassword,
        ConsumerKey = this.ConsumerKey,
        SignatureMethod = this.SignatureMethod.ToRequestValue(),
        SignatureTreatment = this.SignatureTreatment,
        Signature = signature,
        Timestamp = timestamp,
        Nonce = nonce,
        Version = this.Version ?? "1.0",
        TokenSecret = this.TokenSecret,
        ConsumerSecret = this.ConsumerSecret
      };
    }

    public virtual OAuthWebQueryInfo BuildProtectedResourceInfo(
      string method,
      WebParameterCollection parameters,
      string url)
    {
      this.ValidateProtectedResourceState();
      if (parameters == null)
        parameters = new WebParameterCollection();
      NameValueCollection queryString = HttpUtility.ParseQueryString(new Uri(url).Query);
      foreach (string allKey in queryString.AllKeys)
      {
        switch (method.ToUpperInvariant())
        {
          case "POST":
            parameters.Add((WebPair) new HttpPostParameter(allKey, queryString[allKey]));
            break;
          default:
            parameters.Add(allKey, queryString[allKey]);
            break;
        }
      }
      string timestamp = OAuthTools.GetTimestamp();
      string nonce = OAuthTools.GetNonce();
      this.AddAuthParameters((ICollection<WebPair>) parameters, timestamp, nonce);
      string signature = OAuthTools.GetSignature(this.SignatureMethod, this.SignatureTreatment, OAuthTools.ConcatenateRequestElements(method, url, parameters), this.ConsumerSecret, this.TokenSecret);
      return new OAuthWebQueryInfo()
      {
        WebMethod = method,
        ParameterHandling = this.ParameterHandling,
        ConsumerKey = this.ConsumerKey,
        Token = this.Token,
        SignatureMethod = this.SignatureMethod.ToRequestValue(),
        SignatureTreatment = this.SignatureTreatment,
        Signature = signature,
        Timestamp = timestamp,
        Nonce = nonce,
        Version = this.Version ?? "1.0",
        Callback = this.CallbackUrl,
        ConsumerSecret = this.ConsumerSecret,
        TokenSecret = this.TokenSecret
      };
    }

    private void ValidateTokenRequestState()
    {
      if (this.RequestTokenUrl.IsNullOrBlank())
        throw new ArgumentException("You must specify a request token URL");
      if (this.ConsumerKey.IsNullOrBlank())
        throw new ArgumentException("You must specify a consumer key");
      if (this.ConsumerSecret.IsNullOrBlank())
        throw new ArgumentException("You must specify a consumer secret");
    }

    private void ValidateAccessRequestState()
    {
      if (this.AccessTokenUrl.IsNullOrBlank())
        throw new ArgumentException("You must specify an access token URL");
      if (this.ConsumerKey.IsNullOrBlank())
        throw new ArgumentException("You must specify a consumer key");
      if (this.ConsumerSecret.IsNullOrBlank())
        throw new ArgumentException("You must specify a consumer secret");
      if (this.Token.IsNullOrBlank())
        throw new ArgumentException("You must specify a token");
    }

    private void ValidateClientAuthAccessRequestState()
    {
      if (this.AccessTokenUrl.IsNullOrBlank())
        throw new ArgumentException("You must specify an access token URL");
      if (this.ConsumerKey.IsNullOrBlank())
        throw new ArgumentException("You must specify a consumer key");
      if (this.ConsumerSecret.IsNullOrBlank())
        throw new ArgumentException("You must specify a consumer secret");
      if (this.ClientUsername.IsNullOrBlank() || this.ClientPassword.IsNullOrBlank())
        throw new ArgumentException("You must specify user credentials");
    }

    private void ValidateProtectedResourceState()
    {
      if (this.ConsumerKey.IsNullOrBlank())
        throw new ArgumentException("You must specify a consumer key");
      if (this.ConsumerSecret.IsNullOrBlank())
        throw new ArgumentException("You must specify a consumer secret");
    }

    private void AddAuthParameters(ICollection<WebPair> parameters, string timestamp, string nonce)
    {
      WebParameterCollection parameterCollection1 = new WebParameterCollection();
      parameterCollection1.Add(new WebPair("oauth_consumer_key", this.ConsumerKey));
      parameterCollection1.Add(new WebPair("oauth_nonce", nonce));
      parameterCollection1.Add(new WebPair("oauth_signature_method", this.SignatureMethod.ToRequestValue()));
      parameterCollection1.Add(new WebPair("oauth_timestamp", timestamp));
      parameterCollection1.Add(new WebPair("oauth_version", this.Version ?? "1.0"));
      WebParameterCollection parameterCollection2 = parameterCollection1;
      if (!this.Token.IsNullOrBlank())
        parameterCollection2.Add(new WebPair("oauth_token", this.Token));
      if (!this.CallbackUrl.IsNullOrBlank())
        parameterCollection2.Add(new WebPair("oauth_callback", this.CallbackUrl));
      if (!this.Verifier.IsNullOrBlank())
        parameterCollection2.Add(new WebPair("oauth_verifier", this.Verifier));
      if (!this.SessionHandle.IsNullOrBlank())
        parameterCollection2.Add(new WebPair("oauth_session_handle", this.SessionHandle));
      foreach (WebPair webPair in (WebPairCollection) parameterCollection2)
        parameters.Add(webPair);
    }

    private void AddXAuthParameters(
      ICollection<WebPair> parameters,
      string timestamp,
      string nonce)
    {
      WebParameterCollection parameterCollection = new WebParameterCollection();
      parameterCollection.Add(new WebPair("x_auth_username", this.ClientUsername));
      parameterCollection.Add(new WebPair("x_auth_password", this.ClientPassword));
      parameterCollection.Add(new WebPair("x_auth_mode", "client_auth"));
      parameterCollection.Add(new WebPair("oauth_consumer_key", this.ConsumerKey));
      parameterCollection.Add(new WebPair("oauth_signature_method", this.SignatureMethod.ToRequestValue()));
      parameterCollection.Add(new WebPair("oauth_timestamp", timestamp));
      parameterCollection.Add(new WebPair("oauth_nonce", nonce));
      parameterCollection.Add(new WebPair("oauth_version", this.Version ?? "1.0"));
      foreach (WebPair webPair in (WebPairCollection) parameterCollection)
        parameters.Add(webPair);
    }
  }
}
