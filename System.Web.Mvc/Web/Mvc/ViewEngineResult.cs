// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ViewEngineResult
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.Mvc
{
  public class ViewEngineResult
  {
    public ViewEngineResult(IEnumerable<string> searchedLocations)
    {
      this.SearchedLocations = searchedLocations != null ? searchedLocations : throw new ArgumentNullException(nameof (searchedLocations));
    }

    public ViewEngineResult(IView view, IViewEngine viewEngine)
    {
      if (view == null)
        throw new ArgumentNullException(nameof (view));
      if (viewEngine == null)
        throw new ArgumentNullException(nameof (viewEngine));
      this.View = view;
      this.ViewEngine = viewEngine;
    }

    public IEnumerable<string> SearchedLocations { get; private set; }

    public IView View { get; private set; }

    public IViewEngine ViewEngine { get; private set; }
  }
}
