// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.MSSMainWindow
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MahApps.Metro.Controls;
using MSS.Business.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS.Client.UI.Desktop.View
{
  public partial class MSSMainWindow : MetroWindow, IComponentConnector
  {
    internal Button Logout;
    internal TextBlock logoutButtonText;
    internal Button btnMeters;
    internal Button btnArchiving;
    internal Button btnOrders;
    internal Button btnSettings;
    internal Button btnStructures;
    internal Button btnUsers;
    internal Button btnReporting;
    internal Button btnConfiguration;
    internal Button btnDataCollectors;
    private bool _contentLoaded;

    public MSSMainWindow()
    {
      this.InitializeComponent();
      this.Icon = (ImageSource) new BitmapImage(new Uri(CustomerConfiguration.GetPropertyValue("LauncherIcon")));
    }

    private void Drag_Window(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.DragMove();
    }

    private void Minimize_Window(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.WindowState = WindowState.Minimized;
    }

    private void Close_Window(object sender, MouseButtonEventArgs e) => this.Close();

    private void Maximize_Window(object sender, MouseButtonEventArgs e)
    {
      if (this.WindowState == WindowState.Normal)
        this.WindowState = WindowState.Maximized;
      else
        this.WindowState = WindowState.Normal;
    }

    private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
    {
      Mouse.OverrideCursor = Cursors.Arrow;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/mssmainwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.Logout = (Button) target;
          break;
        case 2:
          this.logoutButtonText = (TextBlock) target;
          break;
        case 3:
          ((UIElement) target).MouseEnter += new MouseEventHandler(this.UIElement_OnMouseEnter);
          break;
        case 4:
          this.btnMeters = (Button) target;
          break;
        case 5:
          this.btnArchiving = (Button) target;
          break;
        case 6:
          this.btnOrders = (Button) target;
          break;
        case 7:
          this.btnSettings = (Button) target;
          break;
        case 8:
          this.btnStructures = (Button) target;
          break;
        case 9:
          this.btnUsers = (Button) target;
          break;
        case 10:
          this.btnReporting = (Button) target;
          break;
        case 11:
          this.btnConfiguration = (Button) target;
          break;
        case 12:
          this.btnDataCollectors = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
