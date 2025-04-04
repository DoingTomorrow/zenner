// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ControlsHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class ControlsHelper
  {
    public static readonly DependencyProperty DisabledVisualElementVisibilityProperty = DependencyProperty.RegisterAttached("DisabledVisualElementVisibility", typeof (Visibility), typeof (ControlsHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty ContentCharacterCasingProperty = DependencyProperty.RegisterAttached("ContentCharacterCasing", typeof (CharacterCasing), typeof (ControlsHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) CharacterCasing.Normal, FrameworkPropertyMetadataOptions.AffectsMeasure), (ValidateValueCallback) (value => CharacterCasing.Normal <= (CharacterCasing) value && (CharacterCasing) value <= CharacterCasing.Upper));
    public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.RegisterAttached("HeaderFontSize", typeof (double), typeof (ControlsHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) 26.67, new PropertyChangedCallback(ControlsHelper.HeaderFontSizePropertyChangedCallback))
    {
      Inherits = true
    });
    public static readonly DependencyProperty HeaderFontStretchProperty = DependencyProperty.RegisterAttached("HeaderFontStretch", typeof (FontStretch), typeof (ControlsHelper), (PropertyMetadata) new UIPropertyMetadata((object) FontStretches.Normal));
    public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.RegisterAttached("HeaderFontWeight", typeof (FontWeight), typeof (ControlsHelper), (PropertyMetadata) new UIPropertyMetadata((object) FontWeights.Normal));
    public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.RegisterAttached("ButtonWidth", typeof (double), typeof (ControlsHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) 22.0, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty FocusBorderBrushProperty = DependencyProperty.RegisterAttached("FocusBorderBrush", typeof (Brush), typeof (ControlsHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof (Brush), typeof (ControlsHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof (CornerRadius), typeof (ControlsHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof (PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof (NumericUpDown))]
    public static Visibility GetDisabledVisualElementVisibility(UIElement element)
    {
      return (Visibility) element.GetValue(ControlsHelper.DisabledVisualElementVisibilityProperty);
    }

    public static void SetDisabledVisualElementVisibility(UIElement element, Visibility value)
    {
      element.SetValue(ControlsHelper.DisabledVisualElementVisibilityProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (ContentControl))]
    [AttachedPropertyBrowsableForType(typeof (DropDownButton))]
    [AttachedPropertyBrowsableForType(typeof (WindowCommands))]
    public static CharacterCasing GetContentCharacterCasing(UIElement element)
    {
      return (CharacterCasing) element.GetValue(ControlsHelper.ContentCharacterCasingProperty);
    }

    public static void SetContentCharacterCasing(UIElement element, CharacterCasing value)
    {
      element.SetValue(ControlsHelper.ContentCharacterCasingProperty, (object) value);
    }

    private static void HeaderFontSizePropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(e.NewValue is double) || !(dependencyObject is MetroTabItem metroTabItem))
        return;
      if (metroTabItem.closeButton == null)
        metroTabItem.ApplyTemplate();
      if (metroTabItem.closeButton == null || metroTabItem.contentSite == null)
        return;
      double num1 = Math.Round(Math.Ceiling((double) e.NewValue * metroTabItem.FontFamily.LineSpacing)) / 2.8;
      Thickness thickness = metroTabItem.Padding;
      double top1 = thickness.Top;
      double num2 = num1 - top1;
      thickness = metroTabItem.Padding;
      double bottom1 = thickness.Bottom;
      double num3 = num2 - bottom1;
      thickness = metroTabItem.contentSite.Margin;
      double top2 = thickness.Top;
      double num4 = num3 - top2;
      thickness = metroTabItem.contentSite.Margin;
      double bottom2 = thickness.Bottom;
      double top3 = num4 - bottom2;
      Thickness margin = metroTabItem.closeButton.Margin;
      metroTabItem.newButtonMargin = new Thickness(margin.Left, top3, margin.Right, margin.Bottom);
      metroTabItem.closeButton.Margin = metroTabItem.newButtonMargin;
      metroTabItem.closeButton.UpdateLayout();
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (MetroTabItem))]
    [AttachedPropertyBrowsableForType(typeof (TabItem))]
    [AttachedPropertyBrowsableForType(typeof (GroupBox))]
    public static double GetHeaderFontSize(UIElement element)
    {
      return (double) element.GetValue(ControlsHelper.HeaderFontSizeProperty);
    }

    public static void SetHeaderFontSize(UIElement element, double value)
    {
      element.SetValue(ControlsHelper.HeaderFontSizeProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (MetroTabItem))]
    [AttachedPropertyBrowsableForType(typeof (TabItem))]
    [AttachedPropertyBrowsableForType(typeof (GroupBox))]
    public static FontStretch GetHeaderFontStretch(UIElement element)
    {
      return (FontStretch) element.GetValue(ControlsHelper.HeaderFontStretchProperty);
    }

    public static void SetHeaderFontStretch(UIElement element, FontStretch value)
    {
      element.SetValue(ControlsHelper.HeaderFontStretchProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (MetroTabItem))]
    [AttachedPropertyBrowsableForType(typeof (TabItem))]
    [AttachedPropertyBrowsableForType(typeof (GroupBox))]
    public static FontWeight GetHeaderFontWeight(UIElement element)
    {
      return (FontWeight) element.GetValue(ControlsHelper.HeaderFontWeightProperty);
    }

    public static void SetHeaderFontWeight(UIElement element, FontWeight value)
    {
      element.SetValue(ControlsHelper.HeaderFontWeightProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static double GetButtonWidth(DependencyObject obj)
    {
      return (double) obj.GetValue(ControlsHelper.ButtonWidthProperty);
    }

    public static void SetButtonWidth(DependencyObject obj, double value)
    {
      obj.SetValue(ControlsHelper.ButtonWidthProperty, (object) value);
    }

    public static void SetFocusBorderBrush(DependencyObject obj, Brush value)
    {
      obj.SetValue(ControlsHelper.FocusBorderBrushProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TextBox))]
    [AttachedPropertyBrowsableForType(typeof (CheckBox))]
    [AttachedPropertyBrowsableForType(typeof (RadioButton))]
    [AttachedPropertyBrowsableForType(typeof (DatePicker))]
    [AttachedPropertyBrowsableForType(typeof (ComboBox))]
    public static Brush GetFocusBorderBrush(DependencyObject obj)
    {
      return (Brush) obj.GetValue(ControlsHelper.FocusBorderBrushProperty);
    }

    public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
    {
      obj.SetValue(ControlsHelper.MouseOverBorderBrushProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (TextBox))]
    [AttachedPropertyBrowsableForType(typeof (CheckBox))]
    [AttachedPropertyBrowsableForType(typeof (RadioButton))]
    [AttachedPropertyBrowsableForType(typeof (DatePicker))]
    [AttachedPropertyBrowsableForType(typeof (ComboBox))]
    [AttachedPropertyBrowsableForType(typeof (Tile))]
    public static Brush GetMouseOverBorderBrush(DependencyObject obj)
    {
      return (Brush) obj.GetValue(ControlsHelper.MouseOverBorderBrushProperty);
    }

    [Category("MahApps.Metro")]
    public static CornerRadius GetCornerRadius(UIElement element)
    {
      return (CornerRadius) element.GetValue(ControlsHelper.CornerRadiusProperty);
    }

    public static void SetCornerRadius(UIElement element, CornerRadius value)
    {
      element.SetValue(ControlsHelper.CornerRadiusProperty, (object) value);
    }
  }
}
