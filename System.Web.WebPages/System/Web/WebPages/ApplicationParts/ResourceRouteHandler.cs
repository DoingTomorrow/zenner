// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ApplicationParts.ResourceRouteHandler
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Globalization;
using System.Web.Routing;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages.ApplicationParts
{
  internal class ResourceRouteHandler : IRouteHandler
  {
    private ApplicationPartRegistry _partRegistry;

    public ResourceRouteHandler(ApplicationPartRegistry partRegistry)
    {
      this._partRegistry = partRegistry;
    }

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      string requiredString1 = requestContext.RouteData.GetRequiredString("module");
      ApplicationPart applicationPart = this._partRegistry[requiredString1];
      if (applicationPart == null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.ApplicationPart_ModuleCannotBeFound, new object[1]
        {
          (object) requiredString1
        }));
      string requiredString2 = requestContext.RouteData.GetRequiredString("path");
      return (IHttpHandler) new ResourceHandler(applicationPart, requiredString2);
    }
  }
}
