// Decompiled with JetBrains decompiler
// Type: CommonWPF.HeaderImageConverter
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

#nullable disable
namespace CommonWPF
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
