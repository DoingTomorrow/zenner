// Decompiled with JetBrains decompiler
// Type: S3_Handler.TDC_ZeroCalibrationCheck
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class TDC_ZeroCalibrationCheck : Form
  {
    private TDC_Calibration calibration;
    private S3_Meter myMeter;
    private S3_HandlerFunctions myFunctions;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Button buttonRun;
    private TextBox textBoxResults;
    private Label labelLoops;
    private TextBox textBoxLoops;
    private Button buttonClear;
    private Label label1;
    private TextBox textBoxMediaFilter;
    private TextBox textBoxPercent;

    public TDC_ZeroCalibrationCheck(S3_HandlerFunctions MyFunctions)
    {
      this.myFunctions = MyFunctions;
      this.calibration = MyFunctions.volumeGraphCalibration as TDC_Calibration;
      this.myMeter = MyFunctions.MyMeters.WorkMeter;
      this.InitializeComponent();
      this.myFunctions.BreakRequest = false;
      this.myFunctions.OnS3_HandlerMessage += new EventHandler<GMM_EventArgs>(this.S3_HandlerMessage);
    }

    private void S3_HandlerMessage(object sender, GMM_EventArgs TheMessage)
    {
      if (TheMessage.TheMessageType != GMM_EventArgs.MessageType.MessageAndProgressPercentage)
        return;
      this.textBoxPercent.Text = TheMessage.InfoNumber.ToString("d02") + "%";
      if (this.buttonRun.Text != "Stop")
        TheMessage.Cancel = true;
    }

    private void buttonRun_Click(object sender, EventArgs e)
    {
      if (this.buttonRun.Text == "Run")
      {
        this.buttonRun.Text = "Stop";
        this.myFunctions.BreakRequest = false;
        try
        {
          StringBuilder stringBuilder = new StringBuilder();
          int num1 = 1000000000;
          int num2 = 0;
          while (!this.myFunctions.BreakRequest)
          {
            stringBuilder.Length = 0;
            int numberOfLoops = int.Parse(this.textBoxLoops.Text);
            int mediaFilterRange = int.Parse(this.textBoxMediaFilter.Text);
            TDC_Calibration.CalibrationInfo calibrationInfo;
            if (!this.calibration.CalibrateZeroFlow(numberOfLoops, mediaFilterRange, 50f, out calibrationInfo))
            {
              stringBuilder.AppendLine("Calibration error");
              this.textBoxResults.AppendText(stringBuilder.ToString());
              break;
            }
            if (calibrationInfo.tdcZeroCalibrationValue < num1)
              num1 = calibrationInfo.tdcZeroCalibrationValue;
            if (calibrationInfo.tdcZeroCalibrationValue > num2)
              num2 = calibrationInfo.tdcZeroCalibrationValue;
            int num3 = num2 - num1;
            stringBuilder.AppendLine("------------------------------------------------------------------------------------------------------------------------");
            stringBuilder.Append(" Time = " + DateTime.Now.ToString("HH:mm:ss.fff"));
            stringBuilder.Append("| ZeroFlowOffset = " + calibrationInfo.tdcZeroCalibrationValue.ToString("d04"));
            stringBuilder.Append("| min = " + num1.ToString("d04"));
            stringBuilder.Append("| max = " + num2.ToString("d04"));
            stringBuilder.Append("| maxJitter = " + num3.ToString("d04"));
            stringBuilder.Append("| loops = " + numberOfLoops.ToString("d04"));
            stringBuilder.Append("| medianFilter = " + mediaFilterRange.ToString("d04"));
            stringBuilder.AppendLine();
            for (int index1 = 0; index1 < calibrationInfo.diffValues.GetLength(1); ++index1)
            {
              stringBuilder.Append("Up/Down/Diff n[" + index1.ToString("d02") + "]: ");
              for (int index2 = 0; index2 < 4; ++index2)
              {
                stringBuilder.Append("; ");
                stringBuilder.Append(calibrationInfo.counterValuesUp[index2, index1].ToString("d08"));
              }
              for (int index3 = 0; index3 < 4; ++index3)
              {
                stringBuilder.Append("; ");
                stringBuilder.Append(calibrationInfo.counterValuesDown[index3, index1].ToString("d08"));
              }
              for (int index4 = 0; index4 < 4; ++index4)
              {
                stringBuilder.Append("; ");
                stringBuilder.Append(calibrationInfo.diffValues[index4, index1].ToString("d05"));
              }
              stringBuilder.AppendLine();
            }
            this.textBoxResults.AppendText(stringBuilder.ToString());
          }
        }
        catch (Exception ex)
        {
          this.textBoxLoops.Text = "5";
          this.textBoxMediaFilter.Text = "3";
          this.textBoxResults.AppendText(Environment.NewLine);
          this.textBoxResults.AppendText("Exception");
          this.textBoxResults.AppendText(Environment.NewLine);
          this.textBoxResults.AppendText(ex.ToString());
        }
        this.Enabled = false;
        this.buttonRun.Text = "Run";
        this.myFunctions.BreakRequest = false;
        this.Enabled = true;
      }
      else
      {
        this.Enabled = false;
        this.buttonRun.Text = "Run";
        this.myFunctions.BreakRequest = true;
      }
    }

    private void buttonClear_Click(object sender, EventArgs e) => this.textBoxResults.Clear();

    private void textBoxMediaFilter_TextChanged(object sender, EventArgs e)
    {
    }

    private void TDC_ZeroCalibrationCheck_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.myFunctions.OnS3_HandlerMessage -= new EventHandler<GMM_EventArgs>(this.S3_HandlerMessage);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.buttonRun = new Button();
      this.textBoxResults = new TextBox();
      this.labelLoops = new Label();
      this.textBoxLoops = new TextBox();
      this.buttonClear = new Button();
      this.label1 = new Label();
      this.textBoxMediaFilter = new TextBox();
      this.textBoxPercent = new TextBox();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(997, 41);
      this.zennerCoroprateDesign2.TabIndex = 18;
      this.buttonRun.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRun.Location = new Point(910, 376);
      this.buttonRun.Name = "buttonRun";
      this.buttonRun.Size = new Size(75, 23);
      this.buttonRun.TabIndex = 19;
      this.buttonRun.Text = "Run";
      this.buttonRun.UseVisualStyleBackColor = true;
      this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
      this.textBoxResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxResults.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxResults.Location = new Point(13, 47);
      this.textBoxResults.Multiline = true;
      this.textBoxResults.Name = "textBoxResults";
      this.textBoxResults.ScrollBars = ScrollBars.Both;
      this.textBoxResults.Size = new Size(972, 313);
      this.textBoxResults.TabIndex = 20;
      this.labelLoops.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.labelLoops.AutoSize = true;
      this.labelLoops.Location = new Point(13, 383);
      this.labelLoops.Name = "labelLoops";
      this.labelLoops.Size = new Size(70, 13);
      this.labelLoops.TabIndex = 21;
      this.labelLoops.Text = "Median loops";
      this.textBoxLoops.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxLoops.Location = new Point(89, 379);
      this.textBoxLoops.Name = "textBoxLoops";
      this.textBoxLoops.Size = new Size(100, 20);
      this.textBoxLoops.TabIndex = 22;
      this.textBoxLoops.Text = "5";
      this.buttonClear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonClear.Location = new Point(829, 376);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new Size(75, 23);
      this.buttonClear.TabIndex = 19;
      this.buttonClear.Text = "Clear";
      this.buttonClear.UseVisualStyleBackColor = true;
      this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(210, 386);
      this.label1.Name = "label1";
      this.label1.Size = new Size(64, 13);
      this.label1.TabIndex = 21;
      this.label1.Text = "Median filter";
      this.textBoxMediaFilter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxMediaFilter.Location = new Point(280, 380);
      this.textBoxMediaFilter.Name = "textBoxMediaFilter";
      this.textBoxMediaFilter.Size = new Size(100, 20);
      this.textBoxMediaFilter.TabIndex = 22;
      this.textBoxMediaFilter.Text = "3";
      this.textBoxMediaFilter.TextChanged += new System.EventHandler(this.textBoxMediaFilter_TextChanged);
      this.textBoxPercent.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxPercent.Location = new Point(475, 380);
      this.textBoxPercent.Name = "textBoxPercent";
      this.textBoxPercent.Size = new Size(100, 20);
      this.textBoxPercent.TabIndex = 23;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(997, 408);
      this.Controls.Add((Control) this.textBoxPercent);
      this.Controls.Add((Control) this.textBoxMediaFilter);
      this.Controls.Add((Control) this.textBoxLoops);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.labelLoops);
      this.Controls.Add((Control) this.textBoxResults);
      this.Controls.Add((Control) this.buttonClear);
      this.Controls.Add((Control) this.buttonRun);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (TDC_ZeroCalibrationCheck);
      this.Text = nameof (TDC_ZeroCalibrationCheck);
      this.FormClosed += new FormClosedEventHandler(this.TDC_ZeroCalibrationCheck_FormClosed);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
