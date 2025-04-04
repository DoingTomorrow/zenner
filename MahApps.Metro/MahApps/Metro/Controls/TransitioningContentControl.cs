// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.TransitioningContentControl
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class TransitioningContentControl : ContentControl
  {
    internal const string PresentationGroup = "PresentationStates";
    internal const string NormalState = "Normal";
    internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";
    internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";
    private bool _allowIsTransitioningWrite;
    private Storyboard _currentTransition;
    public const TransitionType DefaultTransitionState = TransitionType.Default;
    public static readonly DependencyProperty IsTransitioningProperty = DependencyProperty.Register(nameof (IsTransitioning), typeof (bool), typeof (TransitioningContentControl), new PropertyMetadata(new PropertyChangedCallback(TransitioningContentControl.OnIsTransitioningPropertyChanged)));
    public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register(nameof (Transition), typeof (TransitionType), typeof (TransitioningContentControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) TransitionType.Default, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(TransitioningContentControl.OnTransitionPropertyChanged)));
    public static readonly DependencyProperty RestartTransitionOnContentChangeProperty = DependencyProperty.Register(nameof (RestartTransitionOnContentChange), typeof (bool), typeof (TransitioningContentControl), new PropertyMetadata((object) false, new PropertyChangedCallback(TransitioningContentControl.OnRestartTransitionOnContentChangePropertyChanged)));
    public static readonly DependencyProperty CustomVisualStatesProperty = DependencyProperty.Register(nameof (CustomVisualStates), typeof (ObservableCollection<VisualState>), typeof (TransitioningContentControl), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty CustomVisualStatesNameProperty = DependencyProperty.Register(nameof (CustomVisualStatesName), typeof (string), typeof (TransitioningContentControl), new PropertyMetadata((object) "CustomTransition"));

    private ContentPresenter CurrentContentPresentationSite { get; set; }

    private ContentPresenter PreviousContentPresentationSite { get; set; }

    public event RoutedEventHandler TransitionCompleted;

    public ObservableCollection<VisualState> CustomVisualStates
    {
      get
      {
        return (ObservableCollection<VisualState>) this.GetValue(TransitioningContentControl.CustomVisualStatesProperty);
      }
      set => this.SetValue(TransitioningContentControl.CustomVisualStatesProperty, (object) value);
    }

    public string CustomVisualStatesName
    {
      get => (string) this.GetValue(TransitioningContentControl.CustomVisualStatesNameProperty);
      set
      {
        this.SetValue(TransitioningContentControl.CustomVisualStatesNameProperty, (object) value);
      }
    }

    public bool IsTransitioning
    {
      get => (bool) this.GetValue(TransitioningContentControl.IsTransitioningProperty);
      private set
      {
        this._allowIsTransitioningWrite = true;
        this.SetValue(TransitioningContentControl.IsTransitioningProperty, (object) value);
        this._allowIsTransitioningWrite = false;
      }
    }

    public TransitionType Transition
    {
      get => (TransitionType) this.GetValue(TransitioningContentControl.TransitionProperty);
      set => this.SetValue(TransitioningContentControl.TransitionProperty, (object) value);
    }

    public bool RestartTransitionOnContentChange
    {
      get
      {
        return (bool) this.GetValue(TransitioningContentControl.RestartTransitionOnContentChangeProperty);
      }
      set
      {
        this.SetValue(TransitioningContentControl.RestartTransitionOnContentChangeProperty, (object) value);
      }
    }

    private static void OnIsTransitioningPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TransitioningContentControl transitioningContentControl = (TransitioningContentControl) d;
      if (!transitioningContentControl._allowIsTransitioningWrite)
      {
        transitioningContentControl.IsTransitioning = (bool) e.OldValue;
        throw new InvalidOperationException();
      }
    }

    private Storyboard CurrentTransition
    {
      get => this._currentTransition;
      set
      {
        if (this._currentTransition != null)
          this._currentTransition.Completed -= new EventHandler(this.OnTransitionCompleted);
        this._currentTransition = value;
        if (this._currentTransition == null)
          return;
        this._currentTransition.Completed += new EventHandler(this.OnTransitionCompleted);
      }
    }

    private static void OnTransitionPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TransitioningContentControl transitioningContentControl = (TransitioningContentControl) d;
      TransitionType oldValue = (TransitionType) e.OldValue;
      TransitionType newValue = (TransitionType) e.NewValue;
      if (transitioningContentControl.IsTransitioning)
        transitioningContentControl.AbortTransition();
      Storyboard storyboard = transitioningContentControl.GetStoryboard(newValue);
      if (storyboard == null)
      {
        if (VisualStates.TryGetVisualStateGroup((DependencyObject) transitioningContentControl, "PresentationStates") == null)
        {
          transitioningContentControl.CurrentTransition = (Storyboard) null;
        }
        else
        {
          transitioningContentControl.SetValue(TransitioningContentControl.TransitionProperty, (object) oldValue);
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "Temporary removed exception message", new object[1]
          {
            (object) newValue
          }));
        }
      }
      else
        transitioningContentControl.CurrentTransition = storyboard;
    }

    private static void OnRestartTransitionOnContentChangePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((TransitioningContentControl) d).OnRestartTransitionOnContentChangeChanged((bool) e.OldValue, (bool) e.NewValue);
    }

    protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
    {
    }

    public TransitioningContentControl()
    {
      this.CustomVisualStates = new ObservableCollection<VisualState>();
      this.DefaultStyleKey = (object) typeof (TransitioningContentControl);
    }

    public override void OnApplyTemplate()
    {
      if (this.IsTransitioning)
        this.AbortTransition();
      if (this.CustomVisualStates != null && this.CustomVisualStates.Any<VisualState>())
      {
        VisualStateGroup visualStateGroup = VisualStates.TryGetVisualStateGroup((DependencyObject) this, "PresentationStates");
        if (visualStateGroup != null)
        {
          foreach (VisualState customVisualState in (Collection<VisualState>) this.CustomVisualStates)
            visualStateGroup.States.Add((object) customVisualState);
        }
      }
      base.OnApplyTemplate();
      this.PreviousContentPresentationSite = this.GetTemplateChild("PreviousContentPresentationSite") as ContentPresenter;
      this.CurrentContentPresentationSite = this.GetTemplateChild("CurrentContentPresentationSite") as ContentPresenter;
      if (this.CurrentContentPresentationSite != null)
      {
        if (this.ContentTemplateSelector != null)
          this.CurrentContentPresentationSite.ContentTemplate = this.ContentTemplateSelector.SelectTemplate(this.Content, (DependencyObject) this);
        this.CurrentContentPresentationSite.Content = this.Content;
      }
      Storyboard storyboard = this.GetStoryboard(this.Transition);
      this.CurrentTransition = storyboard;
      if (storyboard == null)
      {
        TransitionType transition = this.Transition;
        this.Transition = TransitionType.Default;
        throw new ArgumentException(string.Format("'{0}' Transition could not be found!", (object) transition), "Transition");
      }
      VisualStateManager.GoToState((FrameworkElement) this, "Normal", false);
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
      base.OnContentChanged(oldContent, newContent);
      this.StartTransition(oldContent, newContent);
    }

    private void StartTransition(object oldContent, object newContent)
    {
      if (this.CurrentContentPresentationSite == null || this.PreviousContentPresentationSite == null)
        return;
      if (this.RestartTransitionOnContentChange)
        this.CurrentTransition.Completed -= new EventHandler(this.OnTransitionCompleted);
      if (this.ContentTemplateSelector != null)
      {
        this.PreviousContentPresentationSite.ContentTemplate = this.ContentTemplateSelector.SelectTemplate(oldContent, (DependencyObject) this);
        this.CurrentContentPresentationSite.ContentTemplate = this.ContentTemplateSelector.SelectTemplate(newContent, (DependencyObject) this);
      }
      this.CurrentContentPresentationSite.Content = newContent;
      this.PreviousContentPresentationSite.Content = oldContent;
      if (this.IsTransitioning && !this.RestartTransitionOnContentChange)
        return;
      if (this.RestartTransitionOnContentChange)
        this.CurrentTransition.Completed += new EventHandler(this.OnTransitionCompleted);
      this.IsTransitioning = true;
      VisualStateManager.GoToState((FrameworkElement) this, "Normal", false);
      VisualStateManager.GoToState((FrameworkElement) this, this.GetTransitionName(this.Transition), true);
    }

    public void ReloadTransition()
    {
      if (this.CurrentContentPresentationSite == null || this.PreviousContentPresentationSite == null)
        return;
      if (this.RestartTransitionOnContentChange)
        this.CurrentTransition.Completed -= new EventHandler(this.OnTransitionCompleted);
      if (this.IsTransitioning && !this.RestartTransitionOnContentChange)
        return;
      if (this.RestartTransitionOnContentChange)
        this.CurrentTransition.Completed += new EventHandler(this.OnTransitionCompleted);
      this.IsTransitioning = true;
      VisualStateManager.GoToState((FrameworkElement) this, "Normal", false);
      VisualStateManager.GoToState((FrameworkElement) this, this.GetTransitionName(this.Transition), true);
    }

    private void OnTransitionCompleted(object sender, EventArgs e)
    {
      this.AbortTransition();
      RoutedEventHandler transitionCompleted = this.TransitionCompleted;
      if (transitionCompleted == null)
        return;
      transitionCompleted((object) this, new RoutedEventArgs());
    }

    public void AbortTransition()
    {
      VisualStateManager.GoToState((FrameworkElement) this, "Normal", false);
      this.IsTransitioning = false;
      if (this.PreviousContentPresentationSite == null)
        return;
      this.PreviousContentPresentationSite.Content = (object) null;
    }

    private Storyboard GetStoryboard(TransitionType newTransition)
    {
      VisualStateGroup visualStateGroup = VisualStates.TryGetVisualStateGroup((DependencyObject) this, "PresentationStates");
      Storyboard storyboard = (Storyboard) null;
      if (visualStateGroup != null)
      {
        string transitionName = this.GetTransitionName(newTransition);
        storyboard = visualStateGroup.States.OfType<VisualState>().Where<VisualState>((Func<VisualState, bool>) (state => state.Name == transitionName)).Select<VisualState, Storyboard>((Func<VisualState, Storyboard>) (state => state.Storyboard)).FirstOrDefault<Storyboard>();
      }
      return storyboard;
    }

    private string GetTransitionName(TransitionType transition)
    {
      switch (transition)
      {
        case TransitionType.Normal:
          return "Normal";
        case TransitionType.Up:
          return "UpTransition";
        case TransitionType.Down:
          return "DownTransition";
        case TransitionType.Right:
          return "RightTransition";
        case TransitionType.RightReplace:
          return "RightReplaceTransition";
        case TransitionType.Left:
          return "LeftTransition";
        case TransitionType.LeftReplace:
          return "LeftReplaceTransition";
        case TransitionType.Custom:
          return this.CustomVisualStatesName;
        default:
          return "DefaultTransition";
      }
    }
  }
}
