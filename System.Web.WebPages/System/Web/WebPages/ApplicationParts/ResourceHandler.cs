// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.ApplicationParts.ResourceHandler
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using Microsoft.Internal.Web.Utils;
using System.Globalization;
using System.IO;
using System.Web.WebPages.Resources;

#nullable disable
namespace System.Web.WebPages.ApplicationParts
{
  internal class ResourceHandler : IHttpHandler
  {
    private readonly string _path;
    private readonly ApplicationPart _applicationPart;

    public ResourceHandler(ApplicationPart applicationPart, string path)
    {
      if (applicationPart == null)
        throw new ArgumentNullException(nameof (applicationPart));
      if (string.IsNullOrEmpty(path))
        throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, nameof (path));
      this._applicationPart = applicationPart;
      this._path = path;
    }

    public bool IsReusable => true;

    public void ProcessRequest(HttpContext context)
    {
      this.ProcessRequest((HttpResponseBase) new HttpResponseWrapper(context.Response));
    }

    internal void ProcessRequest(HttpResponseBase response)
    {
      string str = this._path;
      if (!str.StartsWith("~/", StringComparison.Ordinal))
        str = "~/" + str;
      using (Stream resourceStream = this._applicationPart.GetResourceStream(str))
      {
        if (resourceStream == null)
          throw new HttpException(404, string.Format((IFormatProvider) CultureInfo.CurrentCulture, WebPageResources.ApplicationPart_ResourceNotFound, new object[1]
          {
            (object) this._path
          }));
        response.ContentType = Microsoft.Internal.Web.Utils.MimeMapping.GetMimeMapping(str);
        resourceStream.CopyTo(response.OutputStream);
      }
    }
  }
}
