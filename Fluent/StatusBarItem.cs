// Decompiled with JetBrains decompiler
// Type: Fluent.StatusBarItem
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Fluent
{
  public class StatusBarItem : System.Windows.Controls.Primitives.StatusBarItem
  {
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (StatusBarItem), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (string), typeof (StatusBarItem), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(StatusBarItem.OnValueChanged)));
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool), typeof (StatusBarItem), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(StatusBarItem.OnIsCheckedChanged)));

    public string Title
    {
      get => (string) this.GetValue(StatusBarItem.TitleProperty);
      set => this.SetValue(StatusBarItem.TitleProperty, (object) value);
    }

    public string Value
    {
      get => (string) this.GetValue(StatusBarItem.ValueProperty);
      set => this.SetValue(StatusBarItem.ValueProperty, (object) value);
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      d.CoerceValue(ContentControl.ContentProperty);
    }

    public bool IsChecked
    {
      get => (bool) this.GetValue(StatusBarItem.IsCheckedProperty);
      set => this.SetValue(StatusBarItem.IsCheckedProperty, (object) value);
    }

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      StatusBarItem statusBarItem = d as StatusBarItem;
      statusBarItem.CoerceValue(UIElement.VisibilityProperty);
      if ((bool) e.NewValue)
        statusBarItem.RaiseChecked();
      else
        statusBarItem.RaiseUnchecked();
    }

    public event RoutedEventHandler Checked;

    public event RoutedEventHandler Unchecked;

    private void RaiseChecked()
    {
      if (this.Checked == null)
        return;
      this.Checked((object) this, new RoutedEventArgs());
    }

    private void RaiseUnchecked()
    {
      if (this.Unchecked == null)
        return;
      this.Unchecked((object) this, new RoutedEventArgs());
    }

    static StatusBarItem()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (StatusBarItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (StatusBarItem)));
      UIElement.VisibilityProperty.AddOwner(typeof (StatusBarItem), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(StatusBarItem.CoerceVisibility)));
      ContentControl.ContentProperty.AddOwner(typeof (StatusBarItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(StatusBarItem.OnContentChanged), new CoerceValueCallback(StatusBarItem.CoerceContent)));
    }

    private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      d.CoerceValue(StatusBarItem.ValueProperty);
    }

    private static object CoerceContent(DependencyObject d, object basevalue)
    {
      StatusBarItem statusBarItem = (StatusBarItem) d;
      return basevalue == null && statusBarItem.Value != null ? (object) statusBarItem.Value : basevalue;
    }

    private static object CoerceVisibility(DependencyObject d, object basevalue)
    {
      return !(d as StatusBarItem).IsChecked ? (object) Visibility.Collapsed : basevalue;
    }
  }
}
