// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ITempDataProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.Mvc
{
  public interface ITempDataProvider
  {
    IDictionary<string, object> LoadTempData(ControllerContext controllerContext);

    void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values);
  }
}
