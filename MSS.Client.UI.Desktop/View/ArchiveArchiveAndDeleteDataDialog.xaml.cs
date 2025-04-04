// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Archive.ArchiveAndDeleteDataDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MSS.Business.Utils;
using MSS.Client.UI.Common;
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
namespace MSS.Client.UI.Desktop.View.Archive
{
  public partial class ArchiveAndDeleteDataDialog : ResizableMetroWindow, IComponentConnector
  {
    internal ArchiveAndDeleteDataDialog ArchiveAndDeleteWindow;
    internal Button ButtonYes;
    internal Button ButtonNo;
    private bool _contentLoaded;

    public ArchiveAndDeleteDataDialog()
    {
      this.InitializeComponent();
      this.Icon = (ImageSource) new BitmapImage(new Uri(CustomerConfiguration.GetPropertyValue("LauncherIcon")));
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~ArchiveAndDeleteDataDialog()
    {
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/archive/archiveanddeletedatadialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ArchiveAndDeleteWindow = (ArchiveAndDeleteDataDialog) target;
          break;
        case 2:
          this.ButtonYes = (Button) target;
          break;
        case 3:
          this.ButtonNo = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
