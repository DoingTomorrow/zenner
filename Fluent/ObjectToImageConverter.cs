// Decompiled with JetBrains decompiler
// Type: Fluent.ObjectToImageConverter
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Globalization;
using System.Net.Cache;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Fluent
{
  public class ObjectToImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch (value)
      {
        case string _:
          return (object) new Image()
          {
            Stretch = Stretch.None,
            Source = (ImageSource) new BitmapImage(new Uri(value as string, UriKind.RelativeOrAbsolute), new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore))
          };
        case ImageSource _:
          return (object) new Image()
          {
            Stretch = Stretch.None,
            Source = (ImageSource) value
          };
        default:
          return value;
      }
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
