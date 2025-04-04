// Decompiled with JetBrains decompiler
// Type: Fluent.KeyTipService
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  internal class KeyTipService
  {
    private Ribbon ribbon;
    private DispatcherTimer timer;
    private KeyTipAdorner activeAdornerChain;
    private IInputElement backUpFocusedElement;
    private Window window;
    private bool attached;
    private HwndSource attachedHwndSource;

    public KeyTipService(Ribbon ribbon)
    {
      this.ribbon = ribbon;
      if (!ribbon.IsLoaded)
        ribbon.Loaded += new RoutedEventHandler(this.OnDelayedInitialization);
      else
        this.Attach();
      this.timer = new DispatcherTimer(TimeSpan.FromSeconds(0.7), DispatcherPriority.SystemIdle, new EventHandler(this.OnDelayedShow), Dispatcher.CurrentDispatcher);
      this.timer.Stop();
    }

    private void OnDelayedInitialization(object sender, EventArgs args)
    {
      this.ribbon.Loaded -= new RoutedEventHandler(this.OnDelayedInitialization);
      this.Attach();
    }

    public void Attach()
    {
      if (this.attached)
        return;
      this.attached = true;
      if (DesignerProperties.GetIsInDesignMode((DependencyObject) this.ribbon))
        return;
      this.window = KeyTipService.GetElementWindow((UIElement) this.ribbon);
      if (this.window == null)
        return;
      this.window.KeyDown += new KeyEventHandler(this.OnWindowKeyDown);
      this.window.KeyUp += new KeyEventHandler(this.OnWindowKeyUp);
      this.attachedHwndSource = (HwndSource) PresentationSource.FromVisual((Visual) this.window);
      if (this.attachedHwndSource == null)
        return;
      this.attachedHwndSource.AddHook(new HwndSourceHook(this.WindowProc));
    }

    public void Detach()
    {
      if (!this.attached)
        return;
      this.attached = false;
      if (this.window != null)
      {
        this.window.KeyDown -= new KeyEventHandler(this.OnWindowKeyDown);
        this.window.KeyUp -= new KeyEventHandler(this.OnWindowKeyUp);
      }
      if (this.attachedHwndSource == null || this.attachedHwndSource.IsDisposed)
        return;
      this.attachedHwndSource.RemoveHook(new HwndSourceHook(this.WindowProc));
    }

    private IntPtr WindowProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      if ((msg >= 161 && msg <= 173 || msg == 134) && this.activeAdornerChain != null && this.activeAdornerChain.IsAdornerChainAlive)
      {
        this.activeAdornerChain.Terminate();
        this.activeAdornerChain = (KeyTipAdorner) null;
      }
      return IntPtr.Zero;
    }

    private void OnWindowKeyDown(object sender, KeyEventArgs e)
    {
      if (e.IsRepeat)
        return;
      this.timer.Stop();
      if (this.ribbon.IsCollapsed || e.Key != Key.System || e.SystemKey != Key.LeftAlt && e.SystemKey != Key.RightAlt && e.SystemKey != Key.F10 && e.SystemKey != Key.Space)
        return;
      if (this.activeAdornerChain == null || !this.activeAdornerChain.IsAdornerChainAlive)
      {
        this.activeAdornerChain = (KeyTipAdorner) null;
        this.timer.Start();
      }
      else
      {
        this.activeAdornerChain.Terminate();
        this.activeAdornerChain = (KeyTipAdorner) null;
      }
    }

    private void OnWindowKeyUp(object sender, KeyEventArgs e)
    {
      if (this.ribbon.IsCollapsed)
        return;
      if (e.Key == Key.System && (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F10 || e.SystemKey == Key.Space))
      {
        e.Handled = true;
        if (this.timer.IsEnabled)
        {
          this.timer.Stop();
          this.backUpFocusedElement = Keyboard.FocusedElement;
          this.ribbon.Focusable = true;
          this.ribbon.Focus();
          this.Show();
        }
        else
        {
          if (this.activeAdornerChain == null || !this.activeAdornerChain.IsAdornerChainAlive)
            return;
          this.backUpFocusedElement = Keyboard.FocusedElement;
          this.ribbon.Focusable = true;
          this.ribbon.Focus();
        }
      }
      else
        this.timer.Stop();
    }

    private void RestoreFocuses()
    {
      if (this.backUpFocusedElement != null)
        this.backUpFocusedElement.Focus();
      this.ribbon.Focusable = false;
    }

    private void OnAdornerChainTerminated(object sender, EventArgs e)
    {
      this.RestoreFocuses();
      ((KeyTipAdorner) sender).Terminated -= new EventHandler(this.OnAdornerChainTerminated);
    }

    private void OnDelayedShow(object sender, EventArgs e)
    {
      if (this.activeAdornerChain == null)
        this.Show();
      this.timer.Stop();
    }

    private void Show()
    {
      if (!this.window.IsActive)
      {
        this.RestoreFocuses();
      }
      else
      {
        this.activeAdornerChain = new KeyTipAdorner((UIElement) this.ribbon, (UIElement) this.ribbon, (KeyTipAdorner) null);
        this.activeAdornerChain.Terminated += new EventHandler(this.OnAdornerChainTerminated);
        if (this.ribbon.Menu is Backstage menu && menu.IsOpen)
        {
          if (!string.IsNullOrEmpty(KeyTip.GetKeys((DependencyObject) menu)))
            this.activeAdornerChain.Forward(KeyTip.GetKeys((DependencyObject) menu), false);
          else
            this.activeAdornerChain.Attach();
        }
        else
          this.activeAdornerChain.Attach();
      }
    }

    private static Window GetElementWindow(UIElement element)
    {
      do
      {
        element = VisualTreeHelper.GetParent((DependencyObject) element) as UIElement;
        if (element == null)
          return (Window) null;
      }
      while (!(element is Window elementWindow));
      return elementWindow;
    }
  }
}
