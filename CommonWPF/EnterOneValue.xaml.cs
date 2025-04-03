// Decompiled with JetBrains decompiler
// Type: CommonWPF.EnterOneValue
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace CommonWPF
{
  public partial class EnterOneValue : Window, IComponentConnector
  {
    private string returnValue;
    internal Button ButtonOk;
    internal TextBlock TextBlockInfo;
    internal TextBox TextBoxValue;
    private bool _contentLoaded;

    private EnterOneValue(string title, string info, string defaultValue)
    {
      this.InitializeComponent();
      this.Title = title;
      this.TextBlockInfo.Text = info;
      this.TextBoxValue.Text = defaultValue;
    }

    public static string GetOneValue(string title, string info, string defaultValue = null)
    {
      EnterOneValue enterOneValue = new EnterOneValue(title, info, defaultValue);
      enterOneValue.ShowDialog();
      return enterOneValue.returnValue;
    }

    private void ButtonOk_Click(object sender, RoutedEventArgs e)
    {
      this.returnValue = this.TextBoxValue.Text.Trim();
      this.Close();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/enteronevalue.xaml", UriKind.Relative));
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
          this.ButtonOk = (Button) target;
          this.ButtonOk.Click += new RoutedEventHandler(this.ButtonOk_Click);
          break;
        case 2:
          this.TextBlockInfo = (TextBlock) target;
          break;
        case 3:
          this.TextBoxValue = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
