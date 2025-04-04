// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ChildActionValueProvider
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace System.Web.Mvc
{
  public sealed class ChildActionValueProvider(ControllerContext controllerContext) : 
    DictionaryValueProvider<object>((IDictionary<string, object>) controllerContext.RouteData.Values, CultureInfo.InvariantCulture)
  {
    private static string _childActionValuesKey = Guid.NewGuid().ToString();

    internal static string ChildActionValuesKey => ChildActionValueProvider._childActionValuesKey;

    public override ValueProviderResult GetValue(string key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      ValueProviderResult valueProviderResult = base.GetValue(ChildActionValueProvider.ChildActionValuesKey);
      return valueProviderResult != null && valueProviderResult.RawValue is DictionaryValueProvider<object> rawValue ? rawValue.GetValue(key) : (ValueProviderResult) null;
    }
  }
}
