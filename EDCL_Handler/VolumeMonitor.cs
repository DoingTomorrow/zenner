// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.VolumeMonitor
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZENNER.CommonLibrary;

#nullable disable
namespace EDCL_Handler
{
  public class VolumeMonitor : Form
  {
    private static Logger logger = LogManager.GetLogger(nameof (VolumeMonitor));
    private EDCL_HandlerFunctions MyFunctions;
    private string fileName;
    private bool isCanceled;
    private uint xValue;
    private VolumeMonitorEventHandler addData;
    private int graphUpdate = 0;
    private int last_A = 0;
    private int last_B = 0;
    private Decimal DIFF_COIL_DETECTION = 10M;
    private List<VolumeMonitorEventArgs> volumeMonitorData;
    private IContainer components = (IContainer) null;
    private Chart chart;
    private Button btnPrint;
    private Button btnExport;
    private Button btnStop;
    private Button btnStart;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblStatus;
    private BackgroundWorker backgroundWorker;
    private NumericUpDown txtVisibleRange;
    private Label label1;
    private Label label2;
    private Label label3;
    private NumericUpDown txtYmin;
    private NumericUpDown txtYmax;
    private Button btnClear;
    private Label label4;
    private Label label5;
    private Label label6;
    private TextBox txtPulseForwardCount;
    private TextBox textBox2;
    private TextBox txtPulseErrorCount;
    private TextBox txtPulseReturnCount;
    private Button btnReadInterval;
    private Button btnSaveToDB;
    private Button btnExportEncabulator;

    public VolumeMonitor()
    {
      this.InitializeComponent();
      this.ResetShowData();
      this.chart.Titles.Add(new Title("EDCL Coil Test", Docking.Top, new Font(this.Font.FontFamily, 16f, FontStyle.Bold), Color.DarkSlateGray));
      this.chart.Legends.Add(new Legend("Legend"));
      this.chart.Legends["Legend"].DockedToChartArea = "Default";
      this.chart.Legends["Legend"].Docking = Docking.Bottom;
      this.chart.Legends["Legend"].IsDockedInsideChartArea = false;
      this.chart.ChartAreas[0].AxisY.Minimum = 0.0;
      this.chart.ChartAreas[0].AxisY.Maximum = (double) byte.MaxValue;
      this.chart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
      this.chart.ChartAreas[0].CursorY.IsUserEnabled = true;
      this.chart.ChartAreas[0].AxisX.Minimum = 0.0;
      this.chart.ChartAreas[0].AxisX.Maximum = (double) uint.MaxValue;
      this.chart.ChartAreas[0].CursorX.IsUserEnabled = true;
      this.chart.ChartAreas[0].AxisY2.Minimum = 0.0;
      this.chart.ChartAreas[0].AxisY2.Maximum = 1.0;
      this.chart.ChartAreas[0].AxisY2.Interval = 1.0;
      this.chart.Series.Add("Coil A");
      this.chart.Series[0].ChartType = SeriesChartType.FastLine;
      this.chart.Series[0].IsVisibleInLegend = true;
      this.chart.Series[0].Legend = "Legend";
      this.chart.Series[0].BorderWidth = 2;
      this.chart.Series[0].ShadowOffset = 1;
      this.chart.Series.Add("Coil B");
      this.chart.Series[1].ChartType = SeriesChartType.FastLine;
      this.chart.Series[1].IsVisibleInLegend = true;
      this.chart.Series[1].Legend = "Legend";
      this.chart.Series[1].BorderWidth = 2;
      this.chart.Series[1].ShadowOffset = 1;
      this.chart.Series.Add("Tamper");
      this.chart.Series[2].ChartType = SeriesChartType.FastPoint;
      this.chart.Series[2].IsVisibleInLegend = true;
      this.chart.Series[2].Legend = "Legend";
      this.chart.Series[2].BorderWidth = 1;
      this.chart.Series[2].ShadowOffset = 1;
      this.chart.Series[2].YAxisType = AxisType.Secondary;
      this.chart.Series.Add("Removal");
      this.chart.Series[3].ChartType = SeriesChartType.FastPoint;
      this.chart.Series[3].IsVisibleInLegend = true;
      this.chart.Series[3].Legend = "Legend";
      this.chart.Series[3].BorderWidth = 1;
      this.chart.Series[3].ShadowOffset = 1;
      this.chart.Series[3].YAxisType = AxisType.Secondary;
      this.volumeMonitorData = new List<VolumeMonitorEventArgs>();
    }

