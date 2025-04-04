// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Orders.PrintPreviewWindow
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
  public partial class PrintPreviewWindow : ResizableMetroWindow, IComponentConnector
  {
    internal Grid OrderGrid;
    internal System.Windows.Controls.Label ExportedOrderLabel;
    internal Grid StructureRootGrid;
    internal RadTreeListView treeListView;
    private bool _contentLoaded;

    public PrintPreviewWindow()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    ~PrintPreviewWindow()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/orders/printpreviewwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.OrderGrid = (Grid) target;
          break;
        case 2:
          this.ExportedOrderLabel = (System.Windows.Controls.Label) target;
          break;
        case 3:
          this.StructureRootGrid = (Grid) target;
          break;
        case 4:
          this.treeListView = (RadTreeListView) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
