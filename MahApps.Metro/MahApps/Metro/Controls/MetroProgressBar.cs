// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.MetroProgressBar
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class MetroProgressBar : ProgressBar
  {
    public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register(nameof (EllipseDiameter), typeof (double), typeof (MetroProgressBar), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty EllipseOffsetProperty = DependencyProperty.Register(nameof (EllipseOffset), typeof (double), typeof (MetroProgressBar), new PropertyMetadata((object) 0.0));
    private readonly object lockme = new object();
    private Storyboard indeterminateStoryboard;

    static MetroProgressBar()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (MetroProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (MetroProgressBar)));
      ProgressBar.IsIndeterminateProperty.OverrideMetadata(typeof (MetroProgressBar), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(MetroProgressBar.OnIsIndeterminateChanged)));
    }

    public MetroProgressBar()
    {
      this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.VisibleChangedHandler);
    }

    private void VisibleChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (!this.IsIndeterminate)
        return;
      MetroProgressBar.ToggleIndeterminate(this, (bool) e.OldValue, (bool) e.NewValue);
    }

    private static void OnIsIndeterminateChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      MetroProgressBar.ToggleIndeterminate(dependencyObject as MetroProgressBar, (bool) e.OldValue, (bool) e.NewValue);
    }

    private static void ToggleIndeterminate(MetroProgressBar bar, bool oldValue, bool newValue)
    {
      if (bar == null || newValue == oldValue)
        return;
      VisualState indeterminate = bar.GetIndeterminate();
      FrameworkElement templateChild = bar.GetTemplateChild("ContainingGrid") as FrameworkElement;
      if (indeterminate == null || templateChild == null)
        return;
      if (oldValue && indeterminate.Storyboard != null)
      {
        indeterminate.Storyboard.Stop(templateChild);
        indeterminate.Storyboard.Remove(templateChild);
      }
      if (!newValue)
        return;
      Action method = (Action) (() =>
      {
        bar.InvalidateMeasure();
        bar.InvalidateArrange();
        bar.ResetStoryboard(bar.ActualWidth, false);
      });
      bar.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) method);
    }

    public double EllipseDiameter
    {
      get => (double) this.GetValue(MetroProgressBar.EllipseDiameterProperty);
      set => this.SetValue(MetroProgressBar.EllipseDiameterProperty, (object) value);
    }

    public double EllipseOffset
    {
      get => (double) this.GetValue(MetroProgressBar.EllipseOffsetProperty);
      set => this.SetValue(MetroProgressBar.EllipseOffsetProperty, (object) value);
    }

    private void SizeChangedHandler(object sender, SizeChangedEventArgs e)
    {
      double actualWidth = this.ActualWidth;
      MetroProgressBar metroProgressBar = this;
      if (this.Visibility != Visibility.Visible || !this.IsIndeterminate)
        return;
      metroProgressBar.ResetStoryboard(actualWidth, true);
    }

    private void ResetStoryboard(double width, bool removeOldStoryboard)
    {
      if (!this.IsIndeterminate)
        return;
      lock (this.lockme)
      {
        double num1 = this.CalcContainerAnimStart(width);
        double num2 = this.CalcContainerAnimEnd(width);
        double num3 = this.CalcEllipseAnimWell(width);
        double num4 = this.CalcEllipseAnimEnd(width);
        try
        {
          VisualState indeterminate = this.GetIndeterminate();
          if (indeterminate == null || this.indeterminateStoryboard == null)
            return;
          Storyboard storyboard = this.indeterminateStoryboard.Clone();
          Timeline timeline = storyboard.Children.First<Timeline>((Func<Timeline, bool>) (t => t.Name == "MainDoubleAnim"));
          timeline.SetValue(DoubleAnimation.FromProperty, (object) num1);
          timeline.SetValue(DoubleAnimation.ToProperty, (object) num2);
          string[] strArray = new string[5]
          {
            "E1",
            "E2",
            "E3",
            "E4",
            "E5"
          };
          foreach (string str in strArray)
          {
            string elemName = str;
            DoubleAnimationUsingKeyFrames animationUsingKeyFrames = (DoubleAnimationUsingKeyFrames) storyboard.Children.First<Timeline>((Func<Timeline, bool>) (t => t.Name == elemName + "Anim"));
            DoubleKeyFrame keyFrame1;
            DoubleKeyFrame keyFrame2;
            DoubleKeyFrame keyFrame3;
            if (elemName == "E1")
            {
              keyFrame1 = animationUsingKeyFrames.KeyFrames[1];
              keyFrame2 = animationUsingKeyFrames.KeyFrames[2];
              keyFrame3 = animationUsingKeyFrames.KeyFrames[3];
            }
            else
            {
              keyFrame1 = animationUsingKeyFrames.KeyFrames[2];
              keyFrame2 = animationUsingKeyFrames.KeyFrames[3];
              keyFrame3 = animationUsingKeyFrames.KeyFrames[4];
            }
            keyFrame1.Value = num3;
            keyFrame2.Value = num3;
            keyFrame3.Value = num4;
            keyFrame1.InvalidateProperty(DoubleKeyFrame.ValueProperty);
            keyFrame2.InvalidateProperty(DoubleKeyFrame.ValueProperty);
            keyFrame3.InvalidateProperty(DoubleKeyFrame.ValueProperty);
            animationUsingKeyFrames.InvalidateProperty(Storyboard.TargetPropertyProperty);
            animationUsingKeyFrames.InvalidateProperty(Storyboard.TargetNameProperty);
          }
          FrameworkElement templateChild = (FrameworkElement) this.GetTemplateChild("ContainingGrid");
          if (removeOldStoryboard && indeterminate.Storyboard != null)
          {
            indeterminate.Storyboard.Stop(templateChild);
            indeterminate.Storyboard.Remove(templateChild);
          }
          indeterminate.Storyboard = storyboard;
          if (indeterminate.Storyboard == null)
            return;
          indeterminate.Storyboard.Begin(templateChild, true);
        }
        catch (Exception ex)
        {
        }
      }
    }

    private VisualState GetIndeterminate()
    {
      if (!(this.GetTemplateChild("ContainingGrid") is FrameworkElement templateChild1))
      {
        this.ApplyTemplate();
        if (!(this.GetTemplateChild("ContainingGrid") is FrameworkElement templateChild1))
          return (VisualState) null;
      }
      IList visualStateGroups = VisualStateManager.GetVisualStateGroups(templateChild1);
      return visualStateGroups == null ? (VisualState) null : visualStateGroups.Cast<VisualStateGroup>().SelectMany<VisualStateGroup, VisualState>((Func<VisualStateGroup, IEnumerable<VisualState>>) (group => group.States.Cast<VisualState>())).FirstOrDefault<VisualState>((Func<VisualState, bool>) (state => state.Name == "Indeterminate"));
    }

    private void SetEllipseDiameter(double width)
    {
      if (width <= 180.0)
        this.EllipseDiameter = 4.0;
      else if (width <= 280.0)
        this.EllipseDiameter = 5.0;
      else
        this.EllipseDiameter = 6.0;
    }

    private void SetEllipseOffset(double width)
    {
      if (width <= 180.0)
        this.EllipseOffset = 4.0;
      else if (width <= 280.0)
        this.EllipseOffset = 7.0;
      else
        this.EllipseOffset = 9.0;
    }

    private double CalcContainerAnimStart(double width)
    {
      if (width <= 180.0)
        return -34.0;
      return width <= 280.0 ? -50.5 : -63.0;
    }

    private double CalcContainerAnimEnd(double width)
    {
      double num = 272.0 / 625.0 * width;
      if (width <= 180.0)
        return num - 25.731;
      return width <= 280.0 ? num + 27.84 : num + 58.862;
    }

    private double CalcEllipseAnimWell(double width) => width * 1.0 / 3.0;

    private double CalcEllipseAnimEnd(double width) => width * 2.0 / 3.0;

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      lock (this.lockme)
        this.indeterminateStoryboard = this.TryFindResource((object) "IndeterminateStoryboard") as Storyboard;
      this.Loaded -= new RoutedEventHandler(this.LoadedHandler);
      this.Loaded += new RoutedEventHandler(this.LoadedHandler);
    }

    private void LoadedHandler(object sender, RoutedEventArgs routedEventArgs)
    {
      this.Loaded -= new RoutedEventHandler(this.LoadedHandler);
      this.SizeChangedHandler((object) null, (SizeChangedEventArgs) null);
      this.SizeChanged += new SizeChangedEventHandler(this.SizeChangedHandler);
    }

    protected override void OnInitialized(EventArgs e)
    {
      base.OnInitialized(e);
      if (this.EllipseDiameter.Equals(0.0))
        this.SetEllipseDiameter(this.ActualWidth);
      if (!this.EllipseOffset.Equals(0.0))
        return;
      this.SetEllipseOffset(this.ActualWidth);
    }
  }
}
