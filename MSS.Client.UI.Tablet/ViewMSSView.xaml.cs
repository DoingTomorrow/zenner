// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.MSSView
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Business.Utils;
using MSS.Client.UI.Common;
using MSS.Client.UI.Common.FileProvider;
using MSS.Client.UI.Common.FileProvider.Interfaces;
using MSS.Client.UI.Tablet.CustomControls;
using MSS.DIConfiguration;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View
{
  public partial class MSSView : KeyboardMetroWindow, IComponentConnector
  {
    public Func<string, bool> FileExists = (Func<string, bool>) (s => File.Exists(string.Format("C:\\ProgramData\\MSS\\{0}_HelpFile.pdf", (object) s)));
    internal System.Windows.Controls.Button Download;
    internal TextBlock downloadButtonText;
    internal System.Windows.Controls.Button ShowConficts;
    internal TextBlock showConflictsButtonText;
    internal TabletButton Sync;
    internal TabletButton RemoveOldOrders;
    internal TabletButton Settings;
    internal System.Windows.Controls.TextBox SearchTextBox;
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
    internal System.Windows.Controls.Button downloadUsers;
    internal Grid UserControlCanvas;
    internal RadBusyIndicator BusyIndicator;
    private bool _contentLoaded;

    public MSSView()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
      DIConfigurator.GetConfigurator().Bind<IFileServiceLocator>().To<FileServiceLocatorImpl>();
      this.Icon = (ImageSource) new BitmapImage(new Uri(CustomerConfiguration.GetPropertyValue("LauncherIcon")));
    }

    ~MSSView()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
    }

    private void DisplaySettingsChanged(object sender, EventArgs e)
    {
      if (Screen.PrimaryScreen.Bounds.Height <= Screen.PrimaryScreen.Bounds.Width)
        ;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/mssview.xaml", UriKind.Relative));
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
      switch (connectionId)
      {
        case 1:
          this.Download = (System.Windows.Controls.Button) target;
          break;
        case 2:
          this.downloadButtonText = (TextBlock) target;
          break;
        case 3:
          this.ShowConficts = (System.Windows.Controls.Button) target;
          break;
        case 4:
          this.showConflictsButtonText = (TextBlock) target;
          break;
        case 5:
          this.Sync = (TabletButton) target;
          break;
        case 6:
          this.RemoveOldOrders = (TabletButton) target;
          break;
        case 7:
          this.Settings = (TabletButton) target;
          break;
        case 8:
          this.SearchTextBox = (System.Windows.Controls.TextBox) target;
          break;
        case 9:
          this.MenuScroll = (ScrollViewer) target;
          break;
        case 10:
          this.Structures = (ToggleButton) target;
          break;
        case 11:
          this.Meters = (ToggleButton) target;
          break;
        case 12:
          this.Orders = (ToggleButton) target;
          break;
        case 13:
          this.DataCollectors = (ToggleButton) target;
          break;
        case 14:
          this.Jobs = (ToggleButton) target;
          break;
        case 15:
          this.Reporting = (ToggleButton) target;
          break;
        case 16:
          this.Configuration = (ToggleButton) target;
          break;
        case 17:
          this.Users = (ToggleButton) target;
          break;
        case 18:
          this.Archiving = (ToggleButton) target;
          break;
        case 19:
          this.downloadUsers = (System.Windows.Controls.Button) target;
          break;
        case 20:
          this.UserControlCanvas = (Grid) target;
          break;
        case 21:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
