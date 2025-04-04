// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.UnvalidatedRequestValues
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Specialized;

#nullable disable
namespace System.Web.Helpers
{
  public sealed class UnvalidatedRequestValues
  {
    private readonly HttpRequestBase _request;
    private readonly Func<NameValueCollection> _formGetter;
    private readonly Func<NameValueCollection> _queryStringGetter;

    internal UnvalidatedRequestValues(
      HttpRequestBase request,
      Func<NameValueCollection> formGetter,
      Func<NameValueCollection> queryStringGetter)
    {
      this._request = request;
      this._formGetter = formGetter;
      this._queryStringGetter = queryStringGetter;
    }

    public NameValueCollection Form => this._formGetter();

    public NameValueCollection QueryString => this._queryStringGetter();

    public string this[string key]
    {
      get
      {
        string str1 = this.QueryString[key];
        if (str1 != null)
          return str1;
        string str2 = this.Form[key];
        if (str2 != null)
          return str2;
        HttpCookie cookie = this._request.Cookies[key];
        return cookie != null ? cookie.Value : this._request.ServerVariables[key] ?? (string) null;
      }
    }
  }
}
