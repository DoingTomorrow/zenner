// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Structures.SetEvaluationFactorDialog
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
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Structures
{
  public partial class SetEvaluationFactorDialog : 
    ResizableMetroWindow,
    IComponentConnector,
    IStyleConnector
  {
    internal RadTreeListView treeListView;
    internal RadGridView HeaterGridView;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public SetEvaluationFactorDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~SetEvaluationFactorDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      GridViewRow gridViewRow = ((DependencyObject) sender).ParentOfType<GridViewRow>();
      if (gridViewRow == null)
        return;
      gridViewRow.IsSelected = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/structures/setevaluationfactordialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.treeListView = (RadTreeListView) target;
          break;
        case 2:
          this.HeaterGridView = (RadGridView) target;
          break;
        case 4:
          this.OkButton = (Button) target;
          break;
        case 5:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 3)
        return;
      ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_OnClick);
    }
  }
}
