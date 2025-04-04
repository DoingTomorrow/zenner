// Decompiled with JetBrains decompiler
// Type: Fluent.SplitButton
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

#nullable disable
namespace Fluent
{
  [TemplatePart(Name = "PART_Button", Type = typeof (ButtonBase))]
  public class SplitButton : DropDownButton, ICommandSource
  {
    private ToggleButton button;
    public static readonly DependencyProperty CommandParameterProperty = ButtonBase.CommandParameterProperty.AddOwner(typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CommandProperty = ButtonBase.CommandProperty.AddOwner(typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CommandTargetProperty = ButtonBase.CommandTargetProperty.AddOwner(typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof (GroupName), typeof (string), typeof (SplitButton), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool), typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SplitButton.OnIsCheckedChanged), new CoerceValueCallback(SplitButton.CoerceIsChecked)));
    public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(nameof (IsCheckable), typeof (bool), typeof (SplitButton), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty DropDownToolTipProperty = DependencyProperty.Register(nameof (DropDownToolTip), typeof (object), typeof (SplitButton), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsButtonEnabledProperty = DependencyProperty.Register(nameof (IsButtonEnabled), typeof (bool), typeof (SplitButton), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsDefinitiveProperty = DependencyProperty.Register(nameof (IsDefinitive), typeof (bool), typeof (SplitButton), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly RoutedEvent ClickEvent = ButtonBase.ClickEvent.AddOwner(typeof (SplitButton));
    public static readonly RoutedEvent CheckedEvent = System.Windows.Controls.Primitives.ToggleButton.CheckedEvent.AddOwner(typeof (SplitButton));
    public static readonly RoutedEvent UncheckedEvent = System.Windows.Controls.Primitives.ToggleButton.UncheckedEvent.AddOwner(typeof (SplitButton));
    public static readonly DependencyProperty CanAddButtonToQuickAccessToolBarProperty = DependencyProperty.Register(nameof (CanAddButtonToQuickAccessToolBar), typeof (bool), typeof (SplitButton), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(RibbonControl.OnCanAddToQuickAccessToolbarChanged)));

    protected override IEnumerator LogicalChildren
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        if (this.Items != null)
          arrayList.AddRange((ICollection) this.Items);
        if (this.button != null)
          arrayList.Add((object) this.button);
        return arrayList.GetEnumerator();
      }
    }

    [Localizability(LocalizationCategory.NeverLocalize)]
    [Bindable(true)]
    [Category("Action")]
    public ICommand Command
    {
      get => (ICommand) this.GetValue(SplitButton.CommandProperty);
      set => this.SetValue(SplitButton.CommandProperty, (object) value);
    }

    [Localizability(LocalizationCategory.NeverLocalize)]
    [Category("Action")]
    [Bindable(true)]
    public object CommandParameter
    {
      get => this.GetValue(SplitButton.CommandParameterProperty);
      set => this.SetValue(SplitButton.CommandParameterProperty, value);
    }

    [Bindable(true)]
    [Category("Action")]
    public IInputElement CommandTarget
    {
      get => (IInputElement) this.GetValue(SplitButton.CommandTargetProperty);
      set => this.SetValue(SplitButton.CommandTargetProperty, (object) value);
    }

    public string GroupName
    {
      get => (string) this.GetValue(SplitButton.GroupNameProperty);
      set => this.SetValue(SplitButton.GroupNameProperty, (object) value);
    }

    public bool IsChecked
    {
      get => (bool) this.GetValue(SplitButton.IsCheckedProperty);
      set => this.SetValue(SplitButton.IsCheckedProperty, (object) value);
    }

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      SplitButton source = d as SplitButton;
      if (!source.IsCheckable)
        return;
      if ((bool) e.NewValue)
        source.RaiseEvent(new RoutedEventArgs(SplitButton.CheckedEvent, (object) source));
      else
        source.RaiseEvent(new RoutedEventArgs(SplitButton.UncheckedEvent, (object) source));
      if (Mouse.Captured != source)
        return;
      Mouse.Capture((IInputElement) null);
    }

    private static object CoerceIsChecked(DependencyObject d, object basevalue)
    {
      return !(d as SplitButton).IsCheckable ? (object) false : basevalue;
    }

    public bool IsCheckable
    {
      get => (bool) this.GetValue(SplitButton.IsCheckableProperty);
      set => this.SetValue(SplitButton.IsCheckableProperty, (object) value);
    }

    public object DropDownToolTip
    {
      get => this.GetValue(SplitButton.DropDownToolTipProperty);
      set => this.SetValue(SplitButton.DropDownToolTipProperty, value);
    }

