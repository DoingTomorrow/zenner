// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.QueryStringValueProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Globalization;
using System.Web.Helpers;

#nullable disable
namespace System.Web.Mvc
{
  public sealed class QueryStringValueProvider : NameValueCollectionValueProvider
  {
    public QueryStringValueProvider(ControllerContext controllerContext)
      : this(controllerContext, (IUnvalidatedRequestValues) new UnvalidatedRequestValuesWrapper(controllerContext.HttpContext.Request.Unvalidated()))
    {
    }

    internal QueryStringValueProvider(
      ControllerContext controllerContext,
      IUnvalidatedRequestValues unvalidatedValues)
      : base(controllerContext.HttpContext.Request.QueryString, unvalidatedValues.QueryString, CultureInfo.InvariantCulture)
    {
    }
  }
}
