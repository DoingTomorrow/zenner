// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.TemplateStack
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace System.Web.WebPages
{
  public static class TemplateStack
  {
    private static readonly object _contextKey = new object();

    public static ITemplateFile GetCurrentTemplate(HttpContextBase httpContext)
    {
      return httpContext != null ? TemplateStack.GetStack(httpContext).FirstOrDefault<ITemplateFile>() : throw new ArgumentNullException(nameof (httpContext));
    }

    public static ITemplateFile Pop(HttpContextBase httpContext)
    {
      return httpContext != null ? TemplateStack.GetStack(httpContext).Pop() : throw new ArgumentNullException(nameof (httpContext));
    }

    public static void Push(HttpContextBase httpContext, ITemplateFile templateFile)
    {
      if (templateFile == null)
        throw new ArgumentNullException(nameof (templateFile));
      if (httpContext == null)
        throw new ArgumentNullException(nameof (httpContext));
      TemplateStack.GetStack(httpContext).Push(templateFile);
    }

    private static Stack<ITemplateFile> GetStack(HttpContextBase httpContext)
    {
      if (!(httpContext.Items[TemplateStack._contextKey] is Stack<ITemplateFile> stack))
      {
        stack = new Stack<ITemplateFile>();
        httpContext.Items[TemplateStack._contextKey] = (object) stack;
      }
      return stack;
    }
  }
}
