// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Jobs.AssignStructureDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

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
namespace MSS.Client.UI.Desktop.View.Jobs
{
  public partial class AssignStructureDialog : ResizableMetroWindow, IComponentConnector
  {
    internal TextBlock SearchTextBlock;
    internal TextBox SearchTextBox;
    internal MouseBinding MouseBinding;
    internal Grid StructureRootGrid;
    internal RadTreeListView treeListView;
    internal Button OkButton;
    internal Button CanceButton;
    private bool _contentLoaded;

    public AssignStructureDialog()
    {
      this.InitializeComponent();
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~AssignStructureDialog()
    {
      --this.MouseBinding.MouseAction;
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/jobs/assignstructuredialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.SearchTextBlock = (TextBlock) target;
          break;
        case 2:
          this.SearchTextBox = (TextBox) target;
          break;
        case 3:
          this.MouseBinding = (MouseBinding) target;
          break;
        case 4:
          this.StructureRootGrid = (Grid) target;
          break;
        case 5:
          this.treeListView = (RadTreeListView) target;
          break;
        case 6:
          this.OkButton = (Button) target;
          break;
        case 7:
          this.CanceButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
