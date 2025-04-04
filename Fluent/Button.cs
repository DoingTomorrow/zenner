// Decompiled with JetBrains decompiler
// Type: Fluent.Button
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  [System.Windows.Markup.ContentProperty("Header")]
  public class Button : System.Windows.Controls.Button, IRibbonControl, IKeyTipedControl, IQuickAccessItemProvider
  {
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (Button));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.AttachSizeDefinition(typeof (Button));
    public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof (Button));
    public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof (Button), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(Button.OnIconChanged)));
    public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof (LargeIcon), typeof (object), typeof (Button), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, (CoerceValueCallback) null));
    public static readonly DependencyProperty IsDefinitiveProperty = DependencyProperty.Register(nameof (IsDefinitive), typeof (bool), typeof (Button), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (Button), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (Button));

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(Button.SizeProperty);
      set => this.SetValue(Button.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(Button.SizeDefinitionProperty);
      set => this.SetValue(Button.SizeDefinitionProperty, (object) value);
    }

    public object Header
    {
      get => (object) (string) this.GetValue(Button.HeaderProperty);
      set => this.SetValue(Button.HeaderProperty, value);
    }

    public object Icon
    {
      get => this.GetValue(Button.IconProperty);
      set => this.SetValue(Button.IconProperty, value);
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Button button = d as Button;
      if (e.OldValue is FrameworkElement oldValue)
        button.RemoveLogicalChild((object) oldValue);
      if (!(e.NewValue is FrameworkElement newValue))
        return;
      button.AddLogicalChild((object) newValue);
    }

    public object LargeIcon
    {
      get => this.GetValue(Button.LargeIconProperty);
      set => this.SetValue(Button.LargeIconProperty, value);
    }

    public bool IsDefinitive
    {
      get => (bool) this.GetValue(Button.IsDefinitiveProperty);
      set => this.SetValue(Button.IsDefinitiveProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static Button()
    {
      Type type = typeof (Button);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      ContextMenuService.Attach(type);
      ToolTipService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (Button), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(Button.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (Button));
      return basevalue;
    }

    public Button()
    {
      ContextMenuService.Coerce((DependencyObject) this);
      FocusManager.SetIsFocusScope((DependencyObject) this, true);
    }

    protected override void OnClick()
    {
      if (this.IsDefinitive)
        PopupService.RaiseDismissPopupEvent((object) this, DismissPopupMode.Always);
      base.OnClick();
    }

    public virtual FrameworkElement CreateQuickAccessItem()
    {
      Button element = new Button();
      element.Click += (RoutedEventHandler) ((sender, e) => this.RaiseEvent(e));
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) element);
      return (FrameworkElement) element;
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(Button.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(Button.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(Button.QuickAccessElementStyleProperty);
      set => this.SetValue(Button.QuickAccessElementStyleProperty, (object) value);
    }

    public void OnKeyTipPressed()
    {
      this.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, (object) this));
    }
  }
}
