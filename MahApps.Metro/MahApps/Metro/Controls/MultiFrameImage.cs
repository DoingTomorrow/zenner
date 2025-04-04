// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.MultiFrameImage
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class MultiFrameImage : Image
  {
    private readonly List<BitmapSource> _frames = new List<BitmapSource>();

    static MultiFrameImage()
    {
      Image.SourceProperty.OverrideMetadata(typeof (MultiFrameImage), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(MultiFrameImage.OnSourceChanged)));
    }

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((MultiFrameImage) d).UpdateFrameList();
    }

    private void UpdateFrameList()
    {
      this._frames.Clear();
      if (!(this.Source is BitmapFrame source))
        return;
      BitmapDecoder decoder = source.Decoder;
      if (decoder == null || decoder.Frames.Count == 0)
        return;
      this._frames.AddRange((IEnumerable<BitmapSource>) decoder.Frames.GroupBy<BitmapFrame, int>((Func<BitmapFrame, int>) (f => f.PixelWidth * f.PixelHeight)).OrderBy<IGrouping<int, BitmapFrame>, int>((Func<IGrouping<int, BitmapFrame>, int>) (g => g.Key)).Select<IGrouping<int, BitmapFrame>, BitmapFrame>((Func<IGrouping<int, BitmapFrame>, BitmapFrame>) (g => g.OrderByDescending<BitmapFrame, int>((Func<BitmapFrame, int>) (f => f.Format.BitsPerPixel)).First<BitmapFrame>())));
    }

    protected override void OnRender(DrawingContext dc)
    {
      if (this._frames.Count == 0)
      {
        base.OnRender(dc);
      }
      else
      {
        double width1 = this.RenderSize.Width;
        Size renderSize = this.RenderSize;
        double height1 = renderSize.Height;
        double minSize = Math.Max(width1, height1);
        BitmapSource bitmapSource1 = this._frames.FirstOrDefault<BitmapSource>((Func<BitmapSource, bool>) (f => f.Width >= minSize && f.Height >= minSize)) ?? this._frames.Last<BitmapSource>();
        DrawingContext drawingContext = dc;
        BitmapSource bitmapSource2 = bitmapSource1;
        renderSize = this.RenderSize;
        double width2 = renderSize.Width;
        renderSize = this.RenderSize;
        double height2 = renderSize.Height;
        Rect rectangle = new Rect(0.0, 0.0, width2, height2);
        drawingContext.DrawImage((ImageSource) bitmapSource2, rectangle);
      }
    }
  }
}
