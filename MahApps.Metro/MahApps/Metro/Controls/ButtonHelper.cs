// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ButtonHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

#nullable disable
namespace MahApps.Metro.Controls
{
  public static class ButtonHelper
  {
    [Obsolete("This property will be deleted in the next release. You should use ContentCharacterCasing attached property located in ControlsHelper.")]
    public static readonly DependencyProperty PreserveTextCaseProperty = DependencyProperty.RegisterAttached("PreserveTextCase", typeof (bool), typeof (ButtonHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(ButtonHelper.PreserveTextCasePropertyChangedCallback)));
    [Obsolete("This property will be deleted in the next release. You should use CornerRadius attached property located in ControlsHelper.")]
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof (CornerRadius), typeof (ButtonHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    private static void PreserveTextCasePropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(e.NewValue is bool) || !(dependencyObject is Button element))
        return;
      int num = (bool) e.NewValue ? 0 : 2;
      ControlsHelper.SetContentCharacterCasing((UIElement) element, (CharacterCasing) num);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Button))]
    public static bool GetPreserveTextCase(UIElement element)
    {
      return (bool) element.GetValue(ButtonHelper.PreserveTextCaseProperty);
    }

    public static void SetPreserveTextCase(UIElement element, bool value)
    {
      element.SetValue(ButtonHelper.PreserveTextCaseProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (Button))]
    [AttachedPropertyBrowsableForType(typeof (ToggleButton))]
    public static CornerRadius GetCornerRadius(UIElement element)
    {
      return ControlsHelper.GetCornerRadius(element);
    }

    public static void SetCornerRadius(UIElement element, CornerRadius value)
    {
      ControlsHelper.SetCornerRadius(element, value);
    }
  }
}
