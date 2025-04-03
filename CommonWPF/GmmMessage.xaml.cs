// Decompiled with JetBrains decompiler
// Type: CommonWPF.GmmMessage
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
using System.Windows.Media;

#nullable disable
namespace CommonWPF
{
  public partial class GmmMessage : Window, IComponentConnector
  {
    private bool WindowIsModal = false;
    internal Button ButtonCancel;
    internal Button ButtonOk;
    internal Button ButtonPrint;
    internal TextBox TextBoxMessage;
    private bool _contentLoaded;

    public static void Show(string message, string headerInfo = null, bool useCourier = false)
    {
      GmmMessage gmmMessage = new GmmMessage();
      gmmMessage.TextBoxMessage.Text = message;
      if (headerInfo != null)
        gmmMessage.Title = headerInfo;
      if (useCourier)
        gmmMessage.TextBoxMessage.FontFamily = new FontFamily("Courier New");
      gmmMessage.WindowIsModal = true;
      gmmMessage.ShowDialog();
    }

    public static void Show_Ok(string message, string headerInfo = null, bool useCourier = false)
    {
      GmmMessage gmmMessage = new GmmMessage();
      gmmMessage.TextBoxMessage.Text = message;
      if (headerInfo != null)
        gmmMessage.Title = headerInfo;
      if (useCourier)
        gmmMessage.TextBoxMessage.FontFamily = new FontFamily("Courier New");
      gmmMessage.ShowDialog();
    }

    public static bool Show_OkCancel(string message, string headerInfo = null, bool useCurier = false)
    {
      GmmMessage gmmMessage = new GmmMessage(true);
      gmmMessage.TextBoxMessage.Text = message;
      if (headerInfo != null)
        gmmMessage.Title = headerInfo;
      if (useCurier)
        gmmMessage.TextBoxMessage.FontFamily = new FontFamily("Courier New");
      gmmMessage.ShowDialog();
      return gmmMessage.DialogResult.HasValue && gmmMessage.DialogResult.Value;
    }

    internal GmmMessage(bool showCancle = false)
    {
      this.InitializeComponent();
      if (!showCancle)
        return;
      this.ButtonCancel.Visibility = Visibility.Visible;
    }

    private void ButtonOk_Click(object sender, RoutedEventArgs e)
    {
      if (!this.WindowIsModal)
        this.DialogResult = new bool?(true);
      this.Close();
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      if (!this.WindowIsModal)
        this.DialogResult = new bool?(false);
      this.Close();
    }

    private void ButtonPrint_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        PrintDialog printDialog = new PrintDialog();
        bool? nullable = printDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        printDialog.PrintVisual((Visual) this.TextBoxMessage, "");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/gmmmessage.xaml", UriKind.Relative));
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
          this.ButtonCancel = (Button) target;
          this.ButtonCancel.Click += new RoutedEventHandler(this.ButtonCancel_Click);
          break;
        case 2:
          this.ButtonOk = (Button) target;
          this.ButtonOk.Click += new RoutedEventHandler(this.ButtonOk_Click);
          break;
        case 3:
          this.ButtonPrint = (Button) target;
          this.ButtonPrint.Click += new RoutedEventHandler(this.ButtonPrint_Click);
          break;
        case 4:
          this.TextBoxMessage = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
