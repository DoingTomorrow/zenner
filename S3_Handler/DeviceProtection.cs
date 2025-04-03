// Decompiled with JetBrains decompiler
// Type: S3_Handler.DeviceProtection
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class DeviceProtection : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Button buttonSave;
    private DataGridView dataGridViewNotProtectedRanges;
    private Label label1;
    private DataGridViewTextBoxColumn LableName;
    private DataGridViewTextBoxColumn FromAddress;
    private DataGridViewTextBoxColumn ToAddress;

    public DeviceProtection(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      if (this.MyFunctions.MyMeters.WorkMeter == null)
        this.Close();
      else
        this.LoadProtection();
    }

    private void LoadProtection()
    {
      S3_Meter workMeter = this.MyFunctions.MyMeters.WorkMeter;
      List<KeyValuePair<int, int>> protectionRanges = this.MyFunctions.MyMeters.WorkMeter.MyWriteProtTableManager.GetProtectionRanges();
      this.dataGridViewNotProtectedRanges.Rows.Clear();
      for (int index1 = 0; index1 < protectionRanges.Count; ++index1)
      {
        this.dataGridViewNotProtectedRanges.Rows.Add();
        DataGridViewCell cell1 = this.dataGridViewNotProtectedRanges.Rows[index1].Cells[1];
        KeyValuePair<int, int> keyValuePair = protectionRanges[index1];
        int key1 = keyValuePair.Key;
        string str1 = key1.ToString("x04");
        cell1.Value = (object) str1;
        DataGridViewCell cell2 = this.dataGridViewNotProtectedRanges.Rows[index1].Cells[2];
        keyValuePair = protectionRanges[index1];
        key1 = keyValuePair.Value;
        string str2 = key1.ToString("x04");
        cell2.Value = (object) str2;
        keyValuePair = protectionRanges[index1];
        int key2 = keyValuePair.Key;
        int index2 = workMeter.MyParameters.AddressLables.IndexOfValue(key2);
        if (index2 >= 0)
        {
          this.dataGridViewNotProtectedRanges.Rows[index1].Cells[0].Value = (object) workMeter.MyParameters.AddressLables.Keys[index2];
        }
        else
        {
          int index3 = workMeter.MyParameters.ParameterByAddress.IndexOfKey(key2);
          this.dataGridViewNotProtectedRanges.Rows[index1].Cells[0].Value = index3 < 0 ? (object) "---" : (object) workMeter.MyParameters.ParameterByAddress.Values[index3].Name;
        }
      }
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      try
      {
        List<KeyValuePair<int, int>> ranges = new List<KeyValuePair<int, int>>();
        for (int index = 0; index < this.dataGridViewNotProtectedRanges.Rows.Count - 1; ++index)
        {
          int key = int.Parse(this.dataGridViewNotProtectedRanges.Rows[index].Cells[1].Value.ToString(), NumberStyles.HexNumber);
          int num = int.Parse(this.dataGridViewNotProtectedRanges.Rows[index].Cells[2].Value.ToString(), NumberStyles.HexNumber);
          ranges.Add(new KeyValuePair<int, int>(key, num));
        }
        this.MyFunctions.MyMeters.NewWorkMeter("Protection changed");
        this.MyFunctions.MyMeters.WorkMeter.MyWriteProtTableManager.SetProtectionRanges(ranges);
        this.MyFunctions.MyMeters.WorkMeter.Compile();
        this.LoadProtection();
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Exception, ex.ToString());
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
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
      this.buttonSave = new Button();
      this.dataGridViewNotProtectedRanges = new DataGridView();
      this.label1 = new Label();
      this.LableName = new DataGridViewTextBoxColumn();
      this.FromAddress = new DataGridViewTextBoxColumn();
      this.ToAddress = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dataGridViewNotProtectedRanges).BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(730, 45);
      this.zennerCoroprateDesign2.TabIndex = 19;
      this.buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSave.Location = new Point(605, 468);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new Size(113, 23);
      this.buttonSave.TabIndex = 20;
      this.buttonSave.Text = "Save";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      this.dataGridViewNotProtectedRanges.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewNotProtectedRanges.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewNotProtectedRanges.Columns.AddRange((DataGridViewColumn) this.LableName, (DataGridViewColumn) this.FromAddress, (DataGridViewColumn) this.ToAddress);
      this.dataGridViewNotProtectedRanges.Location = new Point(16, 77);
      this.dataGridViewNotProtectedRanges.Name = "dataGridViewNotProtectedRanges";
      this.dataGridViewNotProtectedRanges.Size = new Size(545, 414);
      this.dataGridViewNotProtectedRanges.TabIndex = 21;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 51);
      this.label1.Name = "label1";
      this.label1.Size = new Size(146, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Not protected memory ranges";
      this.LableName.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader;
      this.LableName.HeaderText = "Lable name";
      this.LableName.Name = "LableName";
      this.LableName.ReadOnly = true;
      this.LableName.Width = 21;
      this.FromAddress.HeaderText = "From address";
      this.FromAddress.Name = "FromAddress";
      this.ToAddress.HeaderText = "To address";
      this.ToAddress.Name = "ToAddress";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(730, 503);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dataGridViewNotProtectedRanges);
      this.Controls.Add((Control) this.buttonSave);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (DeviceProtection);
      this.Text = "WritePermission";
      ((ISupportInitialize) this.dataGridViewNotProtectedRanges).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
