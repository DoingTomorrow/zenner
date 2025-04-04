// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.MetroNavigationWindow
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

#nullable disable
namespace MahApps.Metro.Controls
{
  [System.Windows.Markup.ContentProperty("OverlayContent")]
  public class MetroNavigationWindow : MetroWindow, IUriContext, IComponentConnector
  {
    public static readonly DependencyProperty OverlayContentProperty = DependencyProperty.Register(nameof (OverlayContent), typeof (object), typeof (MetroNavigationWindow));
    public static readonly DependencyProperty PageContentProperty = DependencyProperty.Register(nameof (PageContent), typeof (object), typeof (MetroNavigationWindow));
    internal Button PART_BackButton;
    internal Button PART_ForwardButton;
    internal Label PART_Title;
    internal System.Windows.Controls.Frame PART_Frame;
    private bool _contentLoaded;

    public MetroNavigationWindow()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.MetroNavigationWindow_Loaded);
      this.Closing += new CancelEventHandler(this.MetroNavigationWindow_Closing);
    }

    private void MetroNavigationWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.PART_Frame.Navigated += new NavigatedEventHandler(this.PART_Frame_Navigated);
      this.PART_Frame.Navigating += new NavigatingCancelEventHandler(this.PART_Frame_Navigating);
      this.PART_Frame.NavigationFailed += new NavigationFailedEventHandler(this.PART_Frame_NavigationFailed);
      this.PART_Frame.NavigationProgress += new NavigationProgressEventHandler(this.PART_Frame_NavigationProgress);
      this.PART_Frame.NavigationStopped += new NavigationStoppedEventHandler(this.PART_Frame_NavigationStopped);
      this.PART_Frame.LoadCompleted += new LoadCompletedEventHandler(this.PART_Frame_LoadCompleted);
      this.PART_Frame.FragmentNavigation += new FragmentNavigationEventHandler(this.PART_Frame_FragmentNavigation);
      this.PART_BackButton.Click += new RoutedEventHandler(this.PART_BackButton_Click);
      this.PART_ForwardButton.Click += new RoutedEventHandler(this.PART_ForwardButton_Click);
    }

    [DebuggerNonUserCode]
    private void PART_ForwardButton_Click(object sender, RoutedEventArgs e)
    {
      if (!this.CanGoForward)
        return;
      this.GoForward();
    }

    [DebuggerNonUserCode]
    private void PART_Frame_FragmentNavigation(object sender, FragmentNavigationEventArgs e)
    {
      if (this.FragmentNavigation == null)
        return;
      this.FragmentNavigation((object) this, e);
    }

    [DebuggerNonUserCode]
    private void PART_Frame_LoadCompleted(object sender, NavigationEventArgs e)
    {
      if (this.LoadCompleted == null)
        return;
      this.LoadCompleted((object) this, e);
    }

    [DebuggerNonUserCode]
    private void PART_Frame_NavigationStopped(object sender, NavigationEventArgs e)
    {
      if (this.NavigationStopped == null)
        return;
      this.NavigationStopped((object) this, e);
    }

    [DebuggerNonUserCode]
    private void PART_Frame_NavigationProgress(object sender, NavigationProgressEventArgs e)
    {
      if (this.NavigationProgress == null)
        return;
      this.NavigationProgress((object) this, e);
    }

    [DebuggerNonUserCode]
    private void PART_Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
      if (this.NavigationFailed == null)
        return;
      this.NavigationFailed((object) this, e);
    }

    [DebuggerNonUserCode]
    private void PART_Frame_Navigating(object sender, NavigatingCancelEventArgs e)
    {
      if (this.Navigating == null)
        return;
      this.Navigating((object) this, e);
    }

    [DebuggerNonUserCode]
    private void PART_BackButton_Click(object sender, RoutedEventArgs e)
    {
      if (!this.CanGoBack)
        return;
      this.GoBack();
    }

    [DebuggerNonUserCode]
    private void MetroNavigationWindow_Closing(object sender, CancelEventArgs e)
    {
      this.PART_Frame.FragmentNavigation -= new FragmentNavigationEventHandler(this.PART_Frame_FragmentNavigation);
      this.PART_Frame.Navigating -= new NavigatingCancelEventHandler(this.PART_Frame_Navigating);
      this.PART_Frame.NavigationFailed -= new NavigationFailedEventHandler(this.PART_Frame_NavigationFailed);
      this.PART_Frame.NavigationProgress -= new NavigationProgressEventHandler(this.PART_Frame_NavigationProgress);
      this.PART_Frame.NavigationStopped -= new NavigationStoppedEventHandler(this.PART_Frame_NavigationStopped);
      this.PART_Frame.LoadCompleted -= new LoadCompletedEventHandler(this.PART_Frame_LoadCompleted);
      this.PART_Frame.Navigated -= new NavigatedEventHandler(this.PART_Frame_Navigated);
      this.PART_ForwardButton.Click -= new RoutedEventHandler(this.PART_ForwardButton_Click);
      this.PART_BackButton.Click -= new RoutedEventHandler(this.PART_BackButton_Click);
      this.Loaded -= new RoutedEventHandler(this.MetroNavigationWindow_Loaded);
      this.Closing -= new CancelEventHandler(this.MetroNavigationWindow_Closing);
    }

    [DebuggerNonUserCode]
    private void PART_Frame_Navigated(object sender, NavigationEventArgs e)
    {
      this.PART_Title.Content = (object) ((Page) this.PART_Frame.Content).Title;
      ((IUriContext) this).BaseUri = e.Uri;
      this.PageContent = this.PART_Frame.Content;
      this.PART_BackButton.IsEnabled = this.CanGoBack;
      this.PART_ForwardButton.IsEnabled = this.CanGoForward;
      if (this.Navigated == null)
        return;
      this.Navigated((object) this, e);
    }

    public object OverlayContent
    {
      get => this.GetValue(MetroNavigationWindow.OverlayContentProperty);
      set => this.SetValue(MetroNavigationWindow.OverlayContentProperty, value);
    }

    public object PageContent
    {
      get => this.GetValue(MetroNavigationWindow.PageContentProperty);
      private set => this.SetValue(MetroNavigationWindow.PageContentProperty, value);
    }

    public IEnumerable ForwardStack => this.PART_Frame.ForwardStack;

    public IEnumerable BackStack => this.PART_Frame.BackStack;

    public NavigationService NavigationService => this.PART_Frame.NavigationService;

    public bool CanGoBack => this.PART_Frame.CanGoBack;

    public bool CanGoForward => this.PART_Frame.CanGoForward;

    Uri IUriContext.BaseUri { get; set; }

    public Uri Source
    {
      get => this.PART_Frame.Source;
      set => this.PART_Frame.Source = value;
    }

    [DebuggerNonUserCode]
    public void AddBackEntry(CustomContentState state) => this.PART_Frame.AddBackEntry(state);

    [DebuggerNonUserCode]
    public JournalEntry RemoveBackEntry() => this.PART_Frame.RemoveBackEntry();

    [DebuggerNonUserCode]
    public void GoBack() => this.PART_Frame.GoBack();

    [DebuggerNonUserCode]
    public void GoForward() => this.PART_Frame.GoForward();

    [DebuggerNonUserCode]
    public bool Navigate(object content) => this.PART_Frame.Navigate(content);

    [DebuggerNonUserCode]
    public bool Navigate(Uri source) => this.PART_Frame.Navigate(source);

    [DebuggerNonUserCode]
    public bool Navigate(object content, object extraData)
    {
      return this.PART_Frame.Navigate(content, extraData);
    }

    [DebuggerNonUserCode]
    public bool Navigate(Uri source, object extraData)
    {
      return this.PART_Frame.Navigate(source, extraData);
    }

    [DebuggerNonUserCode]
    public void StopLoading() => this.PART_Frame.StopLoading();

    public event FragmentNavigationEventHandler FragmentNavigation;

    public event NavigatingCancelEventHandler Navigating;

    public event NavigationFailedEventHandler NavigationFailed;

    public event NavigationProgressEventHandler NavigationProgress;

    public event NavigationStoppedEventHandler NavigationStopped;

    public event NavigatedEventHandler Navigated;

    public event LoadCompletedEventHandler LoadCompleted;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MahApps.Metro;component/themes/metronavigationwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.PART_BackButton = (Button) target;
          break;
        case 2:
          this.PART_ForwardButton = (Button) target;
          break;
        case 3:
          this.PART_Title = (Label) target;
          break;
        case 4:
          this.PART_Frame = (System.Windows.Controls.Frame) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
