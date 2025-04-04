// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Converters.IsNullConverter
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace MahApps.Metro.Converters
{
  public sealed class IsNullConverter : IValueConverter
  {
    private static IsNullConverter _instance;

    private IsNullConverter()
    {
    }

    public static IsNullConverter Instance
    {
      get => IsNullConverter._instance ?? (IsNullConverter._instance = new IsNullConverter());
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (value == null);
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }
}
