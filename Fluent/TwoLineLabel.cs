// Decompiled with JetBrains decompiler
// Type: Fluent.TwoLineLabel
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

#nullable disable
namespace Fluent
{
  [TemplatePart(Name = "PART_Glyph", Type = typeof (InlineUIContainer))]
  [TemplatePart(Name = "PART_TextRun2", Type = typeof (TextBlock))]
  [TemplatePart(Name = "PART_TextRun", Type = typeof (TextBlock))]
  public class TwoLineLabel : Control
  {
    private TextBlock textRun;
    private TextBlock textRun2;
    public static readonly DependencyProperty HasTwoLinesProperty = DependencyProperty.Register(nameof (HasTwoLines), typeof (bool), typeof (TwoLineLabel), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(TwoLineLabel.OnHasTwoLinesChanged)));
    public static readonly DependencyProperty HasGlyphProperty = DependencyProperty.Register(nameof (HasGlyph), typeof (bool), typeof (TwoLineLabel), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(TwoLineLabel.OnHasGlyphChanged)));
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (TwoLineLabel), (PropertyMetadata) new UIPropertyMetadata((object) nameof (TwoLineLabel), new PropertyChangedCallback(TwoLineLabel.OnTextChanged)));

    public bool HasTwoLines
    {
      get => (bool) this.GetValue(TwoLineLabel.HasTwoLinesProperty);
      set => this.SetValue(TwoLineLabel.HasTwoLinesProperty, (object) value);
    }

    private static void OnHasTwoLinesChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as TwoLineLabel).UpdateTextRun();
    }

    public bool HasGlyph
    {
      get => (bool) this.GetValue(TwoLineLabel.HasGlyphProperty);
      set => this.SetValue(TwoLineLabel.HasGlyphProperty, (object) value);
    }

    private static void OnHasGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      (d as TwoLineLabel).UpdateTextRun();
    }

    public string Text
    {
      get => (string) this.GetValue(TwoLineLabel.TextProperty);
      set => this.SetValue(TwoLineLabel.TextProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static TwoLineLabel()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (TwoLineLabel), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (TwoLineLabel)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (TwoLineLabel), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(TwoLineLabel.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (TwoLineLabel));
      return basevalue;
    }

    public TwoLineLabel() => this.Focusable = false;

    public override void OnApplyTemplate()
    {
      this.textRun = this.GetTemplateChild("PART_TextRun") as TextBlock;
      this.textRun2 = this.GetTemplateChild("PART_TextRun2") as TextBlock;
      this.UpdateTextRun();
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      (d as TwoLineLabel).UpdateTextRun();
    }

    private void UpdateTextRun()
    {
      if (this.textRun == null || this.textRun2 == null || this.Text == null)
        return;
      this.textRun.Text = this.Text;
      this.textRun2.Text = "";
      string str = this.Text.Trim();
      if (!this.HasTwoLines)
        return;
      int num1 = this.Text.Length / 2;
      int num2 = str.LastIndexOf(" ", num1, num1);
      int num3 = str.IndexOf(" ", num1, StringComparison.CurrentCulture);
      if (num2 == -1 && num3 == -1)
        return;
      if (num2 == -1)
      {
        this.textRun.Text = str.Substring(0, num3);
        this.textRun2.Text = str.Substring(num3) + " ";
      }
      else if (num3 == -1)
      {
        this.textRun.Text = str.Substring(0, num2);
        this.textRun2.Text = str.Substring(num2) + " ";
      }
      else if (Math.Abs(num1 - num2) < Math.Abs(num1 - num3))
      {
        this.textRun.Text = str.Substring(0, num2);
        this.textRun2.Text = str.Substring(num2) + " ";
      }
      else
      {
        this.textRun.Text = str.Substring(0, num3);
        this.textRun2.Text = str.Substring(num3) + " ";
      }
    }
  }
}
