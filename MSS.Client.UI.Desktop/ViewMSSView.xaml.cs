// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.MSSView
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MahApps.Metro.Controls;
using Microsoft.CSharp.RuntimeBinder;
using MSS.Business.Modules.LicenseManagement;
using MSS.Business.Utils;
using MSS.Client.UI.Common.FileProvider;
using MSS.Client.UI.Common.FileProvider.Interfaces;
using MSS.DIConfiguration;
using Ninject;
using Ninject.Parameters;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View
{
  public partial class MSSView : MetroWindow, IComponentConnector
  {
    private const int MONITOR_DEFAULTTONEAREST = 2;
    private const int MIN_HEIGHT = 600;
    private const int MIN_WIDTH = 990;
    public Func<string, bool> FileExists = (Func<string, bool>) (s => File.Exists(string.Format("C:\\ProgramData\\MSS\\{0}_HelpFile.pdf", (object) s)));
    internal TextBox SearchTextBox;
    internal Button Download;
    internal TextBlock downloadButtonText;
    internal Button ShowConficts;
    internal TextBlock showConflictsButtonText;
    internal Button Update;
    internal TextBlock updateButtonText;
    internal Button Send;
    internal TextBlock sendButtonText;
    internal Button Settings;
    internal TextBlock settingsButtonText;
    internal Button About;
    internal TextBlock aboutButtonText;
    internal Button Logout;
    internal TextBlock logoutButtonText;
    internal Button BtnUp;
    internal Button Btndown;
    internal ScrollViewer MenuScroll;
    internal ToggleButton Structures;
    internal ToggleButton Meters;
    internal ToggleButton Orders;
    internal ToggleButton DataCollectors;
    internal ToggleButton Jobs;
    internal ToggleButton Reporting;
    internal ToggleButton Configuration;
    internal ToggleButton Users;
    internal ToggleButton Archiving;
    internal Button downloadUsers;
    internal Grid UserControlCanvas;
    internal RadBusyIndicator BusyIndicator;
    private bool _contentLoaded;

    private static int GetEdge(MSSView.RECT rc)
    {
      return rc.top != rc.left || rc.bottom <= rc.right ? (rc.top != rc.left || rc.bottom >= rc.right ? (rc.top <= rc.left ? 2 : 3) : 1) : 0;
    }

    public MSSView()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(this.win_SourceInitialized);
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
      DIConfigurator.GetConfigurator().Bind<IFileServiceLocator>().To<FileServiceLocatorImpl>();
      this.Icon = (ImageSource) new BitmapImage(new Uri(CustomerConfiguration.GetPropertyValue("LauncherIcon")));
    }

    ~MSSView()
    {
      this.SourceInitialized -= new EventHandler(this.win_SourceInitialized);
      this.Btndown.Click -= new RoutedEventHandler(this.Btndown_OnClick);
      this.BtnUp.Click -= new RoutedEventHandler(this.BtnUp_OnClick);
      this.KeyUp -= new KeyEventHandler(this.WindowSizingAdapter_KeyUp);
    }

    private static MSSView.MINMAXINFO AdjustWorkingAreaForAutoHide(
      IntPtr monitorContainingApplication,
      MSSView.MINMAXINFO mmi)
    {
      IntPtr window = MSSView.FindWindow("Shell_TrayWnd", (string) null);
      IntPtr num1 = MSSView.MonitorFromWindow(window, 2);
      if (!monitorContainingApplication.Equals((object) num1))
        return mmi;
      MSSView.APPBARDATA pData = new MSSView.APPBARDATA();
      pData.cbSize = Marshal.SizeOf<MSSView.APPBARDATA>(pData);
      pData.hWnd = window;
      MSSView.SHAppBarMessage(5, ref pData);
      int edge = MSSView.GetEdge(pData.rc);
      if (!Convert.ToBoolean(MSSView.SHAppBarMessage(4, ref pData)))
        return mmi;
      int num2 = 13;
      switch (edge)
      {
        case 0:
          mmi.ptMaxPosition.x += num2;
          mmi.ptMaxTrackSize.x -= num2;
          mmi.ptMaxSize.x -= num2;
          break;
        case 1:
          mmi.ptMaxPosition.y += num2;
          mmi.ptMaxTrackSize.y -= num2;
          mmi.ptMaxSize.y -= num2;
          break;
        case 2:
          mmi.ptMaxSize.x -= num2;
          mmi.ptMaxTrackSize.x -= num2;
          break;
        case 3:
          mmi.ptMaxSize.y -= num2;
          mmi.ptMaxTrackSize.y -= num2;
          break;
        default:
          return mmi;
      }
      return mmi;
    }

    private IntPtr WindowProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      switch (msg)
      {
        case 36:
          MSSView.WmGetMinMaxInfo(hwnd, lParam);
          handled = true;
          break;
        case 70:
          MSSView.WINDOWPOS structure = (MSSView.WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (MSSView.WINDOWPOS));
          if ((structure.flags & 2) != 0 || (Window) HwndSource.FromHwnd(hwnd)?.RootVisual == null)
            return IntPtr.Zero;
          bool flag = false;
          if (structure.cx < 990)
          {
            structure.cx = 990;
            flag = true;
          }
          if (structure.cy < 600)
          {
            structure.cy = 600;
            flag = true;
          }
          if (!flag)
            return IntPtr.Zero;
          Marshal.StructureToPtr<MSSView.WINDOWPOS>(structure, lParam, true);
          handled = true;
          break;
      }
      return (IntPtr) 0;
    }

    private void win_SourceInitialized(object sender, EventArgs e)
    {
      HwndSource.FromHwnd(new WindowInteropHelper((Window) this).Handle).AddHook(new HwndSourceHook(this.WindowProc));
    }

    private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
    {
      MSSView.MINMAXINFO minmaxinfo = (MSSView.MINMAXINFO) Marshal.PtrToStructure(lParam, typeof (MSSView.MINMAXINFO));
      int flags = 2;
      IntPtr num = MSSView.MonitorFromWindow(hwnd, flags);
      if (num != IntPtr.Zero)
      {
        MSSView.MONITORINFO lpmi = new MSSView.MONITORINFO();
        MSSView.GetMonitorInfo(num, lpmi);
        MSSView.RECT rcWork = lpmi.rcWork;
        MSSView.RECT rcMonitor = lpmi.rcMonitor;
        minmaxinfo.ptMaxPosition.x = Math.Abs(rcWork.left - rcMonitor.left);
        minmaxinfo.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top);
        minmaxinfo.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left);
        minmaxinfo.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top);
        minmaxinfo = MSSView.AdjustWorkingAreaForAutoHide(num, minmaxinfo);
      }
      Marshal.StructureToPtr<MSSView.MINMAXINFO>(minmaxinfo, lParam, true);
    }

    [DllImport("user32")]
    internal static extern bool GetMonitorInfo(IntPtr hMonitor, MSSView.MONITORINFO lpmi);

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(ref Point lpPoint);

    [DllImport("User32")]
    internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

    [DllImport("user32", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("shell32", CallingConvention = CallingConvention.StdCall)]
    public static extern int SHAppBarMessage(int dwMessage, ref MSSView.APPBARDATA pData);

    private void Btndown_OnClick(object sender, RoutedEventArgs e)
    {
      ScrollViewer menuScroll = this.MenuScroll;
      menuScroll?.ScrollToVerticalOffset(menuScroll.VerticalOffset + 50.0);
    }

    private void BtnUp_OnClick(object sender, RoutedEventArgs e)
    {
      ScrollViewer menuScroll = this.MenuScroll;
      menuScroll?.ScrollToVerticalOffset(menuScroll.VerticalOffset - 50.0);
    }

    private void WindowSizingAdapter_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.F1)
        return;
      this.BusyIndicator.IsBusy = true;
      Task.Run((Action) (() => DIConfigurator.GetConfigurator().Get<IFileServiceLocator>((IParameter) new ConstructorArgument("LicenseType", (object) LicenseHelper.GetCurrentLicenseType(LicenseHelper.GetValidHardwareKey()))).GetProvider((Func<string, bool>) (userLicenseType => this.FileExists(userLicenseType))).OpenFile((Action) (() => Application.Current.Dispatcher.Invoke<object>((Func<object>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        if (MSSView.\u003C\u003Eo__26.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MSSView.\u003C\u003Eo__26.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DisplayWarning", (IEnumerable<Type>) null, typeof (MSSView), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return MSSView.\u003C\u003Eo__26.\u003C\u003Ep__0.Target((CallSite) MSSView.\u003C\u003Eo__26.\u003C\u003Ep__0, this.DataContext);
      })))))).ContinueWith<bool>((Func<Task, bool>) (_ => Application.Current.Dispatcher.Invoke<bool>((Func<bool>) (() => this.BusyIndicator.IsBusy = false))));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/mssview.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((UIElement) target).KeyUp += new KeyEventHandler(this.WindowSizingAdapter_KeyUp);
          break;
        case 2:
          this.SearchTextBox = (TextBox) target;
          break;
        case 3:
          this.Download = (Button) target;
          break;
        case 4:
          this.downloadButtonText = (TextBlock) target;
          break;
        case 5:
          this.ShowConficts = (Button) target;
          break;
        case 6:
          this.showConflictsButtonText = (TextBlock) target;
          break;
        case 7:
          this.Update = (Button) target;
          break;
        case 8:
          this.updateButtonText = (TextBlock) target;
          break;
        case 9:
          this.Send = (Button) target;
          break;
        case 10:
          this.sendButtonText = (TextBlock) target;
          break;
        case 11:
          this.Settings = (Button) target;
          break;
        case 12:
          this.settingsButtonText = (TextBlock) target;
          break;
        case 13:
          this.About = (Button) target;
          break;
        case 14:
          this.aboutButtonText = (TextBlock) target;
          break;
        case 15:
          this.Logout = (Button) target;
          break;
        case 16:
          this.logoutButtonText = (TextBlock) target;
          break;
        case 17:
          this.BtnUp = (Button) target;
          this.BtnUp.Click += new RoutedEventHandler(this.BtnUp_OnClick);
          break;
        case 18:
          this.Btndown = (Button) target;
          this.Btndown.Click += new RoutedEventHandler(this.Btndown_OnClick);
          break;
        case 19:
          this.MenuScroll = (ScrollViewer) target;
          break;
        case 20:
          this.Structures = (ToggleButton) target;
          break;
        case 21:
          this.Meters = (ToggleButton) target;
          break;
        case 22:
          this.Orders = (ToggleButton) target;
          break;
        case 23:
          this.DataCollectors = (ToggleButton) target;
          break;
        case 24:
          this.Jobs = (ToggleButton) target;
          break;
        case 25:
          this.Reporting = (ToggleButton) target;
          break;
        case 26:
          this.Configuration = (ToggleButton) target;
          break;
        case 27:
          this.Users = (ToggleButton) target;
          break;
        case 28:
          this.Archiving = (ToggleButton) target;
          break;
        case 29:
          this.downloadUsers = (Button) target;
          break;
        case 30:
          this.UserControlCanvas = (Grid) target;
          break;
        case 31:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    internal struct WINDOWPOS
    {
      public IntPtr hwnd;
      public IntPtr hwndInsertAfter;
      public int x;
      public int y;
      public int cx;
      public int cy;
      public int flags;
    }

    public enum SWP : uint
    {
      NOSIZE = 1,
      NOMOVE = 2,
      NOZORDER = 4,
      NOREDRAW = 8,
      NOACTIVATE = 16, // 0x00000010
      FRAMECHANGED = 32, // 0x00000020
      SHOWWINDOW = 64, // 0x00000040
      HIDEWINDOW = 128, // 0x00000080
      NOCOPYBITS = 256, // 0x00000100
      NOOWNERZORDER = 512, // 0x00000200
      NOSENDCHANGING = 1024, // 0x00000400
    }

    public struct POINT(int x, int y)
    {
      public int x = x;
      public int y = y;
    }

    public struct MINMAXINFO
    {
      public MSSView.POINT ptReserved;
      public MSSView.POINT ptMaxSize;
      public MSSView.POINT ptMaxPosition;
      public MSSView.POINT ptMinTrackSize;
      public MSSView.POINT ptMaxTrackSize;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MONITORINFO
    {
      public int cbSize = Marshal.SizeOf(typeof (MSSView.MONITORINFO));
      public MSSView.RECT rcMonitor = new MSSView.RECT();
      public MSSView.RECT rcWork = new MSSView.RECT();
      public int dwFlags = 0;
    }

    public struct RECT
    {
      public int left;
      public int top;
      public int right;
      public int bottom;
      public static readonly MSSView.RECT Empty = new MSSView.RECT();

      public int Width => Math.Abs(this.right - this.left);

      public int Height => this.bottom - this.top;

      public RECT(int left, int top, int right, int bottom)
      {
        this.left = left;
        this.top = top;
        this.right = right;
        this.bottom = bottom;
      }

      public RECT(MSSView.RECT rcSrc)
      {
        this.left = rcSrc.left;
        this.top = rcSrc.top;
        this.right = rcSrc.right;
        this.bottom = rcSrc.bottom;
      }

      public bool IsEmpty => this.left >= this.right || this.top >= this.bottom;

      public override string ToString()
      {
        if (this == MSSView.RECT.Empty)
          return "RECT {Empty}";
        return "RECT { left : " + (object) this.left + " / top : " + (object) this.top + " / right : " + (object) this.right + " / bottom : " + (object) this.bottom + " }";
      }

      public override bool Equals(object obj) => obj is Rect && this == (MSSView.RECT) obj;

      public override int GetHashCode()
      {
        return this.left.GetHashCode() + this.top.GetHashCode() + this.right.GetHashCode() + this.bottom.GetHashCode();
      }

      public static bool operator ==(MSSView.RECT rect1, MSSView.RECT rect2)
      {
        return rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom;
      }

      public static bool operator !=(MSSView.RECT rect1, MSSView.RECT rect2) => !(rect1 == rect2);
    }

    public struct APPBARDATA
    {
      public int cbSize;
      public IntPtr hWnd;
      public int uCallbackMessage;
      public int uEdge;
      public MSSView.RECT rc;
      public bool lParam;
    }

    public enum ABMsg
    {
      ABM_NEW,
      ABM_REMOVE,
      ABM_QUERYPOS,
      ABM_SETPOS,
      ABM_GETSTATE,
      ABM_GETTASKBARPOS,
      ABM_ACTIVATE,
      ABM_GETAUTOHIDEBAR,
      ABM_SETAUTOHIDEBAR,
      ABM_WINDOWPOSCHANGED,
      ABM_SETSTATE,
    }

    public enum ABEdge
    {
      ABE_LEFT,
      ABE_TOP,
      ABE_RIGHT,
      ABE_BOTTOM,
    }
  }
}
