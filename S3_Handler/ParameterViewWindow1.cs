// Decompiled with JetBrains decompiler
// Type: S3_Handler.ParameterViewWindow1
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class ParameterViewWindow1 : Form
  {
    private const int GridCol_S3_Name = 0;
    private const int GridCol_TranslatedName = 1;
    private const int GridCol_Bytes = 2;
    private const int GridCol_Address = 3;
    private const int GridCol_ValueHex = 4;
    private const int GridCol_ValueDec = 5;
    private Color ChangedColor = Color.Yellow;
    private S3_Meter MyMeter;
    private bool InitDone = false;
    private TdcMatrixEditor matrixEditor;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private DataGridView dataGridViewParameters;
    private TextBox textBoxDescription;
    private Label label1;
    private Button buttonOk;
    private Button buttonAccept;
    private TextBox textBoxSearchName;
    private Button buttonSearch;
    private Button buttonCheckValuesFromCsvFile;

    public ParameterViewWindow1() => this.InitializeComponent();

    internal void ShowParameterList(S3_Meter MyMeter)
    {
      if (MyMeter.CloneInfo == null)
        this.Text = "Parameter view ";
      else
        this.Text = "Parameter view (" + MyMeter.CloneInfo + ")";
      this.SuspendLayout();
      ZR_ClassLibMessages.ClearErrors();
      this.InitDone = false;
      this.MyMeter = MyMeter;
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add(new DataColumn("S3_Name", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Name", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Bytes", typeof (string)));
      dataTable.Columns.Add(new DataColumn("Address", typeof (string)));
      dataTable.Columns.Add(new DataColumn("ValueHex", typeof (string)));
      dataTable.Columns.Add(new DataColumn("ValueDec", typeof (string)));
      foreach (S3_Parameter s3Parameter in (IEnumerable<S3_Parameter>) MyMeter.MyParameters.ParameterByName.Values)
      {
        if (s3Parameter.ByteSize != 0)
        {
          DataRow row = dataTable.NewRow();
          if (s3Parameter.Statics.S3_VarType == S3_VariableTypes.TDC_Matrix)
          {
            row[0] = (object) s3Parameter.Name;
            row[1] = (object) ("TDC_Matrix; rows:" + s3Parameter.Statics.MinValue.ToString() + "; columns:" + s3Parameter.Statics.MaxValue.ToString());
            row[2] = (object) "";
            row[3] = (object) s3Parameter.BlockStartAddress.ToString("x04");
            row[4] = (object) "";
            row[5] = (object) "";
          }
          else if (s3Parameter.Statics.S3_VarType == S3_VariableTypes.ByteArray)
          {
            row[0] = (object) s3Parameter.Name;
            row[1] = (object) "ByteArray";
            row[2] = (object) ("size:" + s3Parameter.Statics.MinValue.ToString());
            row[3] = (object) s3Parameter.BlockStartAddress.ToString("x04");
            row[4] = (object) "";
            row[5] = (object) "";
          }
          else
          {
            row[0] = (object) s3Parameter.Name;
            row[1] = (object) s3Parameter.GetTranslatedParameterName();
            row[2] = (object) s3Parameter.ByteSize.ToString();
            row[3] = (object) s3Parameter.BlockStartAddress.ToString("x04");
            row[4] = (object) s3Parameter.GetUlongValue().ToString("x");
            row[5] = (object) s3Parameter.GetDisplayString();
          }
          dataTable.Rows.Add(row);
          this.matrixEditor = (TdcMatrixEditor) null;
        }
      }
      this.textBoxDescription.Clear();
      this.dataGridViewParameters.ClearSelection();
      this.dataGridViewParameters.DataSource = (object) dataTable;
      this.dataGridViewParameters.Columns[0].ReadOnly = true;
      this.dataGridViewParameters.Columns[1].ReadOnly = true;
      this.dataGridViewParameters.Columns[2].ReadOnly = true;
      this.dataGridViewParameters.Columns[3].ReadOnly = true;
      this.InitDone = true;
      if (this.dataGridViewParameters.Rows.Count > 0)
        this.dataGridViewParameters.Rows[0].Cells[4].Selected = true;
      this.ResumeLayout();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void dataGridView1_SelectionChanged(object sender, EventArgs e)
    {
      if (!this.InitDone)
        return;
      if (this.dataGridViewParameters.SelectedCells.Count == 0)
      {
        this.textBoxDescription.Clear();
      }
      else
      {
        string str = ((DataRowView) this.dataGridViewParameters.Rows[this.dataGridViewParameters.SelectedCells[0].RowIndex].DataBoundItem).Row[0].ToString();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(S3_Parameter.GetTranslatedParameterNameByName(str));
        stringBuilder.AppendLine("(" + str + ")");
        stringBuilder.AppendLine();
        S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[str];
        stringBuilder.AppendLine("Type: " + s3Parameter.Statics.S3_VarType.ToString());
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(S3_Parameter.GetTranslatedParameterDescription(str));
        this.textBoxDescription.Text = stringBuilder.ToString();
      }
    }

    private void dataGridViewParameters_CellBeginEdit(
      object sender,
      DataGridViewCellCancelEventArgs e)
    {
      if (e.ColumnIndex != 4 && e.ColumnIndex != 5)
      {
        e.Cancel = true;
      }
      else
      {
        S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[((DataRowView) this.dataGridViewParameters.Rows[e.RowIndex].DataBoundItem).Row[0].ToString()];
        if (s3Parameter.Statics.S3_VarType != S3_VariableTypes.TDC_Matrix)
          return;
        if (this.matrixEditor == null)
        {
          this.matrixEditor = new TdcMatrixEditor();
          int minValue = (int) s3Parameter.Statics.MinValue;
          int maxValue = (int) s3Parameter.Statics.MaxValue;
          this.matrixEditor.tempValues = new float[minValue];
          this.matrixEditor.flowValues = new float[maxValue];
          this.matrixEditor.matrixValues = new float[minValue, maxValue];
          int blockStartAddress = s3Parameter.BlockStartAddress;
          int index1 = 0;
          while (index1 < minValue)
          {
            this.matrixEditor.tempValues[index1] = (float) this.MyMeter.MyDeviceMemory.GetShortValue(blockStartAddress) / 100f;
            ++index1;
            blockStartAddress += 2;
          }
          int index2 = 0;
          while (index2 < maxValue)
          {
            this.matrixEditor.flowValues[index2] = this.MyMeter.MyDeviceMemory.GetFloatValue(blockStartAddress);
            ++index2;
            blockStartAddress += 4;
          }
          for (int index3 = 0; index3 < minValue; ++index3)
          {
            int index4 = 0;
            while (index4 < maxValue)
            {
              this.matrixEditor.matrixValues[index3, index4] = this.MyMeter.MyDeviceMemory.GetFloatValue(blockStartAddress);
              ++index4;
              blockStartAddress += 4;
            }
          }
        }
        int num = (int) this.matrixEditor.ShowDialog();
        this.SetChangeColors(e.RowIndex, false);
        e.Cancel = true;
      }
    }

    private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      try
      {
        if (e.ColumnIndex != 4 && e.ColumnIndex != 5)
          return;
        DataRow row = ((DataRowView) this.dataGridViewParameters.Rows[e.RowIndex].DataBoundItem).Row;
        string lower = row[e.ColumnIndex].ToString().Trim().ToLower();
        S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[row[0].ToString()];
        ulong ulongValue = s3Parameter.GetUlongValue();
        ulong UlongValue;
        if (e.ColumnIndex == 4)
        {
          if (!ulong.TryParse(lower, NumberStyles.HexNumber, (IFormatProvider) null, out UlongValue))
            UlongValue = ulongValue;
        }
        else if (!s3Parameter.GetUlongValueFromDisplayString(lower, out UlongValue))
          UlongValue = ulongValue;
        row[4] = (object) UlongValue.ToString("x");
        row[5] = (object) s3Parameter.GetDisplayStringFromUlongValue(UlongValue);
        this.SetChangeColors(e.RowIndex, (long) UlongValue == (long) ulongValue);
      }
      catch
      {
      }
    }

    private void SetChangeColors(int rowIndex, bool isNotChanged)
    {
      if (isNotChanged)
      {
        this.dataGridViewParameters.Rows[rowIndex].Cells[4].Style.BackColor = this.dataGridViewParameters.Rows[rowIndex].Cells[0].Style.BackColor;
        this.dataGridViewParameters.Rows[rowIndex].Cells[5].Style.BackColor = this.dataGridViewParameters.Rows[rowIndex].Cells[0].Style.BackColor;
      }
      else
      {
        this.dataGridViewParameters.Rows[rowIndex].Cells[4].Style.BackColor = this.ChangedColor;
        this.dataGridViewParameters.Rows[rowIndex].Cells[5].Style.BackColor = this.ChangedColor;
      }
    }

    private void RefreshChangeInfos()
    {
      for (int index = 0; index < this.dataGridViewParameters.Rows.Count; ++index)
      {
        DataRow row = ((DataRowView) this.dataGridViewParameters.Rows[index].DataBoundItem).Row;
        string str = row[5].ToString();
        string displayString = this.MyMeter.MyParameters.ParameterByName[row[0].ToString()].GetDisplayString();
        this.SetChangeColors(index, displayString == str);
      }
    }

    private void buttonAccept_Click(object sender, EventArgs e) => this.SaveChanges();

    private void buttonOk_Click(object sender, EventArgs e)
    {
      this.SaveChanges();
      this.Hide();
    }

    private void SaveChanges()
    {
      bool flag = false;
      for (int index1 = 0; index1 < this.dataGridViewParameters.Rows.Count; ++index1)
      {
        if (this.dataGridViewParameters.Rows[index1].Cells[5].Style.BackColor == this.ChangedColor)
        {
          if (!flag)
          {
            if (!this.MyMeter.MyFunctions.MyMeters.NewWorkMeter("Parameter changed"))
            {
              ZR_ClassLibMessages.ShowAndClearErrors("Meter parameter", "Change parameter error");
              return;
            }
            this.MyMeter = this.MyMeter.MyFunctions.MyMeters.WorkMeter;
            flag = true;
          }
          DataRow row = ((DataRowView) this.dataGridViewParameters.Rows[index1].DataBoundItem).Row;
          string DisplayString = row[5].ToString();
          S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName[row[0].ToString()];
          if (s3Parameter.Statics.S3_VarType == S3_VariableTypes.TDC_Matrix)
          {
            int minValue = (int) s3Parameter.Statics.MinValue;
            int maxValue = (int) s3Parameter.Statics.MaxValue;
            int blockStartAddress = s3Parameter.BlockStartAddress;
            int index2 = 0;
            while (index2 < minValue)
            {
              this.MyMeter.MyDeviceMemory.SetShortValue(blockStartAddress, (short) ((double) this.matrixEditor.tempValues[index2] * 100.0));
              ++index2;
              blockStartAddress += 2;
            }
            int index3 = 0;
            while (index3 < maxValue)
            {
              this.MyMeter.MyDeviceMemory.SetFloatValue(blockStartAddress, this.matrixEditor.flowValues[index3]);
              ++index3;
              blockStartAddress += 4;
            }
            for (int index4 = 0; index4 < minValue; ++index4)
            {
              int index5 = 0;
              while (index5 < maxValue)
              {
                this.MyMeter.MyDeviceMemory.SetFloatValue(blockStartAddress, this.matrixEditor.matrixValues[index4, index5]);
                ++index5;
                blockStartAddress += 4;
              }
            }
            this.matrixEditor = (TdcMatrixEditor) null;
          }
          else
            s3Parameter.SetFromDisplayString(DisplayString);
        }
      }
      this.MyMeter.MyIdentification.LoadDeviceIdFromParameter();
      this.RefreshChangeInfos();
      this.MyMeter.MyFunctions.SendMessageSwitchThread(20, GMM_EventArgs.MessageType.StatusChanged);
    }

    private void dataGridViewParameters_Sorted(object sender, EventArgs e)
    {
      this.RefreshChangeInfos();
    }

    private void buttonSearch_Click(object sender, EventArgs e) => this.SearchParameter();

    private void textBoxSearchName_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyValue != 13)
        return;
      this.SearchParameter();
      e.Handled = true;
    }

    private void SearchParameter()
    {
      string str = this.textBoxSearchName.Text.Trim();
      int index = 0;
      while (index < this.dataGridViewParameters.Rows.Count && !(((DataRowView) this.dataGridViewParameters.Rows[index].DataBoundItem).Row[0].ToString() == str))
        ++index;
      if (index >= this.dataGridViewParameters.Rows.Count)
      {
        index = 0;
        while (index < this.dataGridViewParameters.Rows.Count && !((DataRowView) this.dataGridViewParameters.Rows[index].DataBoundItem).Row[0].ToString().Contains(str))
          ++index;
      }
      if (index >= this.dataGridViewParameters.Rows.Count)
      {
        str = str.ToLower();
        index = 0;
        while (index < this.dataGridViewParameters.Rows.Count && !(((DataRowView) this.dataGridViewParameters.Rows[index].DataBoundItem).Row[0].ToString().ToLower() == str))
          ++index;
      }
      if (index >= this.dataGridViewParameters.Rows.Count)
      {
        index = 0;
        while (index < this.dataGridViewParameters.Rows.Count && !((DataRowView) this.dataGridViewParameters.Rows[index].DataBoundItem).Row[0].ToString().ToLower().Contains(str))
          ++index;
      }
      if (index >= this.dataGridViewParameters.Rows.Count)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Parameter view", "Parameter not found", true);
      }
      else
        this.dataGridViewParameters.CurrentCell = this.dataGridViewParameters.Rows[index].Cells[5];
    }

    private void buttonCheckValuesFromCsvFile_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = "";
      int num1 = 0;
      int num2 = 0;
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Compare data file (*.csv)|*.csv|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        openFileDialog.Title = "Read compare data csv file";
        openFileDialog.CheckFileExists = true;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
          {
            while (true)
            {
              string[] strArray;
              string key;
              do
              {
                do
                {
                  str = streamReader.ReadLine();
                  if (str != null)
                    ++num2;
                  else
                    goto label_39;
                }
                while (str.Trim().Length == 0);
                strArray = str.Split(';');
                key = strArray[0].Trim();
              }
              while (key.Length == 0);
              ++num1;
              DataRow dataRow = (DataRow) null;
              int index1;
              for (index1 = 0; index1 < this.dataGridViewParameters.Rows.Count; ++index1)
              {
                dataRow = ((DataRowView) this.dataGridViewParameters.Rows[index1].DataBoundItem).Row;
                if (dataRow[0].ToString() == key)
                  break;
              }
              if (index1 >= this.dataGridViewParameters.Rows.Count)
              {
                stringBuilder.AppendLine("'" + key + "' not found");
              }
              else
              {
                int index2 = this.MyMeter.MyParameters.ParameterByName.IndexOfKey(key);
                if (index2 < 0)
                {
                  stringBuilder.AppendLine("Parameter " + key + " not found");
                }
                else
                {
                  S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName.Values[index2];
                  string lower = strArray[1].Trim().ToLower();
                  string s = dataRow[5].ToString();
                  if (lower.Contains(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator) || s.Contains(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator))
                  {
                    double num3 = double.Parse(lower);
                    double num4 = double.Parse(s);
                    if (num3 != num4)
                      stringBuilder.AppendLine("Diff on " + key + " ; Meter value: " + num4.ToString() + "; Compare value: " + num3.ToString());
                  }
                  else if (lower.ToLower().StartsWith("0x"))
                  {
                    long num5 = long.Parse(lower.Substring(2), NumberStyles.HexNumber);
                    long num6 = long.Parse(s);
                    if (num5 != num6)
                      stringBuilder.AppendLine("Diff on " + key + " ; Meter value: 0x" + num6.ToString("x") + "; Compare value: 0x" + num5.ToString("x"));
                  }
                  else if (s3Parameter.Statics.S3_VarType == S3_VariableTypes.INT32)
                  {
                    int num7 = int.Parse(lower);
                    int num8 = int.Parse(s);
                    if (num7 != num8)
                      stringBuilder.AppendLine("Diff on " + key + " ; Meter value: " + num8.ToString() + "; Compare value: " + num7.ToString());
                  }
                  else if (s3Parameter.Statics.S3_VarType == S3_VariableTypes.INT16)
                  {
                    short num9 = short.Parse(lower);
                    short num10 = short.Parse(s);
                    if ((int) num9 != (int) num10)
                      stringBuilder.AppendLine("Diff on " + key + " ; Meter value: " + num10.ToString() + "; Compare value: " + num9.ToString());
                  }
                  else if (s3Parameter.Statics.S3_VarType == S3_VariableTypes.INT8)
                  {
                    sbyte num11 = sbyte.Parse(lower);
                    sbyte num12 = sbyte.Parse(s);
                    if ((int) num11 != (int) num12)
                      stringBuilder.AppendLine("Diff on " + key + " ; Meter value: " + num12.ToString() + "; Compare value: " + num11.ToString());
                  }
                  else
                  {
                    long num13 = long.Parse(lower);
                    long num14 = long.Parse(s);
                    if (num13 != num14)
                      stringBuilder.AppendLine("Diff on " + key + " ; Meter value: " + num14.ToString() + "; Compare value: " + num13.ToString());
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        stringBuilder.AppendLine("Exception");
        stringBuilder.AppendLine("FileLine: " + str);
        stringBuilder.AppendLine(ex.ToString());
      }
label_39:
      stringBuilder.AppendLine(num1.ToString() + " parameters checked");
      int num15 = (int) GMM_MessageBox.ShowMessage("Check message", stringBuilder.ToString());
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
      this.dataGridViewParameters = new DataGridView();
      this.textBoxDescription = new TextBox();
      this.label1 = new Label();
      this.buttonOk = new Button();
      this.buttonAccept = new Button();
      this.textBoxSearchName = new TextBox();
      this.buttonSearch = new Button();
      this.buttonCheckValuesFromCsvFile = new Button();
      ((ISupportInitialize) this.dataGridViewParameters).BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1268, 105);
      this.zennerCoroprateDesign2.TabIndex = 16;
      this.dataGridViewParameters.AllowUserToAddRows = false;
      this.dataGridViewParameters.AllowUserToDeleteRows = false;
      this.dataGridViewParameters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewParameters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewParameters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewParameters.EditMode = DataGridViewEditMode.EditOnEnter;
      this.dataGridViewParameters.Location = new Point(0, 40);
      this.dataGridViewParameters.Name = "dataGridViewParameters";
      this.dataGridViewParameters.Size = new Size(984, 630);
      this.dataGridViewParameters.TabIndex = 17;
      this.dataGridViewParameters.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.dataGridViewParameters_CellBeginEdit);
      this.dataGridViewParameters.CellEndEdit += new DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
      this.dataGridViewParameters.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
      this.dataGridViewParameters.Sorted += new System.EventHandler(this.dataGridViewParameters_Sorted);
      this.textBoxDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxDescription.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxDescription.Location = new Point(991, 123);
      this.textBoxDescription.Multiline = true;
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.ReadOnly = true;
      this.textBoxDescription.Size = new Size(265, 448);
      this.textBoxDescription.TabIndex = 18;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(990, 107);
      this.label1.Name = "label1";
      this.label1.Size = new Size(60, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "Description";
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.Location = new Point(1144, 634);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(112, 27);
      this.buttonOk.TabIndex = 20;
      this.buttonOk.Text = "Ok";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      this.buttonAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonAccept.Location = new Point(1026, 634);
      this.buttonAccept.Name = "buttonAccept";
      this.buttonAccept.Size = new Size(112, 27);
      this.buttonAccept.TabIndex = 20;
      this.buttonAccept.Text = "Accept";
      this.buttonAccept.UseVisualStyleBackColor = true;
      this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
      this.textBoxSearchName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxSearchName.Location = new Point(993, 53);
      this.textBoxSearchName.Name = "textBoxSearchName";
      this.textBoxSearchName.Size = new Size(263, 20);
      this.textBoxSearchName.TabIndex = 21;
      this.textBoxSearchName.KeyDown += new KeyEventHandler(this.textBoxSearchName_KeyDown);
      this.buttonSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSearch.Location = new Point(1200, 79);
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.Size = new Size(56, 23);
      this.buttonSearch.TabIndex = 22;
      this.buttonSearch.Text = "Search";
      this.buttonSearch.UseVisualStyleBackColor = true;
      this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
      this.buttonCheckValuesFromCsvFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCheckValuesFromCsvFile.Location = new Point(993, 79);
      this.buttonCheckValuesFromCsvFile.Name = "buttonCheckValuesFromCsvFile";
      this.buttonCheckValuesFromCsvFile.Size = new Size(201, 23);
      this.buttonCheckValuesFromCsvFile.TabIndex = 22;
      this.buttonCheckValuesFromCsvFile.Text = "Check values from csv file";
      this.buttonCheckValuesFromCsvFile.UseVisualStyleBackColor = true;
      this.buttonCheckValuesFromCsvFile.Click += new System.EventHandler(this.buttonCheckValuesFromCsvFile_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1268, 673);
      this.Controls.Add((Control) this.buttonCheckValuesFromCsvFile);
      this.Controls.Add((Control) this.buttonSearch);
      this.Controls.Add((Control) this.textBoxSearchName);
      this.Controls.Add((Control) this.buttonAccept);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxDescription);
      this.Controls.Add((Control) this.dataGridViewParameters);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (ParameterViewWindow1);
      this.Text = "Parameter view and edit";
      ((ISupportInitialize) this.dataGridViewParameters).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