    private void VolumeMonitor_Load(object sender, EventArgs e)
    {
      this.MyFunctions.OnVolumeMonitorDataReceived += new VolumeMonitorEventHandler(this.MyFunctions_OnVolumeMonitorDataReceived);
      string str = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
      this.fileName = "EDC Coil Test";
      foreach (char ch in str)
        this.fileName = this.fileName.Replace(ch.ToString(), "");
      this.chart.Series.SuspendUpdates();
      if (this.MyFunctions.myMeters.EncabulatorData == null)
        return;
      this.volumeMonitorData.Clear();
      this.chart.Series[0].Points.Clear();
      this.chart.Series[1].Points.Clear();
      this.chart.Series[2].Points.Clear();
      this.chart.Series[3].Points.Clear();
      this.chart.ChartAreas["Default"].AxisX.Minimum = 0.0;
      this.chart.ChartAreas["Default"].AxisX.Maximum = (double) (long) this.txtVisibleRange.Value;
      this.chart.Series.ResumeUpdates();
      this.chart.Series.Invalidate();
      this.chart.Series.SuspendUpdates();
      foreach (VolumeMonitorEventArgs e1 in this.MyFunctions.myMeters.EncabulatorData)
      {
        this.MyFunctions_OnVolumeMonitorDataReceived((object) this, e1);
        Application.DoEvents();
      }
    }

