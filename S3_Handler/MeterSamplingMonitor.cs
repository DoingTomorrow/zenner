// Decompiled with JetBrains decompiler
// Type: S3_Handler.MeterSamplingMonitor
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class MeterSamplingMonitor : Form
  {
    private const string ClassMessageName = "S3_Handler MeterSamplingMonitor";
    private S3_HandlerFunctions MyFunctions;
    private int[] DiagnositcValues = new int[500];
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Button buttonRunMonitor;
    private TextBox textBoxInfos;
    private Button buttonRunMonitorLoop;
    private Chart chartMeterMonitor;
    private CheckBox checkBoxShowPoints;
    private TextBox textBoxMinMax;
    private TextBox textBoxHysteresis;
    private TextBox textBoxPulses;
    private Label label1;
    private Label label2;
    private Label label3;

    internal MeterSamplingMonitor(S3_HandlerFunctions MyFunctions)
    {
      this.InitializeComponent();
      this.MyFunctions = MyFunctions;
      this.chartMeterMonitor.Series[0].Points.Clear();
      for (int xValue = 0; xValue < this.DiagnositcValues.Length; ++xValue)
        this.chartMeterMonitor.Series[0].Points.AddXY((double) xValue, (double) this.DiagnositcValues[xValue]);
    }

    private void buttonRunMonitor_Click(object sender, EventArgs e)
    {
      this.RunMonitor();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool RunMonitor()
    {
      bool flag = false;
      try
      {
        this.buttonRunMonitor.Enabled = false;
        this.textBoxInfos.Text = "ConnectMeter.. ";
        ZR_ClassLibMessages.ClearErrors();
        this.textBoxInfos.Refresh();
        Application.DoEvents();
        if (this.MyFunctions.MyMeters.ConnectedMeter == null && !this.MyFunctions.ConnectDevice())
        {
          ZR_ClassLibMessages.ShowAndClearErrors();
          return false;
        }
        if ((this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.HardwareMask & 64U) > 0U)
        {
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Device has no sample monitor function");
          ZR_ClassLibMessages.ShowAndClearErrors();
          return false;
        }
        this.textBoxInfos.AppendText("Done");
        this.textBoxInfos.AppendText(Environment.NewLine);
        this.textBoxInfos.AppendText("Request data ...");
        this.textBoxInfos.AppendText(Environment.NewLine);
        this.textBoxInfos.Refresh();
        Application.DoEvents();
        for (int index = 1; index < 500; ++index)
          this.DiagnositcValues[index] = 0;
        MeterSamplingData samplingData;
        if (this.MyFunctions.RunMeterSampling(out samplingData))
        {
          this.textBoxInfos.AppendText("Data received");
          for (int index = 0; index < 500; ++index)
            this.DiagnositcValues[index] = (int) samplingData.SamplingValues[index];
        }
        else
        {
          this.textBoxInfos.AppendText(Environment.NewLine);
          this.textBoxInfos.AppendText("Sampling error");
        }
        this.chartMeterMonitor.Series[0].Points.Clear();
        this.textBoxMinMax.Clear();
        this.textBoxHysteresis.Clear();
        this.textBoxPulses.Clear();
        if (this.checkBoxShowPoints.Checked)
          this.chartMeterMonitor.Series[0].ChartType = SeriesChartType.FastPoint;
        else
          this.chartMeterMonitor.Series[0].ChartType = SeriesChartType.Line;
        for (int xValue = 0; xValue < this.DiagnositcValues.Length; ++xValue)
          this.chartMeterMonitor.Series[0].Points.AddXY((double) xValue, (double) this.DiagnositcValues[xValue]);
        if (samplingData != null)
        {
          this.textBoxMinMax.Text = samplingData.MinValue.ToString() + " / " + samplingData.MaxValue.ToString();
          this.textBoxHysteresis.Text = ((int) samplingData.MaxValue - (int) samplingData.MinValue).ToString();
          this.textBoxPulses.Text = samplingData.Pulses.ToString();
        }
      }
      catch (Exception ex)
      {
        this.textBoxInfos.AppendText(Environment.NewLine);
        this.textBoxInfos.AppendText("Exception !!!");
        this.textBoxInfos.AppendText(Environment.NewLine);
        this.textBoxInfos.AppendText(Environment.NewLine);
        this.textBoxInfos.AppendText(ex.ToString());
      }
      finally
      {
        this.buttonRunMonitor.Enabled = true;
      }
      if (!flag)
        Thread.Sleep(500);
      else
        Thread.Sleep(100);
      return flag;
    }

    private void buttonRunMonitorLoop_Click(object sender, EventArgs e)
    {
      if (this.buttonRunMonitorLoop.Text == "Break loop")
      {
        this.buttonRunMonitorLoop.Text = "Wait for break";
      }
      else
      {
        if (this.buttonRunMonitorLoop.Text != "Run monitor loop")
          return;
        this.buttonRunMonitorLoop.Text = "Break loop";
        this.buttonRunMonitor.Enabled = false;
        while (this.buttonRunMonitorLoop.Text == "Break loop")
        {
          this.RunMonitor();
          Application.DoEvents();
        }
        this.buttonRunMonitor.Enabled = true;
        this.buttonRunMonitorLoop.Text = "Run monitor loop";
      }
    }

    private void checkBoxShowPoints_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ChartArea chartArea = new ChartArea();
      Series series = new Series();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.buttonRunMonitor = new Button();
      this.textBoxInfos = new TextBox();
      this.buttonRunMonitorLoop = new Button();
      this.chartMeterMonitor = new Chart();
      this.checkBoxShowPoints = new CheckBox();
      this.textBoxMinMax = new TextBox();
      this.textBoxHysteresis = new TextBox();
      this.textBoxPulses = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.chartMeterMonitor.BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(998, 45);
      this.zennerCoroprateDesign2.TabIndex = 18;
      this.buttonRunMonitor.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRunMonitor.Location = new Point(765, 609);
      this.buttonRunMonitor.Name = "buttonRunMonitor";
      this.buttonRunMonitor.Size = new Size(221, 23);
      this.buttonRunMonitor.TabIndex = 54;
      this.buttonRunMonitor.Text = "Run monitor";
      this.buttonRunMonitor.UseVisualStyleBackColor = true;
      this.buttonRunMonitor.Click += new System.EventHandler(this.buttonRunMonitor_Click);
      this.textBoxInfos.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxInfos.Location = new Point(12, 557);
      this.textBoxInfos.Multiline = true;
      this.textBoxInfos.Name = "textBoxInfos";
      this.textBoxInfos.ScrollBars = ScrollBars.Both;
      this.textBoxInfos.Size = new Size(567, 75);
      this.textBoxInfos.TabIndex = 55;
      this.buttonRunMonitorLoop.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRunMonitorLoop.Location = new Point(765, 580);
      this.buttonRunMonitorLoop.Name = "buttonRunMonitorLoop";
      this.buttonRunMonitorLoop.Size = new Size(221, 23);
      this.buttonRunMonitorLoop.TabIndex = 54;
      this.buttonRunMonitorLoop.Text = "Run monitor loop";
      this.buttonRunMonitorLoop.UseVisualStyleBackColor = true;
      this.buttonRunMonitorLoop.Click += new System.EventHandler(this.buttonRunMonitorLoop_Click);
      this.chartMeterMonitor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      chartArea.AxisX.IntervalType = DateTimeIntervalType.Number;
      chartArea.AxisX.MajorGrid.Interval = 100.0;
      chartArea.AxisX.MajorGrid.IntervalOffset = 0.0;
      chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Number;
      chartArea.AxisX.MajorTickMark.Interval = 100.0;
      chartArea.AxisX.MajorTickMark.IntervalOffset = 0.0;
      chartArea.AxisX.Minimum = 0.0;
      chartArea.Name = "ChartArea1";
      this.chartMeterMonitor.ChartAreas.Add(chartArea);
      this.chartMeterMonitor.Location = new Point(12, 50);
      this.chartMeterMonitor.Name = "chartMeterMonitor";
      series.ChartArea = "ChartArea1";
      series.ChartType = SeriesChartType.FastPoint;
      series.EmptyPointStyle.MarkerSize = 1;
      series.Name = "Series1";
      this.chartMeterMonitor.Series.Add(series);
      this.chartMeterMonitor.Size = new Size(974, 501);
      this.chartMeterMonitor.TabIndex = 56;
      this.chartMeterMonitor.Text = "Meter monitor";
      this.checkBoxShowPoints.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.checkBoxShowPoints.AutoSize = true;
      this.checkBoxShowPoints.Location = new Point(765, 557);
      this.checkBoxShowPoints.Name = "checkBoxShowPoints";
      this.checkBoxShowPoints.Size = new Size(84, 17);
      this.checkBoxShowPoints.TabIndex = 57;
      this.checkBoxShowPoints.Text = "Show points";
      this.checkBoxShowPoints.UseVisualStyleBackColor = true;
      this.checkBoxShowPoints.CheckedChanged += new System.EventHandler(this.checkBoxShowPoints_CheckedChanged);
      this.textBoxMinMax.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxMinMax.Location = new Point(647, 560);
      this.textBoxMinMax.Name = "textBoxMinMax";
      this.textBoxMinMax.Size = new Size(100, 20);
      this.textBoxMinMax.TabIndex = 58;
      this.textBoxHysteresis.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxHysteresis.Location = new Point(647, 586);
      this.textBoxHysteresis.Name = "textBoxHysteresis";
      this.textBoxHysteresis.Size = new Size(100, 20);
      this.textBoxHysteresis.TabIndex = 58;
      this.textBoxPulses.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxPulses.Location = new Point(647, 612);
      this.textBoxPulses.Name = "textBoxPulses";
      this.textBoxPulses.Size = new Size(100, 20);
      this.textBoxPulses.TabIndex = 58;
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(585, 561);
      this.label1.Name = "label1";
      this.label1.Size = new Size(55, 13);
      this.label1.TabIndex = 59;
      this.label1.Text = "Min / Max";
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(585, 589);
      this.label2.Name = "label2";
      this.label2.Size = new Size(55, 13);
      this.label2.TabIndex = 59;
      this.label2.Text = "Signal rise";
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(585, 614);
      this.label3.Name = "label3";
      this.label3.Size = new Size(48, 13);
      this.label3.TabIndex = 59;
      this.label3.Text = "Impulses";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(998, 644);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxPulses);
      this.Controls.Add((Control) this.textBoxHysteresis);
      this.Controls.Add((Control) this.textBoxMinMax);
      this.Controls.Add((Control) this.checkBoxShowPoints);
      this.Controls.Add((Control) this.chartMeterMonitor);
      this.Controls.Add((Control) this.textBoxInfos);
      this.Controls.Add((Control) this.buttonRunMonitorLoop);
      this.Controls.Add((Control) this.buttonRunMonitor);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.MinimumSize = new Size(700, 450);
      this.Name = nameof (MeterSamplingMonitor);
      this.Text = nameof (MeterSamplingMonitor);
      this.chartMeterMonitor.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
