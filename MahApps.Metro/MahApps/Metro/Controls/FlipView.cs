// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.FlipView
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplatePart(Name = "PART_Presenter", Type = typeof (TransitioningContentControl))]
  [TemplatePart(Name = "PART_BackButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_ForwardButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_UpButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_DownButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_BannerGrid", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_BannerLabel", Type = typeof (Label))]
  public class FlipView : Selector
  {
    private const string PART_Presenter = "PART_Presenter";
    private const string PART_BackButton = "PART_BackButton";
    private const string PART_ForwardButton = "PART_ForwardButton";
    private const string PART_UpButton = "PART_UpButton";
    private const string PART_DownButton = "PART_DownButton";
    private const string PART_BannerGrid = "PART_BannerGrid";
    private const string PART_BannerLabel = "PART_BannerLabel";
    private TransitioningContentControl presenter;
    private Button backButton;
    private Button forwardButton;
    private Button upButton;
    private Button downButton;
    private Grid bannerGrid;
    private Label bannerLabel;
    private Storyboard showBannerStoryboard;
    private Storyboard hideBannerStoryboard;
    private Storyboard hideControlStoryboard;
    private Storyboard showControlStoryboard;
    private EventHandler hideControlStoryboardCompletedHandler;
    private bool loaded;
    private bool controlsVisibilityOverride;
    public static readonly DependencyProperty UpTransitionProperty = DependencyProperty.Register(nameof (UpTransition), typeof (TransitionType), typeof (FlipView), new PropertyMetadata((object) TransitionType.Up));
    public static readonly DependencyProperty DownTransitionProperty = DependencyProperty.Register(nameof (DownTransition), typeof (TransitionType), typeof (FlipView), new PropertyMetadata((object) TransitionType.Down));
    public static readonly DependencyProperty LeftTransitionProperty = DependencyProperty.Register(nameof (LeftTransition), typeof (TransitionType), typeof (FlipView), new PropertyMetadata((object) TransitionType.LeftReplace));
    public static readonly DependencyProperty RightTransitionProperty = DependencyProperty.Register(nameof (RightTransition), typeof (TransitionType), typeof (FlipView), new PropertyMetadata((object) TransitionType.RightReplace));
    public static readonly DependencyProperty MouseOverGlowEnabledProperty = DependencyProperty.Register(nameof (MouseOverGlowEnabled), typeof (bool), typeof (FlipView), new PropertyMetadata((object) true));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (FlipView), new PropertyMetadata((object) Orientation.Horizontal));
    public static readonly DependencyProperty IsBannerEnabledProperty = DependencyProperty.Register(nameof (IsBannerEnabled), typeof (bool), typeof (FlipView), (PropertyMetadata) new UIPropertyMetadata((object) true, new PropertyChangedCallback(FlipView.OnIsBannerEnabledPropertyChangedCallback)));
    public static readonly DependencyProperty BannerTextProperty = DependencyProperty.Register(nameof (BannerText), typeof (string), typeof (FlipView), (PropertyMetadata) new FrameworkPropertyMetadata((object) "Banner", FrameworkPropertyMetadataOptions.AffectsRender, (PropertyChangedCallback) ((d, e) => FlipView.ExecuteWhenLoaded((FlipView) d, (Action) (() => ((FlipView) d).ChangeBannerText((string) e.NewValue))))));

    static FlipView()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (FlipView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (FlipView)));
    }

    public FlipView()
    {
      this.Unloaded += new RoutedEventHandler(this.FlipView_Unloaded);
      this.Loaded += new RoutedEventHandler(this.FlipView_Loaded);
      this.MouseLeftButtonDown += new MouseButtonEventHandler(this.FlipView_MouseLeftButtonDown);
    }

    private void FlipView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.Focus();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is FlipViewItem;

    protected override DependencyObject GetContainerForItemOverride()
    {
      FlipViewItem containerForItemOverride = new FlipViewItem();
      containerForItemOverride.HorizontalAlignment = HorizontalAlignment.Stretch;
      return (DependencyObject) containerForItemOverride;
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      if (element != item)
        element.SetCurrentValue(FrameworkElement.DataContextProperty, item);
      base.PrepareContainerForItemOverride(element, item);
    }

    private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.DetectControlButtonsStatus();
    }

    private void DetectControlButtonsStatus()
    {
      if (this.controlsVisibilityOverride || this.backButton == null || this.forwardButton == null)
        return;
      this.backButton.Visibility = Visibility.Hidden;
      this.forwardButton.Visibility = Visibility.Hidden;
      this.upButton.Visibility = Visibility.Hidden;
      this.downButton.Visibility = Visibility.Hidden;
      if (this.Items.Count > 0)
      {
        if (this.Orientation == Orientation.Horizontal)
        {
          this.backButton.Visibility = this.SelectedIndex == 0 ? Visibility.Hidden : Visibility.Visible;
          this.forwardButton.Visibility = this.SelectedIndex == this.Items.Count - 1 ? Visibility.Hidden : Visibility.Visible;
        }
        else
        {
          this.upButton.Visibility = this.SelectedIndex == 0 ? Visibility.Hidden : Visibility.Visible;
          this.downButton.Visibility = this.SelectedIndex == this.Items.Count - 1 ? Visibility.Hidden : Visibility.Visible;
        }
      }
      else
      {
        this.backButton.Visibility = Visibility.Hidden;
        this.forwardButton.Visibility = Visibility.Hidden;
        this.upButton.Visibility = Visibility.Hidden;
        this.downButton.Visibility = Visibility.Hidden;
      }
    }

    private void FlipView_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.backButton == null || this.forwardButton == null)
        this.ApplyTemplate();
      if (this.loaded)
        return;
      this.backButton.Click += new RoutedEventHandler(this.backButton_Click);
      this.forwardButton.Click += new RoutedEventHandler(this.forwardButton_Click);
      this.upButton.Click += new RoutedEventHandler(this.upButton_Click);
      this.downButton.Click += new RoutedEventHandler(this.downButton_Click);
      this.SelectionChanged += new SelectionChangedEventHandler(this.FlipView_SelectionChanged);
      this.PreviewKeyDown += new KeyEventHandler(this.FlipView_PreviewKeyDown);
      this.SelectedIndex = 0;
      this.DetectControlButtonsStatus();
      this.ShowBanner();
      this.loaded = true;
    }

    private void FlipView_Unloaded(object sender, RoutedEventArgs e)
    {
      this.Unloaded -= new RoutedEventHandler(this.FlipView_Unloaded);
      this.MouseLeftButtonDown -= new MouseButtonEventHandler(this.FlipView_MouseLeftButtonDown);
      this.SelectionChanged -= new SelectionChangedEventHandler(this.FlipView_SelectionChanged);
      this.PreviewKeyDown -= new KeyEventHandler(this.FlipView_PreviewKeyDown);
      this.backButton.Click -= new RoutedEventHandler(this.backButton_Click);
      this.forwardButton.Click -= new RoutedEventHandler(this.forwardButton_Click);
      this.upButton.Click -= new RoutedEventHandler(this.upButton_Click);
      this.downButton.Click -= new RoutedEventHandler(this.downButton_Click);
      if (this.hideControlStoryboardCompletedHandler != null)
        this.hideControlStoryboard.Completed -= this.hideControlStoryboardCompletedHandler;
      this.loaded = false;
    }

    private void FlipView_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Left:
          this.GoBack();
          e.Handled = true;
          break;
        case Key.Right:
          this.GoForward();
          e.Handled = true;
          break;
      }
      if (!e.Handled)
        return;
      this.Focus();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.showBannerStoryboard = ((Storyboard) this.Template.Resources[(object) "ShowBannerStoryboard"]).Clone();
      this.hideBannerStoryboard = ((Storyboard) this.Template.Resources[(object) "HideBannerStoryboard"]).Clone();
      this.showControlStoryboard = ((Storyboard) this.Template.Resources[(object) "ShowControlStoryboard"]).Clone();
      this.hideControlStoryboard = ((Storyboard) this.Template.Resources[(object) "HideControlStoryboard"]).Clone();
      this.presenter = this.GetTemplateChild("PART_Presenter") as TransitioningContentControl;
      this.backButton = this.GetTemplateChild("PART_BackButton") as Button;
      this.forwardButton = this.GetTemplateChild("PART_ForwardButton") as Button;
      this.upButton = this.GetTemplateChild("PART_UpButton") as Button;
      this.downButton = this.GetTemplateChild("PART_DownButton") as Button;
      this.bannerGrid = this.GetTemplateChild("PART_BannerGrid") as Grid;
      this.bannerLabel = this.GetTemplateChild("PART_BannerLabel") as Label;
      this.bannerLabel.Opacity = this.IsBannerEnabled ? 1.0 : 0.0;
    }

    protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
    {
      base.OnItemsSourceChanged(oldValue, newValue);
      this.SelectedIndex = 0;
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
      base.OnItemsChanged(e);
      this.DetectControlButtonsStatus();
    }

    private void forwardButton_Click(object sender, RoutedEventArgs e) => this.GoForward();

    private void backButton_Click(object sender, RoutedEventArgs e) => this.GoBack();

    private void downButton_Click(object sender, RoutedEventArgs e) => this.GoForward();

    private void upButton_Click(object sender, RoutedEventArgs e) => this.GoBack();

    public void GoBack()
    {
      if (this.SelectedIndex <= 0)
        return;
      this.presenter.Transition = this.Orientation == Orientation.Horizontal ? this.RightTransition : this.UpTransition;
      --this.SelectedIndex;
    }

    public void GoForward()
    {
      if (this.SelectedIndex >= this.Items.Count - 1)
        return;
      this.presenter.Transition = this.Orientation == Orientation.Horizontal ? this.LeftTransition : this.DownTransition;
      ++this.SelectedIndex;
    }

    public void ShowControlButtons()
    {
      this.controlsVisibilityOverride = false;
      FlipView.ExecuteWhenLoaded(this, (Action) (() =>
      {
        this.backButton.Visibility = Visibility.Visible;
        this.forwardButton.Visibility = Visibility.Visible;
      }));
    }

    public void HideControlButtons()
    {
      this.controlsVisibilityOverride = true;
      FlipView.ExecuteWhenLoaded(this, (Action) (() =>
      {
        this.backButton.Visibility = Visibility.Hidden;
        this.forwardButton.Visibility = Visibility.Hidden;
      }));
    }

    private void ShowBanner()
    {
      if (!this.IsBannerEnabled)
        return;
      this.bannerGrid.BeginStoryboard(this.showBannerStoryboard);
    }

    private void HideBanner()
    {
      if (this.ActualHeight <= 0.0)
        return;
      this.bannerLabel.BeginStoryboard(this.hideControlStoryboard);
      this.bannerGrid.BeginStoryboard(this.hideBannerStoryboard);
    }

    private static void ExecuteWhenLoaded(FlipView flipview, Action body)
    {
      if (flipview.IsLoaded)
      {
        Dispatcher.CurrentDispatcher.Invoke(body);
      }
      else
      {
        RoutedEventHandler handler = (RoutedEventHandler) null;
        handler = (RoutedEventHandler) ((o, a) =>
        {
          flipview.Loaded -= handler;
          Dispatcher.CurrentDispatcher.Invoke(body);
        });
        flipview.Loaded += handler;
      }
    }

    public TransitionType UpTransition
    {
      get => (TransitionType) this.GetValue(FlipView.UpTransitionProperty);
      set => this.SetValue(FlipView.UpTransitionProperty, (object) value);
    }

    public TransitionType DownTransition
    {
      get => (TransitionType) this.GetValue(FlipView.DownTransitionProperty);
      set => this.SetValue(FlipView.DownTransitionProperty, (object) value);
    }

    public TransitionType LeftTransition
    {
      get => (TransitionType) this.GetValue(FlipView.LeftTransitionProperty);
      set => this.SetValue(FlipView.LeftTransitionProperty, (object) value);
    }

    public TransitionType RightTransition
    {
      get => (TransitionType) this.GetValue(FlipView.RightTransitionProperty);
      set => this.SetValue(FlipView.RightTransitionProperty, (object) value);
    }

    public bool MouseOverGlowEnabled
    {
      get => (bool) this.GetValue(FlipView.MouseOverGlowEnabledProperty);
      set => this.SetValue(FlipView.MouseOverGlowEnabledProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(FlipView.OrientationProperty);
      set => this.SetValue(FlipView.OrientationProperty, (object) value);
    }

    public string BannerText
    {
      get => (string) this.GetValue(FlipView.BannerTextProperty);
      set => this.SetValue(FlipView.BannerTextProperty, (object) value);
    }

    private void ChangeBannerText(string value = null)
    {
      if (this.IsBannerEnabled)
      {
        string newValue = value ?? this.BannerText;
        if (newValue == null)
          return;
        if (this.hideControlStoryboardCompletedHandler != null)
          this.hideControlStoryboard.Completed -= this.hideControlStoryboardCompletedHandler;
        this.hideControlStoryboardCompletedHandler = (EventHandler) ((sender, e) =>
        {
          try
          {
            this.hideControlStoryboard.Completed -= this.hideControlStoryboardCompletedHandler;
            this.bannerLabel.Content = (object) newValue;
            this.bannerLabel.BeginStoryboard(this.showControlStoryboard, HandoffBehavior.SnapshotAndReplace);
          }
          catch (Exception ex)
          {
          }
        });
        this.hideControlStoryboard.Completed += this.hideControlStoryboardCompletedHandler;
        this.bannerLabel.BeginStoryboard(this.hideControlStoryboard, HandoffBehavior.SnapshotAndReplace);
      }
      else
        FlipView.ExecuteWhenLoaded(this, (Action) (() => this.bannerLabel.Content = (object) (value ?? this.BannerText)));
    }

    public bool IsBannerEnabled
    {
      get => (bool) this.GetValue(FlipView.IsBannerEnabledProperty);
      set => this.SetValue(FlipView.IsBannerEnabledProperty, (object) value);
    }

    private static void OnIsBannerEnabledPropertyChangedCallback(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FlipView flipview = (FlipView) d;
      if (!flipview.IsLoaded)
        FlipView.ExecuteWhenLoaded(flipview, (Action) (() =>
        {
          flipview.ApplyTemplate();
          if ((bool) e.NewValue)
          {
            flipview.ChangeBannerText(flipview.BannerText);
            flipview.ShowBanner();
          }
          else
            flipview.HideBanner();
        }));
      else if ((bool) e.NewValue)
      {
        flipview.ChangeBannerText(flipview.BannerText);
        flipview.ShowBanner();
      }
      else
        flipview.HideBanner();
    }
  }
}
