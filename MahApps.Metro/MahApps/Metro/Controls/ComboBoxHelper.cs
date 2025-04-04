// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ComboBoxHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class ComboBoxHelper
  {
    public static readonly DependencyProperty EnableVirtualizationWithGroupingProperty = DependencyProperty.RegisterAttached("EnableVirtualizationWithGrouping", typeof (bool), typeof (ComboBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, new PropertyChangedCallback(ComboBoxHelper.EnableVirtualizationWithGroupingPropertyChangedCallback)));
    public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.RegisterAttached("MaxLength", typeof (int), typeof (ComboBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0), new ValidateValueCallback(ComboBoxHelper.MaxLengthValidateValue));
    public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.RegisterAttached("CharacterCasing", typeof (CharacterCasing), typeof (ComboBoxHelper), (PropertyMetadata) new FrameworkPropertyMetadata((object) CharacterCasing.Normal), new ValidateValueCallback(ComboBoxHelper.CharacterCasingValidateValue));

    private static void EnableVirtualizationWithGroupingPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(dependencyObject is ComboBox comboBox) || e.NewValue == e.OldValue)
        return;
      comboBox.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, e.NewValue);
      comboBox.SetValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty, e.NewValue);
      comboBox.SetValue(ScrollViewer.CanContentScrollProperty, e.NewValue);
    }

    public static void SetEnableVirtualizationWithGrouping(DependencyObject obj, bool value)
    {
      obj.SetValue(ComboBoxHelper.EnableVirtualizationWithGroupingProperty, (object) value);
    }

    [Category("MahApps.Metro")]
    public static bool GetEnableVirtualizationWithGrouping(DependencyObject obj)
    {
      return (bool) obj.GetValue(ComboBoxHelper.EnableVirtualizationWithGroupingProperty);
    }

    private static bool MaxLengthValidateValue(object value) => (int) value >= 0;

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (ComboBox))]
    public static int GetMaxLength(UIElement obj)
    {
      return (int) obj.GetValue(ComboBoxHelper.MaxLengthProperty);
    }

    public static void SetMaxLength(UIElement obj, int value)
    {
      obj.SetValue(ComboBoxHelper.MaxLengthProperty, (object) value);
    }

    private static bool CharacterCasingValidateValue(object value)
    {
      return CharacterCasing.Normal <= (CharacterCasing) value && (CharacterCasing) value <= CharacterCasing.Upper;
    }

    [Category("MahApps.Metro")]
    [AttachedPropertyBrowsableForType(typeof (ComboBox))]
    public static CharacterCasing GetCharacterCasing(UIElement obj)
    {
      return (CharacterCasing) obj.GetValue(ComboBoxHelper.CharacterCasingProperty);
    }

    public static void SetCharacterCasing(UIElement obj, CharacterCasing value)
    {
      obj.SetValue(ComboBoxHelper.CharacterCasingProperty, (object) value);
    }
  }
}
