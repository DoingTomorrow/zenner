// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.SessionStateUtil
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Razor;
using System.Web.SessionState;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages
{
  internal static class SessionStateUtil
  {
    private static readonly ConcurrentDictionary<Type, SessionStateBehavior?> _sessionStateBehaviorCache = new ConcurrentDictionary<Type, SessionStateBehavior?>();

    internal static void SetUpSessionState(HttpContextBase context, IHttpHandler handler)
    {
      SessionStateUtil.SetUpSessionState(context, handler, SessionStateUtil._sessionStateBehaviorCache);
    }

    internal static void SetUpSessionState(
      HttpContextBase context,
      IHttpHandler handler,
      ConcurrentDictionary<Type, SessionStateBehavior?> cache)
    {
      WebPageHttpHandler webPageHttpHandler = handler as WebPageHttpHandler;
      SessionStateBehavior? sessionStateBehavior = SessionStateUtil.GetSessionStateBehavior((WebPageExecutingBase) webPageHttpHandler.RequestedPage, cache);
      if (sessionStateBehavior.HasValue)
      {
        context.SetSessionStateBehavior(sessionStateBehavior.Value);
      }
      else
      {
        WebPageRenderingBase page = webPageHttpHandler.StartPage;
        do
        {
          if (page is StartPage startPage)
          {
            sessionStateBehavior = SessionStateUtil.GetSessionStateBehavior((WebPageExecutingBase) page, cache);
            page = startPage.ChildPage;
          }
        }
        while (startPage != null);
        if (!sessionStateBehavior.HasValue)
          return;
        context.SetSessionStateBehavior(sessionStateBehavior.Value);
      }
    }

    private static SessionStateBehavior? GetSessionStateBehavior(
      WebPageExecutingBase page,
      ConcurrentDictionary<Type, SessionStateBehavior?> cache)
    {
      return cache.GetOrAdd(page.GetType(), (Func<Type, SessionStateBehavior?>) (type =>
      {
        SessionStateBehavior result = SessionStateBehavior.Default;
        Type type1 = type;
        bool flag = false;
        Type attributeType = typeof (RazorDirectiveAttribute);
        int num = flag ? 1 : 0;
        List<RazorDirectiveAttribute> list = ((IEnumerable<RazorDirectiveAttribute>) type1.GetCustomAttributes(attributeType, num != 0)).Where<RazorDirectiveAttribute>((Func<RazorDirectiveAttribute, bool>) (attr => StringComparer.OrdinalIgnoreCase.Equals("sessionstate", attr.Name))).ToList<RazorDirectiveAttribute>();
        if (!list.Any<RazorDirectiveAttribute>())
          return new SessionStateBehavior?();
        RazorDirectiveAttribute directiveAttribute = list.Count <= 1 ? list[0] : throw new InvalidOperationException(WebPageResources.SessionState_TooManyValues);
        bool ignoreCase = true;
        if (!Enum.TryParse<SessionStateBehavior>(directiveAttribute.Value, ignoreCase, out result))
        {
          IEnumerable<string> values = Enum.GetValues(typeof (SessionStateBehavior)).Cast<SessionStateBehavior>().Select<SessionStateBehavior, string>((Func<SessionStateBehavior, string>) (s => s.ToString()));
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.SessionState_InvalidValue, new object[3]
          {
            (object) directiveAttribute.Value,
            (object) page.VirtualPath,
            (object) string.Join(", ", values)
          }));
        }
        return new SessionStateBehavior?(result);
      }));
    }
  }
}
