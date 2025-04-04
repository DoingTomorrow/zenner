// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.ToLowerConverter
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace MahApps.Metro.Converters
{
  [MarkupExtensionReturnType(typeof (IValueConverter))]
  public class ToLowerConverter : MarkupConverter
  {
    private static ToLowerConverter _instance;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return (object) ToLowerConverter._instance ?? (object) (ToLowerConverter._instance = new ToLowerConverter());
    }

    protected override object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return !(value is string str) ? value : (object) str.ToLower();
    }

    protected override object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }
}
