// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.UrlRewriterHelper
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Specialized;

#nullable disable
namespace System.Web.Mvc
{
  internal class UrlRewriterHelper
  {
    private const string UrlWasRewrittenServerVar = "IIS_WasUrlRewritten";
    private const string UrlRewriterEnabledServerVar = "IIS_UrlRewriteModule";
    private object _lockObject = new object();
    private bool _urlRewriterIsTurnedOnValue;
    private volatile bool _urlRewriterIsTurnedOnCalculated;

    private static bool WasThisRequestRewritten(HttpContextBase httpContext)
    {
      NameValueCollection serverVariables = httpContext.Request.ServerVariables;
      return serverVariables != null && serverVariables["IIS_WasUrlRewritten"] != null;
    }

    private bool IsUrlRewriterTurnedOn(HttpContextBase httpContext)
    {
      if (!this._urlRewriterIsTurnedOnCalculated)
      {
        lock (this._lockObject)
        {
          if (!this._urlRewriterIsTurnedOnCalculated)
          {
            NameValueCollection serverVariables = httpContext.Request.ServerVariables;
            this._urlRewriterIsTurnedOnValue = serverVariables != null && serverVariables["IIS_UrlRewriteModule"] != null;
            this._urlRewriterIsTurnedOnCalculated = true;
          }
        }
      }
      return this._urlRewriterIsTurnedOnValue;
    }

    public virtual bool WasRequestRewritten(HttpContextBase httpContext)
    {
      return this.IsUrlRewriterTurnedOn(httpContext) && UrlRewriterHelper.WasThisRequestRewritten(httpContext);
    }
  }
}
