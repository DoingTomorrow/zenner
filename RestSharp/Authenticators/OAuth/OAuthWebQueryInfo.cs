// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.OAuthWebQueryInfo
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;

#nullable disable
namespace RestSharp.Authenticators.OAuth
{
  [Serializable]
  public class OAuthWebQueryInfo
  {
    public virtual string ConsumerKey { get; set; }

    public virtual string Token { get; set; }

    public virtual string Nonce { get; set; }

    public virtual string Timestamp { get; set; }

    public virtual string SignatureMethod { get; set; }

    public virtual string Signature { get; set; }

    public virtual string Version { get; set; }

    public virtual string Callback { get; set; }

    public virtual string Verifier { get; set; }

    public virtual string ClientMode { get; set; }

    public virtual string ClientUsername { get; set; }

    public virtual string ClientPassword { get; set; }

    public virtual string UserAgent { get; set; }

    public virtual string WebMethod { get; set; }

    public virtual OAuthParameterHandling ParameterHandling { get; set; }

    public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }

    internal virtual string ConsumerSecret { get; set; }

    internal virtual string TokenSecret { get; set; }
  }
}
