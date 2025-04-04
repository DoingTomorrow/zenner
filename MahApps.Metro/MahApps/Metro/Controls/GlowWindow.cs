// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.GlowWindow
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using MahApps.Metro.Models.Win32;
using MahApps.Metro.Native;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class GlowWindow : Window, IComponentConnector
  {
    private readonly Func<Point, Cursor> getCursor;
    private readonly Func<Point, HitTestValues> getHitTestValue;
    private readonly Func<RECT, double> getLeft;
    private readonly Func<RECT, double> getTop;
    private readonly Func<RECT, double> getWidth;
    private readonly Func<RECT, double> getHeight;
    private const double edgeSize = 20.0;
    private const double glowSize = 6.0;
    private IntPtr handle;
    private IntPtr ownerHandle;
    private bool closing;
    private HwndSource hwndSource;
    private PropertyChangeNotifier resizeModeChangeNotifier;
    internal GlowWindow glowWindow;
    private Glow glow;
    private bool _contentLoaded;

    public GlowWindow(Window owner, GlowDirection direction)
    {
      GlowWindow glowWindow = this;
      this.InitializeComponent();
      this.IsGlowing = true;
      this.AllowsTransparency = true;
      this.Closing += (CancelEventHandler) ((sender, e) => e.Cancel = !this.closing);
      this.Owner = owner;
      this.glow.Visibility = Visibility.Collapsed;
      this.glow.SetBinding(Glow.GlowBrushProperty, (BindingBase) new Binding("GlowBrush")
      {
        Source = (object) owner
      });
      this.glow.SetBinding(Glow.NonActiveGlowBrushProperty, (BindingBase) new Binding("NonActiveGlowBrush")
      {
        Source = (object) owner
      });
      this.glow.SetBinding(Control.BorderThicknessProperty, (BindingBase) new Binding("BorderThickness")
      {
        Source = (object) owner
      });
      this.glow.Direction = direction;
      switch (direction)
      {
        case GlowDirection.Left:
          this.glow.Orientation = Orientation.Vertical;
          this.glow.HorizontalAlignment = HorizontalAlignment.Right;
          this.getLeft = (Func<RECT, double>) (rect => (double) rect.left - 6.0 + 1.0);
          this.getTop = (Func<RECT, double>) (rect => (double) (rect.top - 2));
          this.getWidth = (Func<RECT, double>) (rect => 6.0);
          this.getHeight = (Func<RECT, double>) (rect => (double) (rect.Height + 4));
          this.getHitTestValue = (Func<Point, HitTestValues>) (p =>
          {
            if (new Rect(0.0, 0.0, this.ActualWidth, 20.0).Contains(p))
              return HitTestValues.HTTOPLEFT;
            return !new Rect(0.0, this.ActualHeight - 20.0, this.ActualWidth, 20.0).Contains(p) ? HitTestValues.HTLEFT : HitTestValues.HTBOTTOMLEFT;
          });
          this.getCursor = (Func<Point, Cursor>) (p =>
          {
            if (owner.ResizeMode == ResizeMode.NoResize || owner.ResizeMode == ResizeMode.CanMinimize)
              return owner.Cursor;
            Rect rect = new Rect(0.0, 0.0, glowWindow.ActualWidth, 20.0);
            if (rect.Contains(p))
              return Cursors.SizeNWSE;
            rect = new Rect(0.0, glowWindow.ActualHeight - 20.0, glowWindow.ActualWidth, 20.0);
            return !rect.Contains(p) ? Cursors.SizeWE : Cursors.SizeNESW;
          });
          break;
        case GlowDirection.Right:
          this.glow.Orientation = Orientation.Vertical;
          this.glow.HorizontalAlignment = HorizontalAlignment.Left;
          this.getLeft = (Func<RECT, double>) (rect => (double) (rect.right - 1));
          this.getTop = (Func<RECT, double>) (rect => (double) (rect.top - 2));
          this.getWidth = (Func<RECT, double>) (rect => 6.0);
          this.getHeight = (Func<RECT, double>) (rect => (double) (rect.Height + 4));
          this.getHitTestValue = (Func<Point, HitTestValues>) (p =>
          {
            if (new Rect(0.0, 0.0, this.ActualWidth, 20.0).Contains(p))
              return HitTestValues.HTTOPRIGHT;
            return !new Rect(0.0, this.ActualHeight - 20.0, this.ActualWidth, 20.0).Contains(p) ? HitTestValues.HTRIGHT : HitTestValues.HTBOTTOMRIGHT;
          });
          this.getCursor = (Func<Point, Cursor>) (p =>
          {
            if (owner.ResizeMode == ResizeMode.NoResize || owner.ResizeMode == ResizeMode.CanMinimize)
              return owner.Cursor;
            Rect rect = new Rect(0.0, 0.0, glowWindow.ActualWidth, 20.0);
            if (rect.Contains(p))
              return Cursors.SizeNESW;
            rect = new Rect(0.0, glowWindow.ActualHeight - 20.0, glowWindow.ActualWidth, 20.0);
            return !rect.Contains(p) ? Cursors.SizeWE : Cursors.SizeNWSE;
          });
          break;
        case GlowDirection.Top:
          this.glow.Orientation = Orientation.Horizontal;
          this.glow.VerticalAlignment = VerticalAlignment.Bottom;
          this.getLeft = (Func<RECT, double>) (rect => (double) (rect.left - 2));
          this.getTop = (Func<RECT, double>) (rect => (double) rect.top - 6.0 + 1.0);
          this.getWidth = (Func<RECT, double>) (rect => (double) (rect.Width + 4));
          this.getHeight = (Func<RECT, double>) (rect => 6.0);
          this.getHitTestValue = (Func<Point, HitTestValues>) (p =>
          {
            if (new Rect(0.0, 0.0, 14.0, this.ActualHeight).Contains(p))
              return HitTestValues.HTTOPLEFT;
            return !new Rect(this.Width - 20.0 + 6.0, 0.0, 14.0, this.ActualHeight).Contains(p) ? HitTestValues.HTTOP : HitTestValues.HTTOPRIGHT;
          });
          this.getCursor = (Func<Point, Cursor>) (p =>
          {
            if (owner.ResizeMode == ResizeMode.NoResize || owner.ResizeMode == ResizeMode.CanMinimize)
              return owner.Cursor;
            Rect rect = new Rect(0.0, 0.0, 14.0, glowWindow.ActualHeight);
            if (rect.Contains(p))
              return Cursors.SizeNWSE;
            // ISSUE: explicit non-virtual call
            rect = new Rect(__nonvirtual (glowWindow.Width) - 20.0 + 6.0, 0.0, 14.0, glowWindow.ActualHeight);
            return !rect.Contains(p) ? Cursors.SizeNS : Cursors.SizeNESW;
          });
          break;
        case GlowDirection.Bottom:
          this.glow.Orientation = Orientation.Horizontal;
          this.glow.VerticalAlignment = VerticalAlignment.Top;
          this.getLeft = (Func<RECT, double>) (rect => (double) (rect.left - 2));
          this.getTop = (Func<RECT, double>) (rect => (double) (rect.bottom - 1));
          this.getWidth = (Func<RECT, double>) (rect => (double) (rect.Width + 4));
          this.getHeight = (Func<RECT, double>) (rect => 6.0);
          this.getHitTestValue = (Func<Point, HitTestValues>) (p =>
          {
            if (new Rect(0.0, 0.0, 14.0, this.ActualHeight).Contains(p))
              return HitTestValues.HTBOTTOMLEFT;
            return !new Rect(this.Width - 20.0 + 6.0, 0.0, 14.0, this.ActualHeight).Contains(p) ? HitTestValues.HTBOTTOM : HitTestValues.HTBOTTOMRIGHT;
          });
          this.getCursor = (Func<Point, Cursor>) (p =>
          {
            if (owner.ResizeMode == ResizeMode.NoResize || owner.ResizeMode == ResizeMode.CanMinimize)
              return owner.Cursor;
            Rect rect = new Rect(0.0, 0.0, 14.0, glowWindow.ActualHeight);
            if (rect.Contains(p))
              return Cursors.SizeNESW;
            // ISSUE: explicit non-virtual call
            rect = new Rect(__nonvirtual (glowWindow.Width) - 20.0 + 6.0, 0.0, 14.0, glowWindow.ActualHeight);
            return !rect.Contains(p) ? Cursors.SizeNS : Cursors.SizeNWSE;
          });
          break;
      }
      owner.ContentRendered += (EventHandler) ((sender, e) => this.glow.Visibility = Visibility.Visible);
      owner.Activated += (EventHandler) ((sender, e) =>
      {
        this.Update();
        this.glow.IsGlow = true;
      });
      owner.Deactivated += (EventHandler) ((sender, e) => this.glow.IsGlow = false);
      owner.StateChanged += (EventHandler) ((sender, e) => this.Update());
      owner.IsVisibleChanged += (DependencyPropertyChangedEventHandler) ((sender, e) => this.Update());
      owner.Closed += (EventHandler) ((sender, e) =>
      {
        this.closing = true;
        this.Close();
      });
    }

    public Storyboard OpacityStoryboard { get; set; }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.OpacityStoryboard = this.TryFindResource((object) "OpacityStoryboard") as Storyboard;
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);
      this.hwndSource = (HwndSource) PresentationSource.FromVisual((Visual) this);
      if (this.hwndSource == null)
        return;
      WS windowLong = this.hwndSource.Handle.GetWindowLong();
      WSEX dwNewLong = this.hwndSource.Handle.GetWindowLongEx() ^ WSEX.APPWINDOW | WSEX.NOACTIVATE;
      if (this.Owner.ResizeMode == ResizeMode.NoResize || this.Owner.ResizeMode == ResizeMode.CanMinimize)
        dwNewLong |= WSEX.TRANSPARENT;
      int num1 = (int) this.hwndSource.Handle.SetWindowLong(windowLong);
      int num2 = (int) this.hwndSource.Handle.SetWindowLongEx(dwNewLong);
      this.hwndSource.AddHook(new HwndSourceHook(this.WndProc));
      this.handle = this.hwndSource.Handle;
      this.ownerHandle = new WindowInteropHelper(this.Owner).Handle;
      this.resizeModeChangeNotifier = new PropertyChangeNotifier((DependencyObject) this.Owner, Window.ResizeModeProperty);
      this.resizeModeChangeNotifier.ValueChanged += new EventHandler(this.ResizeModeChanged);
    }

    private void ResizeModeChanged(object sender, EventArgs e)
    {
      WSEX windowLongEx = this.hwndSource.Handle.GetWindowLongEx();
      int num = (int) this.hwndSource.Handle.SetWindowLongEx(this.Owner.ResizeMode == ResizeMode.NoResize || this.Owner.ResizeMode == ResizeMode.CanMinimize ? windowLongEx | WSEX.TRANSPARENT : windowLongEx ^ WSEX.TRANSPARENT);
    }

    public void Update()
    {
      if (this.Owner.Visibility == Visibility.Hidden)
      {
        this.Visibility = Visibility.Hidden;
        RECT lpRect;
        if (!(this.ownerHandle != IntPtr.Zero) || !UnsafeNativeMethods.GetWindowRect(this.ownerHandle, out lpRect))
          return;
        this.UpdateCore(lpRect);
      }
      else if (this.Owner.WindowState == WindowState.Normal)
      {
        if (this.closing)
          return;
        this.Visibility = this.IsGlowing ? Visibility.Visible : Visibility.Collapsed;
        this.glow.Visibility = this.IsGlowing ? Visibility.Visible : Visibility.Collapsed;
        RECT lpRect;
        if (!(this.ownerHandle != IntPtr.Zero) || !UnsafeNativeMethods.GetWindowRect(this.ownerHandle, out lpRect))
          return;
        this.UpdateCore(lpRect);
      }
      else
        this.Visibility = Visibility.Collapsed;
    }

    public bool IsGlowing { set; get; }

    internal void UpdateCore(RECT rect)
    {
      NativeMethods.SetWindowPos(this.handle, this.ownerHandle, (int) this.getLeft(rect), (int) this.getTop(rect), (int) this.getWidth(rect), (int) this.getHeight(rect), SWP.NOZORDER | SWP.NOACTIVATE);
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      if (msg == 24 && (int) lParam == 3 && this.Visibility != Visibility.Visible)
        handled = true;
      if (msg == 33)
      {
        handled = true;
        return new IntPtr(3);
      }
      if (msg == 513)
        NativeMethods.PostMessage(this.ownerHandle, 161U, (IntPtr) (int) this.getHitTestValue(new Point((double) ((int) lParam & (int) ushort.MaxValue), (double) ((int) lParam >> 16 & (int) ushort.MaxValue))), IntPtr.Zero);
      if (msg == 132)
      {
        Cursor cursor = this.getCursor(this.PointFromScreen(new Point((double) ((int) lParam & (int) ushort.MaxValue), (double) ((int) lParam >> 16 & (int) ushort.MaxValue))));
        if (cursor != this.Cursor)
          this.Cursor = cursor;
      }
      return IntPtr.Zero;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MahApps.Metro;component/controls/glowwindow.xaml", UriKind.Relative));
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
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.glow = (Glow) target;
        else
          this._contentLoaded = true;
      }
      else
        this.glowWindow = (GlowWindow) target;
    }
  }
}
