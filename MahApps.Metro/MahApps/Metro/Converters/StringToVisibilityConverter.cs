// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.StringToVisibilityConverter
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

#nullable disable
namespace MahApps.Metro.Converters
{
  [ValueConversion(typeof (string), typeof (Visibility))]
  [MarkupExtensionReturnType(typeof (StringToVisibilityConverter))]
  public class StringToVisibilityConverter : MarkupExtension, IValueConverter
  {
    public StringToVisibilityConverter()
    {
      this.FalseEquivalent = Visibility.Collapsed;
      this.OppositeStringValue = false;
    }

    public Visibility FalseEquivalent { get; set; }

    public bool OppositeStringValue { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return (object) new StringToVisibilityConverter()
      {
        FalseEquivalent = this.FalseEquivalent,
        OppositeStringValue = this.OppositeStringValue
      };
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is string) || !(targetType == typeof (Visibility)))
        return value;
      return this.OppositeStringValue ? (object) (Visibility) (((string) value).ToLower().Equals(string.Empty) ? 0 : (int) this.FalseEquivalent) : (object) (Visibility) (((string) value).ToLower().Equals(string.Empty) ? (int) this.FalseEquivalent : 0);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (!(value is Visibility))
        return value;
      return this.OppositeStringValue ? ((Visibility) value != Visibility.Visible ? (object) "visible" : (object) string.Empty) : ((Visibility) value != Visibility.Visible ? (object) string.Empty : (object) "visible");
    }
  }
}
