// Decompiled with JetBrains decompiler
// Type: Fluent.RadioButton
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  [System.Windows.Markup.ContentProperty("Header")]
  public class RadioButton : System.Windows.Controls.RadioButton, IRibbonControl, IKeyTipedControl, IQuickAccessItemProvider
  {
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (RadioButton));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.AttachSizeDefinition(typeof (RadioButton));
    public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof (RadioButton));
    public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof (RadioButton), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(RadioButton.OnIconChanged)));
    public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof (LargeIcon), typeof (ImageSource), typeof (RadioButton), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (RadioButton), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (RadioButton));

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(RadioButton.SizeProperty);
      set => this.SetValue(RadioButton.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(RadioButton.SizeDefinitionProperty);
      set => this.SetValue(RadioButton.SizeDefinitionProperty, (object) value);
    }

    public object Header
    {
      get => (object) (string) this.GetValue(RadioButton.HeaderProperty);
      set => this.SetValue(RadioButton.HeaderProperty, value);
    }

    public object Icon
    {
      get => (object) (ImageSource) this.GetValue(RadioButton.IconProperty);
      set => this.SetValue(RadioButton.IconProperty, value);
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RadioButton radioButton = d as RadioButton;
      if (e.OldValue is FrameworkElement oldValue)
        radioButton.RemoveLogicalChild((object) oldValue);
      if (!(e.NewValue is FrameworkElement newValue))
        return;
      radioButton.AddLogicalChild((object) newValue);
    }

    public ImageSource LargeIcon
    {
      get => (ImageSource) this.GetValue(RadioButton.LargeIconProperty);
      set => this.SetValue(RadioButton.LargeIconProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RadioButton()
    {
      Type type = typeof (RadioButton);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      ContextMenuService.Attach(type);
      ToolTipService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(RadioButton.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (QuickAccessToolBar));
      return basevalue;
    }

    public RadioButton()
    {
      ContextMenuService.Coerce((DependencyObject) this);
      FocusManager.SetIsFocusScope((DependencyObject) this, true);
    }

    public virtual FrameworkElement CreateQuickAccessItem()
    {
      RadioButton quickAccessItem = new RadioButton();
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "IsChecked", System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, BindingMode.TwoWay);
      quickAccessItem.Click += (RoutedEventHandler) ((sender, e) => this.RaiseEvent(e));
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) quickAccessItem);
      return (FrameworkElement) quickAccessItem;
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(RadioButton.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(RadioButton.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(RadioButton.QuickAccessElementStyleProperty);
      set => this.SetValue(RadioButton.QuickAccessElementStyleProperty, (object) value);
    }

    public void OnKeyTipPressed()
    {
      this.IsChecked = new bool?(true);
      this.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, (object) this));
    }
  }
}
