// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.PartialViewResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Text;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class PartialViewResult : ViewResultBase
  {
    protected override ViewEngineResult FindView(ControllerContext context)
    {
      ViewEngineResult partialView = this.ViewEngineCollection.FindPartialView(context, this.ViewName);
      if (partialView.View != null)
        return partialView;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string searchedLocation in partialView.SearchedLocations)
      {
        stringBuilder.AppendLine();
        stringBuilder.Append(searchedLocation);
      }
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_PartialViewNotFound, new object[2]
      {
        (object) this.ViewName,
        (object) stringBuilder
      }));
    }
  }
}
