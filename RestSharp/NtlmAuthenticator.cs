// Decompiled with JetBrains decompiler
// Type: RestSharp.NtlmAuthenticator
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Net;

#nullable disable
namespace RestSharp
{
  public class NtlmAuthenticator : IAuthenticator
  {
    private readonly ICredentials credentials;

    public NtlmAuthenticator()
      : this(CredentialCache.DefaultCredentials)
    {
    }

    public NtlmAuthenticator(string username, string password)
      : this((ICredentials) new NetworkCredential(username, password))
    {
    }

    public NtlmAuthenticator(ICredentials credentials)
    {
      this.credentials = credentials != null ? credentials : throw new ArgumentNullException(nameof (credentials));
    }

    public void Authenticate(IRestClient client, IRestRequest request)
    {
      request.Credentials = this.credentials;
    }
  }
}
