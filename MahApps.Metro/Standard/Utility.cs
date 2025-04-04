// Decompiled with JetBrains decompiler
// Type: Standard.Utility
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Standard
{
  internal static class Utility
  {
    private static readonly Random _randomNumberGenerator = new Random();
    private static readonly Version _osVersion = Environment.OSVersion.Version;
    private static int s_bitDepth;

    private static bool _MemCmp(IntPtr left, IntPtr right, long cb)
    {
      int ofs;
      for (ofs = 0; (long) ofs < cb - 8L; ofs += 8)
      {
        if (Marshal.ReadInt64(left, ofs) != Marshal.ReadInt64(right, ofs))
          return false;
      }
      for (; (long) ofs < cb; ++ofs)
      {
        if ((int) Marshal.ReadByte(left, ofs) != (int) Marshal.ReadByte(right, ofs))
          return false;
      }
      return true;
    }

    public static Exception FailableFunction<T>(Func<T> function, out T result)
    {
      return Utility.FailableFunction<T>(5, function, out result);
    }

    public static T FailableFunction<T>(Func<T> function)
    {
      T result;
      Exception exception = Utility.FailableFunction<T>(function, out result);
      if (exception != null)
        throw exception;
      return result;
    }

    public static T FailableFunction<T>(int maxRetries, Func<T> function)
    {
      T result;
      Exception exception = Utility.FailableFunction<T>(maxRetries, function, out result);
      if (exception != null)
        throw exception;
      return result;
    }

    public static Exception FailableFunction<T>(int maxRetries, Func<T> function, out T result)
    {
      int num = 0;
      while (true)
      {
        try
        {
          result = function();
          return (Exception) null;
        }
        catch (Exception ex)
        {
          if (num == maxRetries)
          {
            result = default (T);
            return ex;
          }
        }
        ++num;
      }
    }

    public static string GetHashString(string value)
    {
      using (MD5 md5 = MD5.Create())
        return ((IEnumerable<byte>) md5.ComputeHash(Encoding.UTF8.GetBytes(value))).Aggregate<byte, StringBuilder>(new StringBuilder(), (Func<StringBuilder, byte, StringBuilder>) ((sb, b) => sb.Append(b.ToString("x2", (IFormatProvider) CultureInfo.InvariantCulture)))).ToString();
    }

    public static int GET_X_LPARAM(IntPtr lParam) => Utility.LOWORD(lParam.ToInt32());

    public static int GET_Y_LPARAM(IntPtr lParam) => Utility.HIWORD(lParam.ToInt32());

    public static int HIWORD(int i) => (int) (short) (i >> 16);

    public static int LOWORD(int i) => (int) (short) (i & (int) ushort.MaxValue);

    public static bool AreStreamsEqual(Stream left, Stream right)
    {
      if (left == null)
        return right == null;
      if (right == null)
        return false;
      if (!left.CanRead || !right.CanRead)
        throw new NotSupportedException("The streams can't be read for comparison");
      if (left.Length != right.Length)
        return false;
      int length = (int) left.Length;
      left.Position = 0L;
      right.Position = 0L;
      int num1 = 0;
      int num2 = 0;
      byte[] buffer1 = new byte[512];
      byte[] buffer2 = new byte[512];
      GCHandle gcHandle1 = GCHandle.Alloc((object) buffer1, GCHandleType.Pinned);
      IntPtr left1 = gcHandle1.AddrOfPinnedObject();
      GCHandle gcHandle2 = GCHandle.Alloc((object) buffer2, GCHandleType.Pinned);
      IntPtr right1 = gcHandle2.AddrOfPinnedObject();
      try
      {
        while (num1 < length)
        {
          int cb = left.Read(buffer1, 0, buffer1.Length);
          int num3 = right.Read(buffer2, 0, buffer2.Length);
          if (cb != num3 || !Utility._MemCmp(left1, right1, (long) cb))
            return false;
          num1 += cb;
          num2 += num3;
        }
        return true;
      }
      finally
      {
        gcHandle1.Free();
        gcHandle2.Free();
      }
    }

    public static bool GuidTryParse(string guidString, out Guid guid)
    {
      Verify.IsNeitherNullNorEmpty(guidString, nameof (guidString));
      try
      {
        guid = new Guid(guidString);
        return true;
      }
      catch (FormatException ex)
      {
      }
      catch (OverflowException ex)
      {
      }
      guid = new Guid();
      return false;
    }

    public static bool IsFlagSet(int value, int mask) => (value & mask) != 0;

    public static bool IsFlagSet(uint value, uint mask) => (value & mask) > 0U;

    public static bool IsFlagSet(long value, long mask) => (value & mask) != 0L;

    public static bool IsFlagSet(ulong value, ulong mask) => (value & mask) > 0UL;

    public static bool IsInterfaceImplemented(Type objectType, Type interfaceType)
    {
      return ((IEnumerable<Type>) objectType.GetInterfaces()).Any<Type>((Func<Type, bool>) (type => type == interfaceType));
    }

    public static string SafeCopyFile(
      string sourceFileName,
      string destFileName,
      SafeCopyFileOptions options)
    {
      switch (options)
      {
        case SafeCopyFileOptions.PreserveOriginal:
          if (File.Exists(destFileName))
            return (string) null;
          File.Copy(sourceFileName, destFileName);
          return destFileName;
        case SafeCopyFileOptions.Overwrite:
          File.Copy(sourceFileName, destFileName, true);
          return destFileName;
        case SafeCopyFileOptions.FindBetterName:
          foreach (string fileName in Utility.GenerateFileNames(Path.GetDirectoryName(destFileName), Path.GetFileNameWithoutExtension(destFileName), Path.GetExtension(destFileName)))
          {
            if (!File.Exists(fileName))
            {
              File.Copy(sourceFileName, fileName);
              return fileName;
            }
          }
          return (string) null;
        default:
          throw new ArgumentException("Invalid enumeration value", nameof (options));
      }
    }

    public static void SafeDeleteFile(string path)
    {
      if (string.IsNullOrEmpty(path))
        return;
      File.Delete(path);
    }

    public static void SafeDispose<T>(ref T disposable) where T : IDisposable
    {
      IDisposable disposable1 = (IDisposable) disposable;
      disposable = default (T);
      disposable1?.Dispose();
    }

    public static void GeneratePropertyString(
      StringBuilder source,
      string propertyName,
      string value)
    {
      if (source.Length != 0)
        source.Append(' ');
      source.Append(propertyName);
      source.Append(": ");
      if (string.IsNullOrEmpty(value))
      {
        source.Append("<null>");
      }
      else
      {
        source.Append('"');
        source.Append(value);
        source.Append('"');
      }
    }

    [Obsolete]
    public static string GenerateToString<T>(T @object) where T : struct
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (PropertyInfo property in typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
      {
        if (stringBuilder.Length != 0)
          stringBuilder.Append(", ");
        object obj = property.GetValue((object) @object, (object[]) null);
        string format = obj == null ? "{0}: <null>" : "{0}: \"{1}\"";
        stringBuilder.AppendFormat(format, (object) property.Name, obj);
      }
      return stringBuilder.ToString();
    }

    public static void CopyStream(Stream destination, Stream source)
    {
      destination.Position = 0L;
      if (source.CanSeek)
      {
        source.Position = 0L;
        destination.SetLength(source.Length);
      }
      byte[] buffer = new byte[4096];
      int count;
      do
      {
        count = source.Read(buffer, 0, buffer.Length);
        if (count != 0)
          destination.Write(buffer, 0, count);
      }
      while (buffer.Length == count);
      destination.Position = 0L;
    }

    public static string HashStreamMD5(Stream stm)
    {
      stm.Position = 0L;
      StringBuilder stringBuilder = new StringBuilder();
      using (MD5 md5 = MD5.Create())
      {
        foreach (byte num in md5.ComputeHash(stm))
          stringBuilder.Append(num.ToString("x2", (IFormatProvider) CultureInfo.InvariantCulture));
      }
      return stringBuilder.ToString();
    }

    public static void EnsureDirectory(string path)
    {
      if (!path.EndsWith("\\", StringComparison.Ordinal))
        path += "\\";
      path = Path.GetDirectoryName(path);
      if (Directory.Exists(path))
        return;
      Directory.CreateDirectory(path);
    }

    public static bool MemCmp(byte[] left, byte[] right, int cb)
    {
      GCHandle gcHandle1 = GCHandle.Alloc((object) left, GCHandleType.Pinned);
      IntPtr left1 = gcHandle1.AddrOfPinnedObject();
      GCHandle gcHandle2 = GCHandle.Alloc((object) right, GCHandleType.Pinned);
      IntPtr right1 = gcHandle2.AddrOfPinnedObject();
      bool flag = Utility._MemCmp(left1, right1, (long) cb);
      gcHandle1.Free();
      gcHandle2.Free();
      return flag;
    }

    public static string UrlDecode(string url)
    {
      if (url == null)
        return (string) null;
      Utility._UrlDecoder urlDecoder = new Utility._UrlDecoder(url.Length, Encoding.UTF8);
      int length = url.Length;
      for (int index = 0; index < length; ++index)
      {
        char ch = url[index];
        switch (ch)
        {
          case '%':
            if (index < length - 2)
            {
              if (url[index + 1] == 'u' && index < length - 5)
              {
                int num1 = Utility._HexToInt(url[index + 2]);
                int num2 = Utility._HexToInt(url[index + 3]);
                int num3 = Utility._HexToInt(url[index + 4]);
                int num4 = Utility._HexToInt(url[index + 5]);
                if (num1 >= 0 && num2 >= 0 && num3 >= 0 && num4 >= 0)
                {
                  urlDecoder.AddChar((char) (num1 << 12 | num2 << 8 | num3 << 4 | num4));
                  index += 5;
                  break;
                }
                goto default;
              }
              else
              {
                int num5 = Utility._HexToInt(url[index + 1]);
                int num6 = Utility._HexToInt(url[index + 2]);
                if (num5 >= 0 && num6 >= 0)
                {
                  urlDecoder.AddByte((byte) (num5 << 4 | num6));
                  index += 2;
                  break;
                }
                goto default;
              }
            }
            else
              goto default;
          case '+':
            urlDecoder.AddByte((byte) 32);
            break;
          default:
            if (((int) ch & 65408) == 0)
            {
              urlDecoder.AddByte((byte) ch);
              break;
            }
            urlDecoder.AddChar(ch);
            break;
        }
      }
      return urlDecoder.GetString();
    }

    public static string UrlEncode(string url)
    {
      if (url == null)
        return (string) null;
      byte[] bytes = Encoding.UTF8.GetBytes(url);
      bool flag = false;
      int num1 = 0;
      foreach (byte b in bytes)
      {
        if (b == (byte) 32)
          flag = true;
        else if (!Utility._UrlEncodeIsSafe(b))
        {
          ++num1;
          flag = true;
        }
      }
      if (flag)
      {
        byte[] numArray1 = new byte[bytes.Length + num1 * 2];
        int num2 = 0;
        foreach (byte b in bytes)
        {
          if (Utility._UrlEncodeIsSafe(b))
            numArray1[num2++] = b;
          else if (b == (byte) 32)
          {
            numArray1[num2++] = (byte) 43;
          }
          else
          {
            byte[] numArray2 = numArray1;
            int index1 = num2;
            int num3 = index1 + 1;
            numArray2[index1] = (byte) 37;
            byte[] numArray3 = numArray1;
            int index2 = num3;
            int num4 = index2 + 1;
            int hex1 = (int) Utility._IntToHex((int) b >> 4 & 15);
            numArray3[index2] = (byte) hex1;
            byte[] numArray4 = numArray1;
            int index3 = num4;
            num2 = index3 + 1;
            int hex2 = (int) Utility._IntToHex((int) b & 15);
            numArray4[index3] = (byte) hex2;
          }
        }
        bytes = numArray1;
      }
      return Encoding.ASCII.GetString(bytes);
    }

    private static bool _UrlEncodeIsSafe(byte b)
    {
      if (Utility._IsAsciiAlphaNumeric(b))
        return true;
      switch ((char) b)
      {
        case '!':
        case '\'':
        case '(':
        case ')':
        case '*':
        case '-':
        case '.':
        case '_':
          return true;
        default:
          return false;
      }
    }

    private static bool _IsAsciiAlphaNumeric(byte b)
    {
      if (b >= (byte) 97 && b <= (byte) 122 || b >= (byte) 65 && b <= (byte) 90)
        return true;
      return b >= (byte) 48 && b <= (byte) 57;
    }

    private static byte _IntToHex(int n) => n <= 9 ? (byte) (n + 48) : (byte) (n - 10 + 65);

    private static int _HexToInt(char h)
    {
      if (h >= '0' && h <= '9')
        return (int) h - 48;
      if (h >= 'a' && h <= 'f')
        return (int) h - 97 + 10;
      return h >= 'A' && h <= 'F' ? (int) h - 65 + 10 : -1;
    }

    public static string MakeValidFileName(string invalidPath)
    {
      return invalidPath.Replace('\\', '_').Replace('/', '_').Replace(':', '_').Replace('*', '_').Replace('?', '_').Replace('"', '_').Replace('<', '_').Replace('>', '_').Replace('|', '_');
    }

    public static IEnumerable<string> GenerateFileNames(
      string directory,
      string primaryFileName,
      string extension)
    {
      Verify.IsNeitherNullNorEmpty(directory, nameof (directory));
      Verify.IsNeitherNullNorEmpty(primaryFileName, nameof (primaryFileName));
      primaryFileName = Utility.MakeValidFileName(primaryFileName);
      for (int i = 0; i <= 50; ++i)
      {
        if (i == 0)
          yield return Path.Combine(directory, primaryFileName) + extension;
        else if (40 >= i)
          yield return Path.Combine(directory, primaryFileName) + " (" + i.ToString((IFormatProvider) null) + ")" + extension;
        else
          yield return Path.Combine(directory, primaryFileName) + " (" + (object) Utility._randomNumberGenerator.Next(41, 9999) + ")" + extension;
      }
    }

    public static bool TryFileMove(string sourceFileName, string destFileName)
    {
      if (File.Exists(destFileName))
        return false;
      try
      {
        File.Move(sourceFileName, destFileName);
      }
      catch (IOException ex)
      {
        return false;
      }
      return true;
    }

    public static void SafeDestroyIcon(ref IntPtr hicon)
    {
      IntPtr handle = hicon;
      hicon = IntPtr.Zero;
      if (!(IntPtr.Zero != handle))
        return;
      NativeMethods.DestroyIcon(handle);
    }

    public static void SafeDeleteObject(ref IntPtr gdiObject)
    {
      IntPtr hObject = gdiObject;
      gdiObject = IntPtr.Zero;
      if (!(IntPtr.Zero != hObject))
        return;
      NativeMethods.DeleteObject(hObject);
    }

    public static void SafeDestroyWindow(ref IntPtr hwnd)
    {
      IntPtr hwnd1 = hwnd;
      hwnd = IntPtr.Zero;
      if (!NativeMethods.IsWindow(hwnd1))
        return;
      NativeMethods.DestroyWindow(hwnd1);
    }

    public static void SafeDisposeImage(ref IntPtr gdipImage)
    {
      IntPtr image = gdipImage;
      gdipImage = IntPtr.Zero;
      if (!(IntPtr.Zero != image))
        return;
      int num = (int) NativeMethods.GdipDisposeImage(image);
    }

    public static void SafeCoTaskMemFree(ref IntPtr ptr)
    {
      IntPtr ptr1 = ptr;
      ptr = IntPtr.Zero;
      if (!(IntPtr.Zero != ptr1))
        return;
      Marshal.FreeCoTaskMem(ptr1);
    }

    public static void SafeFreeHGlobal(ref IntPtr hglobal)
    {
      IntPtr hglobal1 = hglobal;
      hglobal = IntPtr.Zero;
      if (!(IntPtr.Zero != hglobal1))
        return;
      Marshal.FreeHGlobal(hglobal1);
    }

    public static void SafeRelease<T>(ref T comObject) where T : class
    {
      T o = comObject;
      comObject = default (T);
      if ((object) o == null)
        return;
      Marshal.ReleaseComObject((object) o);
    }

    public static bool IsOSVistaOrNewer => Utility._osVersion >= new Version(6, 0);

    public static bool IsOSWindows7OrNewer => Utility._osVersion >= new Version(6, 1);

    public static byte[] GetBytesFromBitmapSource(BitmapSource bmp)
    {
      int pixelWidth = bmp.PixelWidth;
      int pixelHeight = bmp.PixelHeight;
      int stride = pixelWidth * ((bmp.Format.BitsPerPixel + 7) / 8);
      byte[] pixels = new byte[pixelHeight * stride];
      bmp.CopyPixels((Array) pixels, stride, 0);
      return pixels;
    }

    public static BitmapSource GenerateBitmapSource(ImageSource img)
    {
      return Utility.GenerateBitmapSource(img, img.Width, img.Height);
    }

    public static BitmapSource GenerateBitmapSource(
      ImageSource img,
      double renderWidth,
      double renderHeight)
    {
      DrawingVisual drawingVisual = new DrawingVisual();
      using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        drawingContext.DrawImage(img, new Rect(0.0, 0.0, renderWidth, renderHeight));
      RenderTargetBitmap bitmapSource = new RenderTargetBitmap((int) renderWidth, (int) renderHeight, 96.0, 96.0, PixelFormats.Pbgra32);
      bitmapSource.Render((Visual) drawingVisual);
      return (BitmapSource) bitmapSource;
    }

    public static BitmapSource GenerateBitmapSource(
      UIElement element,
      double renderWidth,
      double renderHeight,
      bool performLayout)
    {
      if (performLayout)
      {
        element.Measure(new Size(renderWidth, renderHeight));
        element.Arrange(new Rect(new Size(renderWidth, renderHeight)));
      }
      RenderTargetBitmap bitmapSource = new RenderTargetBitmap((int) renderWidth, (int) renderHeight, 96.0, 96.0, PixelFormats.Pbgra32);
      DrawingVisual drawingVisual = new DrawingVisual();
      using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        drawingContext.DrawRectangle((Brush) new VisualBrush((Visual) element), (Pen) null, new Rect(0.0, 0.0, renderWidth, renderHeight));
      bitmapSource.Render((Visual) drawingVisual);
      return (BitmapSource) bitmapSource;
    }

    public static void SaveToPng(BitmapSource source, string fileName)
    {
      PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
      pngBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
      using (FileStream fileStream = File.Create(fileName))
        pngBitmapEncoder.Save((Stream) fileStream);
    }

    private static int _GetBitDepth()
    {
      if (Utility.s_bitDepth == 0)
      {
        using (SafeDC desktop = SafeDC.GetDesktop())
          Utility.s_bitDepth = NativeMethods.GetDeviceCaps(desktop, DeviceCap.BITSPIXEL) * NativeMethods.GetDeviceCaps(desktop, DeviceCap.PLANES);
      }
      return Utility.s_bitDepth;
    }

    public static BitmapFrame GetBestMatch(IList<BitmapFrame> frames, int width, int height)
    {
      return Utility._GetBestMatch(frames, Utility._GetBitDepth(), width, height);
    }

    private static int _MatchImage(
      BitmapFrame frame,
      int bitDepth,
      int width,
      int height,
      int bpp)
    {
      return 2 * Utility._WeightedAbs(bpp, bitDepth, false) + Utility._WeightedAbs(frame.PixelWidth, width, true) + Utility._WeightedAbs(frame.PixelHeight, height, true);
    }

    private static int _WeightedAbs(int valueHave, int valueWant, bool fPunish)
    {
      int num = valueHave - valueWant;
      if (num < 0)
        num = (fPunish ? -2 : -1) * num;
      return num;
    }

    private static BitmapFrame _GetBestMatch(
      IList<BitmapFrame> frames,
      int bitDepth,
      int width,
      int height)
    {
      int num1 = int.MaxValue;
      int num2 = 0;
      int index1 = 0;
      bool flag = frames[0].Decoder is IconBitmapDecoder;
      for (int index2 = 0; index2 < frames.Count && num1 != 0; ++index2)
      {
        PixelFormat format;
        int bitsPerPixel;
        if (!flag)
        {
          format = frames[index2].Format;
          bitsPerPixel = format.BitsPerPixel;
        }
        else
        {
          format = frames[index2].Thumbnail.Format;
          bitsPerPixel = format.BitsPerPixel;
        }
        int bpp = bitsPerPixel;
        if (bpp == 0)
          bpp = 8;
        int num3 = Utility._MatchImage(frames[index2], bitDepth, width, height, bpp);
        if (num3 < num1)
        {
          index1 = index2;
          num2 = bpp;
          num1 = num3;
        }
        else if (num3 == num1 && num2 < bpp)
        {
          index1 = index2;
          num2 = bpp;
        }
      }
      return frames[index1];
    }

    public static int RGB(Color c) => (int) c.B | (int) c.G << 8 | (int) c.R << 16;

    public static int AlphaRGB(Color c)
    {
      return (int) c.B | (int) c.G << 8 | (int) c.R << 16 | (int) c.A << 24;
    }

    public static Color ColorFromArgbDword(uint color)
    {
      return Color.FromArgb((byte) ((color & 4278190080U) >> 24), (byte) ((color & 16711680U) >> 16), (byte) ((color & 65280U) >> 8), (byte) (color & (uint) byte.MaxValue));
    }

    public static bool AreImageSourcesEqual(ImageSource left, ImageSource right)
    {
      if (left == null)
        return right == null;
      if (right == null)
        return false;
      BitmapSource bitmapSource1 = Utility.GenerateBitmapSource(left);
      BitmapSource bitmapSource2 = Utility.GenerateBitmapSource(right);
      byte[] fromBitmapSource1 = Utility.GetBytesFromBitmapSource(bitmapSource1);
      byte[] fromBitmapSource2 = Utility.GetBytesFromBitmapSource(bitmapSource2);
      return fromBitmapSource1.Length == fromBitmapSource2.Length && Utility.MemCmp(fromBitmapSource1, fromBitmapSource2, fromBitmapSource1.Length);
    }

    public static IntPtr GenerateHICON(ImageSource image, Size dimensions)
    {
      if (image == null)
        return IntPtr.Zero;
      BitmapFrame bestMatch;
      if (image is BitmapFrame bitmapFrame)
      {
        bestMatch = Utility.GetBestMatch((IList<BitmapFrame>) bitmapFrame.Decoder.Frames, (int) dimensions.Width, (int) dimensions.Height);
      }
      else
      {
        Rect rectangle = new Rect(0.0, 0.0, dimensions.Width, dimensions.Height);
        double num1 = dimensions.Width / dimensions.Height;
        double num2 = image.Width / image.Height;
        if (image.Width <= dimensions.Width && image.Height <= dimensions.Height)
          rectangle = new Rect((dimensions.Width - image.Width) / 2.0, (dimensions.Height - image.Height) / 2.0, image.Width, image.Height);
        else if (num1 > num2)
        {
          double width = image.Width / image.Height * dimensions.Width;
          rectangle = new Rect((dimensions.Width - width) / 2.0, 0.0, width, dimensions.Height);
        }
        else if (num1 < num2)
        {
          double height = image.Height / image.Width * dimensions.Height;
          rectangle = new Rect(0.0, (dimensions.Height - height) / 2.0, dimensions.Width, height);
        }
        DrawingVisual drawingVisual = new DrawingVisual();
        DrawingContext drawingContext = drawingVisual.RenderOpen();
        drawingContext.DrawImage(image, rectangle);
        drawingContext.Close();
        RenderTargetBitmap source = new RenderTargetBitmap((int) dimensions.Width, (int) dimensions.Height, 96.0, 96.0, PixelFormats.Pbgra32);
        source.Render((Visual) drawingVisual);
        bestMatch = BitmapFrame.Create((BitmapSource) source);
      }
      using (MemoryStream source = new MemoryStream())
      {
        BitmapEncoder bitmapEncoder = (BitmapEncoder) new PngBitmapEncoder();
        bitmapEncoder.Frames.Add(bestMatch);
        bitmapEncoder.Save((Stream) source);
        using (ManagedIStream managedIstream = new ManagedIStream((Stream) source))
        {
          IntPtr bitmap = IntPtr.Zero;
          try
          {
            IntPtr hbmReturn;
            return NativeMethods.GdipCreateBitmapFromStream((IStream) managedIstream, out bitmap) != Status.Ok || NativeMethods.GdipCreateHICONFromBitmap(bitmap, out hbmReturn) != Status.Ok ? IntPtr.Zero : hbmReturn;
          }
          finally
          {
            Utility.SafeDisposeImage(ref bitmap);
          }
        }
      }
    }

    public static void AddDependencyPropertyChangeListener(
      object component,
      DependencyProperty property,
      EventHandler listener)
    {
      if (component == null)
        return;
      DependencyPropertyDescriptor.FromProperty(property, component.GetType()).AddValueChanged(component, listener);
    }

    public static void RemoveDependencyPropertyChangeListener(
      object component,
      DependencyProperty property,
      EventHandler listener)
    {
      if (component == null)
        return;
      DependencyPropertyDescriptor.FromProperty(property, component.GetType()).RemoveValueChanged(component, listener);
    }

    public static bool IsNonNegative(this Thickness thickness)
    {
      return thickness.Top.IsFiniteAndNonNegative() && thickness.Left.IsFiniteAndNonNegative() && thickness.Bottom.IsFiniteAndNonNegative() && thickness.Right.IsFiniteAndNonNegative();
    }

    public static bool IsValid(this CornerRadius cornerRadius)
    {
      return cornerRadius.TopLeft.IsFiniteAndNonNegative() && cornerRadius.TopRight.IsFiniteAndNonNegative() && cornerRadius.BottomLeft.IsFiniteAndNonNegative() && cornerRadius.BottomRight.IsFiniteAndNonNegative();
    }

    private class _UrlDecoder
    {
      private readonly Encoding _encoding;
      private readonly char[] _charBuffer;
      private readonly byte[] _byteBuffer;
      private int _byteCount;
      private int _charCount;

      public _UrlDecoder(int size, Encoding encoding)
      {
        this._encoding = encoding;
        this._charBuffer = new char[size];
        this._byteBuffer = new byte[size];
      }

      public void AddByte(byte b) => this._byteBuffer[this._byteCount++] = b;

      public void AddChar(char ch)
      {
        this._FlushBytes();
        this._charBuffer[this._charCount++] = ch;
      }

      private void _FlushBytes()
      {
        if (this._byteCount <= 0)
          return;
        this._charCount += this._encoding.GetChars(this._byteBuffer, 0, this._byteCount, this._charBuffer, this._charCount);
        this._byteCount = 0;
      }

      public string GetString()
      {
        this._FlushBytes();
        return this._charCount > 0 ? new string(this._charBuffer, 0, this._charCount) : "";
      }
    }
  }
}
