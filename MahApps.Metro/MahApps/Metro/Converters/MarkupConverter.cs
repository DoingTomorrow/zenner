// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.MarkupConverter
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
  [MarkupExtensionReturnType(typeof (IValueConverter))]
  public abstract class MarkupConverter : MarkupExtension, IValueConverter
  {
    public override object ProvideValue(IServiceProvider serviceProvider) => (object) this;

    protected abstract object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture);

    protected abstract object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture);

    object IValueConverter.Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      try
      {
        return this.Convert(value, targetType, parameter, culture);
      }
      catch
      {
        return DependencyProperty.UnsetValue;
      }
    }

    object IValueConverter.ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      try
      {
        return this.ConvertBack(value, targetType, parameter, culture);
      }
      catch
      {
        return DependencyProperty.UnsetValue;
      }
    }
  }
}
