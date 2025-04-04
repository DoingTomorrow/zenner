// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.MSSLicenseCustomerWindow
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Business.Utils;
using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS.Client.UI.Tablet.View
{
  public partial class MSSLicenseCustomerWindow : ResizableMetroWindow, IComponentConnector
  {
    internal Button SaveCustomerButton;
    internal Button CancelButton;
    internal Button BrowseButton;
    private bool _contentLoaded;

    public bool IsLicenseBrowsed { get; set; }

    public MSSLicenseCustomerWindow()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.Icon = (ImageSource) new BitmapImage(new Uri(CustomerConfiguration.GetPropertyValue("LauncherIcon")));
    }

    ~MSSLicenseCustomerWindow()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/msslicensecustomerwindow.xaml", UriKind.Relative));
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
