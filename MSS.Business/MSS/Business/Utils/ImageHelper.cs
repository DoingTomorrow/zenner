// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.ImageHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS.Business.Utils
{
  public sealed class ImageHelper
  {
    private static readonly ImageHelper instance = new ImageHelper();
    private IDictionary<string[], BitmapImage> bitmapImageDictionaries = (IDictionary<string[], BitmapImage>) new Dictionary<string[], BitmapImage>((IEqualityComparer<string[]>) new ImageHelper.KeyEqualityComparer());

    private ImageHelper()
    {
    }

    public static ImageHelper Instance => ImageHelper.instance;

    public BitmapImage GetBitmapImageFromFiles(string[] files)
    {
      if (this.bitmapImageDictionaries.ContainsKey(files))
        return this.bitmapImageDictionaries[files].Clone();
      BitmapImage bitmapImage = this.ConvertBitmapToBitmapImage(this.Combine(files));
      bitmapImage.Freeze();
      this.bitmapImageDictionaries.Add(files, bitmapImage);
      return bitmapImage;
    }

    private Bitmap Combine(string[] files)
    {
      List<Bitmap> bitmapList = new List<Bitmap>();
      Bitmap bitmap1 = (Bitmap) null;
      try
      {
        int width = 0;
        int height = 0;
        foreach (string file in files)
        {
          Bitmap bitmap2 = new Bitmap(Application.GetResourceStream(new Uri(file, UriKind.RelativeOrAbsolute)).Stream);
          width = 16;
          height = 16;
          bitmapList.Add(bitmap2);
        }
        bitmap1 = new Bitmap(width, height);
        using (Graphics graphics = Graphics.FromImage((Image) bitmap1))
        {
          graphics.Clear(Color.Transparent);
          int num = 0;
          foreach (Bitmap bitmap3 in bitmapList)
          {
            graphics.DrawImage((Image) bitmap3, new Rectangle(0, 0, bitmap3.Width, bitmap3.Height));
            num += bitmap3.Width;
          }
        }
        return bitmap1;
      }
      catch (Exception ex)
      {
        bitmap1?.Dispose();
        throw;
      }
      finally
      {
        foreach (Image image in bitmapList)
          image.Dispose();
      }
    }

    private BitmapImage ConvertBitmapToBitmapImage(Bitmap target)
    {
      MemoryStream memoryStream = new MemoryStream();
      target.Save((Stream) memoryStream, ImageFormat.Png);
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      bitmapImage.StreamSource = (Stream) new MemoryStream(memoryStream.ToArray());
      bitmapImage.EndInit();
      return bitmapImage;
    }

    public Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        BitmapEncoder bitmapEncoder = (BitmapEncoder) new PngBitmapEncoder();
        bitmapEncoder.Frames.Add(BitmapFrame.Create((BitmapSource) bitmapImage));
        bitmapEncoder.Save((Stream) memoryStream);
        Bitmap original = new Bitmap((Stream) memoryStream);
        original.MakeTransparent(Color.Black);
        return new Bitmap((Image) original);
      }
    }

    public BitmapImage Combine2Images(Bitmap firstImage, Bitmap secondImage)
    {
      List<Bitmap> bitmapList = new List<Bitmap>();
      Bitmap target = (Bitmap) null;
      try
      {
        if (firstImage != null)
          bitmapList.Add(firstImage);
        if (secondImage != null)
          bitmapList.Add(secondImage);
        target = new Bitmap(16, 16);
        using (Graphics graphics = Graphics.FromImage((Image) target))
        {
          graphics.Clear(Color.Transparent);
          int num = 0;
          foreach (Bitmap bitmap in bitmapList)
          {
            graphics.DrawImage((Image) bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            num += bitmap.Width;
          }
        }
        return this.ConvertBitmapToBitmapImage(target);
      }
      catch (Exception ex)
      {
        target?.Dispose();
        throw;
      }
      finally
      {
        foreach (Image image in bitmapList)
          image.Dispose();
      }
    }

    private class KeyEqualityComparer : EqualityComparer<string[]>
    {
      public override bool Equals(string[] x, string[] y)
      {
        if (x == null || y == null || x.Length != y.Length)
          return false;
        for (int index = 0; index < x.Length; ++index)
        {
          if (!x[index].Equals(y[index]))
            return false;
        }
        return true;
      }

      public override int GetHashCode(string[] obj)
      {
        if (obj == null)
          return 0;
        string empty = string.Empty;
        foreach (string str in obj)
          empty += str;
        return empty.GetHashCode();
      }
    }
  }
}
