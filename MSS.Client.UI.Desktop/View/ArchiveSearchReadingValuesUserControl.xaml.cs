// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Archive.SearchReadingValuesUserControl
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

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
namespace MSS.Client.UI.Desktop.View.Archive
{
  public partial class SearchReadingValuesUserControl : UserControl, IComponentConnector
  {
    internal RadBusyIndicator BusyIndicator;
    internal TextBox SearchReadingValueBySerialNumberTextBox;
    internal RadGridView ArchiveReadingValueGridView;
    private bool _contentLoaded;

    public SearchReadingValuesUserControl()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/archive/searchreadingvaluesusercontrol.xaml", UriKind.Relative));
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
          this.SearchReadingValueBySerialNumberTextBox = (TextBox) target;
          break;
        case 3:
          this.ArchiveReadingValueGridView = (RadGridView) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
