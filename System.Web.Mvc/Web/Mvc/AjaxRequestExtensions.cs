// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.AjaxRequestExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

#nullable disable
namespace System.Web.Mvc
{
  public static class AjaxRequestExtensions
  {
    public static bool IsAjaxRequest(this HttpRequestBase request)
    {
      if (request == null)
        throw new ArgumentNullException(nameof (request));
      if (request["X-Requested-With"] == "XMLHttpRequest")
        return true;
      return request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }
  }
}
