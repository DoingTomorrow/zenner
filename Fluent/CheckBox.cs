// Decompiled with JetBrains decompiler
// Type: Fluent.CheckBox
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
  public class CheckBox : System.Windows.Controls.CheckBox, IRibbonControl, IKeyTipedControl, IQuickAccessItemProvider
  {
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (CheckBox));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.AttachSizeDefinition(typeof (CheckBox));
    public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof (CheckBox));
    public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof (CheckBox), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(CheckBox.OnIconChanged)));
    public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof (LargeIcon), typeof (ImageSource), typeof (CheckBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (CheckBox), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (CheckBox));

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(CheckBox.SizeProperty);
      set => this.SetValue(CheckBox.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(CheckBox.SizeDefinitionProperty);
      set => this.SetValue(CheckBox.SizeDefinitionProperty, (object) value);
    }

    public object Header
    {
      get => (object) (string) this.GetValue(CheckBox.HeaderProperty);
      set => this.SetValue(CheckBox.HeaderProperty, value);
    }

    public object Icon
    {
      get => (object) (ImageSource) this.GetValue(CheckBox.IconProperty);
      set => this.SetValue(CheckBox.IconProperty, value);
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CheckBox checkBox = d as CheckBox;
      if (e.OldValue is FrameworkElement oldValue)
        checkBox.RemoveLogicalChild((object) oldValue);
      if (!(e.NewValue is FrameworkElement newValue))
        return;
      checkBox.AddLogicalChild((object) newValue);
    }

    public ImageSource LargeIcon
    {
      get => (ImageSource) this.GetValue(CheckBox.LargeIconProperty);
      set => this.SetValue(CheckBox.LargeIconProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static CheckBox()
    {
      Type type = typeof (CheckBox);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      ContextMenuService.Attach(type);
      ToolTipService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (CheckBox), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(CheckBox.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (CheckBox));
      return basevalue;
    }

    public CheckBox()
    {
      ContextMenuService.Coerce((DependencyObject) this);
      FocusManager.SetIsFocusScope((DependencyObject) this, true);
    }

    public virtual FrameworkElement CreateQuickAccessItem()
    {
      CheckBox quickAccessItem = new CheckBox();
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "IsChecked", System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, BindingMode.TwoWay);
      quickAccessItem.Click += (RoutedEventHandler) ((sender, e) => this.RaiseEvent(e));
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) quickAccessItem);
      return (FrameworkElement) quickAccessItem;
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(CheckBox.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(CheckBox.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(CheckBox.QuickAccessElementStyleProperty);
      set => this.SetValue(CheckBox.QuickAccessElementStyleProperty, (object) value);
    }

    public void OnKeyTipPressed()
    {
      bool? isChecked = this.IsChecked;
      this.IsChecked = isChecked.HasValue ? new bool?(!isChecked.GetValueOrDefault()) : new bool?();
      this.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, (object) this));
    }
  }
}
