// Decompiled with JetBrains decompiler
// Type: HandlerLib.SensusToolWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class SensusToolWindow : Window, IComponentConnector
  {
    private static SensusToolWindow window;
    private SENSUSConnector handler;
    public ProgressHandler progress;
    public CancellationToken cancelToken;
    private double progressVal = 0.0;
    private bool isDeveloper = false;
    internal TextBox TextBoxInfo;
    internal ProgressBar progressBar_ONE;
    internal Label LabelState;
    internal Button ButtonReadVolFormat;
    internal ComboBox ComboBoxFormat;
    internal ComboBox ComboBoxTrunc;
    internal Button ButtonSetFormat;
    private bool _contentLoaded;

    private SensusToolWindow() => this.InitializeComponent();

    private void MyWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.cancelToken = new CancellationToken(false);
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        this.progressVal = obj.ProgressPercentage;
        this.progressBar_ONE.Value = this.progressVal;
        this.LabelState.Content = (object) obj.Message;
        this.addInfoText(obj.Message);
      }
    }

    private void addInfoText(string msg, bool newLine = true, bool clear = false, bool force = false)
    {
      if (clear)
        this.TextBoxInfo.Clear();
      TextBox textBoxInfo = this.TextBoxInfo;
      textBoxInfo.Text = textBoxInfo.Text + (newLine ? Environment.NewLine + (this.isDeveloper ? DateTime.Now.ToString() + " - " : string.Empty) : string.Empty) + msg;
      this.TextBoxInfo.CaretIndex = this.TextBoxInfo.LineCount < 0 ? 0 : this.TextBoxInfo.LineCount;
      this.TextBoxInfo.ScrollToEnd();
    }

    public static async Task<byte[]> ShowDialog(Window owner, SENSUSConnector handler)
    {
      await Task.Delay(1);
      SensusToolWindow.window = new SensusToolWindow();
      SensusToolWindow.window.Owner = owner;
      SensusToolWindow.window.handler = handler;
      bool canEnable = handler.myPort != null;
      FirmwareVersion fwV;
      int num;
      if (handler.FwV.Major >= (byte) 2)
      {
        fwV = handler.FwV;
        if (fwV.Minor >= (byte) 2)
        {
          fwV = handler.FwV;
          if (fwV.Revision >= (ushort) 3)
          {
            num = 1;
            goto label_6;
          }
        }
      }
      fwV = handler.FwV;
      num = fwV.Major >= (byte) 3 ? 1 : 0;
label_6:
      bool canEnableFormat = num != 0;
      SensusToolWindow.window.ButtonReadVolFormat.IsEnabled = canEnableFormat;
      SensusToolWindow.window.ButtonSetFormat.IsEnabled = canEnableFormat;
      SensusToolWindow.window.TextBoxInfo.Visibility = Visibility.Visible;
      SensusToolWindow.window.setFormatToComboBox();
      SensusToolWindow.window.setTruncateToComboBox();
      SensusToolWindow.window.isDeveloper = UserManager.CurrentUser.UserRole == UserManager.Role_Developer;
      if (SensusToolWindow.window.handler != null)
      {
        SensusToolWindow.window.addInfoText("Information:");
        SensusToolWindow.window.addInfoText("SENSUS Connector available ...");
        SensusToolWindow.window.addInfoText("SENSUS Connector: " + handler.ToString());
        SensusToolWindow.window.addInfoText("Set Format is " + (canEnableFormat ? " ENANLED " : " NOT ENABLED (firmware to old)"));
      }
      SensusToolWindow.window.Loaded += new RoutedEventHandler(SensusToolWindow.window.MyWindow_Loaded);
      byte[] numArray;
      try
      {
        if (!SensusToolWindow.window.ShowDialog().Value)
          ;
        numArray = (byte[]) null;
      }
      finally
      {
        SensusToolWindow.window.Loaded -= new RoutedEventHandler(SensusToolWindow.window.MyWindow_Loaded);
      }
      return numArray;
    }

    private void setFormatToComboBox()
    {
      for (int index = 4; index <= 9; ++index)
        this.ComboBoxFormat.Items.Add((object) index.ToString());
    }

    private void setTruncateToComboBox()
    {
      for (int index = 0; index <= 5; ++index)
        this.ComboBoxTrunc.Items.Add((object) index.ToString());
    }

    private async void ButtonReadVolFormat_Click(object sender, RoutedEventArgs e)
    {
      this.addInfoText("", clear: true);
      await this.ReadVolFormat();
    }

    private async Task ReadVolFormat()
    {
      byte[] retVal = await this.handler.ReadVolFormatSensusModule(this.progress, this.cancelToken);
      if (retVal != null && (int) retVal[0] == (int) SENSUSConnector.ResponseFORMAT && retVal.Length > 3)
      {
        ushort usFormat = (ushort) retVal[1];
        ushort usTrunc = (ushort) retVal[2];
        this.ComboBoxFormat.SelectedItem = (object) usFormat.ToString();
        this.ComboBoxTrunc.SelectedItem = (object) usTrunc.ToString();
        this.addInfoText(Util.ByteArrayToHexString(retVal));
        retVal = (byte[]) null;
      }
      else if (retVal == null || (int) retVal[0] != (int) SENSUSConnector.ResponseNACK)
      {
        retVal = (byte[]) null;
      }
      else
      {
        this.addInfoText("--> NACK received ... command not possible! ");
        this.addInfoText(Util.ByteArrayToHexString(retVal));
        retVal = (byte[]) null;
      }
    }

    private void ComboBoxFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private void ComboBoxTrunc_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private async void ButtonSetFormat_Click(object sender, RoutedEventArgs e)
    {
      byte[] retVal;
      if (this.ComboBoxFormat.SelectedItem == null || this.ComboBoxTrunc.SelectedItem == null)
      {
        int num = (int) MessageBox.Show("Please select a FORMAT and TRUNCATION first.");
        retVal = (byte[]) null;
      }
      else
      {
        ushort usFormat = 0;
        ushort.TryParse(this.ComboBoxFormat.SelectedItem.ToString(), out usFormat);
        ushort usTrunc = 0;
        ushort.TryParse(this.ComboBoxTrunc.SelectedItem.ToString(), out usTrunc);
        retVal = await this.handler.SetFormat(this.progress, this.cancelToken, (byte) usFormat, (byte) usTrunc);
        this.addInfoText(Util.ByteArrayToHexString(retVal));
        retVal = (byte[]) null;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/sensustoolwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TextBoxInfo = (TextBox) target;
          break;
        case 2:
          this.progressBar_ONE = (ProgressBar) target;
          break;
        case 3:
          this.LabelState = (Label) target;
          break;
        case 4:
          this.ButtonReadVolFormat = (Button) target;
          this.ButtonReadVolFormat.Click += new RoutedEventHandler(this.ButtonReadVolFormat_Click);
          break;
        case 5:
          this.ComboBoxFormat = (ComboBox) target;
          this.ComboBoxFormat.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxFormat_SelectionChanged);
          break;
        case 6:
          this.ComboBoxTrunc = (ComboBox) target;
          this.ComboBoxTrunc.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxTrunc_SelectionChanged);
          break;
        case 7:
          this.ButtonSetFormat = (Button) target;
          this.ButtonSetFormat.Click += new RoutedEventHandler(this.ButtonSetFormat_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
