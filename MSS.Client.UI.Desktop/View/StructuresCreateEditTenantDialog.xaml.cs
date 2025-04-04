// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Structures.CreateEditTenantDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Structures
{
  public partial class CreateEditTenantDialog : ResizableMetroWindow, IComponentConnector
  {
    internal TextBox TenantNrTextBox;
    internal RadComboBox FloorNameBox;
    internal RadComboBox DirectionBox;
    internal Button AddButton;
    internal Button EditButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public CreateEditTenantDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~CreateEditTenantDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.TenantNrTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = CreateEditTenantDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/structures/createedittenantdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TenantNrTextBox = (TextBox) target;
          this.TenantNrTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 2:
          this.FloorNameBox = (RadComboBox) target;
          break;
        case 3:
          this.DirectionBox = (RadComboBox) target;
          break;
        case 4:
          this.AddButton = (Button) target;
          break;
        case 5:
          this.EditButton = (Button) target;
          break;
        case 6:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
