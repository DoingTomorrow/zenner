// Decompiled with JetBrains decompiler
// Type: Fluent.TextBox
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  [ContentProperty("Text")]
  [TemplatePart(Name = "PART_TextBox")]
  public class TextBox : RibbonControl
  {
    private System.Windows.Controls.TextBox textBoxTemplated;
    private System.Windows.Controls.TextBox textBox = new System.Windows.Controls.TextBox();
    private string textBoxContentWhenGotFocus;
    public static readonly DependencyProperty InputWidthProperty = DependencyProperty.Register(nameof (InputWidth), typeof (double), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) double.NaN));
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (TextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (PropertyChangedCallback) null, (CoerceValueCallback) null, true, UpdateSourceTrigger.LostFocus));
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof (IsReadOnly), typeof (bool), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty CharacterCasingProperty = DependencyProperty.Register(nameof (CharacterCasing), typeof (CharacterCasing), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) CharacterCasing.Normal));
    public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(nameof (MaxLength), typeof (int), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) int.MaxValue));
    public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register(nameof (TextAlignment), typeof (TextAlignment), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) TextAlignment.Left));
    public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register(nameof (TextDecorations), typeof (TextDecorationCollection), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) new TextDecorationCollection()));
    public static readonly DependencyProperty IsUndoEnabledProperty = DependencyProperty.Register(nameof (IsUndoEnabled), typeof (bool), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty UndoLimitProperty = DependencyProperty.Register(nameof (UndoLimit), typeof (int), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) 1000));
    public static readonly DependencyProperty AutoWordSelectionProperty = DependencyProperty.Register(nameof (AutoWordSelection), typeof (bool), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) false));
    public static readonly DependencyProperty SelectionBrushProperty = DependencyProperty.Register(nameof (SelectionBrush), typeof (Brush), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 153, byte.MaxValue))));
    public static readonly DependencyProperty SelectionOpacityProperty = DependencyProperty.Register(nameof (SelectionOpacity), typeof (double), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((object) 0.4));
    public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register(nameof (CaretBrush), typeof (Brush), typeof (TextBox), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    public event TextChangedEventHandler TextChanged;

    private void RaiseTextChanged(TextChangedEventArgs args)
    {
      if (this.TextChanged == null)
        return;
      this.TextChanged((object) this, args);
    }

    public event EventHandler SelectionChanged;

    private void RaiseSelectionChanged()
    {
      if (this.SelectionChanged == null)
        return;
      this.SelectionChanged((object) this, EventArgs.Empty);
    }

    public double InputWidth
    {
      get => (double) this.GetValue(TextBox.InputWidthProperty);
      set => this.SetValue(TextBox.InputWidthProperty, (object) value);
    }

    public string Text
    {
      get => (string) this.GetValue(TextBox.TextProperty);
      set => this.SetValue(TextBox.TextProperty, (object) value);
    }

    public bool IsReadOnly
    {
      get => (bool) this.GetValue(TextBox.IsReadOnlyProperty);
      set => this.SetValue(TextBox.IsReadOnlyProperty, (object) value);
    }

    public CharacterCasing CharacterCasing
    {
      get => (CharacterCasing) this.GetValue(TextBox.CharacterCasingProperty);
      set => this.SetValue(TextBox.CharacterCasingProperty, (object) value);
    }

    public int MaxLength
    {
      get => (int) this.GetValue(TextBox.MaxLengthProperty);
      set => this.SetValue(TextBox.MaxLengthProperty, (object) value);
    }

    public TextAlignment TextAlignment
    {
      get => (TextAlignment) this.GetValue(TextBox.TextAlignmentProperty);
      set => this.SetValue(TextBox.TextAlignmentProperty, (object) value);
    }

    public TextDecorationCollection TextDecorations
    {
      get => (TextDecorationCollection) this.GetValue(TextBox.TextDecorationsProperty);
      set => this.SetValue(TextBox.TextDecorationsProperty, (object) value);
    }

    public bool IsUndoEnabled
    {
      get => (bool) this.GetValue(TextBox.IsUndoEnabledProperty);
      set => this.SetValue(TextBox.IsUndoEnabledProperty, (object) value);
    }

    public int UndoLimit
    {
      get => (int) this.GetValue(TextBox.UndoLimitProperty);
      set => this.SetValue(TextBox.UndoLimitProperty, (object) value);
    }

    public bool AutoWordSelection
    {
      get => (bool) this.GetValue(TextBox.AutoWordSelectionProperty);
      set => this.SetValue(TextBox.AutoWordSelectionProperty, (object) value);
    }

    public Brush SelectionBrush
    {
      get => (Brush) this.GetValue(TextBox.SelectionBrushProperty);
      set => this.SetValue(TextBox.SelectionBrushProperty, (object) value);
    }

    public double SelectionOpacity
    {
      get => (double) this.GetValue(TextBox.SelectionOpacityProperty);
      set => this.SetValue(TextBox.SelectionOpacityProperty, (object) value);
    }

    public Brush CaretBrush
    {
      get => (Brush) this.GetValue(TextBox.CaretBrushProperty);
      set => this.SetValue(TextBox.CaretBrushProperty, (object) value);
    }

    public int SelectionStart
    {
      get => this.textBox.SelectionStart;
      set
      {
        if (this.textBoxTemplated != null)
          this.textBoxTemplated.SelectionStart = value;
        else
          this.textBox.SelectionStart = value;
      }
    }

    public int SelectionLength
    {
      get => this.textBox.SelectionLength;
      set
      {
        if (this.textBoxTemplated != null)
          this.textBoxTemplated.SelectionLength = value;
        else
          this.textBox.SelectionLength = value;
      }
    }

    public string SelectedText
    {
      get => this.textBox.SelectedText;
      set
      {
        if (this.textBoxTemplated != null)
          this.textBoxTemplated.SelectedText = value;
        else
          this.textBox.SelectedText = value;
      }
    }

    public bool CanUndo => this.textBoxTemplated != null && this.textBoxTemplated.CanUndo;

    public bool CanRedo => this.textBoxTemplated != null && this.textBoxTemplated.CanRedo;

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static TextBox()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TextBox)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (TextBox), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(TextBox.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (TextBox));
      return basevalue;
    }

    public TextBox()
    {
      this.Focusable = true;
      this.textBox.SelectionChanged += (RoutedEventHandler) ((s, e) => this.RaiseSelectionChanged());
      this.textBox.TextChanged += (TextChangedEventHandler) ((s, e) => this.RaiseTextChanged(e));
      this.textBox.SetBinding(System.Windows.Controls.TextBox.TextProperty, (BindingBase) new Binding(nameof (Text))
      {
        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        Source = (object) this,
        Mode = BindingMode.TwoWay
      });
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (CharacterCasing), System.Windows.Controls.TextBox.CharacterCasingProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (MaxLength), System.Windows.Controls.TextBox.MaxLengthProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (TextAlignment), System.Windows.Controls.TextBox.TextAlignmentProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (TextDecorations), System.Windows.Controls.TextBox.TextDecorationsProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (IsUndoEnabled), TextBoxBase.IsUndoEnabledProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (UndoLimit), TextBoxBase.UndoLimitProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (AutoWordSelection), TextBoxBase.AutoWordSelectionProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (SelectionBrush), TextBoxBase.SelectionBrushProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (SelectionOpacity), TextBoxBase.SelectionOpacityProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (CaretBrush), TextBoxBase.CaretBrushProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this.textBox, (FrameworkElement) this, nameof (IsReadOnly), TextBoxBase.IsReadOnlyProperty, BindingMode.TwoWay);
    }

    public void AppendText(string text)
    {
      if (this.textBoxTemplated != null)
        this.textBoxTemplated.AppendText(text);
      else
        this.textBox.AppendText(text);
    }

    public void Copy()
    {
      if (this.textBoxTemplated != null)
        this.textBoxTemplated.Copy();
      else
        this.textBox.Copy();
    }

    public void Cut()
    {
      if (this.textBoxTemplated != null)
        this.textBoxTemplated.Cut();
      else
        this.textBox.Cut();
    }

    public void Paste()
    {
      if (this.textBoxTemplated != null)
        this.textBoxTemplated.Paste();
      else
        this.textBox.Paste();
    }

    public bool Undo() => this.textBoxTemplated != null && this.textBoxTemplated.Undo();

    public bool Redo() => this.textBoxTemplated != null && this.textBoxTemplated.Redo();

    public void SelectAll()
    {
      if (this.textBoxTemplated != null)
        this.textBoxTemplated.SelectAll();
      else
        this.textBox.SelectAll();
    }

    public void Select(int start, int length)
    {
      if (this.textBoxTemplated != null)
        this.textBoxTemplated.Select(start, length);
      else
        this.textBox.Select(start, length);
    }

    public override void OnApplyTemplate()
    {
      if (this.textBoxTemplated != null)
      {
        this.textBoxTemplated.PreviewKeyDown -= new KeyEventHandler(this.OnTextBoxTemplatedKeyDown);
        this.textBoxTemplated.SelectionChanged -= new RoutedEventHandler(this.OnTextBoxTemplatedSelectionChanged);
        this.textBoxTemplated.LostFocus -= new RoutedEventHandler(this.OnTextBoxTemplatedLostFocus);
        this.textBoxTemplated.GotKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnTextBoxTemplatedGotKeyboardFocus);
        this.textBoxTemplated.TextChanged -= new TextChangedEventHandler(this.OnTextBoxTemplatedTextChanged);
        BindingOperations.ClearAllBindings((DependencyObject) this.textBoxTemplated);
      }
      this.textBoxTemplated = this.GetTemplateChild("PART_TextBox") as System.Windows.Controls.TextBox;
      if (!this.IsTemplateValid())
        return;
      this.textBoxTemplated.Text = this.Text;
      this.textBoxTemplated.Select(this.textBox.SelectionStart, this.textBox.SelectionLength);
      BindingOperations.ClearAllBindings((DependencyObject) this.textBox);
      this.textBoxTemplated.SetBinding(System.Windows.Controls.TextBox.TextProperty, (BindingBase) new Binding("Text")
      {
        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        Source = (object) this,
        Mode = BindingMode.TwoWay
      });
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "CharacterCasing", System.Windows.Controls.TextBox.CharacterCasingProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "MaxLength", System.Windows.Controls.TextBox.MaxLengthProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "TextAlignment", System.Windows.Controls.TextBox.TextAlignmentProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "TextDecorations", System.Windows.Controls.TextBox.TextDecorationsProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "IsUndoEnabled", TextBoxBase.IsUndoEnabledProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "UndoLimit", TextBoxBase.UndoLimitProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "AutoWordSelection", TextBoxBase.AutoWordSelectionProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "SelectionBrush", TextBoxBase.SelectionBrushProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "SelectionOpacity", TextBoxBase.SelectionOpacityProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "CaretBrush", TextBoxBase.CaretBrushProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) this.textBoxTemplated, "IsReadOnly", TextBoxBase.IsReadOnlyProperty, BindingMode.TwoWay);
      this.textBoxTemplated.PreviewKeyDown += new KeyEventHandler(this.OnTextBoxTemplatedKeyDown);
      this.textBoxTemplated.SelectionChanged += new RoutedEventHandler(this.OnTextBoxTemplatedSelectionChanged);
      this.textBoxTemplated.LostFocus += new RoutedEventHandler(this.OnTextBoxTemplatedLostFocus);
      this.textBoxTemplated.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnTextBoxTemplatedGotKeyboardFocus);
      this.textBoxTemplated.TextChanged += new TextChangedEventHandler(this.OnTextBoxTemplatedTextChanged);
    }

    private void OnTextBoxTemplatedTextChanged(object sender, TextChangedEventArgs e)
    {
      this.RaiseTextChanged(e);
    }

    private void OnTextBoxTemplatedGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      this.textBoxContentWhenGotFocus = this.textBoxTemplated.Text;
    }

    private void OnTextBoxTemplatedLostFocus(object sender, RoutedEventArgs e)
    {
      if (!(this.textBoxContentWhenGotFocus != this.textBoxTemplated.Text))
        return;
      this.ExecuteCommand();
    }

    private void OnTextBoxTemplatedSelectionChanged(object sender, RoutedEventArgs e)
    {
      this.textBox.Select(this.textBoxTemplated.SelectionStart, this.textBoxTemplated.SelectionLength);
    }

    public override void OnKeyTipPressed()
    {
      if (!this.IsTemplateValid())
        return;
      this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Delegate) (() =>
      {
        this.textBoxTemplated.SelectAll();
        this.textBoxTemplated.Focus();
      }));
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
      if (e.Key == Key.Return || e.Key == Key.Space)
        return;
      base.OnKeyUp(e);
    }

    private void OnTextBoxTemplatedKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      this.textBoxTemplated.Focusable = false;
      this.Focus();
      this.textBoxTemplated.Focusable = true;
    }

    private bool IsTemplateValid() => this.textBoxTemplated != null;

    public override FrameworkElement CreateQuickAccessItem()
    {
      TextBox element = new TextBox();
      this.BindQuickAccessItem((FrameworkElement) element);
      return (FrameworkElement) element;
    }

    protected void BindQuickAccessItem(FrameworkElement element)
    {
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, element);
      TextBox target = (TextBox) element;
      target.Width = this.Width;
      target.InputWidth = this.InputWidth;
      RibbonControl.Bind((object) this, (FrameworkElement) target, "Text", TextBox.TextProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "IsReadOnly", TextBox.IsReadOnlyProperty, BindingMode.OneWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "CharacterCasing", TextBox.CharacterCasingProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "MaxLength", TextBox.MaxLengthProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "TextAlignment", TextBox.TextAlignmentProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "TextDecorations", TextBox.TextDecorationsProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "IsUndoEnabled", TextBox.IsUndoEnabledProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "UndoLimit", TextBox.UndoLimitProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "AutoWordSelection", TextBox.AutoWordSelectionProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "SelectionBrush", TextBox.SelectionBrushProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "SelectionOpacity", TextBox.SelectionOpacityProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "CaretBrush", TextBox.CaretBrushProperty, BindingMode.TwoWay);
      RibbonControl.Bind((object) this, (FrameworkElement) target, "IsReadOnly", TextBox.IsReadOnlyProperty, BindingMode.TwoWay);
      RibbonControl.BindQuickAccessItem((FrameworkElement) this, element);
    }
  }
}
