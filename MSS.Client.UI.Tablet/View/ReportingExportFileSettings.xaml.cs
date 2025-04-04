// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Reporting.ExportFileSettings
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
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Reporting
{
  public partial class ExportFileSettings : ResizableMetroWindow, IComponentConnector
  {
    internal RadRadioButton IntervalButton;
    internal RadListBox CountriesListBox;
    internal RadListBox SelectedCountries;
    internal RadRadioButton CsvRadioButton;
    internal Button ExportButton;
    private bool _contentLoaded;

    public ExportFileSettings()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
    }

    ~ExportFileSettings()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/reporting/exportfilesettings.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.IntervalButton = (RadRadioButton) target;
          break;
        case 2:
          this.CountriesListBox = (RadListBox) target;
          break;
        case 3:
          this.SelectedCountries = (RadListBox) target;
          break;
        case 4:
          this.CsvRadioButton = (RadRadioButton) target;
          break;
        case 5:
          this.ExportButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
