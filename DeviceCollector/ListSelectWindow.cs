// Decompiled with JetBrains decompiler
// Type: DeviceCollector.ListSelectWindow
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using CorporateDesign;
using HandlerLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class ListSelectWindow : Form
  {
    private DeviceCollectorFunctions myFunctions;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private GroupBox groupBoxSelectList;
    private Button buttonMBusListSet;
    private TextBox textBoxMBusListNumber;
    private Label label1;
    private GroupBox groupBox1;
    private Button buttonRead;
    private Label labelMBusMaxList;
    private Label labelRadioMaxList;
    private TextBox textBoxRadioListNumber;
    private Button buttonRadioListSet;
    private Label label2;
    private Label labelMBusSublists;
    private Label label5;

    public ListSelectWindow(DeviceCollectorFunctions myFunctions)
    {
      this.myFunctions = myFunctions;
      this.InitializeComponent();
    }

    private void buttonMBusListSet_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (this.myFunctions.SelectParameterList(int.Parse(this.textBoxMBusListNumber.Text), 0))
      {
        int num = (int) GMM_MessageBox.ShowMessage("Set device parameter list", "Set parameter list done");
        this.Close();
      }
      else if (ZR_ClassLibMessages.GetLastError() == ZR_ClassLibMessages.LastErrors.NAK_Received)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Set device parameter list", "Parameter list not available!", true);
        ZR_ClassLibMessages.ClearErrors();
      }
      else
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("Set device parameter list", "Set parameter list error!", true);
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonRadioListSet_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (this.myFunctions.SelectParameterList(int.Parse(this.textBoxRadioListNumber.Text), 1))
      {
        int num = (int) GMM_MessageBox.ShowMessage("Set device parameter list", "Set parameter list done");
        this.Close();
      }
      else if (ZR_ClassLibMessages.GetLastError() == ZR_ClassLibMessages.LastErrors.NAK_Received)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Set device parameter list", "Parameter list not available!", true);
        ZR_ClassLibMessages.ClearErrors();
      }
      else
      {
        int num1 = (int) GMM_MessageBox.ShowMessage("Set device parameter list", "Set parameter list error!", true);
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonRead_Click(object sender, EventArgs e)
    {
      ParameterListInfo parameterListInfo = this.myFunctions.ReadParameterList();
      if (parameterListInfo == null)
        return;
      this.labelMBusMaxList.Text = string.Format("[0...{0}]", (object) (parameterListInfo.S3_Device.MaxList - 1));
      this.labelMBusSublists.Text = parameterListInfo.S3_Device.Sublists.ToString();
      this.textBoxMBusListNumber.Text = parameterListInfo.S3_Device.SelectedList.ToString();
      if (parameterListInfo.Radio != null)
      {
        this.buttonRadioListSet.Enabled = true;
        this.textBoxRadioListNumber.Enabled = true;
        this.labelRadioMaxList.Text = string.Format("[0...{0}]", (object) (parameterListInfo.Radio.MaxList - 1));
        this.textBoxRadioListNumber.Text = parameterListInfo.Radio.SelectedList.ToString();
      }
      else
      {
        this.buttonRadioListSet.Enabled = false;
        this.textBoxRadioListNumber.Enabled = false;
        this.labelRadioMaxList.Text = "[...]";
        this.textBoxRadioListNumber.Text = "";
      }
    }

    private void textBoxMBusListNumber_TextChanged(object sender, EventArgs e)
    {
      if (int.Parse(this.textBoxMBusListNumber.Text) <= (int) byte.MaxValue)
        return;
      this.textBoxMBusListNumber.Text = "0";
    }

    private void textBoxListNumber_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void buttonWrite_Click(object sender, EventArgs e)
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
      this.groupBoxSelectList = new GroupBox();
      this.labelMBusSublists = new Label();
      this.label5 = new Label();
      this.labelMBusMaxList = new Label();
      this.buttonMBusListSet = new Button();
      this.textBoxMBusListNumber = new TextBox();
      this.label1 = new Label();
      this.groupBox1 = new GroupBox();
      this.labelRadioMaxList = new Label();
      this.textBoxRadioListNumber = new TextBox();
      this.buttonRadioListSet = new Button();
      this.label2 = new Label();
      this.buttonRead = new Button();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.groupBoxSelectList.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.groupBoxSelectList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxSelectList.Controls.Add((Control) this.labelMBusSublists);
      this.groupBoxSelectList.Controls.Add((Control) this.label5);
      this.groupBoxSelectList.Controls.Add((Control) this.labelMBusMaxList);
      this.groupBoxSelectList.Controls.Add((Control) this.buttonMBusListSet);
      this.groupBoxSelectList.Controls.Add((Control) this.textBoxMBusListNumber);
      this.groupBoxSelectList.Controls.Add((Control) this.label1);
      this.groupBoxSelectList.Location = new Point(12, 46);
      this.groupBoxSelectList.Name = "groupBoxSelectList";
      this.groupBoxSelectList.Size = new Size(323, 74);
      this.groupBoxSelectList.TabIndex = 20;
      this.groupBoxSelectList.TabStop = false;
      this.groupBoxSelectList.Text = "MBus parameter list";
      this.labelMBusSublists.AutoSize = true;
      this.labelMBusSublists.Location = new Point(140, 50);
      this.labelMBusSublists.Name = "labelMBusSublists";
      this.labelMBusSublists.Size = new Size(13, 13);
      this.labelMBusSublists.TabIndex = 6;
      this.labelMBusSublists.Text = "0";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(88, 50);
      this.label5.Name = "label5";
      this.label5.Size = new Size(46, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Sublists:";
      this.labelMBusMaxList.AutoSize = true;
      this.labelMBusMaxList.Location = new Point(140, 24);
      this.labelMBusMaxList.Name = "labelMBusMaxList";
      this.labelMBusMaxList.Size = new Size(34, 13);
      this.labelMBusMaxList.TabIndex = 4;
      this.labelMBusMaxList.Text = "[0...?]";
      this.buttonMBusListSet.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonMBusListSet.Location = new Point(224, 17);
      this.buttonMBusListSet.Name = "buttonMBusListSet";
      this.buttonMBusListSet.Size = new Size(83, 31);
      this.buttonMBusListSet.TabIndex = 3;
      this.buttonMBusListSet.Text = "Set";
      this.buttonMBusListSet.UseVisualStyleBackColor = true;
      this.buttonMBusListSet.Click += new System.EventHandler(this.buttonMBusListSet_Click);
      this.textBoxMBusListNumber.Location = new Point(83, 21);
      this.textBoxMBusListNumber.Name = "textBoxMBusListNumber";
      this.textBoxMBusListNumber.Size = new Size(51, 20);
      this.textBoxMBusListNumber.TabIndex = 2;
      this.textBoxMBusListNumber.Text = "0";
      this.textBoxMBusListNumber.TextChanged += new System.EventHandler(this.textBoxMBusListNumber_TextChanged);
      this.textBoxMBusListNumber.KeyPress += new KeyPressEventHandler(this.textBoxListNumber_KeyPress);
      this.label1.Location = new Point(9, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(68, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "List number";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.labelRadioMaxList);
      this.groupBox1.Controls.Add((Control) this.textBoxRadioListNumber);
      this.groupBox1.Controls.Add((Control) this.buttonRadioListSet);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Location = new Point(12, 126);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(323, 57);
      this.groupBox1.TabIndex = 21;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Radio parameter list";
      this.labelRadioMaxList.AutoSize = true;
      this.labelRadioMaxList.Location = new Point(139, 25);
      this.labelRadioMaxList.Name = "labelRadioMaxList";
      this.labelRadioMaxList.Size = new Size(34, 13);
      this.labelRadioMaxList.TabIndex = 5;
      this.labelRadioMaxList.Text = "[0...?]";
      this.textBoxRadioListNumber.Location = new Point(82, 22);
      this.textBoxRadioListNumber.Name = "textBoxRadioListNumber";
      this.textBoxRadioListNumber.Size = new Size(51, 20);
      this.textBoxRadioListNumber.TabIndex = 4;
      this.textBoxRadioListNumber.Text = "0";
      this.buttonRadioListSet.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRadioListSet.Location = new Point(224, 16);
      this.buttonRadioListSet.Name = "buttonRadioListSet";
      this.buttonRadioListSet.Size = new Size(83, 31);
      this.buttonRadioListSet.TabIndex = 4;
      this.buttonRadioListSet.Text = "Set";
      this.buttonRadioListSet.UseVisualStyleBackColor = true;
      this.buttonRadioListSet.Click += new System.EventHandler(this.buttonRadioListSet_Click);
      this.label2.Location = new Point(8, 21);
      this.label2.Name = "label2";
      this.label2.Size = new Size(68, 20);
      this.label2.TabIndex = 4;
      this.label2.Text = "List number";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.buttonRead.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRead.Location = new Point(236, 192);
      this.buttonRead.Name = "buttonRead";
      this.buttonRead.Size = new Size(83, 31);
      this.buttonRead.TabIndex = 3;
      this.buttonRead.Text = "Read settings";
      this.buttonRead.UseVisualStyleBackColor = true;
      this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(355, 41);
      this.zennerCoroprateDesign2.TabIndex = 19;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(355, 235);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.buttonRead);
      this.Controls.Add((Control) this.groupBoxSelectList);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (ListSelectWindow);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (ListSelectWindow);
      this.groupBoxSelectList.ResumeLayout(false);
      this.groupBoxSelectList.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
