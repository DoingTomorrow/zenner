// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.ExtendedSearch
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace SmokeDetectorHandler
{
  public class ExtendedSearch : Form
  {
    private DateTime? start;
    private DateTime? end;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Label label5;
    private NumericUpDown txtRadioTransmitIntervalLessAs;
    private Button btnSearchByTransmitInterval;
    private Button btnSearchBySerialnumber;
    private Label label1;
    private TextBox txtSerialnumber;

    public ExtendedSearch() => this.InitializeComponent();

    internal static void ShowDialog(Form owner, DateTime? start, DateTime? end)
    {
      using (ExtendedSearch extendedSearch = new ExtendedSearch())
      {
        extendedSearch.start = start;
        extendedSearch.end = end;
        int num = (int) extendedSearch.ShowDialog((IWin32Window) owner);
      }
    }

    private void btnSearchByTransmitInterval_Click(object sender, EventArgs e)
    {
      SmokeDetectorDatabase.StartLoadMeterFilterByRadioTransmitIntervalLessAs(this.start, this.end, Convert.ToUInt16(this.txtRadioTransmitIntervalLessAs.Value));
      this.Close();
    }

    private void btnSearchBySerialnumber_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.txtSerialnumber.Text))
        return;
      SmokeDetectorDatabase.StartLoadMeterFilterBySerialnumber(this.txtSerialnumber.Text);
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ExtendedSearch));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.label5 = new Label();
      this.txtRadioTransmitIntervalLessAs = new NumericUpDown();
      this.btnSearchByTransmitInterval = new Button();
      this.btnSearchBySerialnumber = new Button();
      this.label1 = new Label();
      this.txtSerialnumber = new TextBox();
      this.txtRadioTransmitIntervalLessAs.BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(363, 41);
      this.zennerCoroprateDesign1.TabIndex = 21;
      this.label5.Location = new Point(4, 51);
      this.label5.Name = "label5";
      this.label5.Size = new Size(161, 15);
      this.label5.TabIndex = 75;
      this.label5.Tag = (object) "";
      this.label5.Text = "Radio transmit interval less as:";
      this.label5.TextAlign = ContentAlignment.MiddleRight;
      this.txtRadioTransmitIntervalLessAs.Location = new Point(171, 50);
      this.txtRadioTransmitIntervalLessAs.Maximum = new Decimal(new int[4]
      {
        (int) ushort.MaxValue,
        0,
        0,
        0
      });
      this.txtRadioTransmitIntervalLessAs.Name = "txtRadioTransmitIntervalLessAs";
      this.txtRadioTransmitIntervalLessAs.Size = new Size(77, 20);
      this.txtRadioTransmitIntervalLessAs.TabIndex = 74;
      this.txtRadioTransmitIntervalLessAs.Value = new Decimal(new int[4]
      {
        180,
        0,
        0,
        0
      });
      this.btnSearchByTransmitInterval.Location = new Point(254, 47);
      this.btnSearchByTransmitInterval.Name = "btnSearchByTransmitInterval";
      this.btnSearchByTransmitInterval.Size = new Size(75, 23);
      this.btnSearchByTransmitInterval.TabIndex = 76;
      this.btnSearchByTransmitInterval.Text = "Search";
      this.btnSearchByTransmitInterval.UseVisualStyleBackColor = true;
      this.btnSearchByTransmitInterval.Click += new EventHandler(this.btnSearchByTransmitInterval_Click);
      this.btnSearchBySerialnumber.Location = new Point(254, 82);
      this.btnSearchBySerialnumber.Name = "btnSearchBySerialnumber";
      this.btnSearchBySerialnumber.Size = new Size(75, 23);
      this.btnSearchBySerialnumber.TabIndex = 79;
      this.btnSearchBySerialnumber.Text = "Search";
      this.btnSearchBySerialnumber.UseVisualStyleBackColor = true;
      this.btnSearchBySerialnumber.Click += new EventHandler(this.btnSearchBySerialnumber_Click);
      this.label1.Location = new Point(12, 86);
      this.label1.Name = "label1";
      this.label1.Size = new Size(83, 15);
      this.label1.TabIndex = 78;
      this.label1.Tag = (object) "";
      this.label1.Text = "Serialnumber:";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.txtSerialnumber.Location = new Point(101, 84);
      this.txtSerialnumber.Name = "txtSerialnumber";
      this.txtSerialnumber.Size = new Size(147, 20);
      this.txtSerialnumber.TabIndex = 80;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(363, 131);
      this.Controls.Add((Control) this.txtSerialnumber);
      this.Controls.Add((Control) this.btnSearchBySerialnumber);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnSearchByTransmitInterval);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtRadioTransmitIntervalLessAs);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ExtendedSearch);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Extended search";
      this.txtRadioTransmitIntervalLessAs.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
