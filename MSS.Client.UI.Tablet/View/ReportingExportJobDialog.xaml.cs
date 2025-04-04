// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Reporting.ExportJobDialog
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
namespace MSS.Client.UI.Tablet.View.Reporting
{
  public partial class ExportJobDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadComboBox JobPeriodicityComboBox;
    internal RadDateTimePicker StartDateTimePicker;
    internal CheckBox JobArchiveAfterExportCheckBox;
    internal CheckBox JobDeleteAfterExportCheckBox;
    internal RadRadioButton IntervalButton;
    internal RadListBox CountriesListBox;
    internal RadListBox SelectedCountries;
    internal RadRadioButton CsvRadioButton;
    internal Button BrowseFolderButton;
    internal Button AddButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public ExportJobDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      LocalizationManager.Manager = new LocalizationManager()
      {
        Culture = MSS.Localisation.Resources.Culture
      };
    }

    ~ExportJobDialog()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/reporting/exportjobdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.JobPeriodicityComboBox = (RadComboBox) target;
          break;
        case 2:
          this.StartDateTimePicker = (RadDateTimePicker) target;
          break;
        case 3:
          this.JobArchiveAfterExportCheckBox = (CheckBox) target;
          break;
        case 4:
          this.JobDeleteAfterExportCheckBox = (CheckBox) target;
          break;
        case 5:
          this.IntervalButton = (RadRadioButton) target;
          break;
        case 6:
          this.CountriesListBox = (RadListBox) target;
          break;
        case 7:
          this.SelectedCountries = (RadListBox) target;
          break;
        case 8:
          this.CsvRadioButton = (RadRadioButton) target;
          break;
        case 9:
          this.BrowseFolderButton = (Button) target;
          break;
        case 10:
          this.AddButton = (Button) target;
          break;
        case 11:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
