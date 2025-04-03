// Decompiled with JetBrains decompiler
// Type: CommonWPF.ExceptionViewer
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using NLog;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace CommonWPF
{
  public partial class ExceptionViewer : Window, IComponentConnector
  {
    private static Logger ExceptionLogger = LogManager.GetLogger(nameof (ExceptionLogger));
    private List<TextBlock> StackTraceBlocks;
    private StringBuilder exceptionText = new StringBuilder();
    internal StackPanel StackPanalAnchor;
    internal CheckBox CheckBoxStackTrace;
    internal Button ButtonCopy;
    internal TextBlock TextBlockHeaderText;
    internal ExceptionLayer ExceptionLayer1;
    private bool _contentLoaded;

    public static void Show(Exception ex, string headerInfo = null)
    {
      new ExceptionViewer(ex, headerInfo).ShowDialog();
    }

    public ExceptionViewer(Exception ex, string headerInfo)
    {
      this.InitializeComponent();
      if (!string.IsNullOrEmpty(headerInfo))
      {
        this.exceptionText.AppendLine(headerInfo);
        this.TextBlockHeaderText.Text = headerInfo;
        this.TextBlockHeaderText.Visibility = Visibility.Visible;
      }
      else
        this.TextBlockHeaderText.Visibility = Visibility.Collapsed;
      this.exceptionText.AppendLine(ex.ToString());
      this.ExceptionLayer1.TextBlockMessage.Text = ex.GetType().Name + Environment.NewLine + ex.Message;
      this.ExceptionLayer1.TextBlockStackTrace.Text = ex.StackTrace;
      this.StackTraceBlocks = new List<TextBlock>();
      this.StackTraceBlocks.Add(this.ExceptionLayer1.TextBlockStackTrace);
      this.WorkInnerException(ex, this.ExceptionLayer1);
      ExceptionViewer.ExceptionLogger.Error(this.exceptionText.ToString());
    }

    private void WorkInnerException(Exception theException, ExceptionLayer workLayer)
    {
      StackPanel stackPanel = new StackPanel();
      workLayer.NextLayer.Content = (object) stackPanel;
      if (theException.InnerException != null)
      {
        ExceptionLayer exceptionLayer = new ExceptionLayer();
        exceptionLayer.LabelInnerException.Visibility = Visibility.Visible;
        exceptionLayer.Margin = new Thickness(20.0, 20.0, 0.0, 0.0);
        exceptionLayer.TextBlockMessage.Text = theException.InnerException.GetType().Name + Environment.NewLine + theException.InnerException.Message;
        exceptionLayer.TextBlockStackTrace.Text = theException.InnerException.StackTrace;
        stackPanel.Children.Add((UIElement) exceptionLayer);
        this.StackTraceBlocks.Add(exceptionLayer.TextBlockStackTrace);
        if (theException.InnerException.InnerException != null)
          this.WorkInnerException(theException.InnerException, exceptionLayer);
      }
      if (!(theException is AggregateException))
        return;
      foreach (Exception innerException in ((AggregateException) theException).InnerExceptions)
      {
        ExceptionLayer exceptionLayer = new ExceptionLayer();
        exceptionLayer.LabelInnerException.Content = (object) "Inner aggregate exception";
        exceptionLayer.LabelInnerException.Visibility = Visibility.Visible;
        exceptionLayer.Margin = new Thickness(0.0, 20.0, 0.0, 0.0);
        exceptionLayer.TextBlockMessage.Text = innerException.GetType().Name + Environment.NewLine + innerException.Message;
        exceptionLayer.TextBlockStackTrace.Text = innerException.StackTrace;
        stackPanel.Children.Add((UIElement) exceptionLayer);
        this.StackTraceBlocks.Add(exceptionLayer.TextBlockStackTrace);
        this.exceptionText.AppendLine();
        this.exceptionText.AppendLine("Exception at aggregate exceptions list");
        this.exceptionText.AppendLine(innerException.ToString());
        if (innerException.InnerException != null)
          this.WorkInnerException(innerException, exceptionLayer);
      }
    }

    private void CheckBoxStackTrace_Checked(object sender, RoutedEventArgs e)
    {
      this.WindowState = WindowState.Maximized;
      foreach (UIElement stackTraceBlock in this.StackTraceBlocks)
        stackTraceBlock.Visibility = Visibility.Visible;
    }

    private void CheckBoxStackTrace_Unchecked(object sender, RoutedEventArgs e)
    {
      foreach (UIElement stackTraceBlock in this.StackTraceBlocks)
        stackTraceBlock.Visibility = Visibility.Collapsed;
    }

    private void ButtonCopy_Click(object sender, RoutedEventArgs e)
    {
      Clipboard.SetDataObject((object) this.exceptionText.ToString());
      int num = (int) MessageBox.Show("Exception text copied to clipboard");
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/exceptionviewer.xaml", UriKind.Relative));
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
          this.StackPanalAnchor = (StackPanel) target;
          break;
        case 2:
          this.CheckBoxStackTrace = (CheckBox) target;
          this.CheckBoxStackTrace.Checked += new RoutedEventHandler(this.CheckBoxStackTrace_Checked);
          this.CheckBoxStackTrace.Unchecked += new RoutedEventHandler(this.CheckBoxStackTrace_Unchecked);
          break;
        case 3:
          this.ButtonCopy = (Button) target;
          this.ButtonCopy.Click += new RoutedEventHandler(this.ButtonCopy_Click);
          break;
        case 4:
          this.TextBlockHeaderText = (TextBlock) target;
          break;
        case 5:
          this.ExceptionLayer1 = (ExceptionLayer) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
