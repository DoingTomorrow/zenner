// Decompiled with JetBrains decompiler
// Type: WpfKb.Behaviors.AutoHideBehavior
// Assembly: WpfKb, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B294CC70-CB21-4202-BD7A-A4E6693370B9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\WpfKb.dll

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace WpfKb.Behaviors
{
  public class AutoHideBehavior : BehaviorBase<FrameworkElement>
  {
    public static readonly DependencyProperty ActionWhenClickedProperty = DependencyProperty.Register(nameof (ActionWhenClicked), typeof (AutoHideBehavior.ClickAction), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) AutoHideBehavior.ClickAction.Show));
    public static readonly DependencyProperty AreAnimationsEnabledProperty = DependencyProperty.Register(nameof (AreAnimationsEnabled), typeof (bool), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty HideDelayProperty = DependencyProperty.Register(nameof (HideDelay), typeof (double), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) 5.0));
    public static readonly DependencyProperty HideDurationProperty = DependencyProperty.Register(nameof (HideDuration), typeof (double), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) 0.5));
    public static readonly DependencyProperty IsAllowedToHideProperty = DependencyProperty.Register(nameof (IsAllowedToHide), typeof (bool), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(AutoHideBehavior.OnIsAllowedToHidePropertyChanged)));
    public static readonly DependencyProperty IsAllowedToShowProperty = DependencyProperty.Register(nameof (IsAllowedToShow), typeof (bool), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof (IsShown), typeof (bool), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(AutoHideBehavior.OnIsShownPropertyChanged)));
    public static readonly DependencyProperty MaxOpacityProperty = DependencyProperty.Register(nameof (MaxOpacity), typeof (double), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) 1.0));
    public static readonly DependencyProperty MinOpacityProperty = DependencyProperty.Register(nameof (MinOpacity), typeof (double), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) 0.0));
    public static readonly DependencyProperty ShowDurationProperty = DependencyProperty.Register(nameof (ShowDuration), typeof (double), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) 0.0));
    public static readonly DependencyProperty TimerIntervalProperty = DependencyProperty.Register(nameof (TimerInterval), typeof (double), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) 0.3));
    public static readonly DependencyProperty KeyBoardInputTypeProperty = DependencyProperty.Register(nameof (KeyBoardInputType), typeof (string), typeof (AutoHideBehavior), (PropertyMetadata) new UIPropertyMetadata((object) "alphanumeric", new PropertyChangedCallback(AutoHideBehavior.OnIsInputTypeChanged)));
    private DispatcherTimer _timer;
    private DateTime _lastActivityTime;

    private static void OnIsInputTypeChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
    }

    public AutoHideBehavior.ClickAction ActionWhenClicked
    {
      get
      {
        return (AutoHideBehavior.ClickAction) this.GetValue(AutoHideBehavior.ActionWhenClickedProperty);
      }
      set => this.SetValue(AutoHideBehavior.ActionWhenClickedProperty, (object) value);
    }

    public string KeyBoardInputType
    {
      get => (string) this.GetValue(AutoHideBehavior.KeyBoardInputTypeProperty);
      set => this.SetValue(AutoHideBehavior.KeyBoardInputTypeProperty, (object) value);
    }

    public bool AreAnimationsEnabled
    {
      get => (bool) this.GetValue(AutoHideBehavior.AreAnimationsEnabledProperty);
      set => this.SetValue(AutoHideBehavior.AreAnimationsEnabledProperty, (object) value);
    }

    public double HideDelay
    {
      get => (double) this.GetValue(AutoHideBehavior.HideDelayProperty);
      set => this.SetValue(AutoHideBehavior.HideDelayProperty, (object) value);
    }

    public double HideDuration
    {
      get => (double) this.GetValue(AutoHideBehavior.HideDurationProperty);
      set => this.SetValue(AutoHideBehavior.HideDurationProperty, (object) value);
    }

    public bool IsAllowedToHide
    {
      get => (bool) this.GetValue(AutoHideBehavior.IsAllowedToHideProperty);
      set => this.SetValue(AutoHideBehavior.IsAllowedToHideProperty, (object) value);
    }

    public bool IsAllowedToShow
    {
      get => (bool) this.GetValue(AutoHideBehavior.IsAllowedToShowProperty);
      set => this.SetValue(AutoHideBehavior.IsAllowedToShowProperty, (object) value);
    }

    public bool IsShown
    {
      get => (bool) this.GetValue(AutoHideBehavior.IsShownProperty);
      set => this.SetValue(AutoHideBehavior.IsShownProperty, (object) value);
    }

    public double MaxOpacity
    {
      get => (double) this.GetValue(AutoHideBehavior.MaxOpacityProperty);
      set => this.SetValue(AutoHideBehavior.MaxOpacityProperty, (object) value);
    }

    public double MinOpacity
    {
      get => (double) this.GetValue(AutoHideBehavior.MinOpacityProperty);
      set => this.SetValue(AutoHideBehavior.MinOpacityProperty, (object) value);
    }

    public double ShowDuration
    {
      get => (double) this.GetValue(AutoHideBehavior.ShowDurationProperty);
      set => this.SetValue(AutoHideBehavior.ShowDurationProperty, (object) value);
    }

    public double TimerInterval
    {
      get => (double) this.GetValue(AutoHideBehavior.TimerIntervalProperty);
      set => this.SetValue(AutoHideBehavior.TimerIntervalProperty, (object) value);
    }

    protected override void OnSetup()
    {
      base.OnSetup();
      this.Dispatcher.ShutdownStarted += new EventHandler(this.Dispatcher_ShutdownStarted);
      this.AssociatedObject.PreviewMouseDown += new MouseButtonEventHandler(this.HandlePreviewMouseDown);
      this.Show();
      this.PrepareToHide();
    }

    protected override void OnCleanup()
    {
      base.OnCleanup();
      this.AssociatedObject.PreviewMouseDown -= new MouseButtonEventHandler(this.HandlePreviewMouseDown);
      if (this._timer != null)
        this._timer.Tick -= new EventHandler(this.Tick);
      this.Dispatcher.ShutdownStarted -= new EventHandler(this.Dispatcher_ShutdownStarted);
    }

    private void Dispatcher_ShutdownStarted(object sender, EventArgs e) => this.OnCleanup();

    private static void OnIsAllowedToHidePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((AutoHideBehavior) d).PingActivity();
    }

    private static void OnIsShownPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      AutoHideBehavior autoHideBehavior = (AutoHideBehavior) d;
      if ((bool) e.NewValue)
        autoHideBehavior.Show();
      else
        autoHideBehavior.Hide();
    }

    private void HandlePreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      this._lastActivityTime = DateTime.Now;
      switch (this.ActionWhenClicked)
      {
        case AutoHideBehavior.ClickAction.Show:
          this.Show();
          break;
        case AutoHideBehavior.ClickAction.AcceleratedHide:
          this.HideFast();
          break;
      }
    }

    private void PrepareToHide()
    {
      this.PingActivity();
      if (this._timer != null)
        return;
      this._timer = new DispatcherTimer(TimeSpan.FromSeconds(this.TimerInterval), DispatcherPriority.Background, new EventHandler(this.Tick), this.Dispatcher);
    }

    private void Tick(object sender, EventArgs e)
    {
      this.AssociatedObject.IsHitTestVisible = this.AssociatedObject.Opacity > 0.0;
      if (!(DateTime.Now - this._lastActivityTime > TimeSpan.FromSeconds(this.HideDelay)))
        return;
      if (this.AssociatedObject.Opacity >= this.MaxOpacity && this.IsAllowedToHide)
        this.Hide();
      else
        this._lastActivityTime = DateTime.Now;
    }

    public void PingActivity() => this._lastActivityTime = DateTime.Now;

    public void RefreshKeyboard() => this._lastActivityTime = DateTime.Now;

    public void Show()
    {
      if (this.AssociatedObject == null)
        return;
      this.PingActivity();
      this.PrepareToHide();
      if (!this.IsAllowedToShow)
        return;
      Duration duration = this.AreAnimationsEnabled ? new Duration(TimeSpan.FromSeconds(this.ShowDuration)) : new Duration(TimeSpan.Zero);
      this.IsShown = true;
      this.AssociatedObject.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) new DoubleAnimation(this.MaxOpacity, duration));
    }

    public void Hide()
    {
      if (this.AssociatedObject == null)
        return;
      Duration duration = this.AreAnimationsEnabled ? new Duration(TimeSpan.FromSeconds(this.HideDuration)) : new Duration(TimeSpan.Zero);
      this.IsShown = false;
      this.AssociatedObject.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) new DoubleAnimation(this.MinOpacity, duration));
    }

    public void HideFast()
    {
      if (this.AssociatedObject == null)
        return;
      this.IsShown = false;
      this.AssociatedObject.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) null);
      this.AssociatedObject.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) new DoubleAnimation(this.MinOpacity, new Duration(TimeSpan.Zero)));
    }

    public enum ClickAction
    {
      None,
      Show,
      AcceleratedHide,
    }
  }
}
