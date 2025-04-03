// Decompiled with JetBrains decompiler
// Type: HandlerLib.View.MBus_IO_Management
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib.View
{
  public partial class MBus_IO_Management : Window, IComponentConnector
  {
    private uint OkCounts = 0;
    private uint ErrorCounts = 0;
    private Common16BitCommands Commands16Bit;
    internal Button ButtonSingleAccess;
    internal Button ButtonBreak;
    internal TextBlock TextBlockOkCounts;
    internal TextBlock TextBlockErrorCounts;
    internal CheckBox CheckBoxInputState1;
    internal CheckBox CheckBoxOutputState1;
    internal CheckBox CheckBoxOutputSet1;
    internal CheckBox CheckBoxOutputMask1;
    internal CheckBox CheckBoxInputState2;
    internal CheckBox CheckBoxOutputState2;
    internal CheckBox CheckBoxOutputSet2;
    internal CheckBox CheckBoxOutputMask2;
    internal CheckBox CheckBoxInputState3;
    internal CheckBox CheckBoxOutputState3;
    internal CheckBox CheckBoxOutputSet3;
    internal CheckBox CheckBoxOutputMask3;
    private bool _contentLoaded;

    public MBus_IO_Management(Common16BitCommands commands16Bit)
    {
      this.Commands16Bit = commands16Bit;
      this.InitializeComponent();
    }

    private void ButtonSingleAccess_Click(object sender, RoutedEventArgs e)
    {
      ProgressHandler progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgressHandler));
      CancellationToken token = new CancellationToken();
      uint NewOutputMask = 0;
      uint NewOutputState = 0;
      uint OldOutputState = 0;
      uint OldInputState = 0;
      if (this.CheckBoxOutputSet1.IsChecked.Value)
        NewOutputState |= 1U;
      if (this.CheckBoxOutputSet2.IsChecked.Value)
        NewOutputState |= 2U;
      if (this.CheckBoxOutputSet3.IsChecked.Value)
        NewOutputState |= 4U;
      if (this.CheckBoxOutputMask1.IsChecked.Value)
        NewOutputMask |= 1U;
      if (this.CheckBoxOutputMask2.IsChecked.Value)
        NewOutputMask |= 2U;
      if (this.CheckBoxOutputMask3.IsChecked.Value)
        NewOutputMask |= 4U;
      try
      {
        this.Commands16Bit.DigitalInputsAndOutputs(progress, token, NewOutputMask, NewOutputState, ref OldOutputState, ref OldInputState);
        ++this.OkCounts;
        this.TextBlockOkCounts.Text = this.OkCounts.ToString();
      }
      catch
      {
        ++this.ErrorCounts;
        this.TextBlockErrorCounts.Text = this.ErrorCounts.ToString();
        return;
      }
      this.CheckBoxInputState1.IsChecked = new bool?((OldInputState & 1U) > 0U);
      this.CheckBoxInputState2.IsChecked = new bool?((OldInputState & 2U) > 0U);
      this.CheckBoxInputState3.IsChecked = new bool?((OldInputState & 4U) > 0U);
      this.CheckBoxOutputState1.IsChecked = new bool?((OldInputState & 1U) > 0U);
      this.CheckBoxOutputState2.IsChecked = new bool?((OldInputState & 2U) > 0U);
      this.CheckBoxOutputState3.IsChecked = new bool?((OldInputState & 4U) > 0U);
    }

    private void OnProgressHandler(ProgressArg obj)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/mbus_io_management.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonSingleAccess = (Button) target;
          this.ButtonSingleAccess.Click += new RoutedEventHandler(this.ButtonSingleAccess_Click);
          break;
        case 2:
          this.ButtonBreak = (Button) target;
          break;
        case 3:
          this.TextBlockOkCounts = (TextBlock) target;
          break;
        case 4:
          this.TextBlockErrorCounts = (TextBlock) target;
          break;
        case 5:
          this.CheckBoxInputState1 = (CheckBox) target;
          break;
        case 6:
          this.CheckBoxOutputState1 = (CheckBox) target;
          break;
        case 7:
          this.CheckBoxOutputSet1 = (CheckBox) target;
          break;
        case 8:
          this.CheckBoxOutputMask1 = (CheckBox) target;
          break;
        case 9:
          this.CheckBoxInputState2 = (CheckBox) target;
          break;
        case 10:
          this.CheckBoxOutputState2 = (CheckBox) target;
          break;
        case 11:
          this.CheckBoxOutputSet2 = (CheckBox) target;
          break;
        case 12:
          this.CheckBoxOutputMask2 = (CheckBox) target;
          break;
        case 13:
          this.CheckBoxInputState3 = (CheckBox) target;
          break;
        case 14:
          this.CheckBoxOutputState3 = (CheckBox) target;
          break;
        case 15:
          this.CheckBoxOutputSet3 = (CheckBox) target;
          break;
        case 16:
          this.CheckBoxOutputMask3 = (CheckBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
