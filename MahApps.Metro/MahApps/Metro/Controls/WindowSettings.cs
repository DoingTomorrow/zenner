// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.WindowSettings
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Native;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class WindowSettings
  {
    public static readonly DependencyProperty WindowPlacementSettingsProperty = DependencyProperty.RegisterAttached("WindowPlacementSettings", typeof (IWindowPlacementSettings), typeof (WindowSettings), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(WindowSettings.OnWindowPlacementSettingsInvalidated)));
    private Window _window;
    private IWindowPlacementSettings _settings;

    public static void SetSave(
      DependencyObject dependencyObject,
      IWindowPlacementSettings windowPlacementSettings)
    {
      dependencyObject.SetValue(WindowSettings.WindowPlacementSettingsProperty, (object) windowPlacementSettings);
    }

    private static void OnWindowPlacementSettingsInvalidated(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(dependencyObject is Window window) || !(e.NewValue is IWindowPlacementSettings))
        return;
      new WindowSettings(window, (IWindowPlacementSettings) e.NewValue).Attach();
    }

    public WindowSettings(Window window, IWindowPlacementSettings windowPlacementSettings)
    {
      this._window = window;
      this._settings = windowPlacementSettings;
    }

    protected virtual void LoadWindowState()
    {
      if (this._settings == null)
        return;
      this._settings.Reload();
      WINDOWPLACEMENT? placement = this._settings.Placement;
      if (!placement.HasValue)
        return;
      placement = this._settings.Placement;
      if (placement.Value.normalPosition.IsEmpty)
        return;
      try
      {
        placement = this._settings.Placement;
        WINDOWPLACEMENT lpwndpl = placement.Value with
        {
          length = Marshal.SizeOf(typeof (WINDOWPLACEMENT)),
          flags = 0
        };
        lpwndpl.showCmd = lpwndpl.showCmd == 2 ? 1 : lpwndpl.showCmd;
        UnsafeNativeMethods.SetWindowPlacement(new WindowInteropHelper(this._window).Handle, ref lpwndpl);
      }
      catch (Exception ex)
      {
        throw new MahAppsException("Failed to set the window state from the settings file", ex);
      }
    }

    protected virtual void SaveWindowState()
    {
      if (this._settings == null)
        return;
      IntPtr handle = new WindowInteropHelper(this._window).Handle;
      WINDOWPLACEMENT lpwndpl = new WINDOWPLACEMENT();
      lpwndpl.length = Marshal.SizeOf((object) lpwndpl);
      UnsafeNativeMethods.GetWindowPlacement(handle, ref lpwndpl);
      if (lpwndpl.showCmd != 0 && lpwndpl.length > 0)
      {
        RECT lpRect;
        if (lpwndpl.showCmd == 1 && UnsafeNativeMethods.GetWindowRect(handle, out lpRect))
          lpwndpl.normalPosition = lpRect;
        if (!lpwndpl.normalPosition.IsEmpty)
          this._settings.Placement = new WINDOWPLACEMENT?(lpwndpl);
      }
      this._settings.Save();
    }

    private void Attach()
    {
      if (this._window == null)
        return;
      this._window.SourceInitialized += new EventHandler(this.WindowSourceInitialized);
      this._window.Closed += new EventHandler(this.WindowClosed);
    }

    private void WindowSourceInitialized(object sender, EventArgs e)
    {
      this.LoadWindowState();
      this._window.StateChanged += new EventHandler(this.WindowStateChanged);
      this._window.Closing += new CancelEventHandler(this.WindowClosing);
    }

    private void WindowStateChanged(object sender, EventArgs e)
    {
      if (this._window.WindowState != WindowState.Minimized)
        return;
      this.SaveWindowState();
    }

    private void WindowClosing(object sender, CancelEventArgs e) => this.SaveWindowState();

    private void WindowClosed(object sender, EventArgs e)
    {
      this.SaveWindowState();
      this._window.StateChanged -= new EventHandler(this.WindowStateChanged);
      this._window.Closing -= new CancelEventHandler(this.WindowClosing);
      this._window.Closed -= new EventHandler(this.WindowClosed);
      this._window.SourceInitialized -= new EventHandler(this.WindowSourceInitialized);
      this._window = (Window) null;
      this._settings = (IWindowPlacementSettings) null;
    }
  }
}
