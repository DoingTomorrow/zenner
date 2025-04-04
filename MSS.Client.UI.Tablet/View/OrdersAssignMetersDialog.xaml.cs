// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.AssignMetersDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Orders
{
  public partial class AssignMetersDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadBusyIndicator BusyIndicator;
    internal Button StartTestReceptionButton;
    internal Button ReadTestReceptionButton;
    internal Button AssignMetersButton;
    internal Button ManuallyAssignMetersButton;
    internal Button AssignMetersFromEntrancesButton;
    internal Button OkButton;
    private bool _contentLoaded;

    public AssignMetersDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~AssignMetersDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/assignmetersdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        case 2:
          this.StartTestReceptionButton = (Button) target;
          break;
        case 3:
          this.ReadTestReceptionButton = (Button) target;
          break;
        case 4:
          this.AssignMetersButton = (Button) target;
          break;
        case 5:
          this.ManuallyAssignMetersButton = (Button) target;
          break;
        case 6:
          this.AssignMetersFromEntrancesButton = (Button) target;
          break;
        case 7:
          this.OkButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
