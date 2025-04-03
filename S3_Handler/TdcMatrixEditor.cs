// Decompiled with JetBrains decompiler
// Type: S3_Handler.TdcMatrixEditor
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace S3_Handler
{
  public class TdcMatrixEditor : Form
  {
    internal float[] tempValues;
    internal float[] flowValues;
    internal float[,] matrixValues;
    private DataTable ParamTable;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private DataGridView dataGridViewMatrix;
    private Button buttonSave;

    public TdcMatrixEditor() => this.InitializeComponent();

    private void TdcMatrixEditor_Load(object sender, EventArgs e)
    {
      this.ParamTable = new DataTable();
      this.ParamTable.Columns.Add(new DataColumn("temp", typeof (float)));
      for (int index = 0; index < this.flowValues.Length; ++index)
        this.ParamTable.Columns.Add(new DataColumn("value" + index.ToString(), typeof (float)));
      DataRow row1 = this.ParamTable.NewRow();
      row1[0] = (object) 0.0;
      for (int index = 0; index < this.flowValues.Length; ++index)
        row1[index + 1] = (object) this.flowValues[index];
      this.ParamTable.Rows.Add(row1);
      for (int index1 = 0; index1 < this.tempValues.Length; ++index1)
      {
        DataRow row2 = this.ParamTable.NewRow();
        row2[0] = (object) this.tempValues[index1];
        for (int index2 = 0; index2 < this.flowValues.Length; ++index2)
          row2[index2 + 1] = (object) this.matrixValues[index1, index2];
        this.ParamTable.Rows.Add(row2);
      }
      this.dataGridViewMatrix.ClearSelection();
      this.dataGridViewMatrix.DataSource = (object) this.ParamTable;
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      try
      {
        DataRow row1 = this.ParamTable.Rows[0];
        for (int index = 0; index < this.flowValues.Length; ++index)
          this.flowValues[index] = (float) row1[index + 1];
        for (int index1 = 0; index1 < this.tempValues.Length; ++index1)
        {
          DataRow row2 = this.ParamTable.Rows[index1 + 1];
          this.tempValues[index1] = (float) row2[0];
          for (int index2 = 0; index2 < this.flowValues.Length; ++index2)
            this.matrixValues[index1, index2] = (float) row2[index2 + 1];
        }
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
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.dataGridViewMatrix = new DataGridView();
      this.buttonSave = new Button();
      ((ISupportInitialize) this.dataGridViewMatrix).BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1283, 45);
      this.zennerCoroprateDesign2.TabIndex = 17;
      this.dataGridViewMatrix.AllowUserToAddRows = false;
      this.dataGridViewMatrix.AllowUserToDeleteRows = false;
      this.dataGridViewMatrix.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewMatrix.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewMatrix.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
      this.dataGridViewMatrix.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMatrix.Location = new Point(13, 51);
      this.dataGridViewMatrix.Name = "dataGridViewMatrix";
      this.dataGridViewMatrix.Size = new Size(1258, 562);
      this.dataGridViewMatrix.TabIndex = 18;
      this.buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSave.Location = new Point(1122, 619);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new Size(149, 23);
      this.buttonSave.TabIndex = 19;
      this.buttonSave.Text = "Save";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new EventHandler(this.buttonSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1283, 654);
      this.Controls.Add((Control) this.buttonSave);
      this.Controls.Add((Control) this.dataGridViewMatrix);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (TdcMatrixEditor);
      this.Text = nameof (TdcMatrixEditor);
      this.Load += new EventHandler(this.TdcMatrixEditor_Load);
      ((ISupportInitialize) this.dataGridViewMatrix).EndInit();
      this.ResumeLayout(false);
    }
  }
}
