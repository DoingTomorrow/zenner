// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.ToggleSwitchButton
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplatePart(Name = "PART_BackgroundTranslate", Type = typeof (TranslateTransform))]
  [TemplatePart(Name = "PART_DraggingThumb", Type = typeof (Thumb))]
  [TemplatePart(Name = "PART_SwitchTrack", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_ThumbIndicator", Type = typeof (Shape))]
  [TemplatePart(Name = "PART_ThumbTranslate", Type = typeof (TranslateTransform))]
  public class ToggleSwitchButton : ToggleButton
  {
    private const string PART_BackgroundTranslate = "PART_BackgroundTranslate";
    private const string PART_DraggingThumb = "PART_DraggingThumb";
    private const string PART_SwitchTrack = "PART_SwitchTrack";
    private const string PART_ThumbIndicator = "PART_ThumbIndicator";
    private const string PART_ThumbTranslate = "PART_ThumbTranslate";
    private TranslateTransform _BackgroundTranslate;
    private Thumb _DraggingThumb;
    private Grid _SwitchTrack;
    private Shape _ThumbIndicator;
    private TranslateTransform _ThumbTranslate;
    [Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
    public static readonly DependencyProperty SwitchForegroundProperty = DependencyProperty.Register(nameof (SwitchForeground), typeof (Brush), typeof (ToggleSwitchButton), new PropertyMetadata((object) null, (PropertyChangedCallback) ((o, e) => ((ToggleSwitchButton) o).OnSwitchBrush = e.NewValue as Brush)));
    public static readonly DependencyProperty OnSwitchBrushProperty = DependencyProperty.Register(nameof (OnSwitchBrush), typeof (Brush), typeof (ToggleSwitchButton), (PropertyMetadata) null);
    public static readonly DependencyProperty OffSwitchBrushProperty = DependencyProperty.Register(nameof (OffSwitchBrush), typeof (Brush), typeof (ToggleSwitchButton), (PropertyMetadata) null);
    public static readonly DependencyProperty ThumbIndicatorBrushProperty = DependencyProperty.Register(nameof (ThumbIndicatorBrush), typeof (Brush), typeof (ToggleSwitchButton), (PropertyMetadata) null);
    public static readonly DependencyProperty ThumbIndicatorDisabledBrushProperty = DependencyProperty.Register(nameof (ThumbIndicatorDisabledBrush), typeof (Brush), typeof (ToggleSwitchButton), (PropertyMetadata) null);
    public static readonly DependencyProperty ThumbIndicatorWidthProperty = DependencyProperty.Register(nameof (ThumbIndicatorWidth), typeof (double), typeof (ToggleSwitchButton), new PropertyMetadata((object) 13.0));
    private DoubleAnimation _thumbAnimation;
    private double? _lastDragPosition;
    private bool _isDragging;

    [Obsolete("This property will be deleted in the next release. You should use OnSwitchBrush and OffSwitchBrush to change the switch's brushes.")]
    public Brush SwitchForeground
    {
      get => (Brush) this.GetValue(ToggleSwitchButton.SwitchForegroundProperty);
      set => this.SetValue(ToggleSwitchButton.SwitchForegroundProperty, (object) value);
    }

    public Brush OnSwitchBrush
    {
      get => (Brush) this.GetValue(ToggleSwitchButton.OnSwitchBrushProperty);
      set => this.SetValue(ToggleSwitchButton.OnSwitchBrushProperty, (object) value);
    }

    public Brush OffSwitchBrush
    {
      get => (Brush) this.GetValue(ToggleSwitchButton.OffSwitchBrushProperty);
      set => this.SetValue(ToggleSwitchButton.OffSwitchBrushProperty, (object) value);
    }

    public Brush ThumbIndicatorBrush
    {
      get => (Brush) this.GetValue(ToggleSwitchButton.ThumbIndicatorBrushProperty);
      set => this.SetValue(ToggleSwitchButton.ThumbIndicatorBrushProperty, (object) value);
    }

    public Brush ThumbIndicatorDisabledBrush
    {
      get => (Brush) this.GetValue(ToggleSwitchButton.ThumbIndicatorDisabledBrushProperty);
      set => this.SetValue(ToggleSwitchButton.ThumbIndicatorDisabledBrushProperty, (object) value);
    }

    public double ThumbIndicatorWidth
    {
      get => (double) this.GetValue(ToggleSwitchButton.ThumbIndicatorWidthProperty);
      set => this.SetValue(ToggleSwitchButton.ThumbIndicatorWidthProperty, (object) value);
    }

    public ToggleSwitchButton()
    {
      this.DefaultStyleKey = (object) typeof (ToggleSwitchButton);
      this.Checked += new RoutedEventHandler(this.ToggleSwitchButton_Checked);
      this.Unchecked += new RoutedEventHandler(this.ToggleSwitchButton_Checked);
    }

    private void ToggleSwitchButton_Checked(object sender, RoutedEventArgs e) => this.UpdateThumb();

    private void UpdateThumb()
    {
      if (this._ThumbTranslate == null || this._SwitchTrack == null || this._ThumbIndicator == null)
        return;
      double destination = this.IsChecked.GetValueOrDefault() ? this.ActualWidth - (this._SwitchTrack.Margin.Left + this._SwitchTrack.Margin.Right + this._ThumbIndicator.ActualWidth) : 0.0;
      this._thumbAnimation = new DoubleAnimation();
      this._thumbAnimation.To = new double?(destination);
      this._thumbAnimation.Duration = (Duration) TimeSpan.FromMilliseconds(500.0);
      this._thumbAnimation.EasingFunction = (IEasingFunction) new ExponentialEase()
      {
        Exponent = 9.0
      };
      this._thumbAnimation.FillBehavior = FillBehavior.Stop;
      AnimationTimeline currentAnimation = (AnimationTimeline) this._thumbAnimation;
      this._thumbAnimation.Completed += (EventHandler) ((sender, e) =>
      {
        if (this._thumbAnimation == null || currentAnimation != this._thumbAnimation)
          return;
        this._ThumbTranslate.X = destination;
        this._thumbAnimation = (DoubleAnimation) null;
      });
      this._ThumbTranslate.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) this._thumbAnimation);
    }

    protected override void OnToggle()
    {
      bool? isChecked = this.IsChecked;
      bool flag = true;
      this.IsChecked = new bool?(isChecked.GetValueOrDefault() != flag || !isChecked.HasValue);
      this.UpdateThumb();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this._BackgroundTranslate = this.GetTemplateChild("PART_BackgroundTranslate") as TranslateTransform;
      this._DraggingThumb = this.GetTemplateChild("PART_DraggingThumb") as Thumb;
      this._SwitchTrack = this.GetTemplateChild("PART_SwitchTrack") as Grid;
      this._ThumbIndicator = this.GetTemplateChild("PART_ThumbIndicator") as Shape;
      this._ThumbTranslate = this.GetTemplateChild("PART_ThumbTranslate") as TranslateTransform;
      if (this._ThumbIndicator != null && this._BackgroundTranslate != null)
        BindingOperations.SetBinding((DependencyObject) this._BackgroundTranslate, TranslateTransform.XProperty, (BindingBase) new Binding("X")
        {
          Source = (object) this._ThumbTranslate
        });
      if (this._DraggingThumb == null || this._SwitchTrack == null || this._ThumbIndicator == null || this._ThumbTranslate == null)
        return;
      this._DraggingThumb.DragStarted += new DragStartedEventHandler(this._DraggingThumb_DragStarted);
      this._DraggingThumb.DragDelta += new DragDeltaEventHandler(this._DraggingThumb_DragDelta);
      this._DraggingThumb.DragCompleted += new DragCompletedEventHandler(this._DraggingThumb_DragCompleted);
      this._SwitchTrack.SizeChanged += new SizeChangedEventHandler(this._SwitchTrack_SizeChanged);
    }

    private void _DraggingThumb_DragStarted(object sender, DragStartedEventArgs e)
    {
      if (this._ThumbTranslate != null)
      {
        this._ThumbTranslate.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) null);
        this._ThumbTranslate.X = this.IsChecked.GetValueOrDefault() ? this.ActualWidth - (this._SwitchTrack.Margin.Left + this._SwitchTrack.Margin.Right + this._ThumbIndicator.ActualWidth) : 0.0;
        this._thumbAnimation = (DoubleAnimation) null;
      }
      this._lastDragPosition = new double?(this._ThumbTranslate.X);
      this._isDragging = false;
    }

    private void _DraggingThumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
      if (!this._lastDragPosition.HasValue)
        return;
      if (Math.Abs(e.HorizontalChange) > 3.0)
        this._isDragging = true;
      if (this._SwitchTrack == null || this._ThumbIndicator == null)
        return;
      this._ThumbTranslate.X = Math.Min(this.ActualWidth - (this._SwitchTrack.Margin.Left + this._SwitchTrack.Margin.Right + this._ThumbIndicator.ActualWidth), Math.Max(0.0, this._lastDragPosition.Value + e.HorizontalChange));
    }

    private void _DraggingThumb_DragCompleted(object sender, DragCompletedEventArgs e)
    {
      this._lastDragPosition = new double?();
      if (!this._isDragging)
      {
        this.OnToggle();
      }
      else
      {
        if (this._ThumbTranslate == null || this._SwitchTrack == null)
          return;
        if (!this.IsChecked.GetValueOrDefault() && this._ThumbTranslate.X + 6.5 >= this._SwitchTrack.ActualWidth / 2.0)
          this.OnToggle();
        else if (this.IsChecked.GetValueOrDefault() && this._ThumbTranslate.X + 6.5 <= this._SwitchTrack.ActualWidth / 2.0)
          this.OnToggle();
        else
          this.UpdateThumb();
      }
    }

    private void _SwitchTrack_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this._ThumbTranslate == null || this._SwitchTrack == null || this._ThumbIndicator == null)
        return;
      this._ThumbTranslate.X = this.IsChecked.GetValueOrDefault() ? this.ActualWidth - (this._SwitchTrack.Margin.Left + this._SwitchTrack.Margin.Right + this._ThumbIndicator.ActualWidth) : 0.0;
    }
  }
}
