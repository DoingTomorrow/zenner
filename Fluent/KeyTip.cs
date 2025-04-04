// Decompiled with JetBrains decompiler
// Type: Fluent.KeyTip
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Fluent
{
  public class KeyTip : Label
  {
    public static readonly DependencyProperty KeysProperty = DependencyProperty.RegisterAttached("Keys", typeof (string), typeof (KeyTip), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(KeyTip.KeysPropertyChanged)));
    public static readonly DependencyProperty AutoPlacementProperty = DependencyProperty.RegisterAttached("AutoPlacement", typeof (bool), typeof (KeyTip), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
    public new static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.RegisterAttached("HorizontalAlignment", typeof (HorizontalAlignment), typeof (KeyTip), (PropertyMetadata) new FrameworkPropertyMetadata((object) HorizontalAlignment.Center));
    public new static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.RegisterAttached("VerticalAlignment", typeof (VerticalAlignment), typeof (KeyTip), (PropertyMetadata) new UIPropertyMetadata((object) VerticalAlignment.Center));
    public new static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin", typeof (Thickness), typeof (KeyTip), (PropertyMetadata) new UIPropertyMetadata((object) new Thickness()));

    private static void KeysPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
    }

    public static void SetKeys(DependencyObject element, string value)
    {
      element.SetValue(KeyTip.KeysProperty, (object) value);
    }

    [Category("KeyTips")]
    [DisplayName("Keys")]
    [Description("Key sequence for the given element")]
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
    public static string GetKeys(DependencyObject element)
    {
      return (string) element.GetValue(KeyTip.KeysProperty);
    }

    public static void SetAutoPlacement(DependencyObject element, bool value)
    {
      element.SetValue(KeyTip.AutoPlacementProperty, (object) value);
    }

    [AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
    [DisplayName("AutoPlacement")]
    [Category("KeyTips")]
    [Description("Whether key tip placement is auto or defined by alignment and margin properties")]
    public static bool GetAutoPlacement(DependencyObject element)
    {
      return (bool) element.GetValue(KeyTip.AutoPlacementProperty);
    }

    public static void SetHorizontalAlignment(DependencyObject element, HorizontalAlignment value)
    {
      element.SetValue(KeyTip.HorizontalAlignmentProperty, (object) value);
    }

    [DisplayName("HorizontalAlignment")]
    [AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
    [Category("KeyTips")]
    [Description("Horizontal alignment of the key tip")]
    public static HorizontalAlignment GetHorizontalAlignment(DependencyObject element)
    {
      return (HorizontalAlignment) element.GetValue(KeyTip.HorizontalAlignmentProperty);
    }

    [AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
    [Description("Vertical alignment of the key tip")]
    [DisplayName("VerticalAlignment")]
    [Category("KeyTips")]
    public static VerticalAlignment GetVerticalAlignment(DependencyObject element)
    {
      return (VerticalAlignment) element.GetValue(KeyTip.VerticalAlignmentProperty);
    }

    public static void SetVerticalAlignment(DependencyObject obj, VerticalAlignment value)
    {
      obj.SetValue(KeyTip.VerticalAlignmentProperty, (object) value);
    }

    [AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
    [Description("Margin of the key tip")]
    [DisplayName("Margin")]
    [Category("KeyTips")]
    public static Thickness GetMargin(DependencyObject obj)
    {
      return (Thickness) obj.GetValue(KeyTip.MarginProperty);
    }

    public static void SetMargin(DependencyObject obj, Thickness value)
    {
      obj.SetValue(KeyTip.MarginProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static KeyTip()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (KeyTip), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (KeyTip)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (KeyTip), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(KeyTip.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (KeyTip));
      return basevalue;
    }
  }
}
