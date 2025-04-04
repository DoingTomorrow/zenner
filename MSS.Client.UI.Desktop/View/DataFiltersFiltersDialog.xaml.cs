// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.DataFilters.FiltersDialog
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
namespace MSS.Client.UI.Desktop.View.DataFilters
{
  public partial class FiltersDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadGridView FiltersGridView;
    internal RadGridView RulesGridView;
    internal Button AddFilter;
    internal TextBlock AddFilterTextBlock;
    internal Button UpdateFilter;
    internal TextBlock UpdateFilterTextBlock;
    internal Button RemoveFilter;
    internal TextBlock RemoveFilterTextBlock;
    internal Button AddRule;
    internal TextBlock AddRuleTextBlock;
    internal Button UpdateRule;
    internal TextBlock UpdateRuleTextBlock;
    internal Button RemoveRule;
    internal TextBlock RemoveRuleTextBlock;
    internal Button OkButton;
    private bool _contentLoaded;

    public FiltersDialog()
    {
      this.InitializeComponent();
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~FiltersDialog()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/datafilters/filtersdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.FiltersGridView = (RadGridView) target;
          break;
        case 2:
          this.RulesGridView = (RadGridView) target;
          break;
        case 3:
          this.AddFilter = (Button) target;
          break;
        case 4:
          this.AddFilterTextBlock = (TextBlock) target;
          break;
        case 5:
          this.UpdateFilter = (Button) target;
          break;
        case 6:
          this.UpdateFilterTextBlock = (TextBlock) target;
          break;
        case 7:
          this.RemoveFilter = (Button) target;
          break;
        case 8:
          this.RemoveFilterTextBlock = (TextBlock) target;
          break;
        case 9:
          this.AddRule = (Button) target;
          break;
        case 10:
          this.AddRuleTextBlock = (TextBlock) target;
          break;
        case 11:
          this.UpdateRule = (Button) target;
          break;
        case 12:
          this.UpdateRuleTextBlock = (TextBlock) target;
          break;
        case 13:
          this.RemoveRule = (Button) target;
          break;
        case 14:
          this.RemoveRuleTextBlock = (TextBlock) target;
          break;
        case 15:
          this.OkButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
