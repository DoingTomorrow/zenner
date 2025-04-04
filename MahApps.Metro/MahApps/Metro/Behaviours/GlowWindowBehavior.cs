// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.GlowWindowBehavior
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Controls;
using MahApps.Metro.Native;
using Standard;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Threading;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public class GlowWindowBehavior : Behavior<Window>
  {
    private static readonly TimeSpan GlowTimerDelay = TimeSpan.FromMilliseconds(200.0);
    private GlowWindow left;
    private GlowWindow right;
    private GlowWindow top;
    private GlowWindow bottom;
    private DispatcherTimer makeGlowVisibleTimer;
    private IntPtr handle;
    private WINDOWPOS _previousWP;

    private bool IsGlowDisabled
    {
      get
      {
        if (!(this.AssociatedObject is MetroWindow associatedObject))
          return false;
        return associatedObject.UseNoneWindowStyle || associatedObject.GlowBrush == null;
      }
    }

    private bool IsWindowTransitionsEnabled
    {
      get
      {
        return this.AssociatedObject is MetroWindow associatedObject && associatedObject.WindowTransitionsEnabled;
      }
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      this.AssociatedObject.SourceInitialized += (EventHandler) ((o, args) =>
      {
        if (this.IsGlowDisabled)
          return;
        this.handle = new WindowInteropHelper(this.AssociatedObject).Handle;
        HwndSource.FromHwnd(this.handle)?.AddHook(new HwndSourceHook(this.AssociatedObjectWindowProc));
      });
      this.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObjectOnLoaded);
      this.AssociatedObject.Unloaded += new RoutedEventHandler(this.AssociatedObjectUnloaded);
    }

    private void AssociatedObjectStateChanged(object sender, EventArgs e)
    {
      if (this.makeGlowVisibleTimer != null)
        this.makeGlowVisibleTimer.Stop();
      if (this.AssociatedObject.WindowState == WindowState.Normal)
      {
        bool flag = this.AssociatedObject is MetroWindow associatedObject && associatedObject.IgnoreTaskbarOnMaximize;
        if (this.makeGlowVisibleTimer != null && SystemParameters.MinimizeAnimation && !flag)
          this.makeGlowVisibleTimer.Start();
        else
          this.RestoreGlow();
      }
      else
        this.HideGlow();
    }

    private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
    {
      if (this.makeGlowVisibleTimer == null)
        return;
      this.makeGlowVisibleTimer.Stop();
      this.makeGlowVisibleTimer.Tick -= new EventHandler(this.makeGlowVisibleTimer_Tick);
      this.makeGlowVisibleTimer = (DispatcherTimer) null;
    }

    private void makeGlowVisibleTimer_Tick(object sender, EventArgs e)
    {
      if (this.makeGlowVisibleTimer != null)
        this.makeGlowVisibleTimer.Stop();
      this.RestoreGlow();
    }

    private void RestoreGlow()
    {
      if (this.left != null)
        this.left.IsGlowing = true;
      if (this.top != null)
        this.top.IsGlowing = true;
      if (this.right != null)
        this.right.IsGlowing = true;
      if (this.bottom != null)
        this.bottom.IsGlowing = true;
      this.Update();
    }

    private void HideGlow()
    {
      if (this.left != null)
        this.left.IsGlowing = false;
      if (this.top != null)
        this.top.IsGlowing = false;
      if (this.right != null)
        this.right.IsGlowing = false;
      if (this.bottom != null)
        this.bottom.IsGlowing = false;
      this.Update();
    }

    private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      if (this.IsGlowDisabled)
        return;
      this.AssociatedObject.StateChanged -= new EventHandler(this.AssociatedObjectStateChanged);
      this.AssociatedObject.StateChanged += new EventHandler(this.AssociatedObjectStateChanged);
      if (this.makeGlowVisibleTimer == null)
      {
        this.makeGlowVisibleTimer = new DispatcherTimer()
        {
          Interval = GlowWindowBehavior.GlowTimerDelay
        };
        this.makeGlowVisibleTimer.Tick += new EventHandler(this.makeGlowVisibleTimer_Tick);
      }
      this.left = new GlowWindow(this.AssociatedObject, GlowDirection.Left);
      this.right = new GlowWindow(this.AssociatedObject, GlowDirection.Right);
      this.top = new GlowWindow(this.AssociatedObject, GlowDirection.Top);
      this.bottom = new GlowWindow(this.AssociatedObject, GlowDirection.Bottom);
      this.Show();
      this.Update();
      if (!this.IsWindowTransitionsEnabled)
      {
        this.AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => this.SetOpacityTo(1.0)));
      }
      else
      {
        this.StartOpacityStoryboard();
        this.AssociatedObject.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.AssociatedObjectIsVisibleChanged);
        this.AssociatedObject.Closing += (CancelEventHandler) ((o, args) =>
        {
          if (args.Cancel)
            return;
          this.AssociatedObject.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.AssociatedObjectIsVisibleChanged);
        });
      }
    }

    private IntPtr AssociatedObjectWindowProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      switch ((MahApps.Metro.Models.Win32.WM) msg)
      {
        case MahApps.Metro.Models.Win32.WM.SIZE:
        case MahApps.Metro.Models.Win32.WM.SIZING:
          this.UpdateCore();
          break;
        case MahApps.Metro.Models.Win32.WM.WINDOWPOSCHANGING:
        case MahApps.Metro.Models.Win32.WM.WINDOWPOSCHANGED:
          WINDOWPOS structure = (WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (WINDOWPOS));
          if (!structure.Equals((object) this._previousWP))
            this.UpdateCore();
          this._previousWP = structure;
          break;
      }
      return IntPtr.Zero;
    }

    private void AssociatedObjectIsVisibleChanged(
      object sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (!this.AssociatedObject.IsVisible)
        this.SetOpacityTo(0.0);
      else
        this.StartOpacityStoryboard();
    }

    private void Update()
    {
      if (this.left != null)
        this.left.Update();
      if (this.right != null)
        this.right.Update();
      if (this.top != null)
        this.top.Update();
      if (this.bottom == null)
        return;
      this.bottom.Update();
    }

    private void UpdateCore()
    {
      MahApps.Metro.Native.RECT lpRect;
      if (!(this.handle != IntPtr.Zero) || !UnsafeNativeMethods.GetWindowRect(this.handle, out lpRect))
        return;
      if (this.left != null)
        this.left.UpdateCore(lpRect);
      if (this.right != null)
        this.right.UpdateCore(lpRect);
      if (this.top != null)
        this.top.UpdateCore(lpRect);
      if (this.bottom == null)
        return;
      this.bottom.UpdateCore(lpRect);
    }

    private void SetOpacityTo(double newOpacity)
    {
      if (this.left != null)
        this.left.Opacity = newOpacity;
      if (this.right != null)
        this.right.Opacity = newOpacity;
      if (this.top != null)
        this.top.Opacity = newOpacity;
      if (this.bottom == null)
        return;
      this.bottom.Opacity = newOpacity;
    }

    private void StartOpacityStoryboard()
    {
      if (this.left != null && this.left.OpacityStoryboard != null)
        this.left.BeginStoryboard(this.left.OpacityStoryboard);
      if (this.right != null && this.right.OpacityStoryboard != null)
        this.right.BeginStoryboard(this.right.OpacityStoryboard);
      if (this.top != null && this.top.OpacityStoryboard != null)
        this.top.BeginStoryboard(this.top.OpacityStoryboard);
      if (this.bottom == null || this.bottom.OpacityStoryboard == null)
        return;
      this.bottom.BeginStoryboard(this.bottom.OpacityStoryboard);
    }

    private void Show()
    {
      if (this.left != null)
        this.left.Show();
      if (this.right != null)
        this.right.Show();
      if (this.top != null)
        this.top.Show();
      if (this.bottom == null)
        return;
      this.bottom.Show();
    }
  }
}
