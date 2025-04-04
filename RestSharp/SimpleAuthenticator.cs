// Decompiled with JetBrains decompiler
// Type: RestSharp.SimpleAuthenticator
// Assembly: RestSharp, Version=104.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66303BA4-9448-422A-B110-F461216F493A
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\RestSharp.dll

#nullable disable
namespace RestSharp
{
  public class SimpleAuthenticator : IAuthenticator
  {
    private readonly string _usernameKey;
    private readonly string _username;
    private readonly string _passwordKey;
    private readonly string _password;

    public SimpleAuthenticator(
      string usernameKey,
      string username,
      string passwordKey,
      string password)
    {
      this._usernameKey = usernameKey;
      this._username = username;
      this._passwordKey = passwordKey;
      this._password = password;
    }

    public void Authenticate(IRestClient client, IRestRequest request)
    {
      request.AddParameter(this._usernameKey, (object) this._username);
      request.AddParameter(this._passwordKey, (object) this._password);
    }
  }
}
