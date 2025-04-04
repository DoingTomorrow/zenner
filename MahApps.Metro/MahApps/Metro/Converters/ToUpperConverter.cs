// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.ToUpperConverter
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
  public class ToUpperConverter : MarkupConverter
  {
    private static ToUpperConverter _instance;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return (object) ToUpperConverter._instance ?? (object) (ToUpperConverter._instance = new ToUpperConverter());
    }

    protected override object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return !(value is string str) ? value : (object) str.ToUpper();
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
