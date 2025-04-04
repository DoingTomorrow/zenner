// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Meters.AddNoteDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.CustomControls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Meters
{
  public partial class AddNoteDialog : KeyboardMetroWindow, IComponentConnector
  {
    internal Grid NotesGridTemplate;
    internal TextBox Description;
    private bool _contentLoaded;

    public AddNoteDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.Description.Foreground = (Brush) Brushes.Gray;
    }

    ~AddNoteDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.Description.GotKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.Tb_GotKeyboardFocus);
      this.Description.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.Tb_LostKeyboardFocus);
    }

    private void Tb_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      if (!(sender is TextBox))
        return;
      TextBox textBox = sender as TextBox;
      if (textBox.Foreground == Brushes.Gray)
      {
        textBox.Text = "";
        textBox.Foreground = (Brush) Brushes.Black;
      }
    }

    private void Tb_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      if (!(sender is TextBox))
        return;
      TextBox textBox = sender as TextBox;
      if (textBox.Text.Trim().Equals(""))
      {
        textBox.Foreground = (Brush) Brushes.Gray;
        textBox.Text = MSS.Localisation.Resources.MSS_Client_AddDescription;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/meters/addnotedialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.NotesGridTemplate = (Grid) target;
          break;
        case 2:
          this.Description = (TextBox) target;
          this.Description.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(this.Tb_GotKeyboardFocus);
          this.Description.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.Tb_LostKeyboardFocus);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
