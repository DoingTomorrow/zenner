// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC_Test
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using HandlerLib.NFC;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class NFC_Test : Window, IComponentConnector
  {
    private ProgressHandler progress;
    private CancellationTokenSource cancelTokenSource;
    private NfcRepeater myNfcRepeater;
    private NfcDeviceCommands myCommands;
    private NfcSubunitCommands mySubunitCommands;
    private MiConConnector myMiConConnector;
    internal Button BtnCouplerEcho;
    internal Button BtnCouplerIdent;
    internal Button BtnCouplerRfOff;
    internal Button BtnResetDevice;
    internal Button BtnCouplerRfOn;
    internal Button BtnNDCUSBStartBootloader;
    internal Button BtnNfcAntiCol;
    internal Button BtnNfcGetTagIdent;
    internal Button BtnGetTagStatus;
    internal Button BtnSend;
    internal TextBox TxBxTxDataLen;
    internal TextBox TxBxTxData;
    internal TextBox TxBxCRCData;
    internal TextBox TxBxRxData;
    internal TextBox TxBxRxDataLen;
    internal TextBox TxBxRxCRCData;
    internal Button BtnToggleRf;
    internal CheckBox CheckLoop;
    internal TextBox TxtBxTimeRFOn;
    internal TextBox TxtBxTimeRFOff;
    internal TextBox TxBxResult;
    internal TextBox TxBxResult_HEX;
    private bool _contentLoaded;

    public NFC_Test(CommunicationPortFunctions myPort)
    {
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.cancelTokenSource = new CancellationTokenSource();
      this.myNfcRepeater = new NfcRepeater(myPort);
      this.myCommands = new NfcDeviceCommands(myPort);
      this.mySubunitCommands = this.myCommands.mySubunitCommands;
      this.myMiConConnector = new MiConConnector(this.myCommands);
      this.InitializeComponent();
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    private void ButtonX_Click(object sender, RoutedEventArgs e) => this.Run_Button(sender);

    private async void Run_Button(object sender)
    {
      try
      {
        byte[] response;
        if (sender == this.BtnCouplerEcho)
        {
          string hex = await this.mySubunitCommands.GetEchoAsync(this.progress, this.cancelTokenSource.Token);
          response = Util.HexStringToByteArray(hex);
          hex = (string) null;
          this.Fill_TxtBx(response);
        }
        else if (sender == this.BtnCouplerIdent)
        {
          string hex = await this.mySubunitCommands.ReadIdentificationAsync(this.progress, this.cancelTokenSource.Token);
          response = Util.HexStringToByteArray(hex);
          hex = (string) null;
          this.Fill_TxtBx(response);
        }
        else if (sender == this.BtnCouplerRfOff)
        {
          response = await this.mySubunitCommands.SetRfOffAsync(this.progress, this.cancelTokenSource.Token);
          this.Fill_TxtBx(response);
        }
        else if (sender == this.BtnCouplerRfOn)
        {
          response = await this.mySubunitCommands.SetRfOnAsync(this.progress, this.cancelTokenSource.Token);
          this.Fill_TxtBx(response);
        }
        else if (sender == this.BtnNDCUSBStartBootloader)
        {
          response = await this.mySubunitCommands.NDC_USB_StartBootloader(this.progress, this.cancelTokenSource.Token);
          this.Fill_TxtBx(response);
        }
        else if (sender == this.BtnNfcAntiCol)
        {
          response = await this.mySubunitCommands.NFC_AnticollisionAsync(this.progress, this.cancelTokenSource.Token);
          this.Fill_TxtBx(response);
        }
        else if (sender == this.BtnNfcGetTagIdent)
        {
          response = await this.mySubunitCommands.NFC_GetTagIdentAsync(this.progress, this.cancelTokenSource.Token);
          this.Fill_TxtBx(response);
        }
        else if (sender == this.BtnGetTagStatus)
        {
          response = await this.mySubunitCommands.NFC_GetTagStatusAsync(this.progress, this.cancelTokenSource.Token);
          this.Fill_TxtBx(response);
        }
        else if (sender == this.BtnToggleRf)
        {
          this.BtnToggleRf.IsEnabled = false;
          int TimeOn = (int) Convert.ToInt16(this.TxtBxTimeRFOn.Text);
          int TimeOff = (int) Convert.ToInt16(this.TxtBxTimeRFOff.Text);
          ulong loops = 0;
          bool? isChecked;
          bool flag;
          do
          {
            byte[] numArray1 = await this.mySubunitCommands.SetRfOnAsync(this.progress, this.cancelTokenSource.Token);
            await Task.Delay(TimeOn);
            byte[] numArray2 = await this.mySubunitCommands.SetRfOffAsync(this.progress, this.cancelTokenSource.Token);
            await Task.Delay(TimeOff);
            ++loops;
            this.TxBxResult.Text = "Loops = " + loops.ToString();
            isChecked = this.CheckLoop.IsChecked;
            flag = true;
          }
          while (isChecked.GetValueOrDefault() == flag & isChecked.HasValue);
          this.BtnToggleRf.IsEnabled = true;
        }
        else if (sender == this.BtnResetDevice)
          await this.myMiConConnector.ResetDevice(this.progress, this.cancelTokenSource.Token);
        else if (sender == this.BtnSend)
        {
          byte[] locDATA = Util.HexStringToByteArray(this.TxBxTxDataLen.Text + this.TxBxTxData.Text + this.TxBxCRCData.Text);
          NfcFrame mylocNFCFrame = new NfcFrame(locDATA, this.myNfcRepeater.myConfig.ReadingChannelIdentification);
          await this.myNfcRepeater.GetResultFrameAsync(mylocNFCFrame, this.progress, this.cancelTokenSource.Token);
          this.Fill_TxtBx(mylocNFCFrame.NfcResponseFrame);
          locDATA = (byte[]) null;
          mylocNFCFrame = (NfcFrame) null;
        }
        response = (byte[]) null;
      }
      catch (NfcFrameException ex)
      {
        this.TxBxResult.Text = ex.Message;
      }
      catch (TimeoutException ex)
      {
        this.TxBxResult.Text = ex.Message;
      }
    }

    private void TxBxTxData_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.TxBxTxData.Text.Length % 2 != 0)
        return;
      this.TxBxTxDataLen.Text = (this.TxBxTxData.Text.Length / 2).ToString("X2");
      ushort crc = NfcFrame.createCRC(Util.HexStringToByteArray(this.TxBxTxDataLen.Text + this.TxBxTxData.Text + "0000"), new ushort?(ushort.MaxValue));
      TextBox txBxCrcData = this.TxBxCRCData;
      byte num = (byte) crc;
      string str1 = num.ToString("X2");
      num = (byte) ((uint) crc >> 8);
      string str2 = num.ToString("X2");
      string str3 = str1 + str2;
      txBxCrcData.Text = str3;
    }

    private void Fill_TxtBx(byte[] frame)
    {
      this.TxBxRxDataLen.Text = frame[0].ToString("X2");
      this.TxBxRxData.Text = Util.ByteArrayToHexString(frame, 1, frame.Length - 3);
      this.TxBxRxCRCData.Text = Util.ByteArrayToHexString(frame, frame.Length - 2, 2);
      this.TxBxResult.Text = Encoding.ASCII.GetString(frame);
      this.TxBxResult_HEX.Text = Util.ByteArrayToHexString(frame, 0, frame.Length);
    }

    private void TxBxTxData_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      this.Run_Button((object) this.BtnSend);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/nfc_test.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.BtnCouplerEcho = (Button) target;
          this.BtnCouplerEcho.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 2:
          this.BtnCouplerIdent = (Button) target;
          this.BtnCouplerIdent.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 3:
          this.BtnCouplerRfOff = (Button) target;
          this.BtnCouplerRfOff.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 4:
          this.BtnResetDevice = (Button) target;
          this.BtnResetDevice.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 5:
          this.BtnCouplerRfOn = (Button) target;
          this.BtnCouplerRfOn.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 6:
          this.BtnNDCUSBStartBootloader = (Button) target;
          this.BtnNDCUSBStartBootloader.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 7:
          this.BtnNfcAntiCol = (Button) target;
          this.BtnNfcAntiCol.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 8:
          this.BtnNfcGetTagIdent = (Button) target;
          this.BtnNfcGetTagIdent.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 9:
          this.BtnGetTagStatus = (Button) target;
          this.BtnGetTagStatus.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 10:
          this.BtnSend = (Button) target;
          this.BtnSend.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 11:
          this.TxBxTxDataLen = (TextBox) target;
          break;
        case 12:
          this.TxBxTxData = (TextBox) target;
          this.TxBxTxData.TextChanged += new TextChangedEventHandler(this.TxBxTxData_TextChanged);
          this.TxBxTxData.KeyDown += new KeyEventHandler(this.TxBxTxData_KeyDown);
          break;
        case 13:
          this.TxBxCRCData = (TextBox) target;
          break;
        case 14:
          this.TxBxRxData = (TextBox) target;
          break;
        case 15:
          this.TxBxRxDataLen = (TextBox) target;
          break;
        case 16:
          this.TxBxRxCRCData = (TextBox) target;
          break;
        case 17:
          this.BtnToggleRf = (Button) target;
          this.BtnToggleRf.Click += new RoutedEventHandler(this.ButtonX_Click);
          break;
        case 18:
          this.CheckLoop = (CheckBox) target;
          break;
        case 19:
          this.TxtBxTimeRFOn = (TextBox) target;
          break;
        case 20:
          this.TxtBxTimeRFOff = (TextBox) target;
          break;
        case 21:
          this.TxBxResult = (TextBox) target;
          break;
        case 22:
          this.TxBxResult_HEX = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
