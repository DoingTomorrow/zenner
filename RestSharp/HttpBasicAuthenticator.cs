// Decompiled with JetBrains decompiler
// Type: RestSharp.HttpBasicAuthenticator
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

using System;
using System.Linq;
using System.Text;

#nullable disable
namespace RestSharp
{
  public class HttpBasicAuthenticator : IAuthenticator
  {
    private readonly string _username;
    private readonly string _password;

    public HttpBasicAuthenticator(string username, string password)
    {
      this._password = password;
      this._username = username;
    }

    public void Authenticate(IRestClient client, IRestRequest request)
    {
      if (request.Parameters.Any<Parameter>((Func<Parameter, bool>) (p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase))))
        return;
      string str = string.Format("Basic {0}", (object) Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", (object) this._username, (object) this._password))));
      request.AddParameter("Authorization", (object) str, ParameterType.HttpHeader);
    }
  }
}
