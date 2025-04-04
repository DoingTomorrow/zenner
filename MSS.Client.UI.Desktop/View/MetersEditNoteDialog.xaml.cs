// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Meters.EditNoteDialog
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
using System.Windows.Media;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Meters
{
  public partial class EditNoteDialog : ResizableMetroWindow, IComponentConnector
  {
    internal Grid NotesGridTemplate;
    internal TextBox Description;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public EditNoteDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.Description.Foreground = (Brush) Brushes.Gray;
    }

    ~EditNoteDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.Description.GotKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.Tb_GotKeyboardFocus);
      this.Description.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.Tb_LostKeyboardFocus);
    }

    private void Tb_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      if (!(sender is TextBox))
        return;
      TextBox textBox = sender as TextBox;
      if (textBox.Foreground == Brushes.Gray && textBox.Text == MSS.Localisation.Resources.MSS_Client_AddDescription)
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

    private new void Maximize_Window(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private new void Drag_Window(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.DragMove();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/meters/editnotedialog.xaml", UriKind.Relative));
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
        case 3:
          this.OkButton = (Button) target;
          break;
        case 4:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
