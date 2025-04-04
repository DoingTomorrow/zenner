// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ToggleSwitch
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  [TemplatePart(Name = "Switch", Type = typeof (ToggleButton))]
  public class ToggleSwitch : ContentControl
  {
    private const string CommonStates = "CommonStates";
    private const string NormalState = "Normal";
    private const string DisabledState = "Disabled";
    private const string SwitchPart = "Switch";
    private ToggleButton _toggleButton;
    public static readonly DependencyProperty OnLabelProperty = DependencyProperty.Register(nameof (OnLabel), typeof (string), typeof (ToggleSwitch), new PropertyMetadata((object) "On"));
    public static readonly DependencyProperty OffLabelProperty = DependencyProperty.Register(nameof (OffLabel), typeof (string), typeof (ToggleSwitch), new PropertyMetadata((object) "Off"));
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (object), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    [Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
    public static readonly DependencyProperty SwitchForegroundProperty = DependencyProperty.Register(nameof (SwitchForeground), typeof (Brush), typeof (ToggleSwitch), new PropertyMetadata((object) null, (PropertyChangedCallback) ((o, e) => ((ToggleSwitch) o).OnSwitchBrush = e.NewValue as Brush)));
    public static readonly DependencyProperty OnSwitchBrushProperty = DependencyProperty.Register(nameof (OnSwitchBrush), typeof (Brush), typeof (ToggleSwitch), (PropertyMetadata) null);
    public static readonly DependencyProperty OffSwitchBrushProperty = DependencyProperty.Register(nameof (OffSwitchBrush), typeof (Brush), typeof (ToggleSwitch), (PropertyMetadata) null);
    public static readonly DependencyProperty ThumbIndicatorBrushProperty = DependencyProperty.Register(nameof (ThumbIndicatorBrush), typeof (Brush), typeof (ToggleSwitch), (PropertyMetadata) null);
    public static readonly DependencyProperty ThumbIndicatorDisabledBrushProperty = DependencyProperty.Register(nameof (ThumbIndicatorDisabledBrush), typeof (Brush), typeof (ToggleSwitch), (PropertyMetadata) null);
    public static readonly DependencyProperty ThumbIndicatorWidthProperty = DependencyProperty.Register(nameof (ThumbIndicatorWidth), typeof (double), typeof (ToggleSwitch), new PropertyMetadata((object) 13.0));
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool?), typeof (ToggleSwitch), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ToggleSwitch.OnIsCheckedChanged)));
    public static readonly DependencyProperty CheckChangedCommandProperty = DependencyProperty.Register(nameof (CheckChangedCommand), typeof (ICommand), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CheckedCommandProperty = DependencyProperty.Register(nameof (CheckedCommand), typeof (ICommand), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty UnCheckedCommandProperty = DependencyProperty.Register(nameof (UnCheckedCommand), typeof (ICommand), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CheckChangedCommandParameterProperty = DependencyProperty.Register(nameof (CheckChangedCommandParameter), typeof (object), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CheckedCommandParameterProperty = DependencyProperty.Register(nameof (CheckedCommandParameter), typeof (object), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty UnCheckedCommandParameterProperty = DependencyProperty.Register(nameof (UnCheckedCommandParameter), typeof (object), typeof (ToggleSwitch), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ContentDirectionProperty = DependencyProperty.Register(nameof (ContentDirection), typeof (FlowDirection), typeof (ToggleSwitch), new PropertyMetadata((object) FlowDirection.LeftToRight));
    public static readonly DependencyProperty ToggleSwitchButtonStyleProperty = DependencyProperty.Register(nameof (ToggleSwitchButtonStyle), typeof (Style), typeof (ToggleSwitch), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

    public event EventHandler<RoutedEventArgs> Checked;

    public event EventHandler<RoutedEventArgs> Unchecked;

    public event EventHandler<RoutedEventArgs> Indeterminate;

    public event EventHandler<RoutedEventArgs> Click;

    public string OnLabel
    {
      get => (string) this.GetValue(ToggleSwitch.OnLabelProperty);
      set => this.SetValue(ToggleSwitch.OnLabelProperty, (object) value);
    }

    public string OffLabel
    {
      get => (string) this.GetValue(ToggleSwitch.OffLabelProperty);
      set => this.SetValue(ToggleSwitch.OffLabelProperty, (object) value);
    }

    public object Header
    {
      get => this.GetValue(ToggleSwitch.HeaderProperty);
      set => this.SetValue(ToggleSwitch.HeaderProperty, value);
    }

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) this.GetValue(ToggleSwitch.HeaderTemplateProperty);
      set => this.SetValue(ToggleSwitch.HeaderTemplateProperty, (object) value);
    }

    [Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
    public Brush SwitchForeground
    {
      get => (Brush) this.GetValue(ToggleSwitch.SwitchForegroundProperty);
      set => this.SetValue(ToggleSwitch.SwitchForegroundProperty, (object) value);
    }

    public Brush OnSwitchBrush
    {
      get => (Brush) this.GetValue(ToggleSwitch.OnSwitchBrushProperty);
      set => this.SetValue(ToggleSwitch.OnSwitchBrushProperty, (object) value);
    }

    public Brush OffSwitchBrush
    {
      get => (Brush) this.GetValue(ToggleSwitch.OffSwitchBrushProperty);
      set => this.SetValue(ToggleSwitch.OffSwitchBrushProperty, (object) value);
    }

    public Brush ThumbIndicatorBrush
    {
      get => (Brush) this.GetValue(ToggleSwitch.ThumbIndicatorBrushProperty);
      set => this.SetValue(ToggleSwitch.ThumbIndicatorBrushProperty, (object) value);
    }

    public Brush ThumbIndicatorDisabledBrush
    {
      get => (Brush) this.GetValue(ToggleSwitch.ThumbIndicatorDisabledBrushProperty);
      set => this.SetValue(ToggleSwitch.ThumbIndicatorDisabledBrushProperty, (object) value);
    }

    public double ThumbIndicatorWidth
    {
      get => (double) this.GetValue(ToggleSwitch.ThumbIndicatorWidthProperty);
      set => this.SetValue(ToggleSwitch.ThumbIndicatorWidthProperty, (object) value);
    }

    public FlowDirection ContentDirection
    {
      get => (FlowDirection) this.GetValue(ToggleSwitch.ContentDirectionProperty);
      set => this.SetValue(ToggleSwitch.ContentDirectionProperty, (object) value);
    }

    public Style ToggleSwitchButtonStyle
    {
      get => (Style) this.GetValue(ToggleSwitch.ToggleSwitchButtonStyleProperty);
      set => this.SetValue(ToggleSwitch.ToggleSwitchButtonStyleProperty, (object) value);
    }

    [TypeConverter(typeof (NullableBoolConverter))]
    public bool? IsChecked
    {
      get => (bool?) this.GetValue(ToggleSwitch.IsCheckedProperty);
      set => this.SetValue(ToggleSwitch.IsCheckedProperty, (object) value);
    }

    public ICommand CheckChangedCommand
    {
      get => (ICommand) this.GetValue(ToggleSwitch.CheckChangedCommandProperty);
      set => this.SetValue(ToggleSwitch.CheckChangedCommandProperty, (object) value);
    }

    public ICommand CheckedCommand
    {
      get => (ICommand) this.GetValue(ToggleSwitch.CheckedCommandProperty);
      set => this.SetValue(ToggleSwitch.CheckedCommandProperty, (object) value);
    }

    public ICommand UnCheckedCommand
    {
      get => (ICommand) this.GetValue(ToggleSwitch.UnCheckedCommandProperty);
      set => this.SetValue(ToggleSwitch.UnCheckedCommandProperty, (object) value);
    }

    public object CheckChangedCommandParameter
    {
      get => this.GetValue(ToggleSwitch.CheckChangedCommandParameterProperty);
      set => this.SetValue(ToggleSwitch.CheckChangedCommandParameterProperty, value);
    }

    public object CheckedCommandParameter
    {
      get => this.GetValue(ToggleSwitch.CheckedCommandParameterProperty);
      set => this.SetValue(ToggleSwitch.CheckedCommandParameterProperty, value);
    }

    public object UnCheckedCommandParameter
    {
      get => this.GetValue(ToggleSwitch.UnCheckedCommandParameterProperty);
      set => this.SetValue(ToggleSwitch.UnCheckedCommandParameterProperty, value);
    }

    public event EventHandler IsCheckedChanged;

    private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToggleSwitch sender = (ToggleSwitch) d;
      if (sender._toggleButton == null)
        return;
      bool? oldValue = (bool?) e.OldValue;
      bool? newValue = (bool?) e.NewValue;
      bool? nullable1 = oldValue;
      bool? nullable2 = newValue;
      if ((nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() ? (nullable1.HasValue != nullable2.HasValue ? 1 : 0) : 1) == 0)
        return;
      ICommand checkChangedCommand = sender.CheckChangedCommand;
      object parameter = sender.CheckChangedCommandParameter ?? (object) sender;
      if (checkChangedCommand != null && checkChangedCommand.CanExecute(parameter))
        checkChangedCommand.Execute(parameter);
      EventHandler isCheckedChanged = sender.IsCheckedChanged;
      if (isCheckedChanged == null)
        return;
      isCheckedChanged((object) sender, EventArgs.Empty);
    }

    public ToggleSwitch()
    {
      this.DefaultStyleKey = (object) typeof (ToggleSwitch);
      this.PreviewKeyUp += new KeyEventHandler(this.ToggleSwitch_PreviewKeyUp);
      this.MouseUp += (MouseButtonEventHandler) ((sender, args) => Keyboard.Focus((IInputElement) this));
    }

    private void ToggleSwitch_PreviewKeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Space || e.OriginalSource != sender)
        return;
      bool? isChecked = this.IsChecked;
      this.IsChecked = isChecked.HasValue ? new bool?(!isChecked.GetValueOrDefault()) : new bool?();
    }

    private new void ChangeVisualState(bool useTransitions)
    {
      VisualStateManager.GoToState((FrameworkElement) this, this.IsEnabled ? "Normal" : "Disabled", useTransitions);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      if (this._toggleButton != null)
      {
        this._toggleButton.Checked -= new RoutedEventHandler(this.CheckedHandler);
        this._toggleButton.Unchecked -= new RoutedEventHandler(this.UncheckedHandler);
        this._toggleButton.Indeterminate -= new RoutedEventHandler(this.IndeterminateHandler);
        this._toggleButton.Click -= new RoutedEventHandler(this.ClickHandler);
        BindingOperations.ClearBinding((DependencyObject) this._toggleButton, ToggleButton.IsCheckedProperty);
        this._toggleButton.IsEnabledChanged -= new DependencyPropertyChangedEventHandler(this.IsEnabledHandler);
        this._toggleButton.PreviewMouseUp -= new MouseButtonEventHandler(this.ToggleButtonPreviewMouseUp);
      }
      this._toggleButton = this.GetTemplateChild("Switch") as ToggleButton;
      if (this._toggleButton != null)
      {
        this._toggleButton.Checked += new RoutedEventHandler(this.CheckedHandler);
        this._toggleButton.Unchecked += new RoutedEventHandler(this.UncheckedHandler);
        this._toggleButton.Indeterminate += new RoutedEventHandler(this.IndeterminateHandler);
        this._toggleButton.Click += new RoutedEventHandler(this.ClickHandler);
        Binding binding = new Binding("IsChecked")
        {
          Source = (object) this
        };
        this._toggleButton.SetBinding(ToggleButton.IsCheckedProperty, (BindingBase) binding);
        this._toggleButton.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.IsEnabledHandler);
        this._toggleButton.PreviewMouseUp += new MouseButtonEventHandler(this.ToggleButtonPreviewMouseUp);
      }
      this.ChangeVisualState(false);
    }

    private void ToggleButtonPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      Keyboard.Focus((IInputElement) this);
    }

    private void IsEnabledHandler(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.ChangeVisualState(false);
    }

    private void CheckedHandler(object sender, RoutedEventArgs e)
    {
      ICommand checkedCommand = this.CheckedCommand;
      object parameter = this.CheckedCommandParameter ?? (object) this;
      if (checkedCommand != null && checkedCommand.CanExecute(parameter))
        checkedCommand.Execute(parameter);
      SafeRaise.Raise<RoutedEventArgs>(this.Checked, (object) this, e);
    }

    private void UncheckedHandler(object sender, RoutedEventArgs e)
    {
      ICommand unCheckedCommand = this.UnCheckedCommand;
      object parameter = this.UnCheckedCommandParameter ?? (object) this;
      if (unCheckedCommand != null && unCheckedCommand.CanExecute(parameter))
        unCheckedCommand.Execute(parameter);
      SafeRaise.Raise<RoutedEventArgs>(this.Unchecked, (object) this, e);
    }

    private void IndeterminateHandler(object sender, RoutedEventArgs e)
    {
      SafeRaise.Raise<RoutedEventArgs>(this.Indeterminate, (object) this, e);
    }

    private void ClickHandler(object sender, RoutedEventArgs e)
    {
      SafeRaise.Raise<RoutedEventArgs>(this.Click, (object) this, e);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{{ToggleSwitch IsChecked={0}, Content={1}}}", new object[2]
      {
        (object) this.IsChecked,
        this.Content
      });
    }
  }
}
