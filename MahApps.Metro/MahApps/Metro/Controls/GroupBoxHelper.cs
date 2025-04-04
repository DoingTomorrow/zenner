// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.GroupBoxHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class GroupBoxHelper
  {
    public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.RegisterAttached("HeaderForeground", typeof (Brush), typeof (GroupBoxHelper), (PropertyMetadata) new UIPropertyMetadata((object) Brushes.White));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (GroupBox))]
    [AttachedPropertyBrowsableForType(typeof (Expander))]
    public static Brush GetHeaderForeground(UIElement element)
    {
      return (Brush) element.GetValue(GroupBoxHelper.HeaderForegroundProperty);
    }

    public static void SetHeaderForeground(UIElement element, Brush value)
    {
      element.SetValue(GroupBoxHelper.HeaderForegroundProperty, (object) value);
    }
  }
}
