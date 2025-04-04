// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Text;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ViewResult : ViewResultBase
  {
    private string _masterName;

    public string MasterName
    {
      get => this._masterName ?? string.Empty;
      set => this._masterName = value;
    }

    protected override ViewEngineResult FindView(ControllerContext context)
    {
      ViewEngineResult view = this.ViewEngineCollection.FindView(context, this.ViewName, this.MasterName);
      if (view.View != null)
        return view;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string searchedLocation in view.SearchedLocations)
      {
        stringBuilder.AppendLine();
        stringBuilder.Append(searchedLocation);
      }
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.Common_ViewNotFound, new object[2]
      {
        (object) this.ViewName,
        (object) stringBuilder
      }));
    }
  }
}
