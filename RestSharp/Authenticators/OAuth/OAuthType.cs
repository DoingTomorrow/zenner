// Decompiled with JetBrains decompiler
// Type: RestSharp.Authenticators.OAuth.OAuthType
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;

#nullable disable
namespace RestSharp.Authenticators.OAuth
{
  [Serializable]
  public enum OAuthType
  {
    RequestToken,
    AccessToken,
    ProtectedResource,
    ClientAuthentication,
  }
}
