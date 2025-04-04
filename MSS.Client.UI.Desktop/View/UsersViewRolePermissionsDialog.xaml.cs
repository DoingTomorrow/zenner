// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Users.ViewRolePermissionsDialog
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
namespace MSS.Client.UI.Desktop.View.Users
{
  public partial class ViewRolePermissionsDialog : ResizableMetroWindow, IComponentConnector
  {
    private int _currentPagingNumber = 0;
    internal RadGridView RolePermissionsGridView;
    internal RadDataPager RolePermissionsRadDataPager;
    internal Button CancelButton;
    private bool _contentLoaded;

    public ViewRolePermissionsDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~ViewRolePermissionsDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.RolePermissionsRadDataPager.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
    }

    public void OnPageIndexChange(object sender, PageIndexChangedEventArgs e)
    {
      if (e.NewPageIndex == this._currentPagingNumber)
        return;
      if (e.OldPageIndex >= 0)
        this._currentPagingNumber = e.NewPageIndex;
      else
        ((RadDataPager) sender).MoveToPage(this._currentPagingNumber);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/users/viewrolepermissionsdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.RolePermissionsGridView = (RadGridView) target;
          break;
        case 2:
          this.RolePermissionsRadDataPager = (RadDataPager) target;
          this.RolePermissionsRadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 3:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
