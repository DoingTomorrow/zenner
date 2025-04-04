// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.SplitButton
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  [ContentProperty("ItemsSource")]
  [DefaultEvent("SelectionChanged")]
  [TemplatePart(Name = "PART_Container", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_Button", Type = typeof (Button))]
  [TemplatePart(Name = "PART_ButtonContent", Type = typeof (ContentControl))]
  [TemplatePart(Name = "PART_Popup", Type = typeof (Popup))]
  [TemplatePart(Name = "PART_Expander", Type = typeof (Button))]
  [TemplatePart(Name = "PART_ListBox", Type = typeof (ListBox))]
  public class SplitButton : ItemsControl
  {
    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (SplitButton));
    public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof (SelectionChangedEventHandler), typeof (SplitButton));
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), typeof (bool), typeof (SplitButton));
    public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) -1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), typeof (object), typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public static readonly DependencyProperty ExtraTagProperty = DependencyProperty.Register(nameof (ExtraTag), typeof (object), typeof (SplitButton));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (object), typeof (SplitButton));
    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(nameof (IconTemplate), typeof (DataTemplate), typeof (SplitButton));
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (SplitButton));
    public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof (CommandTarget), typeof (IInputElement), typeof (SplitButton));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (SplitButton));
    public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(nameof (ButtonStyle), typeof (Style), typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty ButtonArrowStyleProperty = DependencyProperty.Register(nameof (ButtonArrowStyle), typeof (Style), typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty ListBoxStyleProperty = DependencyProperty.Register(nameof (ListBoxStyle), typeof (Style), typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty ArrowBrushProperty = DependencyProperty.Register(nameof (ArrowBrush), typeof (Brush), typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
    private Button _clickButton;
    private Button _expander;
    private ListBox _listBox;
    private Popup _popup;

    public event SelectionChangedEventHandler SelectionChanged
    {
      add => this.AddHandler(SplitButton.SelectionChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(SplitButton.SelectionChangedEvent, (Delegate) value);
    }

    public event RoutedEventHandler Click
    {
      add => this.AddHandler(SplitButton.ClickEvent, (Delegate) value);
      remove => this.RemoveHandler(SplitButton.ClickEvent, (Delegate) value);
    }

    public object CommandParameter
    {
      get => this.GetValue(SplitButton.CommandParameterProperty);
      set => this.SetValue(SplitButton.CommandParameterProperty, value);
    }

    public IInputElement CommandTarget
    {
      get => (IInputElement) this.GetValue(SplitButton.CommandTargetProperty);
      set => this.SetValue(SplitButton.CommandTargetProperty, (object) value);
    }

    public ICommand Command
    {
      get => (ICommand) this.GetValue(SplitButton.CommandProperty);
      set => this.SetValue(SplitButton.CommandProperty, (object) value);
    }

    public int SelectedIndex
    {
      get => (int) this.GetValue(SplitButton.SelectedIndexProperty);
      set => this.SetValue(SplitButton.SelectedIndexProperty, (object) value);
    }

    public object SelectedItem
    {
      get => this.GetValue(SplitButton.SelectedItemProperty);
      set => this.SetValue(SplitButton.SelectedItemProperty, value);
    }

    public bool IsExpanded
    {
      get => (bool) this.GetValue(SplitButton.IsExpandedProperty);
      set => this.SetValue(SplitButton.IsExpandedProperty, (object) value);
    }

    public object ExtraTag
    {
      get => this.GetValue(SplitButton.ExtraTagProperty);
      set => this.SetValue(SplitButton.ExtraTagProperty, value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(SplitButton.OrientationProperty);
      set => this.SetValue(SplitButton.OrientationProperty, (object) value);
    }

    [Bindable(true)]
    public object Icon
    {
      get => this.GetValue(SplitButton.IconProperty);
      set => this.SetValue(SplitButton.IconProperty, value);
    }

    [Bindable(true)]
    public DataTemplate IconTemplate
    {
      get => (DataTemplate) this.GetValue(SplitButton.IconTemplateProperty);
      set => this.SetValue(SplitButton.IconTemplateProperty, (object) value);
    }

    public Style ButtonStyle
    {
      get => (Style) this.GetValue(SplitButton.ButtonStyleProperty);
      set => this.SetValue(SplitButton.ButtonStyleProperty, (object) value);
    }

    public Style ButtonArrowStyle
    {
      get => (Style) this.GetValue(SplitButton.ButtonArrowStyleProperty);
      set => this.SetValue(SplitButton.ButtonArrowStyleProperty, (object) value);
    }

    public Style ListBoxStyle
    {
      get => (Style) this.GetValue(SplitButton.ListBoxStyleProperty);
      set => this.SetValue(SplitButton.ListBoxStyleProperty, (object) value);
    }

    public Brush ArrowBrush
    {
      get => (Brush) this.GetValue(SplitButton.ArrowBrushProperty);
      set => this.SetValue(SplitButton.ArrowBrushProperty, (object) value);
    }

    static SplitButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (SplitButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (SplitButton)));
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {
      e.RoutedEvent = SplitButton.ClickEvent;
      this.RaiseEvent(e);
    }

    private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      e.RoutedEvent = SplitButton.SelectionChangedEvent;
      this.RaiseEvent((RoutedEventArgs) e);
      this.IsExpanded = false;
    }

    private void ExpanderClick(object sender, RoutedEventArgs e)
    {
      this.IsExpanded = !this.IsExpanded;
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this._clickButton = this.EnforceInstance<Button>("PART_Button");
      this._expander = this.EnforceInstance<Button>("PART_Expander");
      this._listBox = this.EnforceInstance<ListBox>("PART_ListBox");
      this._popup = this.EnforceInstance<Popup>("PART_Popup");
      this.InitializeVisualElementsContainer();
    }

    private T EnforceInstance<T>(string partName) where T : FrameworkElement, new()
    {
      if (!(this.GetTemplateChild(partName) is T obj))
        obj = new T();
      return obj;
    }

    private void InitializeVisualElementsContainer()
    {
      this._expander.Click -= new RoutedEventHandler(this.ExpanderClick);
      this._clickButton.Click -= new RoutedEventHandler(this.ButtonClick);
      this._listBox.SelectionChanged -= new SelectionChangedEventHandler(this.ListBoxSelectionChanged);
      this._listBox.PreviewMouseLeftButtonDown -= new MouseButtonEventHandler(this.ListBoxPreviewMouseLeftButtonDown);
      this._popup.Opened -= new EventHandler(this.PopupOpened);
      this._popup.Closed -= new EventHandler(this.PopupClosed);
      this._expander.Click += new RoutedEventHandler(this.ExpanderClick);
      this._clickButton.Click += new RoutedEventHandler(this.ButtonClick);
      this._listBox.SelectionChanged += new SelectionChangedEventHandler(this.ListBoxSelectionChanged);
      this._listBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.ListBoxPreviewMouseLeftButtonDown);
      this._popup.Opened += new EventHandler(this.PopupOpened);
      this._popup.Closed += new EventHandler(this.PopupClosed);
    }

    private void ListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!(e.OriginalSource is DependencyObject originalSource) || !(ItemsControl.ContainerFromElement((ItemsControl) this._listBox, originalSource) is ListBoxItem))
        return;
      this.IsExpanded = false;
    }

    private void PopupClosed(object sender, EventArgs e)
    {
      this.ReleaseMouseCapture();
      if (this._expander == null)
        return;
      this._expander.Focus();
    }

    private void PopupOpened(object sender, EventArgs e)
    {
      Mouse.Capture((IInputElement) this, CaptureMode.SubTree);
      Mouse.AddPreviewMouseDownOutsideCapturedElementHandler((DependencyObject) this, new MouseButtonEventHandler(this.OutsideCapturedElementHandler));
    }

    private void OutsideCapturedElementHandler(
      object sender,
      MouseButtonEventArgs mouseButtonEventArgs)
    {
      this.IsExpanded = false;
      Mouse.RemovePreviewMouseDownOutsideCapturedElementHandler((DependencyObject) this, new MouseButtonEventHandler(this.OutsideCapturedElementHandler));
    }
  }
}
