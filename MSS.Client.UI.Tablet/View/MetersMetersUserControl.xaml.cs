// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Meters.MetersUserControl
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
namespace MSS.Client.UI.Tablet.View.Meters
{
  public partial class MetersUserControl : UserControl, IComponentConnector
  {
    internal Button button_UnhandledException;
    internal Button button_HandledException;
    internal Button button_Success;
    internal Button button_Cancel;
    internal Button button_Validation;
    private bool _contentLoaded;

    public MetersUserControl()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/meters/metersusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.button_UnhandledException = (Button) target;
          break;
        case 2:
          this.button_HandledException = (Button) target;
          break;
        case 3:
          this.button_Success = (Button) target;
          break;
        case 4:
          this.button_Cancel = (Button) target;
          break;
        case 5:
          this.button_Validation = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
