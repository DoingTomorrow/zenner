// Decompiled with JetBrains decompiler
// Type: System.Web.Helpers.Validation
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Web.Infrastructure.DynamicValidationHelper;
using System.Collections.Specialized;

#nullable disable
namespace System.Web.Helpers
{
  public static class Validation
  {
    public static UnvalidatedRequestValues Unvalidated(this HttpRequestBase request)
    {
      return ((HttpRequest) null).Unvalidated();
    }

    public static UnvalidatedRequestValues Unvalidated(this HttpRequest request)
    {
      HttpContext current = HttpContext.Current;
      Func<NameValueCollection> formGetter;
      Func<NameValueCollection> queryStringGetter;
      ValidationUtility.GetUnvalidatedCollections(current, ref formGetter, ref queryStringGetter);
      return new UnvalidatedRequestValues((HttpRequestBase) new HttpRequestWrapper(current.Request), formGetter, queryStringGetter);
    }

    public static string Unvalidated(this HttpRequestBase request, string key)
    {
      return request.Unvalidated()[key];
    }

    public static string Unvalidated(this HttpRequest request, string key)
    {
      return request.Unvalidated()[key];
    }
  }
}
