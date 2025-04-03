// Decompiled with JetBrains decompiler
// Type: GMM_Handler.TypeDetailWindow
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace GMM_Handler
{
  public class TypeDetailWindow : Form
  {
    private IContainer components = (IContainer) null;
    private DataGridView dataGridViewDetails;
    private DataGridViewTextBoxColumn InfoName;
    private DataGridViewTextBoxColumn InfoValue;

    public TypeDetailWindow() => this.InitializeComponent();

    internal void SetDataFromRow(DataRow TheRow)
    {
      this.dataGridViewDetails.Rows.Clear();
      for (int index1 = 0; index1 < TheRow.Table.Columns.Count; ++index1)
      {
        string columnName = TheRow.Table.Columns[index1].ColumnName;
        string str = TheRow[index1].ToString();
        if (columnName == "MeterLoad")
        {
          int num = 0;
          string[] strArray1 = str.Split('|');
          if (!(strArray1[0] == ""))
          {
            for (int index2 = 0; index2 < strArray1.Length; ++index2)
            {
              string[] strArray2 = strArray1[index2].Split(';');
              if (strArray2.Length != 1 || !(strArray2[0] == ""))
              {
                for (int index3 = 0; index3 < strArray2.Length; ++index3)
                {
                  if (!(strArray2[index3] == ""))
                  {
                    int index4 = this.dataGridViewDetails.Rows.Add();
                    if (strArray2[index3].StartsWith("M:"))
                    {
                      ++num;
                      this.dataGridViewDetails.Rows[index4].Cells[0].Value = (object) (num.ToString() + ": MeterNumber");
                      this.dataGridViewDetails.Rows[index4].Cells[1].Value = (object) strArray2[index3].Substring(2);
                    }
                    else if (strArray2[index3].StartsWith("P:"))
                    {
                      this.dataGridViewDetails.Rows[index4].Cells[0].Value = (object) (num.ToString() + ": Production");
                      this.dataGridViewDetails.Rows[index4].Cells[1].Value = (object) strArray2[index3].Substring(2);
                    }
                    else if (strArray2[index3].StartsWith("A:"))
                    {
                      this.dataGridViewDetails.Rows[index4].Cells[0].Value = (object) (num.ToString() + ": Aproval");
                      this.dataGridViewDetails.Rows[index4].Cells[1].Value = (object) strArray2[index3].Substring(2);
                    }
                    else if (strArray2[index3].StartsWith("D:"))
                    {
                      this.dataGridViewDetails.Rows[index4].Cells[0].Value = (object) (num.ToString() + ": LastDataFrom");
                      this.dataGridViewDetails.Rows[index4].Cells[1].Value = (object) strArray2[index3].Substring(2);
                    }
                    else
                    {
                      this.dataGridViewDetails.Rows[index4].Cells[0].Value = (object) (num.ToString() + ": MeterInfo");
                      this.dataGridViewDetails.Rows[index4].Cells[1].Value = (object) strArray2[index3];
                    }
                  }
                }
              }
            }
          }
        }
        else
        {
          int index5 = this.dataGridViewDetails.Rows.Add();
          this.dataGridViewDetails.Rows[index5].Cells[0].Value = (object) columnName;
          this.dataGridViewDetails.Rows[index5].Cells[1].Value = (object) str;
        }
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
      this.dataGridViewDetails = new DataGridView();
      this.InfoName = new DataGridViewTextBoxColumn();
      this.InfoValue = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dataGridViewDetails).BeginInit();
      this.SuspendLayout();
      this.dataGridViewDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewDetails.Columns.AddRange((DataGridViewColumn) this.InfoName, (DataGridViewColumn) this.InfoValue);
      this.dataGridViewDetails.Dock = DockStyle.Fill;
      this.dataGridViewDetails.Location = new Point(0, 0);
      this.dataGridViewDetails.Name = "dataGridViewDetails";
      this.dataGridViewDetails.Size = new Size(730, 417);
      this.dataGridViewDetails.TabIndex = 0;
      this.InfoName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.InfoName.HeaderText = "InfoName";
      this.InfoName.Name = "InfoName";
      this.InfoName.Width = 78;
      this.InfoValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.InfoValue.HeaderText = "InfoValue";
      this.InfoValue.Name = "InfoValue";
      this.InfoValue.Width = 77;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(730, 417);
      this.Controls.Add((Control) this.dataGridViewDetails);
      this.Name = nameof (TypeDetailWindow);
      this.Text = nameof (TypeDetailWindow);
      ((ISupportInitialize) this.dataGridViewDetails).EndInit();
      this.ResumeLayout(false);
    }
  }
}
