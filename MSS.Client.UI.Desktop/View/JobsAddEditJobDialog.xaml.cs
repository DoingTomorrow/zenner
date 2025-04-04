// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Jobs.AddEditJobDialog
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
namespace MSS.Client.UI.Desktop.View.Jobs
{
  public partial class AddEditJobDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadRadioButton MbusRadioButton;
    internal RadRadioButton MinomatRadioButton;
    internal RadComboBox songsAutoCompleteBox;
    internal Button BtnStructure;
    internal Button StructureButton;
    internal Button OkButton1;
    internal Button CancelButton1;
    private bool _contentLoaded;

    public AddEditJobDialog()
    {
      this.InitializeComponent();
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~AddEditJobDialog()
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/jobs/addeditjobdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.MbusRadioButton = (RadRadioButton) target;
          break;
        case 2:
          this.MinomatRadioButton = (RadRadioButton) target;
          break;
        case 3:
          this.songsAutoCompleteBox = (RadComboBox) target;
          break;
        case 4:
          this.BtnStructure = (Button) target;
          break;
        case 5:
          this.StructureButton = (Button) target;
          break;
        case 6:
          this.OkButton1 = (Button) target;
          break;
        case 7:
          this.CancelButton1 = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
