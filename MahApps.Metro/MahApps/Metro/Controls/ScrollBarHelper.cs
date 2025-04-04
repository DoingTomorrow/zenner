// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ScrollBarHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class ScrollBarHelper
  {
    public static readonly DependencyProperty VerticalScrollBarOnLeftSideProperty = DependencyProperty.RegisterAttached("VerticalScrollBarOnLeftSide", typeof (bool), typeof (ScrollBarHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (ScrollViewer))]
    public static bool GetVerticalScrollBarOnLeftSide(ScrollViewer obj)
    {
      return (bool) obj.GetValue(ScrollBarHelper.VerticalScrollBarOnLeftSideProperty);
    }

    public static void SetVerticalScrollBarOnLeftSide(ScrollViewer obj, bool value)
    {
      obj.SetValue(ScrollBarHelper.VerticalScrollBarOnLeftSideProperty, (object) value);
    }
  }
}
