// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.MSSLicenseCustomerWindow
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
  public partial class MSSLicenseCustomerWindow : MetroWindow, IComponentConnector
  {
    internal Button SaveCustomerButton;
    internal Button CancelButton;
    internal Button BrowseButton;
    private bool _contentLoaded;

    public bool IsLicenseBrowsed { get; set; }

    public MSSLicenseCustomerWindow()
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

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/msslicensecustomerwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.SaveCustomerButton = (Button) target;
          break;
        case 2:
          this.CancelButton = (Button) target;
          break;
        case 3:
          this.BrowseButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
