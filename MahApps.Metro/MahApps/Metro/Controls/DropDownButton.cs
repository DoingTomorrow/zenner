// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.DropDownButton
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  [System.Windows.Markup.ContentProperty("ItemsSource")]
  [TemplatePart(Name = "PART_Button", Type = typeof (Button))]
  [TemplatePart(Name = "PART_Image", Type = typeof (Image))]
  [TemplatePart(Name = "PART_ButtonContent", Type = typeof (ContentControl))]
  [TemplatePart(Name = "PART_Menu", Type = typeof (ContextMenu))]
  public class DropDownButton : ItemsControl
  {
    public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (DropDownButton));
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), typeof (bool), typeof (DropDownButton), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(DropDownButton.IsExpandedPropertyChangedCallback)));
    public static readonly DependencyProperty ExtraTagProperty = DependencyProperty.Register(nameof (ExtraTag), typeof (object), typeof (DropDownButton));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (DropDownButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure));
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (object), typeof (DropDownButton));
    public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(nameof (IconTemplate), typeof (DataTemplate), typeof (DropDownButton));
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (DropDownButton));
    public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof (CommandTarget), typeof (IInputElement), typeof (DropDownButton));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (DropDownButton));
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (DropDownButton));
    public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(nameof (ButtonStyle), typeof (Style), typeof (DropDownButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty MenuStyleProperty = DependencyProperty.Register(nameof (MenuStyle), typeof (Style), typeof (DropDownButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty ArrowBrushProperty = DependencyProperty.Register(nameof (ArrowBrush), typeof (Brush), typeof (DropDownButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ArrowVisibilityProperty = DependencyProperty.Register(nameof (ArrowVisibility), typeof (Visibility), typeof (DropDownButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) Visibility.Visible, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
    private Button clickButton;
    private ContextMenu menu;

    public event RoutedEventHandler Click
    {
      add => this.AddHandler(DropDownButton.ClickEvent, (Delegate) value);
      remove => this.RemoveHandler(DropDownButton.ClickEvent, (Delegate) value);
    }

    private static void IsExpandedPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      DropDownButton dropDownButton = (DropDownButton) dependencyObject;
      if (dropDownButton.clickButton == null)
        return;
      dropDownButton.menu.Placement = PlacementMode.Bottom;
      dropDownButton.menu.PlacementTarget = (UIElement) dropDownButton.clickButton;
    }

    public object Content
    {
      get => this.GetValue(DropDownButton.ContentProperty);
      set => this.SetValue(DropDownButton.ContentProperty, value);
    }

    public object CommandParameter
    {
      get => this.GetValue(DropDownButton.CommandParameterProperty);
      set => this.SetValue(DropDownButton.CommandParameterProperty, value);
    }

    public IInputElement CommandTarget
    {
      get => (IInputElement) this.GetValue(DropDownButton.CommandTargetProperty);
      set => this.SetValue(DropDownButton.CommandTargetProperty, (object) value);
    }

    public ICommand Command
    {
      get => (ICommand) this.GetValue(DropDownButton.CommandProperty);
      set => this.SetValue(DropDownButton.CommandProperty, (object) value);
    }

    public bool IsExpanded
    {
      get => (bool) this.GetValue(DropDownButton.IsExpandedProperty);
      set => this.SetValue(DropDownButton.IsExpandedProperty, (object) value);
    }

    public object ExtraTag
    {
      get => this.GetValue(DropDownButton.ExtraTagProperty);
      set => this.SetValue(DropDownButton.ExtraTagProperty, value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(DropDownButton.OrientationProperty);
      set => this.SetValue(DropDownButton.OrientationProperty, (object) value);
    }

    [Bindable(true)]
    public object Icon
    {
      get => this.GetValue(DropDownButton.IconProperty);
      set => this.SetValue(DropDownButton.IconProperty, value);
    }

    [Bindable(true)]
    public DataTemplate IconTemplate
    {
      get => (DataTemplate) this.GetValue(DropDownButton.IconTemplateProperty);
      set => this.SetValue(DropDownButton.IconTemplateProperty, (object) value);
    }

    public Style ButtonStyle
    {
      get => (Style) this.GetValue(DropDownButton.ButtonStyleProperty);
      set => this.SetValue(DropDownButton.ButtonStyleProperty, (object) value);
    }

    public Style MenuStyle
    {
      get => (Style) this.GetValue(DropDownButton.MenuStyleProperty);
      set => this.SetValue(DropDownButton.MenuStyleProperty, (object) value);
    }

    public Brush ArrowBrush
    {
      get => (Brush) this.GetValue(DropDownButton.ArrowBrushProperty);
      set => this.SetValue(DropDownButton.ArrowBrushProperty, (object) value);
    }

    public Visibility ArrowVisibility
    {
      get => (Visibility) this.GetValue(DropDownButton.ArrowVisibilityProperty);
      set => this.SetValue(DropDownButton.ArrowVisibilityProperty, (object) value);
    }

    static DropDownButton()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (DropDownButton), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (DropDownButton)));
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {
      this.IsExpanded = true;
      e.RoutedEvent = DropDownButton.ClickEvent;
      this.RaiseEvent(e);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.clickButton = this.EnforceInstance<Button>("PART_Button");
      this.menu = this.EnforceInstance<ContextMenu>("PART_Menu");
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
      this.MouseRightButtonUp -= new MouseButtonEventHandler(this.DropDownButton_MouseRightButtonUp);
      this.clickButton.Click -= new RoutedEventHandler(this.ButtonClick);
      this.MouseRightButtonUp += new MouseButtonEventHandler(this.DropDownButton_MouseRightButtonUp);
      this.clickButton.Click += new RoutedEventHandler(this.ButtonClick);
    }

    private void DropDownButton_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
      e.Handled = true;
    }
  }
}
