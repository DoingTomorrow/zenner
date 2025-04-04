// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Archive.ArchivingUserControl
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Archive
{
  public partial class ArchivingUserControl : UserControl, IComponentConnector
  {
    internal Button ArchiveAndKeepData;
    internal Button ArchiveAndDeleteData;
    internal RadCalendar StartDateCalendar;
    internal RadCalendar EndDateCalendar;
    internal TextBlock ArchiveSelectTitle;
    internal RadGridView ArchivedEntitiesGridView;
    internal System.Windows.Controls.Label FoundNotFoundDataInfo;
    internal Button ExportButton;
    internal RadRadioButton SearchReadingValuesButton;
    internal RadRadioButton SearchOrdersButton;
    internal RadRadioButton SearchStructuresButton;
    internal RadRadioButton SearchJobsButton;
    internal RadRadioButton SearchLogsButton;
    private bool _contentLoaded;

    public ArchivingUserControl()
    {
      this.InitializeComponent();
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/archive/archivingusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ArchiveAndKeepData = (Button) target;
          break;
        case 2:
          this.ArchiveAndDeleteData = (Button) target;
          break;
        case 3:
          this.StartDateCalendar = (RadCalendar) target;
          break;
        case 4:
          this.EndDateCalendar = (RadCalendar) target;
          break;
        case 5:
          this.ArchiveSelectTitle = (TextBlock) target;
          break;
        case 6:
          this.ArchivedEntitiesGridView = (RadGridView) target;
          break;
        case 7:
          this.FoundNotFoundDataInfo = (System.Windows.Controls.Label) target;
          break;
        case 8:
          this.ExportButton = (Button) target;
          break;
        case 9:
          this.SearchReadingValuesButton = (RadRadioButton) target;
          break;
        case 10:
          this.SearchOrdersButton = (RadRadioButton) target;
          break;
        case 11:
          this.SearchStructuresButton = (RadRadioButton) target;
          break;
        case 12:
          this.SearchJobsButton = (RadRadioButton) target;
          break;
        case 13:
          this.SearchLogsButton = (RadRadioButton) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
