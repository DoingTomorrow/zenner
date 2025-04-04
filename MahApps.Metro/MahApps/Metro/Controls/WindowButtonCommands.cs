// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.WindowButtonCommands
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Native;
using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplatePart(Name = "PART_Min", Type = typeof (Button))]
  [TemplatePart(Name = "PART_Max", Type = typeof (Button))]
  [TemplatePart(Name = "PART_Close", Type = typeof (Button))]
  public class WindowButtonCommands : ContentControl, INotifyPropertyChanged
  {
    public static readonly DependencyProperty LightMinButtonStyleProperty = DependencyProperty.Register(nameof (LightMinButtonStyle), typeof (Style), typeof (WindowButtonCommands), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
    public static readonly DependencyProperty LightMaxButtonStyleProperty = DependencyProperty.Register(nameof (LightMaxButtonStyle), typeof (Style), typeof (WindowButtonCommands), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
    public static readonly DependencyProperty LightCloseButtonStyleProperty = DependencyProperty.Register(nameof (LightCloseButtonStyle), typeof (Style), typeof (WindowButtonCommands), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
    public static readonly DependencyProperty DarkMinButtonStyleProperty = DependencyProperty.Register(nameof (DarkMinButtonStyle), typeof (Style), typeof (WindowButtonCommands), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
    public static readonly DependencyProperty DarkMaxButtonStyleProperty = DependencyProperty.Register(nameof (DarkMaxButtonStyle), typeof (Style), typeof (WindowButtonCommands), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
    public static readonly DependencyProperty DarkCloseButtonStyleProperty = DependencyProperty.Register(nameof (DarkCloseButtonStyle), typeof (Style), typeof (WindowButtonCommands), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(nameof (Theme), typeof (Theme), typeof (WindowButtonCommands), new PropertyMetadata((object) Theme.Light, new PropertyChangedCallback(WindowButtonCommands.OnThemeChanged)));
    private static string minimize;
    private static string maximize;
    private static string closeText;
    private static string restore;
    private Button min;
    private Button max;
    private Button close;
    private SafeLibraryHandle user32;
    private MetroWindow _parentWindow;

    public event WindowButtonCommands.ClosingWindowEventHandler ClosingWindow;

    public Style LightMinButtonStyle
    {
      get => (Style) this.GetValue(WindowButtonCommands.LightMinButtonStyleProperty);
      set => this.SetValue(WindowButtonCommands.LightMinButtonStyleProperty, (object) value);
    }

    public Style LightMaxButtonStyle
    {
      get => (Style) this.GetValue(WindowButtonCommands.LightMaxButtonStyleProperty);
      set => this.SetValue(WindowButtonCommands.LightMaxButtonStyleProperty, (object) value);
    }

    public Style LightCloseButtonStyle
    {
      get => (Style) this.GetValue(WindowButtonCommands.LightCloseButtonStyleProperty);
      set => this.SetValue(WindowButtonCommands.LightCloseButtonStyleProperty, (object) value);
    }

    public Style DarkMinButtonStyle
    {
      get => (Style) this.GetValue(WindowButtonCommands.DarkMinButtonStyleProperty);
      set => this.SetValue(WindowButtonCommands.DarkMinButtonStyleProperty, (object) value);
    }

    public Style DarkMaxButtonStyle
    {
      get => (Style) this.GetValue(WindowButtonCommands.DarkMaxButtonStyleProperty);
      set => this.SetValue(WindowButtonCommands.DarkMaxButtonStyleProperty, (object) value);
    }

    public Style DarkCloseButtonStyle
    {
      get => (Style) this.GetValue(WindowButtonCommands.DarkCloseButtonStyleProperty);
      set => this.SetValue(WindowButtonCommands.DarkCloseButtonStyleProperty, (object) value);
    }

    public Theme Theme
    {
      get => (Theme) this.GetValue(WindowButtonCommands.ThemeProperty);
      set => this.SetValue(WindowButtonCommands.ThemeProperty, (object) value);
    }

    private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      ((WindowButtonCommands) d).ApplyTheme();
    }

    public string Minimize
    {
      get
      {
        if (string.IsNullOrEmpty(WindowButtonCommands.minimize))
          WindowButtonCommands.minimize = this.GetCaption(900);
        return WindowButtonCommands.minimize;
      }
    }

    public string Maximize
    {
      get
      {
        if (string.IsNullOrEmpty(WindowButtonCommands.maximize))
          WindowButtonCommands.maximize = this.GetCaption(901);
        return WindowButtonCommands.maximize;
      }
    }

    public string Close
    {
      get
      {
        if (string.IsNullOrEmpty(WindowButtonCommands.closeText))
          WindowButtonCommands.closeText = this.GetCaption(905);
        return WindowButtonCommands.closeText;
      }
    }

    public string Restore
    {
      get
      {
        if (string.IsNullOrEmpty(WindowButtonCommands.restore))
          WindowButtonCommands.restore = this.GetCaption(903);
        return WindowButtonCommands.restore;
      }
    }

    static WindowButtonCommands()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (WindowButtonCommands), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (WindowButtonCommands)));
    }

    public WindowButtonCommands()
    {
      this.Loaded += new RoutedEventHandler(this.WindowButtonCommands_Loaded);
    }

    private void WindowButtonCommands_Loaded(object sender, RoutedEventArgs e)
    {
      this.Loaded -= new RoutedEventHandler(this.WindowButtonCommands_Loaded);
      if (this.ParentWindow != null)
        return;
      this.ParentWindow = this.TryFindParent<MetroWindow>();
    }

    private string GetCaption(int id)
    {
      if (this.user32 == null)
        this.user32 = UnsafeNativeMethods.LoadLibrary(Environment.SystemDirectory + "\\User32.dll");
      StringBuilder lpBuffer = new StringBuilder(256);
      UnsafeNativeMethods.LoadString(this.user32, (uint) id, lpBuffer, lpBuffer.Capacity);
      return lpBuffer.ToString().Replace("&", "");
    }

    public void ApplyTheme()
    {
      if (this.close != null)
      {
        if (this.ParentWindow != null && this.ParentWindow.WindowCloseButtonStyle != null)
          this.close.Style = this.ParentWindow.WindowCloseButtonStyle;
        else
          this.close.Style = this.Theme == Theme.Light ? this.LightCloseButtonStyle : this.DarkCloseButtonStyle;
      }
      if (this.max != null)
      {
        if (this.ParentWindow != null && this.ParentWindow.WindowMaxButtonStyle != null)
          this.max.Style = this.ParentWindow.WindowMaxButtonStyle;
        else
          this.max.Style = this.Theme == Theme.Light ? this.LightMaxButtonStyle : this.DarkMaxButtonStyle;
      }
      if (this.min == null)
        return;
      if (this.ParentWindow != null && this.ParentWindow.WindowMinButtonStyle != null)
        this.min.Style = this.ParentWindow.WindowMinButtonStyle;
      else
        this.min.Style = this.Theme == Theme.Light ? this.LightMinButtonStyle : this.DarkMinButtonStyle;
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.close = this.Template.FindName("PART_Close", (FrameworkElement) this) as Button;
      if (this.close != null)
        this.close.Click += new RoutedEventHandler(this.CloseClick);
      this.max = this.Template.FindName("PART_Max", (FrameworkElement) this) as Button;
      if (this.max != null)
        this.max.Click += new RoutedEventHandler(this.MaximizeClick);
      this.min = this.Template.FindName("PART_Min", (FrameworkElement) this) as Button;
      if (this.min != null)
        this.min.Click += new RoutedEventHandler(this.MinimizeClick);
      this.ApplyTheme();
    }

    protected void OnClosingWindow(ClosingWindowEventHandlerArgs args)
    {
      WindowButtonCommands.ClosingWindowEventHandler closingWindow = this.ClosingWindow;
      if (closingWindow == null)
        return;
      closingWindow((object) this, args);
    }

    private void MinimizeClick(object sender, RoutedEventArgs e)
    {
      if (this.ParentWindow == null)
        return;
      Microsoft.Windows.Shell.SystemCommands.MinimizeWindow((Window) this.ParentWindow);
    }

    private void MaximizeClick(object sender, RoutedEventArgs e)
    {
      if (this.ParentWindow == null)
        return;
      if (this.ParentWindow.WindowState == WindowState.Maximized)
        Microsoft.Windows.Shell.SystemCommands.RestoreWindow((Window) this.ParentWindow);
      else
        Microsoft.Windows.Shell.SystemCommands.MaximizeWindow((Window) this.ParentWindow);
    }

    private void CloseClick(object sender, RoutedEventArgs e)
    {
      ClosingWindowEventHandlerArgs args = new ClosingWindowEventHandlerArgs();
      this.OnClosingWindow(args);
      if (args.Cancelled || this.ParentWindow == null)
        return;
      this.ParentWindow.Close();
    }

    public MetroWindow ParentWindow
    {
      get => this._parentWindow;
      set
      {
        if (object.Equals((object) this._parentWindow, (object) value))
          return;
        this._parentWindow = value;
        this.RaisePropertyChanged(nameof (ParentWindow));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void RaisePropertyChanged(string propertyName = null)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public delegate void ClosingWindowEventHandler(
      object sender,
      ClosingWindowEventHandlerArgs args);
  }
}
