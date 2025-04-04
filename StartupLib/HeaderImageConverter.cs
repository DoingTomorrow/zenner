// Decompiled with JetBrains decompiler
// Type: StartupLib.HeaderImageConverter
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#nullable disable
namespace StartupLib
{
  public class HeaderImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      MemoryStream memoryStream = new MemoryStream();
      ((Image) value).Save((Stream) memoryStream, ImageFormat.Bmp);
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      memoryStream.Seek(0L, SeekOrigin.Begin);
      bitmapImage.StreamSource = (Stream) memoryStream;
      bitmapImage.EndInit();
      return (object) bitmapImage;
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
