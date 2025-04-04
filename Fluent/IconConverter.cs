// Decompiled with JetBrains decompiler
// Type: Fluent.IconConverter
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Fluent
{
  public sealed class IconConverter : IValueConverter
  {
    object IValueConverter.Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (value == null)
      {
        Process currentProcess = Process.GetCurrentProcess();
        if (currentProcess.MainWindowHandle != IntPtr.Zero)
          return (object) (IconConverter.GetDefaultIcon(currentProcess.MainWindowHandle) as BitmapFrame);
        try
        {
          return (object) (IconConverter.GetDefaultIcon(new WindowInteropHelper(Application.Current.MainWindow).Handle) as BitmapFrame);
        }
        catch (InvalidOperationException ex)
        {
          return (object) null;
        }
      }
      else
      {
        if (!(value is BitmapFrame bitmapFrame) || bitmapFrame.Decoder == null)
          return (object) null;
        foreach (BitmapFrame frame in bitmapFrame.Decoder.Frames)
        {
          BitmapSource thumbnail = IconConverter.GetThumbnail(frame);
          if (thumbnail != null)
            return (object) thumbnail;
        }
        return value;
      }
    }

    private static BitmapSource GetThumbnail(BitmapFrame frame)
    {
      try
      {
        return frame != null && frame.PixelWidth == 16 && frame.PixelHeight == 16 && (frame.Format == PixelFormats.Bgra32 || frame.Format == PixelFormats.Bgr24) ? (BitmapSource) frame : (BitmapSource) null;
      }
      catch (Exception ex)
      {
        return (BitmapSource) null;
      }
    }

    [SuppressMessage("Microsoft.Design", "CA1031")]
    private static ImageSource GetDefaultIcon(IntPtr hwnd)
    {
      if (hwnd != IntPtr.Zero)
      {
        try
        {
          IntPtr icon = NativeMethods.SendMessage(hwnd, (int) sbyte.MaxValue, new IntPtr(2), IntPtr.Zero);
          if (icon == IntPtr.Zero)
            icon = NativeMethods.GetClassLongPtr(hwnd, -34);
          if (icon == IntPtr.Zero)
            icon = NativeMethods.LoadImage(IntPtr.Zero, new IntPtr(32512), 1U, (int) SystemParameters.SmallIconWidth, (int) SystemParameters.SmallIconHeight, 32768U);
          if (icon != IntPtr.Zero)
            return (ImageSource) BitmapFrame.Create(System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(icon, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight((int) SystemParameters.SmallIconWidth, (int) SystemParameters.SmallIconHeight)));
        }
        catch
        {
          return (ImageSource) null;
        }
      }
      return (ImageSource) null;
    }

    object IValueConverter.ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
