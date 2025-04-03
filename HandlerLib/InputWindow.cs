// Decompiled with JetBrains decompiler
// Type: HandlerLib.InputWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace HandlerLib
{
  public class InputWindow : Window, IComponentConnector
  {
    internal TextBox InputText;
    internal Button OK_Button;
    private bool _contentLoaded;

    public InputWindow() => this.InitializeComponent();

    private void OkButton_Click(object sender, RoutedEventArgs e) => this.Close();

    public static string Show(string title, string defaultValue)
    {
      InputWindow inputWindow = new InputWindow();
      inputWindow.Title = title;
      inputWindow.InputText.Text = defaultValue;
      inputWindow.ShowDialog();
      return inputWindow.InputText.Text;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/util/inputwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.InputText = (TextBox) target;
          break;
        case 2:
          this.OK_Button = (Button) target;
          this.OK_Button.Click += new RoutedEventHandler(this.OkButton_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
