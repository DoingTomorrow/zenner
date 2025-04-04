// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ExpanderHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class ExpanderHelper
  {
    public static readonly DependencyProperty HeaderUpStyleProperty = DependencyProperty.RegisterAttached("HeaderUpStyle", typeof (Style), typeof (ExpanderHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty HeaderDownStyleProperty = DependencyProperty.RegisterAttached("HeaderDownStyle", typeof (Style), typeof (ExpanderHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty HeaderLeftStyleProperty = DependencyProperty.RegisterAttached("HeaderLeftStyle", typeof (Style), typeof (ExpanderHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty HeaderRightStyleProperty = DependencyProperty.RegisterAttached("HeaderRightStyle", typeof (Style), typeof (ExpanderHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Expander))]
    public static Style GetHeaderUpStyle(UIElement element)
    {
      return (Style) element.GetValue(ExpanderHelper.HeaderUpStyleProperty);
    }

    public static void SetHeaderUpStyle(UIElement element, Style value)
    {
      element.SetValue(ExpanderHelper.HeaderUpStyleProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Expander))]
    public static Style GetHeaderDownStyle(UIElement element)
    {
      return (Style) element.GetValue(ExpanderHelper.HeaderDownStyleProperty);
    }

    public static void SetHeaderDownStyle(UIElement element, Style value)
    {
      element.SetValue(ExpanderHelper.HeaderDownStyleProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Expander))]
    public static Style GetHeaderLeftStyle(UIElement element)
    {
      return (Style) element.GetValue(ExpanderHelper.HeaderLeftStyleProperty);
    }

    public static void SetHeaderLeftStyle(UIElement element, Style value)
    {
      element.SetValue(ExpanderHelper.HeaderLeftStyleProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Expander))]
    public static Style GetHeaderRightStyle(UIElement element)
    {
      return (Style) element.GetValue(ExpanderHelper.HeaderRightStyleProperty);
    }

    public static void SetHeaderRightStyle(UIElement element, Style value)
    {
      element.SetValue(ExpanderHelper.HeaderRightStyleProperty, (object) value);
    }
  }
}