    private void VolumeMonitor_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.btnStart.Enabled)
      {
        this.isCanceled = true;
        Application.DoEvents();
        Thread.Sleep(1000);
      }
      this.MyFunctions.OnVolumeMonitorDataReceived -= new VolumeMonitorEventHandler(this.MyFunctions_OnVolumeMonitorDataReceived);
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (this.backgroundWorker.IsBusy)
        return;
      this.volumeMonitorData.Clear();
      this.chart.Series[0].Points.Clear();
      this.chart.Series[1].Points.Clear();
      this.chart.Series[2].Points.Clear();
      this.chart.Series[3].Points.Clear();
      this.chart.ChartAreas["Default"].AxisX.Minimum = 0.0;
      this.chart.ChartAreas["Default"].AxisX.Maximum = (double) (long) this.txtVisibleRange.Value;
      this.chart.Series.ResumeUpdates();
      this.chart.Series.Invalidate();
      this.chart.Series.SuspendUpdates();
      this.isCanceled = false;
      this.btnStart.Enabled = false;
      this.btnStop.Enabled = true;
      this.btnPrint.Enabled = false;
      this.btnExport.Enabled = false;
      this.xValue = 0U;
      Application.DoEvents();
      this.backgroundWorker.RunWorkerAsync();
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
      this.isCanceled = true;
      this.btnStop.Enabled = false;
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.chart.Series[0].Points.Clear();
      this.chart.Series[1].Points.Clear();
      this.chart.Series[2].Points.Clear();
      this.chart.Series[3].Points.Clear();
      this.MyFunctions.ClearVolumeMonitorData();
      this.txtPulseErrorCount.Text = string.Empty;
      this.txtPulseForwardCount.Text = string.Empty;
      this.txtPulseReturnCount.Text = string.Empty;
      this.chart.Series.ResumeUpdates();
      this.chart.Series.Invalidate();
      this.chart.Series.SuspendUpdates();
    }

    private void btnReadInterval_Click(object sender, EventArgs e)
    {
      int count = (int) this.txtVisibleRange.Value;
      this.isCanceled = false;
      this.xValue = 0U;
      this.chart.Series[0].Points.Clear();
      this.chart.Series[1].Points.Clear();
      this.chart.Series[2].Points.Clear();
      this.chart.Series[3].Points.Clear();
      this.chart.ChartAreas["Default"].AxisX.Minimum = 0.0;
      this.chart.ChartAreas["Default"].AxisX.Maximum = (double) count;
      this.MyFunctions.StartVolumeMonitor(count);
    }

    private void btnPrint_Click(object sender, EventArgs e) => this.chart.Printing.PrintPreview();

    private void btnExport_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Bitmap (*.bmp)|*.bmp|JPEG (*.jpg)|*.jpg|EMF (*.emf)|*.emf|PNG (*.png)|*.png|GIF (*.gif)|*.gif|TIFF (*.tif)|*.tif";
      saveFileDialog.FilterIndex = 2;
      saveFileDialog.RestoreDirectory = true;
      saveFileDialog.FileName = this.fileName;
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      ChartImageFormat format = ChartImageFormat.Bmp;
      if (saveFileDialog.FileName.EndsWith("bmp"))
        format = ChartImageFormat.Bmp;
      else if (saveFileDialog.FileName.EndsWith("jpg"))
        format = ChartImageFormat.Jpeg;
      else if (saveFileDialog.FileName.EndsWith("emf"))
        format = ChartImageFormat.Emf;
      else if (saveFileDialog.FileName.EndsWith("gif"))
        format = ChartImageFormat.Gif;
      else if (saveFileDialog.FileName.EndsWith("png"))
        format = ChartImageFormat.Png;
      else if (saveFileDialog.FileName.EndsWith("tif"))
        format = ChartImageFormat.Tiff;
      this.chart.SaveImage(saveFileDialog.FileName, format);
    }

    private void MyFunctions_OnVolumeMonitorDataReceived(object sender, VolumeMonitorEventArgs e)
    {
      e.Cancel = this.isCanceled;
      if (this.isCanceled || !this.IsHandleCreated || this.IsDisposed)
        return;
      if (this.InvokeRequired)
      {
        try
        {
          if (this.addData == null)
            this.addData = new VolumeMonitorEventHandler(this.MyFunctions_OnVolumeMonitorDataReceived);
          this.Invoke((Delegate) this.addData, sender, (object) e);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "Volume monitor", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
        }
      }
      else
      {
        this.volumeMonitorData.Add(e);
        ++this.graphUpdate;
        if (!(Math.Abs((Decimal) this.last_A - (Decimal) e.CoilDetectionResult_A) > this.DIFF_COIL_DETECTION) && this.graphUpdate % 3 != 0)
          return;
        this.last_A = (int) e.CoilDetectionResult_A;
        this.last_B = (int) e.CoilDetectionResult_B;
        this.chart.Series[0].Points.AddXY((double) this.xValue, (double) e.CoilDetectionResult_A);
        this.chart.Series[1].Points.AddXY((double) this.xValue, (double) e.CoilDetectionResult_B);
        if (e.TamperSensorStatus)
          this.chart.Series[2].Points.AddXY((double) this.xValue, 1.0);
        else
          this.chart.Series[2].Points.AddXY((double) this.xValue, 0.05);
        if (e.RemovalSensorStatus)
          this.chart.Series[3].Points.AddXY((double) this.xValue, 0.95);
        else
          this.chart.Series[3].Points.AddXY((double) this.xValue, 0.0);
        if (this.txtYmax.Value > this.txtYmin.Value)
        {
          this.chart.ChartAreas[0].AxisY.Minimum = (double) (uint) this.txtYmin.Value;
          this.chart.ChartAreas[0].AxisY.Maximum = (double) (uint) this.txtYmax.Value;
          if (this.txtYmax.Value - this.txtYmin.Value > 50M)
            this.chart.ChartAreas[0].AxisY.Interval = (double) (((uint) this.txtYmax.Value - (uint) this.txtYmin.Value) / 10U);
          else if (this.txtYmax.Value - this.txtYmin.Value > 20M)
            this.chart.ChartAreas[0].AxisY.Interval = 5.0;
          else if (this.txtYmax.Value - this.txtYmin.Value > 8M)
            this.chart.ChartAreas[0].AxisY.Interval = 2.0;
          else
            this.chart.ChartAreas[0].AxisY.Interval = 1.0;
        }
        if ((Decimal) this.chart.Series[0].Points.Count > this.txtVisibleRange.Value)
        {
          this.chart.Series[0].Points.RemoveAt(0);
          this.chart.Series[1].Points.RemoveAt(0);
          this.chart.Series[2].Points.RemoveAt(0);
          this.chart.Series[3].Points.RemoveAt(0);
          this.chart.Series[0].Points.SuspendUpdates();
          this.chart.Series[1].Points.SuspendUpdates();
          this.chart.Series[2].Points.SuspendUpdates();
          this.chart.Series[3].Points.SuspendUpdates();
          this.chart.ChartAreas["Default"].AxisX.Minimum = (double) ((long) this.xValue - (long) this.txtVisibleRange.Value);
          this.chart.ChartAreas["Default"].AxisX.Maximum = this.chart.ChartAreas["Default"].AxisX.Minimum + (double) (long) this.txtVisibleRange.Value;
        }
        this.txtPulseForwardCount.Text = e.PulseForwardCount.ToString();
        this.txtPulseReturnCount.Text = e.PulseReturnCount.ToString();
        this.txtPulseErrorCount.Text = e.PulseErrorCount.ToString();
        ++this.xValue;
        this.chart.Series.ResumeUpdates();
        this.chart.Series.Invalidate();
        this.chart.Series.SuspendUpdates();
        this.graphUpdate = 0;
      }
    }

    internal static void ShowDialog(EDCL_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (VolumeMonitor volumeMonitor = new VolumeMonitor())
      {
        volumeMonitor.MyFunctions = MyFunctions;
        int num = (int) volumeMonitor.ShowDialog();
      }
    }

    private void ResetShowData()
    {
      this.isCanceled = false;
      this.lblStatus.Text = string.Empty;
    }

    private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        this.MyFunctions.StartVolumeMonitor();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.btnStart.Enabled = true;
      this.btnStop.Enabled = false;
      this.btnPrint.Enabled = true;
      this.btnExport.Enabled = true;
      this.isCanceled = false;
    }

    private async void btnSaveToDB_Click(object sender, EventArgs e)
    {
      ProgressHandler progress;
      CancellationTokenSource tokenSource;
      DeviceIdentification ident;
      if (this.volumeMonitorData.Count == 0)
      {
        progress = (ProgressHandler) null;
        tokenSource = (CancellationTokenSource) null;
        ident = (DeviceIdentification) null;
      }
      else
      {
        progress = new ProgressHandler((Action<ProgressArg>) (p => { }));
        tokenSource = new CancellationTokenSource();
        int num1 = await this.MyFunctions.ReadDeviceAsync(progress, tokenSource.Token, ReadPartsSelection.AllWithoutLogger);
        ident = this.MyFunctions.GetDeviceIdentification();
        uint? meterId;
        int num2;
        if (ident != null)
        {
          meterId = ident.MeterID;
          num2 = !meterId.HasValue ? 1 : 0;
        }
        else
          num2 = 1;
        if (num2 != 0)
        {
          int num3 = (int) MessageBox.Show("Please write correct MeterID to device!");
          progress = (ProgressHandler) null;
          tokenSource = (CancellationTokenSource) null;
          ident = (DeviceIdentification) null;
        }
        else
        {
          EDCL_HandlerFunctions functions = this.MyFunctions;
          meterId = ident.MeterID;
          int meterID = (int) meterId.Value;
          List<VolumeMonitorEventArgs> volumeMonitorData = this.volumeMonitorData;
          functions.SaveVolumeMonitorData(meterID, volumeMonitorData);
          int num4 = (int) MessageBox.Show("Done!");
          progress = (ProgressHandler) null;
          tokenSource = (CancellationTokenSource) null;
          ident = (DeviceIdentification) null;
        }
      }
    }

    private void btnExportEncabulator_Click(object sender, EventArgs e)
    {
      if (this.volumeMonitorData.Count == 0)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (VolumeMonitorEventArgs monitorEventArgs in this.volumeMonitorData)
        stringBuilder.AppendLine(monitorEventArgs.ToString());
      NotepadHelper.ShowMessage(stringBuilder.ToString(), "Encabulator");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (VolumeMonitor));
      ChartArea chartArea = new ChartArea();
      Legend legend = new Legend();
      this.chart = new Chart();
      this.btnPrint = new Button();
      this.btnExport = new Button();
      this.btnStop = new Button();
      this.btnStart = new Button();
      this.statusStrip = new StatusStrip();
      this.lblStatus = new ToolStripStatusLabel();
      this.backgroundWorker = new BackgroundWorker();
      this.txtVisibleRange = new NumericUpDown();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtYmin = new NumericUpDown();
      this.txtYmax = new NumericUpDown();
      this.btnClear = new Button();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.txtPulseForwardCount = new TextBox();
      this.textBox2 = new TextBox();
      this.txtPulseErrorCount = new TextBox();
      this.txtPulseReturnCount = new TextBox();
      this.btnReadInterval = new Button();
      this.btnSaveToDB = new Button();
      this.btnExportEncabulator = new Button();
      this.chart.BeginInit();
      this.statusStrip.SuspendLayout();
      this.txtVisibleRange.BeginInit();
      this.txtYmin.BeginInit();
      this.txtYmax.BeginInit();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.chart, "chart");
      this.chart.BackColor = Color.FromArgb(211, 223, 240);
      this.chart.BackGradientStyle = GradientStyle.TopBottom;
      this.chart.BackSecondaryColor = Color.White;
      this.chart.BorderlineColor = Color.FromArgb(26, 59, 105);
      this.chart.BorderlineWidth = 2;
      chartArea.Area3DStyle.Inclination = 15;
      chartArea.Area3DStyle.IsClustered = true;
      chartArea.Area3DStyle.IsRightAngleAxes = false;
      chartArea.Area3DStyle.Perspective = 10;
      chartArea.Area3DStyle.Rotation = 10;
      chartArea.Area3DStyle.WallWidth = 0;
      chartArea.AxisX.LabelAutoFitStyle = LabelAutoFitStyles.StaggeredLabels | LabelAutoFitStyles.LabelsAngleStep30 | LabelAutoFitStyles.WordWrap;
      chartArea.AxisX.LabelStyle.Font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold);
      chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
      chartArea.AxisX.LineWidth = 3;
      chartArea.AxisX.MajorGrid.Enabled = false;
      chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
      chartArea.AxisX.MajorTickMark.Enabled = false;
      chartArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.StaggeredLabels | LabelAutoFitStyles.LabelsAngleStep30 | LabelAutoFitStyles.WordWrap;
      chartArea.AxisY.LabelStyle.Font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold);
      chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
      chartArea.AxisY.LineWidth = 2;
      chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
      chartArea.BackColor = Color.FromArgb(64, 165, 191, 228);
      chartArea.BackGradientStyle = GradientStyle.TopBottom;
      chartArea.BackSecondaryColor = Color.White;
      chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
      chartArea.Name = "Default";
      chartArea.ShadowColor = Color.Transparent;
      this.chart.ChartAreas.Add(chartArea);
      legend.BackColor = Color.Transparent;
      legend.Enabled = false;
      legend.Font = new Font("Trebuchet MS", 8.25f, FontStyle.Bold);
      legend.IsTextAutoFit = false;
      legend.Name = "Default";
      this.chart.Legends.Add(legend);
      this.chart.Name = "chart";
      this.chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
      this.btnPrint.BackColor = Color.Transparent;
      componentResourceManager.ApplyResources((object) this.btnPrint, "btnPrint");
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.UseVisualStyleBackColor = false;
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnExport.BackColor = Color.Transparent;
      componentResourceManager.ApplyResources((object) this.btnExport, "btnExport");
      this.btnExport.Name = "btnExport";
      this.btnExport.UseVisualStyleBackColor = false;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnStop.BackColor = SystemColors.Control;
      componentResourceManager.ApplyResources((object) this.btnStop, "btnStop");
      this.btnStop.Name = "btnStop";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.btnStart.BackColor = SystemColors.Control;
      componentResourceManager.ApplyResources((object) this.btnStart, "btnStart");
      this.btnStart.Name = "btnStart";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new EventHandler(this.btnStart_Click);
      this.statusStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.lblStatus
      });
      componentResourceManager.ApplyResources((object) this.statusStrip, "statusStrip");
      this.statusStrip.Name = "statusStrip";
      this.lblStatus.Name = "lblStatus";
      componentResourceManager.ApplyResources((object) this.lblStatus, "lblStatus");
      this.backgroundWorker.WorkerReportsProgress = true;
      this.backgroundWorker.WorkerSupportsCancellation = true;
      this.backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
      this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
      componentResourceManager.ApplyResources((object) this.txtVisibleRange, "txtVisibleRange");
      this.txtVisibleRange.Maximum = new Decimal(new int[4]
      {
        1000000,
        0,
        0,
        0
      });
      this.txtVisibleRange.Minimum = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.txtVisibleRange.Name = "txtVisibleRange";
      this.txtVisibleRange.Value = new Decimal(new int[4]
      {
        900,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.txtYmin, "txtYmin");
      this.txtYmin.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtYmin.Name = "txtYmin";
      this.txtYmin.Value = new Decimal(new int[4]
      {
        50,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.txtYmax, "txtYmax");
      this.txtYmax.Maximum = new Decimal(new int[4]
      {
        (int) byte.MaxValue,
        0,
        0,
        0
      });
      this.txtYmax.Name = "txtYmax";
      this.txtYmax.Value = new Decimal(new int[4]
      {
        140,
        0,
        0,
        0
      });
      componentResourceManager.ApplyResources((object) this.btnClear, "btnClear");
      this.btnClear.Name = "btnClear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      componentResourceManager.ApplyResources((object) this.label4, "label4");
      this.label4.Name = "label4";
      componentResourceManager.ApplyResources((object) this.label5, "label5");
      this.label5.Name = "label5";
      componentResourceManager.ApplyResources((object) this.label6, "label6");
      this.label6.Name = "label6";
      componentResourceManager.ApplyResources((object) this.txtPulseForwardCount, "txtPulseForwardCount");
      this.txtPulseForwardCount.Name = "txtPulseForwardCount";
      this.txtPulseForwardCount.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.textBox2, "textBox2");
      this.textBox2.Name = "textBox2";
      componentResourceManager.ApplyResources((object) this.txtPulseErrorCount, "txtPulseErrorCount");
      this.txtPulseErrorCount.Name = "txtPulseErrorCount";
      this.txtPulseErrorCount.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.txtPulseReturnCount, "txtPulseReturnCount");
      this.txtPulseReturnCount.Name = "txtPulseReturnCount";
      this.txtPulseReturnCount.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.btnReadInterval, "btnReadInterval");
      this.btnReadInterval.Name = "btnReadInterval";
      this.btnReadInterval.UseVisualStyleBackColor = true;
      this.btnReadInterval.Click += new EventHandler(this.btnReadInterval_Click);
      componentResourceManager.ApplyResources((object) this.btnSaveToDB, "btnSaveToDB");
      this.btnSaveToDB.Name = "btnSaveToDB";
      this.btnSaveToDB.UseVisualStyleBackColor = true;
      this.btnSaveToDB.Click += new EventHandler(this.btnSaveToDB_Click);
      componentResourceManager.ApplyResources((object) this.btnExportEncabulator, "btnExportEncabulator");
      this.btnExportEncabulator.Name = "btnExportEncabulator";
      this.btnExportEncabulator.UseVisualStyleBackColor = true;
      this.btnExportEncabulator.Click += new EventHandler(this.btnExportEncabulator_Click);
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btnExportEncabulator);
      this.Controls.Add((Control) this.btnSaveToDB);
      this.Controls.Add((Control) this.btnReadInterval);
      this.Controls.Add((Control) this.txtPulseReturnCount);
      this.Controls.Add((Control) this.txtPulseErrorCount);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.txtPulseForwardCount);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.btnClear);
      this.Controls.Add((Control) this.txtYmax);
      this.Controls.Add((Control) this.txtYmin);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtVisibleRange);
      this.Controls.Add((Control) this.statusStrip);
      this.Controls.Add((Control) this.btnStop);
      this.Controls.Add((Control) this.btnStart);
      this.Controls.Add((Control) this.btnExport);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.chart);
      this.Name = nameof (VolumeMonitor);
      this.FormClosing += new FormClosingEventHandler(this.VolumeMonitor_FormClosing);
      this.Load += new EventHandler(this.VolumeMonitor_Load);
      this.chart.EndInit();
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.txtVisibleRange.EndInit();
      this.txtYmin.EndInit();
      this.txtYmax.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
