// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ControllerInstanceFilterProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.Mvc
{
  public class ControllerInstanceFilterProvider : IFilterProvider
  {
    public IEnumerable<Filter> GetFilters(
      ControllerContext controllerContext,
      ActionDescriptor actionDescriptor)
    {
      if (controllerContext.Controller != null)
        yield return new Filter((object) controllerContext.Controller, FilterScope.First, new int?(int.MinValue));
    }
  }
}
