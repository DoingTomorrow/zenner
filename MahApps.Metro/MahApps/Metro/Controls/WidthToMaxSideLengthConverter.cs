// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.WidthToMaxSideLengthConverter
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Globalization;
using System.Windows.Data;

#nullable disable
namespace MahApps.Metro.Controls
{
  internal class WidthToMaxSideLengthConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return value is double num ? (object) (num <= 20.0 ? 20.0 : num) : (object) null;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
