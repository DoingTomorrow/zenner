// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ScrollViewerOffsetMediator
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class ScrollViewerOffsetMediator : FrameworkElement
  {
    public static readonly DependencyProperty ScrollViewerProperty = DependencyProperty.Register(nameof (ScrollViewer), typeof (ScrollViewer), typeof (ScrollViewerOffsetMediator), new PropertyMetadata((object) null, new PropertyChangedCallback(ScrollViewerOffsetMediator.OnScrollViewerChanged)));
    public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register(nameof (HorizontalOffset), typeof (double), typeof (ScrollViewerOffsetMediator), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ScrollViewerOffsetMediator.OnHorizontalOffsetChanged)));

    public ScrollViewer ScrollViewer
    {
      get => (ScrollViewer) this.GetValue(ScrollViewerOffsetMediator.ScrollViewerProperty);
      set => this.SetValue(ScrollViewerOffsetMediator.ScrollViewerProperty, (object) value);
    }

    public double HorizontalOffset
    {
      get => (double) this.GetValue(ScrollViewerOffsetMediator.HorizontalOffsetProperty);
      set => this.SetValue(ScrollViewerOffsetMediator.HorizontalOffsetProperty, (object) value);
    }

    private static void OnScrollViewerChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ScrollViewerOffsetMediator viewerOffsetMediator = (ScrollViewerOffsetMediator) o;
      ((ScrollViewer) e.NewValue)?.ScrollToHorizontalOffset(viewerOffsetMediator.HorizontalOffset);
    }

    private static void OnHorizontalOffsetChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      ScrollViewerOffsetMediator viewerOffsetMediator = (ScrollViewerOffsetMediator) o;
      if (viewerOffsetMediator.ScrollViewer == null)
        return;
      viewerOffsetMediator.ScrollViewer.ScrollToHorizontalOffset((double) e.NewValue);
    }
  }
}
