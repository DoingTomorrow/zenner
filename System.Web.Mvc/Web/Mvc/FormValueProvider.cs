// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.FormValueProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Helpers;

#nullable disable
namespace System.Web.Mvc
{
  public sealed class FormValueProvider : NameValueCollectionValueProvider
  {
    public FormValueProvider(ControllerContext controllerContext)
      : this(controllerContext, (IUnvalidatedRequestValues) new UnvalidatedRequestValuesWrapper(controllerContext.HttpContext.Request.Unvalidated()))
    {
    }

    internal FormValueProvider(
      ControllerContext controllerContext,
      IUnvalidatedRequestValues unvalidatedValues)
      : base(controllerContext.HttpContext.Request.Form, unvalidatedValues.Form, CultureInfo.CurrentCulture)
    {
    }
  }
}
