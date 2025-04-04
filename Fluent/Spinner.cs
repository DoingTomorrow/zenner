// Decompiled with JetBrains decompiler
// Type: Fluent.Spinner
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  [TemplatePart(Name = "PART_TextBox", Type = typeof (System.Windows.Controls.TextBox))]
  [ContentProperty("Value")]
  [TemplatePart(Name = "PART_ButtonDown", Type = typeof (RepeatButton))]
  [TemplatePart(Name = "PART_ButtonUp", Type = typeof (RepeatButton))]
  public class Spinner : RibbonControl
  {
    private System.Windows.Controls.TextBox textBox;
    private RepeatButton buttonUp;
    private RepeatButton buttonDown;
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (Spinner), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, new PropertyChangedCallback(Spinner.OnValueChanged), new CoerceValueCallback(Spinner.CoerseValue)));
    public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(nameof (Increment), typeof (double), typeof (Spinner), (PropertyMetadata) new UIPropertyMetadata((object) 1.0));
    public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (Spinner), (PropertyMetadata) new UIPropertyMetadata((object) 0.0, new PropertyChangedCallback(Spinner.OnMinimumChanged), new CoerceValueCallback(Spinner.CoerseMinimum)));
    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (Spinner), (PropertyMetadata) new UIPropertyMetadata((object) 10.0, new PropertyChangedCallback(Spinner.OnMaximumChanged), new CoerceValueCallback(Spinner.CoerseMaximum)));
    public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof (Format), typeof (string), typeof (Spinner), (PropertyMetadata) new UIPropertyMetadata((object) "F1", new PropertyChangedCallback(Spinner.OnFormatChanged)));
    public static readonly DependencyProperty DelayProperty = DependencyProperty.Register(nameof (Delay), typeof (int), typeof (Spinner), (PropertyMetadata) new UIPropertyMetadata((object) 400));
    public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (int), typeof (Spinner), (PropertyMetadata) new UIPropertyMetadata((object) 80));
    public static readonly DependencyProperty InputWidthProperty = DependencyProperty.Register(nameof (InputWidth), typeof (double), typeof (Spinner), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));

    public event RoutedPropertyChangedEventHandler<double> ValueChanged;

    [SuppressMessage("Microsoft.Naming", "CA1721")]
    public double Value
    {
      get => (double) this.GetValue(Spinner.ValueProperty);
      set => this.SetValue(Spinner.ValueProperty, (object) value);
    }

    private static object CoerseValue(DependencyObject d, object basevalue)
    {
      Spinner spinner = (Spinner) d;
      double val2_1 = (double) basevalue;
      double val2_2 = Math.Max(spinner.Minimum, val2_1);
      return (object) Math.Min(spinner.Maximum, val2_2);
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Spinner sender = (Spinner) d;
      sender.ValueToTextBoxText();
      if (sender.ValueChanged == null)
        return;
      sender.ValueChanged((object) sender, new RoutedPropertyChangedEventArgs<double>((double) e.OldValue, (double) e.NewValue));
    }

    private void ValueToTextBoxText()
    {
      if (!this.IsTemplateValid())
        return;
      this.textBox.Text = this.Value.ToString(this.Format, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public double Increment
    {
      get => (double) this.GetValue(Spinner.IncrementProperty);
      set => this.SetValue(Spinner.IncrementProperty, (object) value);
    }

    public double Minimum
    {
      get => (double) this.GetValue(Spinner.MinimumProperty);
      set => this.SetValue(Spinner.MinimumProperty, (object) value);
    }

    private static object CoerseMinimum(DependencyObject d, object basevalue)
    {
      Spinner spinner = (Spinner) d;
      double num = (double) basevalue;
      return spinner.Maximum < num ? (object) spinner.Maximum : (object) num;
    }

    private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Spinner spinner = (Spinner) d;
      double num = (double) Spinner.CoerseValue(d, (object) spinner.Value);
      if (num == spinner.Value)
        return;
      spinner.Value = num;
    }

    public double Maximum
    {
      get => (double) this.GetValue(Spinner.MaximumProperty);
      set => this.SetValue(Spinner.MaximumProperty, (object) value);
    }

    private static object CoerseMaximum(DependencyObject d, object basevalue)
    {
      Spinner spinner = (Spinner) d;
      double num = (double) basevalue;
      return spinner.Minimum > num ? (object) spinner.Minimum : (object) num;
    }

    private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Spinner spinner = (Spinner) d;
      double num = (double) Spinner.CoerseValue(d, (object) spinner.Value);
      if (num == spinner.Value)
        return;
      spinner.Value = num;
    }

    public string Format
    {
      get => (string) this.GetValue(Spinner.FormatProperty);
      set => this.SetValue(Spinner.FormatProperty, (object) value);
    }

    private static void OnFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((Spinner) d).ValueToTextBoxText();
    }

    public int Delay
    {
      get => (int) this.GetValue(Spinner.DelayProperty);
      set => this.SetValue(Spinner.DelayProperty, (object) value);
    }

    public int Interval
    {
      get => (int) this.GetValue(Spinner.IntervalProperty);
      set => this.SetValue(Spinner.IntervalProperty, (object) value);
    }

    public double InputWidth
    {
      get => (double) this.GetValue(Spinner.InputWidthProperty);
      set => this.SetValue(Spinner.InputWidthProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static Spinner()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Spinner), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Spinner)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (Spinner), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(Spinner.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (Spinner));
      return basevalue;
    }

    public override void OnApplyTemplate()
    {
      if (this.IsTemplateValid())
      {
        this.buttonUp.Click -= new RoutedEventHandler(this.OnButtonUpClick);
        this.buttonDown.Click -= new RoutedEventHandler(this.OnButtonDownClick);
        BindingOperations.ClearAllBindings((DependencyObject) this.buttonDown);
        BindingOperations.ClearAllBindings((DependencyObject) this.buttonUp);
      }
      this.textBox = this.GetTemplateChild("PART_TextBox") as System.Windows.Controls.TextBox;
      this.buttonUp = this.GetTemplateChild("PART_ButtonUp") as RepeatButton;
      this.buttonDown = this.GetTemplateChild("PART_ButtonDown") as RepeatButton;
      if (!this.IsTemplateValid())
        return;
      RibbonControl.Bind((object) this, (FrameworkElement) this.buttonUp, "Delay", RepeatButton.DelayProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.buttonDown, "Delay", RepeatButton.DelayProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.buttonUp, "Interval", RepeatButton.IntervalProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.buttonDown, "Interval", RepeatButton.IntervalProperty, BindingMode.OneWay);
      this.buttonUp.Click += new RoutedEventHandler(this.OnButtonUpClick);
      this.buttonDown.Click += new RoutedEventHandler(this.OnButtonDownClick);
      this.textBox.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnTextBoxLostKeyboardFocus);
      this.textBox.PreviewKeyDown += new KeyEventHandler(this.OnTextBoxPreviewKeyDown);
      this.ValueToTextBoxText();
    }

    private bool IsTemplateValid()
    {
      return this.textBox != null && this.buttonUp != null && this.buttonDown != null;
    }

    public override void OnKeyTipPressed()
    {
      if (!this.IsTemplateValid())
        return;
      this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Delegate) (() =>
      {
        this.textBox.SelectAll();
        this.textBox.Focus();
      }));
      base.OnKeyTipPressed();
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
      if (e.Key == Key.Return || e.Key == Key.Space)
        return;
      base.OnKeyUp(e);
    }

    private void OnButtonUpClick(object sender, RoutedEventArgs e) => this.Value += this.Increment;

    private void OnButtonDownClick(object sender, RoutedEventArgs e)
    {
      this.Value -= this.Increment;
    }

    private void OnTextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      this.TextBoxTextToValue();
    }

    private void OnTextBoxPreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
        this.TextBoxTextToValue();
      if (e.Key == Key.Escape)
        this.ValueToTextBoxText();
      if (e.Key == Key.Return || e.Key == Key.Escape)
      {
        this.textBox.Focusable = false;
        this.Focus();
        this.textBox.Focusable = true;
        e.Handled = true;
      }
      if (e.Key == Key.Up)
        this.buttonUp.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
      if (e.Key != Key.Down)
        return;
      this.buttonDown.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
    }

    private void TextBoxTextToValue()
    {
      string text = this.textBox.Text;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < text.Length; ++index)
      {
        char c = text[index];
        if (char.IsDigit(c) || c == ',' || c == '+' || c == '-' || c == '.')
          stringBuilder.Append(c);
      }
      double result;
      if (double.TryParse(stringBuilder.ToString(), NumberStyles.Any, (IFormatProvider) CultureInfo.CurrentCulture, out result))
        this.Value = result;
      this.ValueToTextBoxText();
    }

    public override FrameworkElement CreateQuickAccessItem()
    {
      Spinner element = new Spinner();
      this.BindQuickAccessItem((FrameworkElement) element);
      return (FrameworkElement) element;
    }

    protected void BindQuickAccessItem(FrameworkElement element)
    {
      Spinner target = (Spinner) element;
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, element);
      target.Width = this.Width;
      target.InputWidth = this.InputWidth;
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Value", Spinner.ValueProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Increment", Spinner.IncrementProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Minimum", Spinner.MinimumProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Maximum", Spinner.MaximumProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Format", Spinner.FormatProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Delay", Spinner.DelayProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Interval", Spinner.IntervalProperty, BindingMode.OneWay);
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, element);
    }
  }
}
