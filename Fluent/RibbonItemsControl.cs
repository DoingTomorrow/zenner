// Decompiled with JetBrains decompiler
// Type: Fluent.RibbonItemsControl
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  [ContentProperty("Items")]
  public abstract class RibbonItemsControl : 
    ItemsControl,
    IQuickAccessItemProvider,
    IRibbonControl,
    IKeyTipedControl
  {
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (RibbonItemsControl));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.SizeDefinitionProperty.AddOwner(typeof (RibbonItemsControl));
    public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof (RibbonItemsControl));
    public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof (RibbonItemsControl));
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (RibbonItemsControl));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (RibbonItemsControl));

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(RibbonItemsControl.SizeProperty);
      set => this.SetValue(RibbonItemsControl.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(RibbonItemsControl.SizeDefinitionProperty);
      set => this.SetValue(RibbonItemsControl.SizeDefinitionProperty, (object) value);
    }

    public object Header
    {
      get => (object) (string) this.GetValue(RibbonItemsControl.HeaderProperty);
      set => this.SetValue(RibbonItemsControl.HeaderProperty, value);
    }

    public object Icon
    {
      get => (object) (ImageSource) this.GetValue(RibbonItemsControl.IconProperty);
      set => this.SetValue(RibbonItemsControl.IconProperty, value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static RibbonItemsControl()
    {
      Type type = typeof (RibbonItemsControl);
      ToolTipService.Attach(type);
      ContextMenuService.Attach(type);
    }

    protected RibbonItemsControl() => ContextMenuService.Coerce((DependencyObject) this);

    public abstract FrameworkElement CreateQuickAccessItem();

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(RibbonItemsControl.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(RibbonItemsControl.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(RibbonItemsControl.QuickAccessElementStyleProperty);
      set => this.SetValue(RibbonItemsControl.QuickAccessElementStyleProperty, (object) value);
    }

    public virtual void OnKeyTipPressed()
    {
    }

    protected virtual void OnSizePropertyChanged(
      RibbonControlSize previous,
      RibbonControlSize current)
    {
    }
  }
}
