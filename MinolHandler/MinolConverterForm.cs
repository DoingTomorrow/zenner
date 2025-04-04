// Decompiled with JetBrains decompiler
// Type: MinolHandler.MinolConverterForm
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace MinolHandler
{
  public class MinolConverterForm : Form
  {
    private IContainer components = (IContainer) null;
    private DateTimePicker txtDate;
    private Label label2;
    private TextBox txtDateBinary;
    private GroupBox groupBox1;
    private Label lblResult;

    public MinolConverterForm() => this.InitializeComponent();

    private void txtDate_ValueChanged(object sender, EventArgs e)
    {
      ParameterAccess.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(this.txtDate.Value).ToString("X4");
      this.txtDateBinary.Text = ParameterAccess.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(this.txtDate.Value).ToString("X4");
    }

    private void txtDateBinary_TextChanged(object sender, EventArgs e)
    {
      try
      {
        this.lblResult.Text = ParameterAccess.ConvertInt64_YYYYMMMMYYYDDDDD_ToDateTime(Convert.ToInt64("0x" + this.txtDateBinary.Text.Trim(), 16)).ToShortDateString();
      }
      catch
      {
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtDate = new DateTimePicker();
      this.label2 = new Label();
      this.txtDateBinary = new TextBox();
      this.groupBox1 = new GroupBox();
      this.lblResult = new Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.txtDate.Format = DateTimePickerFormat.Short;
      this.txtDate.Location = new Point(6, 19);
      this.txtDate.Name = "txtDate";
      this.txtDate.Size = new Size(92, 20);
      this.txtDate.TabIndex = 0;
      this.txtDate.ValueChanged += new EventHandler(this.txtDate_ValueChanged);
      this.label2.Font = new Font("Tahoma", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(104, 19);
      this.label2.Name = "label2";
      this.label2.Size = new Size(63, 19);
      this.label2.TabIndex = 2;
      this.label2.Text = "<==> 0x";
      this.txtDateBinary.Location = new Point(173, 18);
      this.txtDateBinary.Name = "txtDateBinary";
      this.txtDateBinary.Size = new Size(72, 20);
      this.txtDateBinary.TabIndex = 3;
      this.txtDateBinary.TextChanged += new EventHandler(this.txtDateBinary_TextChanged);
      this.groupBox1.Controls.Add((Control) this.lblResult);
      this.groupBox1.Controls.Add((Control) this.txtDate);
      this.groupBox1.Controls.Add((Control) this.txtDateBinary);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Location = new Point(9, 5);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(256, 77);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Date";
      this.lblResult.AutoSize = true;
      this.lblResult.Location = new Point(62, 46);
      this.lblResult.Name = "lblResult";
      this.lblResult.Size = new Size(0, 13);
      this.lblResult.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(273, 94);
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (MinolConverterForm);
      this.Text = "Converter";
      this.TopMost = true;
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
