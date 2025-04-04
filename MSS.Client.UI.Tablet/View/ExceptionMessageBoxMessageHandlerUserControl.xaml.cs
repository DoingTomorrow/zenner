// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.ExceptionMessageBox.MessageHandlerUserControl
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

#nullable disable
namespace MSS.Client.UI.Tablet.View.ExceptionMessageBox
{
  public partial class MessageHandlerUserControl : UserControl, IComponentConnector
  {
    internal Label label_Success;
    internal Label label_Warning;
    internal Label label_Validation;
    private bool _contentLoaded;

    public MessageHandlerUserControl() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/exceptionmessagebox/messagehandlerusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.label_Success = (Label) target;
          break;
        case 2:
          this.label_Warning = (Label) target;
          break;
        case 3:
          this.label_Validation = (Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
