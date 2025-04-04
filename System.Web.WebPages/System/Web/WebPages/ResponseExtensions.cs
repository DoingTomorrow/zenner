// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ResponseExtensions
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Net;

#nullable disable
namespace System.Web.WebPages
{
  public static class ResponseExtensions
  {
    public static void SetStatus(this HttpResponseBase response, HttpStatusCode httpStatusCode)
    {
      response.SetStatus((int) httpStatusCode);
    }

    public static void SetStatus(this HttpResponseBase response, int httpStatusCode)
    {
      response.StatusCode = httpStatusCode;
      response.End();
    }

    public static void WriteBinary(this HttpResponseBase response, byte[] data, string mimeType)
    {
      response.ContentType = mimeType;
      response.WriteBinary(data);
    }

    public static void WriteBinary(this HttpResponseBase response, byte[] data)
    {
      response.OutputStream.Write(data, 0, data.Length);
    }

    public static void OutputCache(
      this HttpResponseBase response,
      int numberOfSeconds,
      bool sliding = false,
      IEnumerable<string> varyByParams = null,
      IEnumerable<string> varyByHeaders = null,
      IEnumerable<string> varyByContentEncodings = null,
      HttpCacheability cacheability = HttpCacheability.Public)
    {
      ResponseExtensions.OutputCache((HttpContextBase) new HttpContextWrapper(HttpContext.Current), response.Cache, numberOfSeconds, sliding, varyByParams, varyByHeaders, varyByContentEncodings, cacheability);
    }

    internal static void OutputCache(
      HttpContextBase httpContext,
      HttpCachePolicyBase cache,
      int numberOfSeconds,
      bool sliding,
      IEnumerable<string> varyByParams,
      IEnumerable<string> varyByHeaders,
      IEnumerable<string> varyByContentEncodings,
      HttpCacheability cacheability)
    {
      cache.SetCacheability(cacheability);
      cache.SetExpires(httpContext.Timestamp.AddSeconds((double) numberOfSeconds));
      cache.SetMaxAge(new TimeSpan(0, 0, numberOfSeconds));
      cache.SetValidUntilExpires(true);
      cache.SetLastModified(httpContext.Timestamp);
      cache.SetSlidingExpiration(sliding);
      if (varyByParams != null)
      {
        foreach (string varyByParam in varyByParams)
          cache.VaryByParams[varyByParam] = true;
      }
      if (varyByHeaders != null)
      {
        foreach (string varyByHeader in varyByHeaders)
          cache.VaryByHeaders[varyByHeader] = true;
      }
      if (varyByContentEncodings == null)
        return;
      foreach (string byContentEncoding in varyByContentEncodings)
        cache.VaryByContentEncodings[byContentEncoding] = true;
    }
  }
}
