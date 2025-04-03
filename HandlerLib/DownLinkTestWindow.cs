// Decompiled with JetBrains decompiler
// Type: HandlerLib.DownLinkTestWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using HandlerLib.NFC;
using Newtonsoft.Json;
using NLog;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class DownLinkTestWindow : Window, IComponentConnector
  {
    private static Logger DownLinkTestLogger = LogManager.GetLogger("DownLinkTest");
    public const string TriggerConfirmedProtocolData = "FFA55AF5AF";
    private const string ConfigKey = "HandlerLibDownLinkTestWindow";
    public DownLinkTestSetup TestSetup;
    public string ApiKey;
    private byte[] UplinkData;
    private string DevEUI;
    private bool IsPlugin;
    private DownLinkTestWindow.UploadData UploadBefore;
    private DownLinkTestWindow.SendProtocol TriggerFunction;
    private NfcFrame NfcFrame;
    private ProgressHandler Progress;
    private CancellationToken CancelToken;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal TextBox TextBoxElementURL;
    internal TextBox TextBoxElementApiKey;
    internal ComboBox ComboBoxTestSetup;
    internal TextBlock TextBlockDownlinkBytes;
    internal TextBox TextBoxDownlinkProtocol;
    internal Button ButtonPrepareLoRaServerForDownlink;
    internal TextBlock TextBlockLastPrepareTime;
    internal Button ButtonSendDeviceCommand;
    internal TextBlock TextBlockLastSendTime;
    internal Button ButtonLoadUplinkDataFromLoRaServer;
    internal TextBlock TextBlockLastLoadTime;
    internal TextBlock TextBlockUplinkBytes;
    internal TextBox TextBoxUplinkProtocol;
    internal Button ButtonWorkUplinkProtocol;
    private bool _contentLoaded;

    public DownLinkTestWindow(ulong devEUI, bool isPlugin = false)
    {
      this.DevEUI = devEUI.ToString("X016");
      this.IsPlugin = isPlugin;
      this.InitializeComponent();
      this.Title = this.Title + "   DevEUI: " + this.DevEUI;
      DownLinkTestWindow.DownLinkTestLogger.Trace("Prepared for devEUI: " + this.DevEUI);
      this.TextBoxElementURL.Text = "https://stage.element-iot.com/";
      this.TextBoxElementApiKey.Text = "b44a6cccabe2a2a3f06a7c52c18db300";
      this.TestSetup = DownLinkTestSetup.ServerTestByManualStepManagement;
      string str1 = PlugInLoader.GmmConfiguration.GetValue("HandlerLibDownLinkTestWindow", DownLinkTestWindow.ConfigVariables.TestSetup.ToString());
      if (!string.IsNullOrEmpty(str1))
        System.Enum.TryParse<DownLinkTestSetup>(str1, out this.TestSetup);
      string str2 = PlugInLoader.GmmConfiguration.GetValue("HandlerLibDownLinkTestWindow", DownLinkTestWindow.ConfigVariables.ElementURL.ToString());
      if (!string.IsNullOrEmpty(str2))
        this.TextBoxElementURL.Text = str2;
      string str3 = PlugInLoader.GmmConfiguration.GetValue("HandlerLibDownLinkTestWindow", DownLinkTestWindow.ConfigVariables.ElementApiKey.ToString());
      if (!string.IsNullOrEmpty(str3))
        this.TextBoxElementApiKey.Text = str3;
      this.ComboBoxTestSetup.SelectedIndex = (int) this.TestSetup;
    }

    private void TextBoxElementURL_LostFocus(object sender, RoutedEventArgs e)
    {
      if (!this.IsPlugin)
        return;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("HandlerLibDownLinkTestWindow", DownLinkTestWindow.ConfigVariables.ElementURL.ToString(), this.TextBoxElementURL.Text);
    }

    private void TextBoxElementApiKey_LostFocus(object sender, RoutedEventArgs e)
    {
      if (!this.IsPlugin)
        return;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("HandlerLibDownLinkTestWindow", DownLinkTestWindow.ConfigVariables.ElementApiKey.ToString(), this.TextBoxElementApiKey.Text);
    }

    private void Report(string reportString)
    {
      if (this.Progress == null)
        return;
      this.Progress.Report(reportString);
    }

    public byte[] GetUplinkBytes(
      byte[] downlinkProtocol,
      DownLinkTestWindow.SendProtocol triggerFunction,
      NfcFrame nfcFrame,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (!this.CheckAccess())
        return this.Dispatcher.Invoke<byte[]>((Func<byte[]>) (() => this.GetUplinkBytes(downlinkProtocol, triggerFunction, nfcFrame, progress, cancelToken)));
      this.TriggerFunction = triggerFunction;
      this.NfcFrame = nfcFrame;
      this.Progress = progress;
      this.CancelToken = cancelToken;
      this.ButtonPrepareLoRaServerForDownlink.IsEnabled = true;
      this.ButtonSendDeviceCommand.IsEnabled = true;
      this.ButtonLoadUplinkDataFromLoRaServer.IsEnabled = true;
      this.TextBlockDownlinkBytes.Text = downlinkProtocol.Length.ToString();
      byte[] buffer = new byte[downlinkProtocol.Length + 2];
      downlinkProtocol.CopyTo((Array) buffer, 0);
      BitConverter.GetBytes(CRC.CRC_16(downlinkProtocol)).CopyTo((Array) buffer, buffer.Length - 2);
      this.TextBoxDownlinkProtocol.Text = Util.ByteArrayToHexString(buffer);
      this.TextBoxUplinkProtocol.Clear();
      this.TextBlockLastPrepareTime.Visibility = Visibility.Collapsed;
      this.TextBlockLastPrepareTime.Text = string.Empty;
      this.TextBlockLastSendTime.Visibility = Visibility.Collapsed;
      this.TextBlockLastSendTime.Text = string.Empty;
      this.TextBlockLastLoadTime.Visibility = Visibility.Collapsed;
      this.TextBlockLastLoadTime.Text = string.Empty;
      this.UplinkData = (byte[]) null;
      DownLinkTestWindow.DownLinkTestLogger.Trace("Started for protocol: " + this.TextBoxDownlinkProtocol.Text);
      if (this.TestSetup != 0)
      {
        this.ButtonPrepareLoRaServerForDownlink_Click((object) this, (RoutedEventArgs) null);
        this.ButtonSendDeviceCommand_Click((object) this, (RoutedEventArgs) null);
        this.ButtonLoadUplinkDataFromLoRaServer_Click((object) this, (RoutedEventArgs) null);
      }
      if (this.TestSetup != DownLinkTestSetup.ServerTestByAutomaticFlowAndHiddenWindow)
        this.ShowDialog();
      return this.UplinkData;
    }

    private void ButtonSendDeviceCommand_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.Report("Send ConfirmedData trigger by device");
        DownLinkTestWindow.DownLinkTestLogger.Trace("Device: Upload ConfirmedData");
        this.TriggerFunction(this.NfcFrame, this.Progress, this.CancelToken, 0);
        this.TextBlockLastSendTime.Visibility = Visibility.Visible;
        this.TextBlockLastSendTime.Text = DateTime.Now.ToString("HH:mm:ss.FFF");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Send device command error.");
      }
    }

    private void TextBoxUplinkProtocol_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.TextBoxUplinkProtocol.Text.Length > 2)
        this.ButtonWorkUplinkProtocol.IsEnabled = true;
      else
        this.ButtonWorkUplinkProtocol.IsEnabled = false;
    }

    private void ButtonWorkUplinkProtocol_Click(object sender, RoutedEventArgs e) => this.Hide();

    private void ButtonPrepareLoRaServerForDownlink_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.Report("Prepare download by LoRa server");
        DownLinkTestWindow.DownLinkTestLogger.Trace("Prepare download by LoRa server");
        this.PrapareDownload();
        this.TextBlockLastPrepareTime.Visibility = Visibility.Visible;
        if (this.UploadBefore != null)
          this.TextBlockLastPrepareTime.Text = DateTime.Now.ToString("HH:mm:ss.FFF") + " (" + this.UploadBefore.UploadTime.ToString("HH:mm:ss.FFF") + ")";
        else
          this.TextBlockLastPrepareTime.Text = DateTime.Now.ToString("HH:mm:ss.FFF");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonLoadUplinkDataFromLoRaServer_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.Report("Wait upload by LoRa server");
        DownLinkTestWindow.DownLinkTestLogger.Trace("Wait upload by LoRa server");
        DownLinkTestWindow.UploadData upload = this.WaitAndGetUpload();
        if (string.IsNullOrEmpty(upload.UploadDataString))
        {
          this.UplinkData = (byte[]) null;
          this.TextBlockUplinkBytes.Text = (string) null;
        }
        else
        {
          this.UplinkData = Util.HexStringToByteArray(upload.UploadDataString);
          this.TextBlockUplinkBytes.Text = this.UplinkData.Length.ToString();
        }
        this.TextBoxUplinkProtocol.Text = upload.UploadDataString;
        this.Report("Upload by LoRa server done");
        DownLinkTestWindow.DownLinkTestLogger.Trace("Upload done: " + this.TextBoxUplinkProtocol.Text);
        this.TextBlockLastLoadTime.Visibility = Visibility.Visible;
        if (upload != null)
          this.TextBlockLastLoadTime.Text = DateTime.Now.ToString("HH:mm:ss.FFF") + " (" + upload.UploadTime.ToString("HH:mm:ss.FFF") + ")";
        else
          this.TextBlockLastLoadTime.Text = DateTime.Now.ToString("HH:mm:ss.FFF");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ComboBoxTestSetup_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.TestSetup = (DownLinkTestSetup) this.ComboBoxTestSetup.SelectedIndex;
      if (!this.IsPlugin)
        return;
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("HandlerLibDownLinkTestWindow", DownLinkTestWindow.ConfigVariables.TestSetup.ToString(), this.TestSetup.ToString());
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      e.Cancel = true;
      this.Hide();
    }

    private void PrapareDownload()
    {
      this.UploadBefore = this.GetUpload();
      DownLinkTestWindow.POST(this.TextBoxElementURL.Text + "api/v1/devices/by-eui/" + this.DevEUI + "/actions/send_down_frame?auth=" + this.TextBoxElementApiKey.Text, "{\"opts\":{\"payload\":\"" + this.TextBoxDownlinkProtocol.Text + "\",\"port\":1}}");
    }

    private DownLinkTestWindow.UploadData WaitAndGetUpload()
    {
      DateTime dateTime1 = DateTime.Now.AddSeconds(20.0);
      DateTime dateTime2 = DateTime.MinValue;
      DownLinkTestWindow.UploadData upload;
      while (true)
      {
        if (!(DateTime.Now > dateTime1))
        {
          upload = this.GetUpload();
          if (upload != null)
          {
            if (this.UploadBefore == null || upload.UploadTime > this.UploadBefore.UploadTime)
            {
              if (!(upload.UploadDataString != "FFA55AF5AF"))
              {
                if (dateTime2 == upload.UploadTime)
                {
                  DownLinkTestWindow.DownLinkTestLogger.Trace("Last data repeated by server polling");
                }
                else
                {
                  this.Report("ConfirmedData trigger by LoRa server received");
                  DownLinkTestWindow.DownLinkTestLogger.Trace("ConfirmedData trigger by LoRa server received");
                }
              }
              else
                goto label_5;
            }
            else
              DownLinkTestWindow.DownLinkTestLogger.Trace("Server polling. Last data repeated by server");
            dateTime2 = upload.UploadTime;
          }
          else
            DownLinkTestWindow.DownLinkTestLogger.Trace("UploadData == null");
          Thread.Sleep(1000);
        }
        else
          break;
      }
      throw new TimeoutException();
label_5:
      DownLinkTestWindow.DownLinkTestLogger.Trace("Server polling. Deliverd upload != ConfirmedProtocol data");
      return upload;
    }

    private DownLinkTestWindow.UploadData GetUpload()
    {
      DownLinkTestWindow.Root root = JsonConvert.DeserializeObject<DownLinkTestWindow.Root>(DownLinkTestWindow.GET(this.TextBoxElementURL.Text + "api/v1/devices/by-eui/" + this.DevEUI + "/packets?limit=1&packet_type=up&auth=" + this.TextBoxElementApiKey.Text));
      if (root == null || root.body.Count <= 0)
        return (DownLinkTestWindow.UploadData) null;
      return new DownLinkTestWindow.UploadData()
      {
        UploadDataString = root.body[0].payload,
        UploadTime = root.body[0].inserted_at
      };
    }

    public static string POST(string url, string jsonContent)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      httpWebRequest.Method = nameof (POST);
      byte[] bytes = new UTF8Encoding().GetBytes(jsonContent);
      httpWebRequest.ContentLength = (long) bytes.Length;
      httpWebRequest.ContentType = "application/json";
      using (Stream requestStream = httpWebRequest.GetRequestStream())
        requestStream.Write(bytes, 0, bytes.Length);
      using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
      {
        using (Stream responseStream = response.GetResponseStream())
          return new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
      }
    }

    public static string GET(string url)
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      using (Stream responseStream = WebRequest.Create(url).GetResponse().GetResponseStream())
        return new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/downlinktestwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          break;
        case 2:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 3:
          this.TextBoxElementURL = (TextBox) target;
          this.TextBoxElementURL.LostFocus += new RoutedEventHandler(this.TextBoxElementURL_LostFocus);
          break;
        case 4:
          this.TextBoxElementApiKey = (TextBox) target;
          this.TextBoxElementApiKey.LostFocus += new RoutedEventHandler(this.TextBoxElementApiKey_LostFocus);
          break;
        case 5:
          this.ComboBoxTestSetup = (ComboBox) target;
          this.ComboBoxTestSetup.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxTestSetup_SelectionChanged);
          break;
        case 6:
          this.TextBlockDownlinkBytes = (TextBlock) target;
          break;
        case 7:
          this.TextBoxDownlinkProtocol = (TextBox) target;
          break;
        case 8:
          this.ButtonPrepareLoRaServerForDownlink = (Button) target;
          this.ButtonPrepareLoRaServerForDownlink.Click += new RoutedEventHandler(this.ButtonPrepareLoRaServerForDownlink_Click);
          break;
        case 9:
          this.TextBlockLastPrepareTime = (TextBlock) target;
          break;
        case 10:
          this.ButtonSendDeviceCommand = (Button) target;
          this.ButtonSendDeviceCommand.Click += new RoutedEventHandler(this.ButtonSendDeviceCommand_Click);
          break;
        case 11:
          this.TextBlockLastSendTime = (TextBlock) target;
          break;
        case 12:
          this.ButtonLoadUplinkDataFromLoRaServer = (Button) target;
          this.ButtonLoadUplinkDataFromLoRaServer.Click += new RoutedEventHandler(this.ButtonLoadUplinkDataFromLoRaServer_Click);
          break;
        case 13:
          this.TextBlockLastLoadTime = (TextBlock) target;
          break;
        case 14:
          this.TextBlockUplinkBytes = (TextBlock) target;
          break;
        case 15:
          this.TextBoxUplinkProtocol = (TextBox) target;
          this.TextBoxUplinkProtocol.TextChanged += new TextChangedEventHandler(this.TextBoxUplinkProtocol_TextChanged);
          break;
        case 16:
          this.ButtonWorkUplinkProtocol = (Button) target;
          this.ButtonWorkUplinkProtocol.Click += new RoutedEventHandler(this.ButtonWorkUplinkProtocol_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private enum ConfigVariables
    {
      TestSetup,
      ElementURL,
      ElementApiKey,
    }

    public delegate void SendProtocol(
      NfcFrame nfcFrame,
      ProgressHandler progres,
      CancellationToken cancelToken,
      int readTimeOffset);

    private class UploadData
    {
      internal DateTime UploadTime;
      internal string UploadDataString;
    }

    public class GatewayStat
    {
      public long router_id { get; set; }

      public string router_id_hex { get; set; }

      public int rssi { get; set; }

      public double snr { get; set; }

      public long tmst { get; set; }
    }

    public class RegionMeta
    {
      public int bandwidth { get; set; }

      public int bitrate { get; set; }

      public string code { get; set; }

      public int datarate { get; set; }

      public string name { get; set; }

      public int spreadingfactor { get; set; }
    }

    public class Meta
    {
      public bool ack { get; set; }

      public bool adr_ack_req { get; set; }

      public object chan { get; set; }

      public string codr { get; set; }

      public bool confirm { get; set; }

      public int data_rate { get; set; }

      public string datr { get; set; }

      public object dev_nonce { get; set; }

      public int frame_count_up { get; set; }

      public int frame_port { get; set; }

      public double frequency { get; set; }

      public List<DownLinkTestWindow.GatewayStat> gateway_stats { get; set; }

      public object ipol { get; set; }

      public string lns_packet_uuid { get; set; }

      public double lorawan_toa_ms { get; set; }

      public List<object> mac_commands { get; set; }

      public string modu { get; set; }

      public object powe { get; set; }

      public string region { get; set; }

      public DownLinkTestWindow.RegionMeta region_meta { get; set; }

      public object rfch { get; set; }

      public object rx { get; set; }

      public int size { get; set; }

      public int stat { get; set; }
    }

    public class Body
    {
      public object driver { get; set; }

      public string device_id { get; set; }

      public string interface_id { get; set; }

      public bool is_meta { get; set; }

      public DownLinkTestWindow.Meta meta { get; set; }

      public DateTime inserted_at { get; set; }

      public DateTime transceived_at { get; set; }

      public string packet_type { get; set; }

      public string payload_encoding { get; set; }

      public string payload { get; set; }

      public string id { get; set; }
    }

    public class Root
    {
      public List<DownLinkTestWindow.Body> body { get; set; }

      public bool ok { get; set; }

      public string retrieve_after_id { get; set; }

      public int status { get; set; }
    }
  }
}
