// Decompiled with JetBrains decompiler
// Type: CommonWPF.WpfFirmwareToStringConverter
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.Globalization;
using System.Windows.Data;
using ZENNER.CommonLibrary;

#nullable disable
namespace CommonWPF
{
  public class WpfFirmwareToStringConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      value.GetType().ToString();
      uint versionValue;
      if (value.GetType() == typeof (int))
      {
        versionValue = (uint) (int) value;
      }
      else
      {
        if (!(value.GetType() == typeof (uint)))
          return (object) "Illegel version type";
        versionValue = (uint) value;
      }
      return (object) new FirmwareVersion(versionValue).ToString();
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) 0U;
    }
  }
}