    public bool IsButtonEnabled
    {
      get => (bool) this.GetValue(SplitButton.IsButtonEnabledProperty);
      set => this.SetValue(SplitButton.IsButtonEnabledProperty, (object) value);
    }

    public bool IsDefinitive
    {
      get => (bool) this.GetValue(SplitButton.IsDefinitiveProperty);
      set => this.SetValue(SplitButton.IsDefinitiveProperty, (object) value);
    }

    public event RoutedEventHandler Click
    {
      add => this.AddHandler(SplitButton.ClickEvent, (Delegate) value);
      remove => this.RemoveHandler(SplitButton.ClickEvent, (Delegate) value);
    }

    public event RoutedEventHandler Checked
    {
      add => this.AddHandler(SplitButton.CheckedEvent, (Delegate) value);
      remove => this.RemoveHandler(SplitButton.CheckedEvent, (Delegate) value);
    }

    public event RoutedEventHandler Unchecked
    {
      add => this.AddHandler(SplitButton.UncheckedEvent, (Delegate) value);
      remove => this.RemoveHandler(SplitButton.UncheckedEvent, (Delegate) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static SplitButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (SplitButton)));
      FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(SplitButton.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (SplitButton));
      return basevalue;
    }

    public SplitButton()
    {
      ContextMenuService.Coerce((DependencyObject) this);
      this.Click += new RoutedEventHandler(this.OnClick);
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
      if (e.OriginalSource == this)
        return;
      e.Handled = true;
    }

    public override void OnApplyTemplate()
    {
      if (this.button != null)
        this.button.Click -= new RoutedEventHandler(this.OnButtonClick);
      this.button = this.GetTemplateChild("PART_Button") as ToggleButton;
      if (this.button != null)
        this.button.Click += new RoutedEventHandler(this.OnButtonClick);
      base.OnApplyTemplate();
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (PopupService.IsMousePhysicallyOver((UIElement) this.button))
        return;
      base.OnPreviewMouseLeftButtonDown(e);
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
      e.Handled = true;
      this.RaiseEvent(new RoutedEventArgs(SplitButton.ClickEvent, (object) this));
    }

    public override FrameworkElement CreateQuickAccessItem()
    {
      SplitButton element = new SplitButton();
      element.Click += (RoutedEventHandler) ((sender, e) => this.RaiseEvent(e));
      element.Size = RibbonControlSize.Small;
      element.CanAddButtonToQuickAccessToolBar = false;
      this.BindQuickAccessItem((FrameworkElement) element);
      element.DropDownOpened += new EventHandler(((DropDownButton) this).OnQuickAccessOpened);
      return (FrameworkElement) element;
    }

    protected override void BindQuickAccessItem(FrameworkElement element)
    {
      RibbonControl.Bind((object) this, element, "DisplayMemberPath", ItemsControl.DisplayMemberPathProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, element, "GroupStyleSelector", ItemsControl.GroupStyleSelectorProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, element, "ItemContainerStyle", ItemsControl.ItemContainerStyleProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, element, "ItemsPanel", ItemsControl.ItemsPanelProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, element, "ItemStringFormat", ItemsControl.ItemStringFormatProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, element, "ItemTemplate", ItemsControl.ItemTemplateProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, element, "MaxDropDownHeight", DropDownButton.MaxDropDownHeightProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, element, "IsChecked", SplitButton.IsCheckedProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, element, "DropDownToolTip", SplitButton.DropDownToolTipProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, element, "IsCheckable", SplitButton.IsCheckableProperty, BindingMode.Default);
      RibbonControl.Bind((object) this, element, "IsButtonEnabled", SplitButton.IsButtonEnabledProperty, BindingMode.Default);
      RibbonControl.Bind((object) this, element, "ContextMenu", FrameworkElement.ContextMenuProperty, BindingMode.Default);
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, element);
      RibbonControl.Bind((object) this, element, "ResizeMode", DropDownButton.ResizeModeProperty, BindingMode.Default);
      RibbonControl.Bind((object) this, element, "MaxDropDownHeight", DropDownButton.MaxDropDownHeightProperty, BindingMode.Default);
      RibbonControl.Bind((object) this, element, "HasTriangle", DropDownButton.HasTriangleProperty, BindingMode.Default);
    }

    public bool CanAddButtonToQuickAccessToolBar
    {
      get => (bool) this.GetValue(SplitButton.CanAddButtonToQuickAccessToolBarProperty);
      set => this.SetValue(SplitButton.CanAddButtonToQuickAccessToolBarProperty, (object) value);
    }
  }
}
