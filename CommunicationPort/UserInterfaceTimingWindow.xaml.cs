// Decompiled with JetBrains decompiler
// Type: CommunicationPort.UserInterface.TimingWindow
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace CommunicationPort.UserInterface
{
  public partial class TimingWindow : Window, IComponentConnector
  {
    private CommunicationPortWindowFunctions myWindowFunctions;
    private bool formInit;
    internal GmmCorporateControl gmmCorporateControl1;
    internal TextBox TextBoxWakeup;
    internal TextBlock TextBlockWakeup;
    internal TextBox TextBoxAfterWaktup;
    internal TextBlock TextBlockAfterWaktup;
    internal TextBox TextBoxBeforFirstByte;
    internal TextBlock TextBlockBeforFirstByte;
    internal TextBox TextBoxByteOffset;
    internal TextBlock TextBlockByteOffset;
    internal TextBox TextBoxBlockOffset;
    internal TextBlock TextBlockBlockOffset;
    internal TextBox TextBoxWakeupIntervall;
    internal TextBlock TextBlockWakeupRepeat;
    internal TextBox TextBoxOffset;
    internal TextBlock TextBlockOffset;
    internal TextBox TextBoxBeforNextRequest;
    internal TextBlock TextBlockBeforNextRequest;
    internal TextBox TextBoxAfterOpen;
    internal TextBlock TextBlockAfterOpen;
    internal TextBox TextBoxBeforeRepeat;
    internal TextBlock TextBlockBeforeRepeat;
    internal TextBox TextBoxMiConPowerOffTime;
    internal TextBlock TextBlockMiConPowerOffTime;
    internal TextBox TextBoxIrDaPulsLength;
    internal TextBlock TextBlockIrDaPulsLength;
    internal TextBox TextBoxMiConStateTimeout;
    internal TextBlock TextBlockMiConStateTimeout;
    internal Button ButtonShowConfig;
    internal Button ButtonShowChanges;
    private bool _contentLoaded;

    public TimingWindow(CommunicationPortWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.formInit = true;
      this.InitializeComponent();
      this.RefreshAll();
      this.formInit = false;
    }

    private void RefreshAll()
    {
      this.TextBoxAfterOpen.Text = this.myWindowFunctions.portFunctions.configList.TransTime_AfterOpen.ToString();
      this.TextBoxBeforFirstByte.Text = this.myWindowFunctions.portFunctions.configList.RecTime_BeforFirstByte.ToString();
      this.TextBoxByteOffset.Text = this.myWindowFunctions.portFunctions.configList.RecTime_OffsetPerByte.ToString();
      this.TextBoxBlockOffset.Text = this.myWindowFunctions.portFunctions.configList.RecTime_OffsetPerBlock.ToString();
      this.TextBoxOffset.Text = this.myWindowFunctions.portFunctions.configList.RecTime_GlobalOffset.ToString();
      this.TextBoxBeforNextRequest.Text = this.myWindowFunctions.portFunctions.configList.RecTransTime.ToString();
      this.TextBoxBeforeRepeat.Text = this.myWindowFunctions.portFunctions.configList.WaitBeforeRepeatTime.ToString();
      this.TextBoxWakeup.Text = this.myWindowFunctions.portFunctions.configList.TransTime_BreakTime.ToString();
      this.TextBoxAfterWaktup.Text = this.myWindowFunctions.portFunctions.configList.TransTime_AfterBreak.ToString();
      this.TextBoxWakeupIntervall.Text = this.myWindowFunctions.portFunctions.configList.BreakIntervalTime.ToString();
      this.TextBoxMiConPowerOffTime.Text = this.myWindowFunctions.portFunctions.configList.MinoConnectPowerOffTime.ToString();
      this.TextBoxIrDaPulsLength.Text = this.myWindowFunctions.portFunctions.configList.MinoConnectIrDaPulseTime.ToString();
      this.TextBoxMiConStateTimeout.Text = this.myWindowFunctions.portFunctions.communicationObject.PollingErrorTime_ms.ToString();
    }

    private void TextBoxRefreshAllOn_LostFocus(object sender, RoutedEventArgs e)
    {
      if (this.formInit)
        return;
      this.RefreshAll();
    }

    private void TextBoxAfterOpen_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxAfterOpen.Text, out result))
        this.myWindowFunctions.portFunctions.configList.TransTime_AfterOpen = result;
      else
        this.TextBoxAfterOpen.Text = "";
    }

    private void TextBoxWakeup_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxWakeup.Text, out result))
        this.myWindowFunctions.portFunctions.configList.TransTime_BreakTime = result;
      else
        this.TextBoxWakeup.Text = "";
    }

    private void TextBoxAfterWaktup_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxAfterWaktup.Text, out result))
        this.myWindowFunctions.portFunctions.configList.TransTime_AfterBreak = result;
      else
        this.TextBoxAfterWaktup.Text = "";
    }

    private void TextBoxBeforFirstByte_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxBeforFirstByte.Text, out result))
        this.myWindowFunctions.portFunctions.configList.RecTime_BeforFirstByte = result;
      else
        this.TextBoxBeforFirstByte.Text = "";
    }

    private void TextBoxByteOffset_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxByteOffset.Text, out result))
        this.myWindowFunctions.portFunctions.configList.RecTime_OffsetPerByte = result;
      else
        this.TextBoxByteOffset.Text = "";
    }

    private void TextBoxBlockOffset_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxBlockOffset.Text, out result))
        this.myWindowFunctions.portFunctions.configList.RecTime_OffsetPerBlock = result;
      else
        this.TextBoxBlockOffset.Text = "";
    }

    private void TextBoxWakeupIntervall_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxWakeupIntervall.Text, out result))
        this.myWindowFunctions.portFunctions.configList.BreakIntervalTime = result;
      else
        this.TextBoxWakeupIntervall.Text = "";
    }

    private void TextBoxOffset_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxOffset.Text, out result))
        this.myWindowFunctions.portFunctions.configList.RecTime_GlobalOffset = result;
      else
        this.TextBoxOffset.Text = "";
    }

    private void TextBoxBeforNextRequest_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxBeforNextRequest.Text, out result))
        this.myWindowFunctions.portFunctions.configList.RecTransTime = result;
      else
        this.TextBoxBeforNextRequest.Text = "";
    }

    private void TextBoxBeforeRepeat_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxBeforeRepeat.Text, out result))
        this.myWindowFunctions.portFunctions.configList.WaitBeforeRepeatTime = result;
      else
        this.TextBoxBeforeRepeat.Text = "";
    }

    private void TextBoxMiConPowerOffTime_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxMiConPowerOffTime.Text, out result))
        this.myWindowFunctions.portFunctions.configList.MinoConnectPowerOffTime = result;
      else
        this.TextBoxMiConPowerOffTime.Text = "";
    }

    private void TextBoxIrDaPulsLength_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxIrDaPulsLength.Text, out result))
        this.myWindowFunctions.portFunctions.configList.MinoConnectIrDaPulseTime = result;
      else
        this.TextBoxIrDaPulsLength.Text = "";
    }

    private void TextBoxMiConStateTimeout_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.formInit)
        return;
      int result;
      if (int.TryParse(this.TextBoxMiConStateTimeout.Text, out result))
        this.myWindowFunctions.portFunctions.communicationObject.PollingErrorTime_ms = result;
      else
        this.TextBoxMiConStateTimeout.Text = "";
    }

    private void ButtonShowChanges_Click(object sender, RoutedEventArgs e)
    {
    }

    private void ButtonShowConfig_Click(object sender, RoutedEventArgs e)
    {
      ConfigList readoutConfiguration = this.myWindowFunctions.portFunctions.GetReadoutConfiguration();
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> keyValuePair in readoutConfiguration)
        stringBuilder.AppendLine(keyValuePair.Key + ": " + keyValuePair.Value);
      int num = (int) MessageBox.Show(stringBuilder.ToString(), "Current settings");
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommunicationPort;component/userinterface/timingwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 2:
          this.TextBoxWakeup = (TextBox) target;
          this.TextBoxWakeup.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxWakeup.TextChanged += new TextChangedEventHandler(this.TextBoxWakeup_TextChanged);
          break;
        case 3:
          this.TextBlockWakeup = (TextBlock) target;
          break;
        case 4:
          this.TextBoxAfterWaktup = (TextBox) target;
          this.TextBoxAfterWaktup.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxAfterWaktup.TextChanged += new TextChangedEventHandler(this.TextBoxAfterWaktup_TextChanged);
          break;
        case 5:
          this.TextBlockAfterWaktup = (TextBlock) target;
          break;
        case 6:
          this.TextBoxBeforFirstByte = (TextBox) target;
          this.TextBoxBeforFirstByte.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxBeforFirstByte.TextChanged += new TextChangedEventHandler(this.TextBoxBeforFirstByte_TextChanged);
          break;
        case 7:
          this.TextBlockBeforFirstByte = (TextBlock) target;
          break;
        case 8:
          this.TextBoxByteOffset = (TextBox) target;
          this.TextBoxByteOffset.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxByteOffset.TextChanged += new TextChangedEventHandler(this.TextBoxByteOffset_TextChanged);
          break;
        case 9:
          this.TextBlockByteOffset = (TextBlock) target;
          break;
        case 10:
          this.TextBoxBlockOffset = (TextBox) target;
          this.TextBoxBlockOffset.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxBlockOffset.TextChanged += new TextChangedEventHandler(this.TextBoxBlockOffset_TextChanged);
          break;
        case 11:
          this.TextBlockBlockOffset = (TextBlock) target;
          break;
        case 12:
          this.TextBoxWakeupIntervall = (TextBox) target;
          this.TextBoxWakeupIntervall.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxWakeupIntervall.TextChanged += new TextChangedEventHandler(this.TextBoxWakeupIntervall_TextChanged);
          break;
        case 13:
          this.TextBlockWakeupRepeat = (TextBlock) target;
          break;
        case 14:
          this.TextBoxOffset = (TextBox) target;
          this.TextBoxOffset.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxOffset.TextChanged += new TextChangedEventHandler(this.TextBoxOffset_TextChanged);
          break;
        case 15:
          this.TextBlockOffset = (TextBlock) target;
          break;
        case 16:
          this.TextBoxBeforNextRequest = (TextBox) target;
          this.TextBoxBeforNextRequest.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxBeforNextRequest.TextChanged += new TextChangedEventHandler(this.TextBoxBeforNextRequest_TextChanged);
          break;
        case 17:
          this.TextBlockBeforNextRequest = (TextBlock) target;
          break;
        case 18:
          this.TextBoxAfterOpen = (TextBox) target;
          this.TextBoxAfterOpen.TextChanged += new TextChangedEventHandler(this.TextBoxAfterOpen_TextChanged);
          this.TextBoxAfterOpen.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          break;
        case 19:
          this.TextBlockAfterOpen = (TextBlock) target;
          break;
        case 20:
          this.TextBoxBeforeRepeat = (TextBox) target;
          this.TextBoxBeforeRepeat.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxBeforeRepeat.TextChanged += new TextChangedEventHandler(this.TextBoxBeforeRepeat_TextChanged);
          break;
        case 21:
          this.TextBlockBeforeRepeat = (TextBlock) target;
          break;
        case 22:
          this.TextBoxMiConPowerOffTime = (TextBox) target;
          this.TextBoxMiConPowerOffTime.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxMiConPowerOffTime.TextChanged += new TextChangedEventHandler(this.TextBoxMiConPowerOffTime_TextChanged);
          break;
        case 23:
          this.TextBlockMiConPowerOffTime = (TextBlock) target;
          break;
        case 24:
          this.TextBoxIrDaPulsLength = (TextBox) target;
          this.TextBoxIrDaPulsLength.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxIrDaPulsLength.TextChanged += new TextChangedEventHandler(this.TextBoxIrDaPulsLength_TextChanged);
          break;
        case 25:
          this.TextBlockIrDaPulsLength = (TextBlock) target;
          break;
        case 26:
          this.TextBoxMiConStateTimeout = (TextBox) target;
          this.TextBoxMiConStateTimeout.LostFocus += new RoutedEventHandler(this.TextBoxRefreshAllOn_LostFocus);
          this.TextBoxMiConStateTimeout.TextChanged += new TextChangedEventHandler(this.TextBoxMiConStateTimeout_TextChanged);
          break;
        case 27:
          this.TextBlockMiConStateTimeout = (TextBlock) target;
          break;
        case 28:
          this.ButtonShowConfig = (Button) target;
          this.ButtonShowConfig.Click += new RoutedEventHandler(this.ButtonShowConfig_Click);
          break;
        case 29:
          this.ButtonShowChanges = (Button) target;
          this.ButtonShowChanges.Click += new RoutedEventHandler(this.ButtonShowChanges_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
