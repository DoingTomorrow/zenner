// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.BaseMetroDialog
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public abstract class BaseMetroDialog : ContentControl
  {
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (BaseMetroDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty DialogTopProperty = DependencyProperty.Register(nameof (DialogTop), typeof (object), typeof (BaseMetroDialog), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty DialogBottomProperty = DependencyProperty.Register(nameof (DialogBottom), typeof (object), typeof (BaseMetroDialog), new PropertyMetadata((PropertyChangedCallback) null));

    public MetroDialogSettings DialogSettings { get; private set; }

    public string Title
    {
      get => (string) this.GetValue(BaseMetroDialog.TitleProperty);
      set => this.SetValue(BaseMetroDialog.TitleProperty, (object) value);
    }

    public object DialogTop
    {
      get => this.GetValue(BaseMetroDialog.DialogTopProperty);
      set => this.SetValue(BaseMetroDialog.DialogTopProperty, value);
    }

    public object DialogBottom
    {
      get => this.GetValue(BaseMetroDialog.DialogBottomProperty);
      set => this.SetValue(BaseMetroDialog.DialogBottomProperty, value);
    }

    internal SizeChangedEventHandler SizeChangedHandler { get; set; }

    static BaseMetroDialog()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (BaseMetroDialog), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (BaseMetroDialog)));
    }

    protected BaseMetroDialog(MetroWindow owningWindow, MetroDialogSettings settings)
    {
      this.DialogSettings = settings ?? owningWindow.MetroDialogOptions;
      this.OwningWindow = owningWindow;
      this.Initialize();
    }

    protected BaseMetroDialog()
    {
      this.DialogSettings = new MetroDialogSettings();
      this.Initialize();
    }

    private void Initialize()
    {
      if (this.DialogSettings != null && !this.DialogSettings.SuppressDefaultResources)
        this.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml")
        });
      this.Resources.MergedDictionaries.Add(new ResourceDictionary()
      {
        Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml")
      });
      this.Resources.MergedDictionaries.Add(new ResourceDictionary()
      {
        Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml")
      });
      this.Resources.MergedDictionaries.Add(new ResourceDictionary()
      {
        Source = new Uri("pack://application:,,,/MahApps.Metro;component/Themes/Dialogs/BaseMetroDialog.xaml")
      });
      if (this.DialogSettings != null && this.DialogSettings.CustomResourceDictionary != null)
        this.Resources.MergedDictionaries.Add(this.DialogSettings.CustomResourceDictionary);
      this.Loaded += (RoutedEventHandler) ((sender, args) =>
      {
        this.OnLoaded();
        this.HandleTheme();
      });
      ThemeManager.IsThemeChanged += new EventHandler<OnThemeChangedEventArgs>(this.ThemeManager_IsThemeChanged);
      this.Unloaded += new RoutedEventHandler(this.BaseMetroDialog_Unloaded);
    }

    private void BaseMetroDialog_Unloaded(object sender, RoutedEventArgs e)
    {
      ThemeManager.IsThemeChanged -= new EventHandler<OnThemeChangedEventArgs>(this.ThemeManager_IsThemeChanged);
      this.Unloaded -= new RoutedEventHandler(this.BaseMetroDialog_Unloaded);
    }

    private void ThemeManager_IsThemeChanged(object sender, OnThemeChangedEventArgs e)
    {
      this.HandleTheme();
    }

    private void HandleTheme()
    {
      if (this.DialogSettings != null)
      {
        Tuple<AppTheme, Accent> tuple = BaseMetroDialog.DetectTheme(this);
        if (DesignerProperties.GetIsInDesignMode((DependencyObject) this) || tuple == null)
          return;
        AppTheme appTheme = tuple.Item1;
        Accent newAccent = tuple.Item2;
        switch (this.DialogSettings.ColorScheme)
        {
          case MetroDialogColorScheme.Theme:
            ThemeManager.ChangeAppStyle(this.Resources, newAccent, appTheme);
            this.SetValue(Control.BackgroundProperty, ThemeManager.GetResourceFromAppStyle((Window) this.OwningWindow ?? Application.Current.MainWindow, "WhiteColorBrush"));
            this.SetValue(Control.ForegroundProperty, ThemeManager.GetResourceFromAppStyle((Window) this.OwningWindow ?? Application.Current.MainWindow, "BlackBrush"));
            break;
          case MetroDialogColorScheme.Accented:
            ThemeManager.ChangeAppStyle(this.Resources, newAccent, appTheme);
            this.SetValue(Control.BackgroundProperty, ThemeManager.GetResourceFromAppStyle((Window) this.OwningWindow ?? Application.Current.MainWindow, "HighlightBrush"));
            this.SetValue(Control.ForegroundProperty, ThemeManager.GetResourceFromAppStyle((Window) this.OwningWindow ?? Application.Current.MainWindow, "IdealForegroundColorBrush"));
            break;
          case MetroDialogColorScheme.Inverted:
            ThemeManager.ChangeAppStyle(this.Resources, newAccent, ThemeManager.GetInverseAppTheme(appTheme) ?? throw new InvalidOperationException("The inverse dialog theme only works if the window theme abides the naming convention. See ThemeManager.GetInverseAppTheme for more infos"));
            this.SetValue(Control.BackgroundProperty, ThemeManager.GetResourceFromAppStyle((Window) this.OwningWindow ?? Application.Current.MainWindow, "BlackColorBrush"));
            this.SetValue(Control.ForegroundProperty, ThemeManager.GetResourceFromAppStyle((Window) this.OwningWindow ?? Application.Current.MainWindow, "WhiteColorBrush"));
            break;
        }
      }
      if (this.ParentDialogWindow == null)
        return;
      this.ParentDialogWindow.SetValue(Control.BackgroundProperty, (object) this.Background);
      object resourceFromAppStyle = ThemeManager.GetResourceFromAppStyle((Window) this.OwningWindow ?? Application.Current.MainWindow, "AccentColorBrush");
      if (resourceFromAppStyle == null)
        return;
      this.ParentDialogWindow.SetValue(MetroWindow.GlowBrushProperty, resourceFromAppStyle);
    }

    protected virtual void OnLoaded()
    {
    }

    private static Tuple<AppTheme, Accent> DetectTheme(BaseMetroDialog dialog)
    {
      if (dialog == null)
        return (Tuple<AppTheme, Accent>) null;
      MetroWindow parent = dialog.TryFindParent<MetroWindow>();
      Tuple<AppTheme, Accent> tuple1 = parent != null ? ThemeManager.DetectAppStyle((Window) parent) : (Tuple<AppTheme, Accent>) null;
      if (tuple1 != null && tuple1.Item2 != null)
        return tuple1;
      if (Application.Current != null)
      {
        Tuple<AppTheme, Accent> tuple2 = Application.Current.MainWindow is MetroWindow mainWindow ? ThemeManager.DetectAppStyle((Window) mainWindow) : (Tuple<AppTheme, Accent>) null;
        if (tuple2 != null && tuple2.Item2 != null)
          return tuple2;
        Tuple<AppTheme, Accent> tuple3 = ThemeManager.DetectAppStyle(Application.Current);
        if (tuple3 != null && tuple3.Item2 != null)
          return tuple3;
      }
      return (Tuple<AppTheme, Accent>) null;
    }

    public Task WaitForLoadAsync()
    {
      this.Dispatcher.VerifyAccess();
      if (this.IsLoaded)
        return new Task((Action) (() => { }));
      if (!this.DialogSettings.AnimateShow)
        this.Opacity = 1.0;
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      RoutedEventHandler handler = (RoutedEventHandler) null;
      handler = (RoutedEventHandler) ((sender, args) =>
      {
        this.Loaded -= handler;
        this.Focus();
        tcs.TrySetResult((object) null);
      });
      this.Loaded += handler;
      return (Task) tcs.Task;
    }

    public Task RequestCloseAsync()
    {
      if (!this.OnRequestClose())
        return Task.Factory.StartNew((Action) (() => { }));
      return this.ParentDialogWindow == null ? this.OwningWindow.HideMetroDialogAsync(this) : this._WaitForCloseAsync().ContinueWith((Action<Task>) (x => this.ParentDialogWindow.Dispatcher.Invoke((Action) (() => this.ParentDialogWindow.Close()))));
    }

    protected internal virtual void OnShown()
    {
    }

    protected internal virtual void OnClose()
    {
      if (this.ParentDialogWindow == null)
        return;
      this.ParentDialogWindow.Close();
    }

    protected internal virtual bool OnRequestClose() => true;

    protected internal Window ParentDialogWindow { get; internal set; }

    protected internal MetroWindow OwningWindow { get; internal set; }

    public Task WaitUntilUnloadedAsync()
    {
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      this.Unloaded += (RoutedEventHandler) ((s, e) => tcs.TrySetResult((object) null));
      return (Task) tcs.Task;
    }

    public Task _WaitForCloseAsync()
    {
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      if (this.DialogSettings.AnimateHide)
      {
        Storyboard closingStoryboard = this.Resources[(object) "DialogCloseStoryboard"] as Storyboard;
        if (closingStoryboard == null)
          throw new InvalidOperationException("Unable to find the dialog closing storyboard. Did you forget to add BaseMetroDialog.xaml to your merged dictionaries?");
        EventHandler handler = (EventHandler) null;
        handler = (EventHandler) ((sender, args) =>
        {
          closingStoryboard.Completed -= handler;
          tcs.TrySetResult((object) null);
        });
        closingStoryboard = closingStoryboard.Clone();
        closingStoryboard.Completed += handler;
        closingStoryboard.Begin((FrameworkElement) this);
      }
      else
      {
        this.Opacity = 0.0;
        tcs.TrySetResult((object) null);
      }
      return (Task) tcs.Task;
    }
  }
}
