// Decompiled with JetBrains decompiler
// Type: Fluent.ToggleButton
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  [System.Windows.Markup.ContentProperty("Header")]
  public class ToggleButton : 
    System.Windows.Controls.Primitives.ToggleButton,
    IRibbonControl,
    IKeyTipedControl,
    IQuickAccessItemProvider
  {
    public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof (GroupName), typeof (string), typeof (ToggleButton), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(ToggleButton.OnGroupNameChanged)));
    private static readonly Dictionary<string, List<WeakReference>> groupedButtons = new Dictionary<string, List<WeakReference>>();
    public static readonly DependencyProperty SizeProperty = RibbonControl.SizeProperty.AddOwner(typeof (ToggleButton));
    public static readonly DependencyProperty SizeDefinitionProperty = RibbonControl.AttachSizeDefinition(typeof (ToggleButton));
    public static readonly DependencyProperty HeaderProperty = RibbonControl.HeaderProperty.AddOwner(typeof (ToggleButton));
    public static readonly DependencyProperty IconProperty = RibbonControl.IconProperty.AddOwner(typeof (ToggleButton), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(ToggleButton.OnIconChanged)));
    public static readonly DependencyProperty LargeIconProperty = DependencyProperty.Register(nameof (LargeIcon), typeof (object), typeof (ToggleButton), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsDefinitiveProperty = DependencyProperty.Register(nameof (IsDefinitive), typeof (bool), typeof (ToggleButton), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty CanAddToQuickAccessToolBarProperty = RibbonControl.CanAddToQuickAccessToolBarProperty.AddOwner(typeof (ToggleButton), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));
    public static readonly DependencyProperty QuickAccessElementStyleProperty = RibbonControl.QuickAccessElementStyleProperty.AddOwner(typeof (ToggleButton));

    public string GroupName
    {
      get => (string) this.GetValue(ToggleButton.GroupNameProperty);
      set => this.SetValue(ToggleButton.GroupNameProperty, (object) value);
    }

    private static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToggleButton button = (ToggleButton) d;
      string newValue = (string) e.NewValue;
      string oldValue = (string) e.OldValue;
      if (oldValue != null)
        ToggleButton.RemoveFromGroup(oldValue, button);
      if (newValue == null)
        return;
      ToggleButton.AddToGroup(newValue, button);
    }

    private static void RemoveFromGroup(string groupName, ToggleButton button)
    {
      List<WeakReference> weakReferenceList = (List<WeakReference>) null;
      if (!ToggleButton.groupedButtons.TryGetValue(groupName, out weakReferenceList))
        return;
      weakReferenceList.RemoveAt(weakReferenceList.FindIndex((Predicate<WeakReference>) (x => x.IsAlive && (ToggleButton) x.Target == button)));
    }

    private static void AddToGroup(string groupName, ToggleButton button)
    {
      List<WeakReference> weakReferenceList = (List<WeakReference>) null;
      if (!ToggleButton.groupedButtons.TryGetValue(groupName, out weakReferenceList))
      {
        weakReferenceList = new List<WeakReference>();
        ToggleButton.groupedButtons.Add(groupName, weakReferenceList);
      }
      weakReferenceList.Add(new WeakReference((object) button));
    }

    private static IEnumerable<ToggleButton> GetButtonsInGroup(string groupName)
    {
      List<WeakReference> source = (List<WeakReference>) null;
      return !ToggleButton.groupedButtons.TryGetValue(groupName, out source) ? (IEnumerable<ToggleButton>) new List<ToggleButton>() : source.Where<WeakReference>((Func<WeakReference, bool>) (x => x.IsAlive)).Select<WeakReference, ToggleButton>((Func<WeakReference, ToggleButton>) (x => (ToggleButton) x.Target));
    }

    public RibbonControlSize Size
    {
      get => (RibbonControlSize) this.GetValue(ToggleButton.SizeProperty);
      set => this.SetValue(ToggleButton.SizeProperty, (object) value);
    }

    public string SizeDefinition
    {
      get => (string) this.GetValue(ToggleButton.SizeDefinitionProperty);
      set => this.SetValue(ToggleButton.SizeDefinitionProperty, (object) value);
    }

    public object Header
    {
      get => (object) (string) this.GetValue(ToggleButton.HeaderProperty);
      set => this.SetValue(ToggleButton.HeaderProperty, value);
    }

    public object Icon
    {
      get => this.GetValue(ToggleButton.IconProperty);
      set => this.SetValue(ToggleButton.IconProperty, value);
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToggleButton toggleButton = d as ToggleButton;
      if (e.OldValue is FrameworkElement oldValue)
        toggleButton.RemoveLogicalChild((object) oldValue);
      if (!(e.NewValue is FrameworkElement newValue))
        return;
      toggleButton.AddLogicalChild((object) newValue);
    }

    public object LargeIcon
    {
      get => this.GetValue(ToggleButton.LargeIconProperty);
      set => this.SetValue(ToggleButton.LargeIconProperty, value);
    }

    public bool IsDefinitive
    {
      get => (bool) this.GetValue(ToggleButton.IsDefinitiveProperty);
      set => this.SetValue(ToggleButton.IsDefinitiveProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static ToggleButton()
    {
      Type type = typeof (ToggleButton);
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((object) type));
      System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(ToggleButton.OnIsCheckedChanged), new CoerceValueCallback(ToggleButton.CoerceIsChecked)));
      ContextMenuService.Attach(type);
      ToolTipService.Attach(type);
      FrameworkElement.StyleProperty.OverrideMetadata(type, (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(ToggleButton.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (ToggleButton));
      return basevalue;
    }

    private static object CoerceIsChecked(DependencyObject d, object basevalue)
    {
      ToggleButton toggleButton1 = (ToggleButton) d;
      if (toggleButton1.GroupName == null || (bool) basevalue)
        return basevalue;
      foreach (System.Windows.Controls.Primitives.ToggleButton toggleButton2 in ToggleButton.GetButtonsInGroup(toggleButton1.GroupName))
      {
        bool? isChecked = toggleButton2.IsChecked;
        if ((!isChecked.GetValueOrDefault() ? 0 : (isChecked.HasValue ? 1 : 0)) != 0)
          return (object) false;
      }
      return (object) true;
    }

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      bool newValue = (bool) e.NewValue;
      ToggleButton toggleButton1 = (ToggleButton) d;
      if (!newValue || toggleButton1.GroupName == null)
        return;
      foreach (ToggleButton toggleButton2 in ToggleButton.GetButtonsInGroup(toggleButton1.GroupName))
      {
        if (toggleButton2 != toggleButton1)
          toggleButton2.IsChecked = new bool?(false);
      }
    }

    public ToggleButton()
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
      ToggleButton quickAccessItem = new ToggleButton();
      RibbonControl.Bind((object) this, (FrameworkElement) quickAccessItem, "IsChecked", System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, BindingMode.TwoWay);
      quickAccessItem.Click += (RoutedEventHandler) ((sender, e) => this.RaiseEvent(e));
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, (FrameworkElement) quickAccessItem);
      return (FrameworkElement) quickAccessItem;
    }

    public bool CanAddToQuickAccessToolBar
    {
      get => (bool) this.GetValue(ToggleButton.CanAddToQuickAccessToolBarProperty);
      set => this.SetValue(ToggleButton.CanAddToQuickAccessToolBarProperty, (object) value);
    }

    public Style QuickAccessElementStyle
    {
      get => (Style) this.GetValue(ToggleButton.QuickAccessElementStyleProperty);
      set => this.SetValue(ToggleButton.QuickAccessElementStyleProperty, (object) value);
    }

    public void OnKeyTipPressed()
    {
      bool? isChecked = this.IsChecked;
      this.IsChecked = isChecked.HasValue ? new bool?(!isChecked.GetValueOrDefault()) : new bool?();
      this.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent, (object) this));
    }
  }
}
