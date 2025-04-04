// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.DataFilters.UpdateRuleDialog
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
  public partial class UpdateRuleDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadComboBox PhysicalQuantityComboBox;
    internal RadComboBox MeterTypeComboBox;
    internal RadComboBox CalculatioComboBox;
    internal RadComboBox CalculationStartComboBox;
    internal RadComboBox StorageIntervalComboBox;
    internal RadComboBox CreationComboBox;
    internal RadNumericUpDown IndexNumericUpDown;
    internal System.Windows.Controls.Label ValueIdent;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public UpdateRuleDialog()
    {
      this.InitializeComponent();
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~UpdateRuleDialog()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/datafilters/updateruledialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.PhysicalQuantityComboBox = (RadComboBox) target;
          break;
        case 2:
          this.MeterTypeComboBox = (RadComboBox) target;
          break;
        case 3:
          this.CalculatioComboBox = (RadComboBox) target;
          break;
        case 4:
          this.CalculationStartComboBox = (RadComboBox) target;
          break;
        case 5:
          this.StorageIntervalComboBox = (RadComboBox) target;
          break;
        case 6:
          this.CreationComboBox = (RadComboBox) target;
          break;
        case 7:
          this.IndexNumericUpDown = (RadNumericUpDown) target;
          break;
        case 8:
          this.ValueIdent = (System.Windows.Controls.Label) target;
          break;
        case 9:
          this.OkButton = (Button) target;
          break;
        case 10:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
