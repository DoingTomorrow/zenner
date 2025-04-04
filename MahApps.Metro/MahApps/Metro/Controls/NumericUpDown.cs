// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.NumericUpDown
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplatePart(Name = "PART_NumericUp", Type = typeof (RepeatButton))]
  [TemplatePart(Name = "PART_NumericDown", Type = typeof (RepeatButton))]
  [TemplatePart(Name = "PART_TextBox", Type = typeof (TextBox))]
  public class NumericUpDown : Control
  {
    public static readonly RoutedEvent ValueIncrementedEvent = EventManager.RegisterRoutedEvent("ValueIncremented", RoutingStrategy.Bubble, typeof (NumericUpDownChangedRoutedEventHandler), typeof (NumericUpDown));
    public static readonly RoutedEvent ValueDecrementedEvent = EventManager.RegisterRoutedEvent("ValueDecremented", RoutingStrategy.Bubble, typeof (NumericUpDownChangedRoutedEventHandler), typeof (NumericUpDown));
    public static readonly RoutedEvent DelayChangedEvent = EventManager.RegisterRoutedEvent("DelayChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (NumericUpDown));
    public static readonly RoutedEvent MaximumReachedEvent = EventManager.RegisterRoutedEvent("MaximumReached", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (NumericUpDown));
    public static readonly RoutedEvent MinimumReachedEvent = EventManager.RegisterRoutedEvent("MinimumReached", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (NumericUpDown));
    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof (RoutedPropertyChangedEventHandler<double?>), typeof (NumericUpDown));
    public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof (Delay), typeof (int), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) 500, new PropertyChangedCallback(NumericUpDown.OnDelayChanged)), new ValidateValueCallback(NumericUpDown.ValidateDelay));
    public static readonly DependencyProperty TextAlignmentProperty = TextBox.TextAlignmentProperty.AddOwner(typeof (NumericUpDown));
    public static readonly DependencyProperty SpeedupProperty = DependencyProperty.Register(nameof (Speedup), typeof (bool), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(NumericUpDown.OnSpeedupChanged)));
    public static readonly DependencyProperty IsReadOnlyProperty = TextBoxBase.IsReadOnlyProperty.AddOwner(typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(NumericUpDown.IsReadOnlyPropertyChangedCallback)));
    public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(nameof (StringFormat), typeof (string), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) string.Empty, new PropertyChangedCallback(NumericUpDown.OnStringFormatChanged), new CoerceValueCallback(NumericUpDown.CoerceStringFormat)));
    public static readonly DependencyProperty InterceptArrowKeysProperty = DependencyProperty.Register(nameof (InterceptArrowKeys), typeof (bool), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double?), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(NumericUpDown.OnValueChanged), new CoerceValueCallback(NumericUpDown.CoerceValue)));
    public static readonly DependencyProperty ButtonsAlignmentProperty = DependencyProperty.Register(nameof (ButtonsAlignment), typeof (ButtonsAlignment), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) ButtonsAlignment.Right, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));
    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.MinValue, new PropertyChangedCallback(NumericUpDown.OnMinimumChanged)));
    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) double.MaxValue, new PropertyChangedCallback(NumericUpDown.OnMaximumChanged), new CoerceValueCallback(NumericUpDown.CoerceMaximum)));
    public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1.0, new PropertyChangedCallback(NumericUpDown.IntervalChanged)));
    public static readonly DependencyProperty InterceptMouseWheelProperty = DependencyProperty.Register(nameof (InterceptMouseWheel), typeof (bool), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
    public static readonly DependencyProperty TrackMouseWheelWhenMouseOverProperty = DependencyProperty.Register(nameof (TrackMouseWheelWhenMouseOver), typeof (bool), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    public static readonly DependencyProperty HideUpDownButtonsProperty = DependencyProperty.Register(nameof (HideUpDownButtons), typeof (bool), typeof (NumericUpDown), new PropertyMetadata((object) false));
    public static readonly DependencyProperty UpDownButtonsWidthProperty = DependencyProperty.Register(nameof (UpDownButtonsWidth), typeof (double), typeof (NumericUpDown), new PropertyMetadata((object) 20.0));
    public static readonly DependencyProperty InterceptManualEnterProperty = DependencyProperty.Register(nameof (InterceptManualEnter), typeof (bool), typeof (NumericUpDown), new PropertyMetadata((object) true, new PropertyChangedCallback(NumericUpDown.InterceptManualEnterChangedCallback)));
    public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(nameof (Culture), typeof (CultureInfo), typeof (NumericUpDown), new PropertyMetadata((object) null, (PropertyChangedCallback) ((o, e) =>
    {
      if (e.NewValue == e.OldValue)
        return;
      NumericUpDown numericUpDown = (NumericUpDown) o;
      numericUpDown.OnValueChanged(numericUpDown.Value, numericUpDown.Value);
    })));
    public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.Register(nameof (SelectAllOnFocus), typeof (bool), typeof (NumericUpDown), new PropertyMetadata((object) true));
    public static readonly DependencyProperty HasDecimalsProperty = DependencyProperty.Register(nameof (HasDecimals), typeof (bool), typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, new PropertyChangedCallback(NumericUpDown.OnHasDecimalsChanged)));
    private static readonly Regex RegexStringFormatHexadecimal = new Regex("^(?<complexHEX>.*{\\d:X\\d+}.*)?(?<simpleHEX>X\\d+)?$", RegexOptions.Compiled);
    private const double DefaultInterval = 1.0;
    private const int DefaultDelay = 500;
    private const string ElementNumericDown = "PART_NumericDown";
    private const string ElementNumericUp = "PART_NumericUp";
    private const string ElementTextBox = "PART_TextBox";
    private const string ScientificNotationChar = "E";
    private const StringComparison StrComp = StringComparison.InvariantCultureIgnoreCase;
    private Tuple<string, string> _removeFromText = new Tuple<string, string>(string.Empty, string.Empty);
    private Lazy<PropertyInfo> _handlesMouseWheelScrolling = new Lazy<PropertyInfo>();
    private double _internalIntervalMultiplierForCalculation = 1.0;
    private double _internalLargeChange = 100.0;
    private double _intervalValueSinceReset;
    private bool _manualChange;
    private RepeatButton _repeatDown;
    private RepeatButton _repeatUp;
    private TextBox _valueTextBox;
    private ScrollViewer _scrollViewer;

    private static void IsReadOnlyPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.OldValue == e.NewValue || e.NewValue == null)
        return;
      NumericUpDown numericUpDown = (NumericUpDown) dependencyObject;
      numericUpDown.ToggleReadOnlyMode((bool) e.NewValue | !numericUpDown.InterceptManualEnter);
    }

    private static void InterceptManualEnterChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.OldValue == e.NewValue || e.NewValue == null)
        return;
      NumericUpDown numericUpDown = (NumericUpDown) dependencyObject;
      numericUpDown.ToggleReadOnlyMode(!(bool) e.NewValue | numericUpDown.IsReadOnly);
    }

    static NumericUpDown()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (NumericUpDown)));
      Control.VerticalContentAlignmentProperty.OverrideMetadata(typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) VerticalAlignment.Center));
      Control.HorizontalContentAlignmentProperty.OverrideMetadata(typeof (NumericUpDown), (PropertyMetadata) new FrameworkPropertyMetadata((object) HorizontalAlignment.Right));
      EventManager.RegisterClassHandler(typeof (NumericUpDown), UIElement.GotFocusEvent, (Delegate) new RoutedEventHandler(NumericUpDown.OnGotFocus));
    }

    public event RoutedPropertyChangedEventHandler<double?> ValueChanged
    {
      add => this.AddHandler(NumericUpDown.ValueChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(NumericUpDown.ValueChangedEvent, (Delegate) value);
    }

    public event RoutedEventHandler MaximumReached
    {
      add => this.AddHandler(NumericUpDown.MaximumReachedEvent, (Delegate) value);
      remove => this.RemoveHandler(NumericUpDown.MaximumReachedEvent, (Delegate) value);
    }

    public event RoutedEventHandler MinimumReached
    {
      add => this.AddHandler(NumericUpDown.MinimumReachedEvent, (Delegate) value);
      remove => this.RemoveHandler(NumericUpDown.MinimumReachedEvent, (Delegate) value);
    }

    public event NumericUpDownChangedRoutedEventHandler ValueIncremented
    {
      add => this.AddHandler(NumericUpDown.ValueIncrementedEvent, (Delegate) value);
      remove => this.RemoveHandler(NumericUpDown.ValueIncrementedEvent, (Delegate) value);
    }

    public event NumericUpDownChangedRoutedEventHandler ValueDecremented
    {
      add => this.AddHandler(NumericUpDown.ValueDecrementedEvent, (Delegate) value);
      remove => this.RemoveHandler(NumericUpDown.ValueDecrementedEvent, (Delegate) value);
    }

    public event RoutedEventHandler DelayChanged
    {
      add => this.AddHandler(NumericUpDown.DelayChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(NumericUpDown.DelayChangedEvent, (Delegate) value);
    }

    [Bindable(true)]
    [DefaultValue(500)]
    [Category("Behavior")]
    public int Delay
    {
      get => (int) this.GetValue(NumericUpDown.DelayProperty);
      set => this.SetValue(NumericUpDown.DelayProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool InterceptArrowKeys
    {
      get => (bool) this.GetValue(NumericUpDown.InterceptArrowKeysProperty);
      set => this.SetValue(NumericUpDown.InterceptArrowKeysProperty, (object) value);
    }

    [Category("Behavior")]
    [DefaultValue(true)]
    public bool InterceptMouseWheel
    {
      get => (bool) this.GetValue(NumericUpDown.InterceptMouseWheelProperty);
      set => this.SetValue(NumericUpDown.InterceptMouseWheelProperty, (object) value);
    }

    [Category("Behavior")]
    [DefaultValue(false)]
    public bool TrackMouseWheelWhenMouseOver
    {
      get => (bool) this.GetValue(NumericUpDown.TrackMouseWheelWhenMouseOverProperty);
      set => this.SetValue(NumericUpDown.TrackMouseWheelWhenMouseOverProperty, (object) value);
    }

    [Category("Behavior")]
    [DefaultValue(true)]
    public bool InterceptManualEnter
    {
      get => (bool) this.GetValue(NumericUpDown.InterceptManualEnterProperty);
      set => this.SetValue(NumericUpDown.InterceptManualEnterProperty, (object) value);
    }

    [Category("Behavior")]
    [DefaultValue(null)]
    public CultureInfo Culture
    {
      get => (CultureInfo) this.GetValue(NumericUpDown.CultureProperty);
      set => this.SetValue(NumericUpDown.CultureProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue(false)]
    public bool HideUpDownButtons
    {
      get => (bool) this.GetValue(NumericUpDown.HideUpDownButtonsProperty);
      set => this.SetValue(NumericUpDown.HideUpDownButtonsProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue(20.0)]
    public double UpDownButtonsWidth
    {
      get => (double) this.GetValue(NumericUpDown.UpDownButtonsWidthProperty);
      set => this.SetValue(NumericUpDown.UpDownButtonsWidthProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue(ButtonsAlignment.Right)]
    public ButtonsAlignment ButtonsAlignment
    {
      get => (ButtonsAlignment) this.GetValue(NumericUpDown.ButtonsAlignmentProperty);
      set => this.SetValue(NumericUpDown.ButtonsAlignmentProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Behavior")]
    [DefaultValue(1.0)]
    public double Interval
    {
      get => (double) this.GetValue(NumericUpDown.IntervalProperty);
      set => this.SetValue(NumericUpDown.IntervalProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Behavior")]
    [DefaultValue(true)]
    public bool SelectAllOnFocus
    {
      get => (bool) this.GetValue(NumericUpDown.SelectAllOnFocusProperty);
      set => this.SetValue(NumericUpDown.SelectAllOnFocusProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue(false)]
    public bool IsReadOnly
    {
      get => (bool) this.GetValue(NumericUpDown.IsReadOnlyProperty);
      set => this.SetValue(NumericUpDown.IsReadOnlyProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    [DefaultValue(1.7976931348623157E+308)]
    public double Maximum
    {
      get => (double) this.GetValue(NumericUpDown.MaximumProperty);
      set => this.SetValue(NumericUpDown.MaximumProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    [DefaultValue(-1.7976931348623157E+308)]
    public double Minimum
    {
      get => (double) this.GetValue(NumericUpDown.MinimumProperty);
      set => this.SetValue(NumericUpDown.MinimumProperty, (object) value);
    }

    [Category("Common")]
    [DefaultValue(true)]
    public bool Speedup
    {
      get => (bool) this.GetValue(NumericUpDown.SpeedupProperty);
      set => this.SetValue(NumericUpDown.SpeedupProperty, (object) value);
    }

    [Category("Common")]
    public string StringFormat
    {
      get => (string) this.GetValue(NumericUpDown.StringFormatProperty);
      set => this.SetValue(NumericUpDown.StringFormatProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    [DefaultValue(TextAlignment.Right)]
    public TextAlignment TextAlignment
    {
      get => (TextAlignment) this.GetValue(NumericUpDown.TextAlignmentProperty);
      set => this.SetValue(NumericUpDown.TextAlignmentProperty, (object) value);
    }

    [Bindable(true)]
    [Category("Common")]
    [DefaultValue(null)]
    public double? Value
    {
      get => (double?) this.GetValue(NumericUpDown.ValueProperty);
      set => this.SetValue(NumericUpDown.ValueProperty, (object) value);
    }

    private CultureInfo SpecificCultureInfo => this.Culture ?? this.Language.GetSpecificCulture();

    [Bindable(true)]
    [Category("Common")]
    [DefaultValue(true)]
    public bool HasDecimals
    {
      get => (bool) this.GetValue(NumericUpDown.HasDecimalsProperty);
      set => this.SetValue(NumericUpDown.HasDecimalsProperty, (object) value);
    }

    private static void OnGotFocus(object sender, RoutedEventArgs e)
    {
      NumericUpDown numericUpDown = (NumericUpDown) sender;
      if (e.Handled || !numericUpDown.InterceptManualEnter && !numericUpDown.IsReadOnly || numericUpDown._valueTextBox == null)
        return;
      if (e.OriginalSource == numericUpDown)
      {
        numericUpDown._valueTextBox.Focus();
        e.Handled = true;
      }
      else
      {
        if (e.OriginalSource != numericUpDown._valueTextBox || !numericUpDown.SelectAllOnFocus)
          return;
        numericUpDown._valueTextBox.SelectAll();
      }
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this._repeatUp = this.GetTemplateChild("PART_NumericUp") as RepeatButton;
      this._repeatDown = this.GetTemplateChild("PART_NumericDown") as RepeatButton;
      this._valueTextBox = this.GetTemplateChild("PART_TextBox") as TextBox;
      if (this._repeatUp == null || this._repeatDown == null || this._valueTextBox == null)
        throw new InvalidOperationException(string.Format("You have missed to specify {0}, {1} or {2} in your template", (object) "PART_NumericUp", (object) "PART_NumericDown", (object) "PART_TextBox"));
      this.ToggleReadOnlyMode(this.IsReadOnly | !this.InterceptManualEnter);
      this._repeatUp.Click += (RoutedEventHandler) ((o, e) => this.ChangeValueWithSpeedUp(true));
      this._repeatDown.Click += (RoutedEventHandler) ((o, e) => this.ChangeValueWithSpeedUp(false));
      this._repeatUp.PreviewMouseUp += (MouseButtonEventHandler) ((o, e) => this.ResetInternal());
      this._repeatDown.PreviewMouseUp += (MouseButtonEventHandler) ((o, e) => this.ResetInternal());
      this.OnValueChanged(this.Value, this.Value);
      this._scrollViewer = this.TryFindScrollViewer();
    }

    private void ToggleReadOnlyMode(bool isReadOnly)
    {
      if (this._repeatUp == null || this._repeatDown == null || this._valueTextBox == null)
        return;
      if (isReadOnly)
      {
        this._valueTextBox.LostFocus -= new RoutedEventHandler(this.OnTextBoxLostFocus);
        this._valueTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.OnPreviewTextInput);
        this._valueTextBox.PreviewKeyDown -= new KeyEventHandler(this.OnTextBoxKeyDown);
        this._valueTextBox.TextChanged -= new TextChangedEventHandler(this.OnTextChanged);
        DataObject.RemovePastingHandler((DependencyObject) this._valueTextBox, new DataObjectPastingEventHandler(this.OnValueTextBoxPaste));
      }
      else
      {
        this._valueTextBox.LostFocus += new RoutedEventHandler(this.OnTextBoxLostFocus);
        this._valueTextBox.PreviewTextInput += new TextCompositionEventHandler(this.OnPreviewTextInput);
        this._valueTextBox.PreviewKeyDown += new KeyEventHandler(this.OnTextBoxKeyDown);
        this._valueTextBox.TextChanged += new TextChangedEventHandler(this.OnTextChanged);
        DataObject.AddPastingHandler((DependencyObject) this._valueTextBox, new DataObjectPastingEventHandler(this.OnValueTextBoxPaste));
      }
    }

    public void SelectAll()
    {
      if (this._valueTextBox == null)
        return;
      this._valueTextBox.SelectAll();
    }

    protected virtual void OnDelayChanged(int oldDelay, int newDelay)
    {
      if (oldDelay == newDelay)
        return;
      if (this._repeatDown != null)
        this._repeatDown.Delay = newDelay;
      if (this._repeatUp == null)
        return;
      this._repeatUp.Delay = newDelay;
    }

    protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
    }

    protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
    {
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
      base.OnPreviewKeyDown(e);
      if (!this.InterceptArrowKeys)
        return;
      switch (e.Key)
      {
        case Key.Up:
          this.ChangeValueWithSpeedUp(true);
          e.Handled = true;
          break;
        case Key.Down:
          this.ChangeValueWithSpeedUp(false);
          e.Handled = true;
          break;
      }
      if (!e.Handled)
        return;
      this._manualChange = false;
      this.InternalSetText(this.Value);
    }

    protected override void OnPreviewKeyUp(KeyEventArgs e)
    {
      base.OnPreviewKeyUp(e);
      if (e.Key != Key.Down && e.Key != Key.Up)
        return;
      this.ResetInternal();
    }

    protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
    {
      base.OnPreviewMouseWheel(e);
      if (this.InterceptMouseWheel && (this.IsFocused || this._valueTextBox.IsFocused || this.TrackMouseWheelWhenMouseOver))
      {
        bool addInterval = e.Delta > 0;
        this._manualChange = false;
        this.ChangeValueInternal(addInterval);
      }
      if (this._scrollViewer == null || !(this._handlesMouseWheelScrolling.Value != (PropertyInfo) null))
        return;
      if (this.TrackMouseWheelWhenMouseOver)
        this._handlesMouseWheelScrolling.Value.SetValue((object) this._scrollViewer, (object) true, (object[]) null);
      else if (this.InterceptMouseWheel)
        this._handlesMouseWheelScrolling.Value.SetValue((object) this._scrollViewer, (object) this._valueTextBox.IsFocused, (object[]) null);
      else
        this._handlesMouseWheelScrolling.Value.SetValue((object) this._scrollViewer, (object) true, (object[]) null);
    }

    protected void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
      e.Handled = true;
      if (string.IsNullOrWhiteSpace(e.Text) || e.Text.Length != 1)
        return;
      string text = e.Text;
      if (char.IsDigit(text[0]))
      {
        e.Handled = false;
      }
      else
      {
        CultureInfo equivalentCulture = this.SpecificCultureInfo;
        NumberFormatInfo numberFormatInfo = equivalentCulture.NumberFormat;
        TextBox textBox = (TextBox) sender;
        bool flag = textBox.SelectedText == textBox.Text;
        if (numberFormatInfo.NumberDecimalSeparator == text)
        {
          if (!(textBox.Text.All<char>((Func<char, bool>) (i => i.ToString((IFormatProvider) equivalentCulture) != numberFormatInfo.NumberDecimalSeparator)) | flag) || !this.HasDecimals)
            return;
          e.Handled = false;
        }
        else if (numberFormatInfo.NegativeSign == text || text == numberFormatInfo.PositiveSign)
        {
          if (textBox.SelectionStart == 0)
          {
            if (textBox.Text.Length > 1)
            {
              if (!flag && (textBox.Text.StartsWith(numberFormatInfo.NegativeSign, StringComparison.InvariantCultureIgnoreCase) || textBox.Text.StartsWith(numberFormatInfo.PositiveSign, StringComparison.InvariantCultureIgnoreCase)))
                return;
              e.Handled = false;
            }
            else
              e.Handled = false;
          }
          else
          {
            if (textBox.SelectionStart <= 0 || !textBox.Text.ElementAt<char>(textBox.SelectionStart - 1).ToString((IFormatProvider) equivalentCulture).Equals("E", StringComparison.InvariantCultureIgnoreCase))
              return;
            e.Handled = false;
          }
        }
        else
        {
          if (!text.Equals("E", StringComparison.InvariantCultureIgnoreCase) || textBox.SelectionStart <= 0 || textBox.Text.Any<char>((Func<char, bool>) (i => i.ToString((IFormatProvider) equivalentCulture).Equals("E", StringComparison.InvariantCultureIgnoreCase))))
            return;
          e.Handled = false;
        }
      }
    }

    protected virtual void OnSpeedupChanged(bool oldSpeedup, bool newSpeedup)
    {
    }

    protected virtual void OnValueChanged(double? oldValue, double? newValue)
    {
      double? nullable1;
      if (!this._manualChange)
      {
        if (!newValue.HasValue)
        {
          if (this._valueTextBox != null)
            this._valueTextBox.Text = (string) null;
          double? nullable2 = oldValue;
          double? nullable3 = newValue;
          if ((nullable2.GetValueOrDefault() == nullable3.GetValueOrDefault() ? (nullable2.HasValue != nullable3.HasValue ? 1 : 0) : 1) == 0)
            return;
          this.RaiseEvent((RoutedEventArgs) new RoutedPropertyChangedEventArgs<double?>(oldValue, newValue, NumericUpDown.ValueChangedEvent));
          return;
        }
        if (this._repeatUp != null && !this._repeatUp.IsEnabled)
          this._repeatUp.IsEnabled = true;
        if (this._repeatDown != null && !this._repeatDown.IsEnabled)
          this._repeatDown.IsEnabled = true;
        nullable1 = newValue;
        double minimum = this.Minimum;
        if ((nullable1.GetValueOrDefault() <= minimum ? (nullable1.HasValue ? 1 : 0) : 0) != 0)
        {
          if (this._repeatDown != null)
            this._repeatDown.IsEnabled = false;
          this.ResetInternal();
          if (this.IsLoaded)
            this.RaiseEvent(new RoutedEventArgs(NumericUpDown.MinimumReachedEvent));
        }
        nullable1 = newValue;
        double maximum = this.Maximum;
        if ((nullable1.GetValueOrDefault() >= maximum ? (nullable1.HasValue ? 1 : 0) : 0) != 0)
        {
          if (this._repeatUp != null)
            this._repeatUp.IsEnabled = false;
          this.ResetInternal();
          if (this.IsLoaded)
            this.RaiseEvent(new RoutedEventArgs(NumericUpDown.MaximumReachedEvent));
        }
        if (this._valueTextBox != null)
          this.InternalSetText(newValue);
      }
      nullable1 = oldValue;
      double? nullable4 = newValue;
      if ((nullable1.GetValueOrDefault() == nullable4.GetValueOrDefault() ? (nullable1.HasValue != nullable4.HasValue ? 1 : 0) : 1) == 0)
        return;
      this.RaiseEvent((RoutedEventArgs) new RoutedPropertyChangedEventArgs<double?>(oldValue, newValue, NumericUpDown.ValueChangedEvent));
    }

    private static object CoerceMaximum(DependencyObject d, object value)
    {
      double minimum = ((NumericUpDown) d).Minimum;
      double num = (double) value;
      return (object) (num < minimum ? minimum : num);
    }

    private static object CoerceStringFormat(DependencyObject d, object basevalue)
    {
      return basevalue ?? (object) string.Empty;
    }

    private static object CoerceValue(DependencyObject d, object value)
    {
      if (value == null)
        return (object) null;
      NumericUpDown numericUpDown = (NumericUpDown) d;
      double d1 = ((double?) value).Value;
      if (!numericUpDown.HasDecimals)
        d1 = Math.Truncate(d1);
      if (d1 < numericUpDown.Minimum)
        return (object) numericUpDown.Minimum;
      return d1 > numericUpDown.Maximum ? (object) numericUpDown.Maximum : (object) d1;
    }

    private static void IntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((NumericUpDown) d).ResetInternal();
    }

    private static void OnDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      NumericUpDown numericUpDown = (NumericUpDown) d;
      numericUpDown.RaiseChangeDelay();
      numericUpDown.OnDelayChanged((int) e.OldValue, (int) e.NewValue);
    }

    private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      NumericUpDown d1 = (NumericUpDown) d;
      d1.CoerceValue(NumericUpDown.ValueProperty);
      d1.Value = (double?) NumericUpDown.CoerceValue((DependencyObject) d1, (object) d1.Value);
      d1.OnMaximumChanged((double) e.OldValue, (double) e.NewValue);
      d1.EnableDisableUpDown();
    }

    private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      NumericUpDown d1 = (NumericUpDown) d;
      d1.CoerceValue(NumericUpDown.ValueProperty);
      d1.CoerceValue(NumericUpDown.MaximumProperty);
      d1.Value = (double?) NumericUpDown.CoerceValue((DependencyObject) d1, (object) d1.Value);
      d1.OnMinimumChanged((double) e.OldValue, (double) e.NewValue);
      d1.EnableDisableUpDown();
    }

    private static void OnSpeedupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((NumericUpDown) d).OnSpeedupChanged((bool) e.OldValue, (bool) e.NewValue);
    }

    private static void OnStringFormatChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      NumericUpDown numericUpDown = (NumericUpDown) d;
      numericUpDown.SetRemoveStringFormatFromText((string) e.NewValue);
      if (numericUpDown._valueTextBox != null && numericUpDown.Value.HasValue)
        numericUpDown.InternalSetText(numericUpDown.Value);
      numericUpDown.HasDecimals = !NumericUpDown.RegexStringFormatHexadecimal.IsMatch((string) e.NewValue);
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((NumericUpDown) d).OnValueChanged((double?) e.OldValue, (double?) e.NewValue);
    }

    private static void OnHasDecimalsChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      NumericUpDown numericUpDown = (NumericUpDown) d;
      double? nullable = numericUpDown.Value;
      if ((bool) e.NewValue || !numericUpDown.Value.HasValue)
        return;
      numericUpDown.Value = new double?(Math.Truncate(numericUpDown.Value.GetValueOrDefault()));
    }

    private static bool ValidateDelay(object value) => Convert.ToInt32(value) >= 0;

    private void InternalSetText(double? newValue)
    {
      if (!newValue.HasValue)
      {
        this._valueTextBox.Text = (string) null;
      }
      else
      {
        CultureInfo specificCultureInfo = this.SpecificCultureInfo;
        if (string.IsNullOrEmpty(this.StringFormat))
          this._valueTextBox.Text = newValue.Value.ToString((IFormatProvider) specificCultureInfo);
        else
          this.FormatValue(newValue, specificCultureInfo);
        if (!(bool) this.GetValue(TextBoxHelper.IsMonitoringProperty))
          return;
        this.SetValue(TextBoxHelper.TextLengthProperty, (object) this._valueTextBox.Text.Length);
      }
    }

    private void FormatValue(double? newValue, CultureInfo culture)
    {
      Match match = NumericUpDown.RegexStringFormatHexadecimal.Match(this.StringFormat);
      if (match.Success)
      {
        if (match.Groups["simpleHEX"].Success)
        {
          this._valueTextBox.Text = ((int) newValue.Value).ToString(match.Groups["simpleHEX"].Value, (IFormatProvider) culture);
        }
        else
        {
          if (!match.Groups["complexHEX"].Success)
            return;
          this._valueTextBox.Text = string.Format((IFormatProvider) culture, match.Groups["complexHEX"].Value, new object[1]
          {
            (object) (int) newValue.Value
          });
        }
      }
      else if (!this.StringFormat.Contains("{"))
        this._valueTextBox.Text = newValue.Value.ToString(this.StringFormat, (IFormatProvider) culture);
      else
        this._valueTextBox.Text = string.Format((IFormatProvider) culture, this.StringFormat, new object[1]
        {
          (object) newValue.Value
        });
    }

    private ScrollViewer TryFindScrollViewer()
    {
      this._valueTextBox.ApplyTemplate();
      if (this._valueTextBox.Template.FindName("PART_ContentHost", (FrameworkElement) this._valueTextBox) is ScrollViewer name)
        this._handlesMouseWheelScrolling = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => ((IEnumerable<PropertyInfo>) this._scrollViewer.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)).SingleOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (i => i.Name == "HandlesMouseWheelScrolling"))));
      return name;
    }

    private void ChangeValueWithSpeedUp(bool toPositive)
    {
      if (this.IsReadOnly)
        return;
      double num = toPositive ? 1.0 : -1.0;
      if (this.Speedup)
      {
        if ((this._intervalValueSinceReset += this.Interval * this._internalIntervalMultiplierForCalculation) > this.Interval * this._internalLargeChange)
        {
          this._internalLargeChange *= 10.0;
          this._internalIntervalMultiplierForCalculation *= 10.0;
        }
        this.ChangeValueInternal(num * this._internalIntervalMultiplierForCalculation);
      }
      else
        this.ChangeValueInternal(num * this.Interval);
    }

    private void ChangeValueInternal(bool addInterval)
    {
      this.ChangeValueInternal(addInterval ? this.Interval : -this.Interval);
    }

    private void ChangeValueInternal(double interval)
    {
      if (this.IsReadOnly)
        return;
      NumericUpDownChangedRoutedEventArgs e = interval > 0.0 ? new NumericUpDownChangedRoutedEventArgs(NumericUpDown.ValueIncrementedEvent, interval) : new NumericUpDownChangedRoutedEventArgs(NumericUpDown.ValueDecrementedEvent, interval);
      this.RaiseEvent((RoutedEventArgs) e);
      if (e.Handled)
        return;
      this.ChangeValueBy(e.Interval);
      this._valueTextBox.CaretIndex = this._valueTextBox.Text.Length;
    }

    private void ChangeValueBy(double difference)
    {
      this.Value = new double?((double) NumericUpDown.CoerceValue((DependencyObject) this, (object) (this.Value.GetValueOrDefault() + difference)));
    }

    private void EnableDisableDown()
    {
      if (this._repeatDown == null)
        return;
      RepeatButton repeatDown = this._repeatDown;
      double? nullable = this.Value;
      double minimum = this.Minimum;
      int num = nullable.GetValueOrDefault() > minimum ? (nullable.HasValue ? 1 : 0) : 0;
      repeatDown.IsEnabled = num != 0;
    }

    private void EnableDisableUp()
    {
      if (this._repeatUp == null)
        return;
      RepeatButton repeatUp = this._repeatUp;
      double? nullable = this.Value;
      double maximum = this.Maximum;
      int num = nullable.GetValueOrDefault() < maximum ? (nullable.HasValue ? 1 : 0) : 0;
      repeatUp.IsEnabled = num != 0;
    }

    private void EnableDisableUpDown()
    {
      this.EnableDisableUp();
      this.EnableDisableDown();
    }

    private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
    {
      this._manualChange = true;
      if (e.Key != Key.Decimal && e.Key != Key.OemPeriod)
        return;
      TextBox textBox = sender as TextBox;
      if (!textBox.Text.Contains(this.SpecificCultureInfo.NumberFormat.NumberDecimalSeparator))
      {
        int caretIndex = textBox.CaretIndex;
        textBox.Text = textBox.Text.Insert(caretIndex, this.SpecificCultureInfo.NumberFormat.CurrencyDecimalSeparator);
        textBox.CaretIndex = caretIndex + 1;
      }
      e.Handled = true;
    }

    private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
    {
      if (!this.InterceptManualEnter)
        return;
      TextBox textBox = (TextBox) sender;
      this._manualChange = false;
      double convertedValue;
      if (this.ValidateText(textBox.Text, out convertedValue))
      {
        double? nullable = this.Value;
        double num = convertedValue;
        if ((nullable.GetValueOrDefault() == num ? (nullable.HasValue ? 1 : 0) : 0) != 0)
          this.OnValueChanged(this.Value, this.Value);
        if (convertedValue > this.Maximum)
        {
          nullable = this.Value;
          double maximum = this.Maximum;
          if ((nullable.GetValueOrDefault() == maximum ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            this.OnValueChanged(this.Value, this.Value);
          else
            this.SetValue(NumericUpDown.ValueProperty, (object) this.Maximum);
        }
        else if (convertedValue < this.Minimum)
        {
          nullable = this.Value;
          double minimum = this.Minimum;
          if ((nullable.GetValueOrDefault() == minimum ? (nullable.HasValue ? 1 : 0) : 0) != 0)
            this.OnValueChanged(this.Value, this.Value);
          else
            this.SetValue(NumericUpDown.ValueProperty, (object) this.Minimum);
        }
        else
          this.SetValue(NumericUpDown.ValueProperty, (object) convertedValue);
      }
      else
        this.OnValueChanged(this.Value, this.Value);
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
      if (string.IsNullOrEmpty(((TextBox) sender).Text))
      {
        this.Value = new double?();
      }
      else
      {
        double convertedValue;
        if (!this._manualChange || !this.ValidateText(((TextBox) sender).Text, out convertedValue))
          return;
        this.Value = (double?) NumericUpDown.CoerceValue((DependencyObject) this, (object) convertedValue);
        e.Handled = true;
      }
    }

    private void OnValueTextBoxPaste(object sender, DataObjectPastingEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      string text = textBox.Text;
      if (!e.SourceDataObject.GetDataPresent(DataFormats.Text, true))
        return;
      string data = e.SourceDataObject.GetData(DataFormats.Text) as string;
      if (this.ValidateText(text.Substring(0, textBox.SelectionStart) + data + text.Substring(textBox.SelectionStart), out double _))
        return;
      e.CancelCommand();
    }

    private void RaiseChangeDelay()
    {
      this.RaiseEvent(new RoutedEventArgs(NumericUpDown.DelayChangedEvent));
    }

    private void ResetInternal()
    {
      if (this.IsReadOnly)
        return;
      this._internalLargeChange = 100.0 * this.Interval;
      this._internalIntervalMultiplierForCalculation = this.Interval;
      this._intervalValueSinceReset = 0.0;
    }

    private bool ValidateText(string text, out double convertedValue)
    {
      text = this.RemoveStringFormatFromText(text);
      return double.TryParse(text, NumberStyles.Any, (IFormatProvider) this.SpecificCultureInfo, out convertedValue);
    }

    private string RemoveStringFormatFromText(string text)
    {
      if (!string.IsNullOrEmpty(this._removeFromText.Item1))
        text = text.Replace(this._removeFromText.Item1, string.Empty);
      if (!string.IsNullOrEmpty(this._removeFromText.Item2))
        text = text.Replace(this._removeFromText.Item2, string.Empty);
      return text;
    }

    private void SetRemoveStringFormatFromText(string stringFormat)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      string source = stringFormat;
      int length = source.IndexOf("{", StringComparison.InvariantCultureIgnoreCase);
      if (length > -1)
      {
        if (length > 0)
          str1 = source.Substring(0, length);
        str2 = new string(source.SkipWhile<char>((Func<char, bool>) (i => i != '}')).Skip<char>(1).ToArray<char>()).Trim();
      }
      this._removeFromText = new Tuple<string, string>(str1, str2);
    }
  }
}
