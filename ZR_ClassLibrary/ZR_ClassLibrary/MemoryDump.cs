// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MemoryDump
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using CorporateDesign;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class MemoryDump : Form
  {
    private Button buttonOk;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;
    private DataGrid dataGridMemoryData;
    private const string AddressFormat = "x4";
    private const string ValueFormat = "x2";
    private DataTable MyData;
    private ArrayList DataArray;
    private string FileName;
    public int DataStartAddress;
    public int DataEndAddress;
    private Button buttonWriteChanges;
    public int[] DataField;
    private Button FillButton;
    private TextBox FillTextBox;
    private Label label1;
    private TextBox textBoxVon;
    private Label label2;
    private TextBox textBoxBis;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button buttonWriteAll;
    public bool WriteChanges;
    private Button buttonSave;
    private Button buttonRead;
    private bool WriteAll;

    public MemoryDump()
    {
      this.InitializeComponent();
      this.InitGridFormat();
      this.WriteChanges = false;
      this.WriteAll = false;
      this.FileName = "MemoryDump.txt";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dataGridMemoryData = new DataGrid();
      this.buttonOk = new Button();
      this.buttonWriteChanges = new Button();
      this.FillButton = new Button();
      this.FillTextBox = new TextBox();
      this.label1 = new Label();
      this.textBoxVon = new TextBox();
      this.label2 = new Label();
      this.textBoxBis = new TextBox();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.buttonWriteAll = new Button();
      this.buttonSave = new Button();
      this.buttonRead = new Button();
      this.dataGridMemoryData.BeginInit();
      this.SuspendLayout();
      this.dataGridMemoryData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridMemoryData.DataMember = "";
      this.dataGridMemoryData.HeaderForeColor = SystemColors.ControlText;
      this.dataGridMemoryData.Location = new Point(24, 48);
      this.dataGridMemoryData.Name = "dataGridMemoryData";
      this.dataGridMemoryData.PreferredColumnWidth = 35;
      this.dataGridMemoryData.Size = new Size(840, 384);
      this.dataGridMemoryData.TabIndex = 0;
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.Location = new Point(800, 440);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(64, 24);
      this.buttonOk.TabIndex = 1;
      this.buttonOk.Text = "Ok";
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      this.buttonWriteChanges.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonWriteChanges.Location = new Point(696, 440);
      this.buttonWriteChanges.Name = "buttonWriteChanges";
      this.buttonWriteChanges.Size = new Size(96, 24);
      this.buttonWriteChanges.TabIndex = 3;
      this.buttonWriteChanges.Text = "WriteChanges";
      this.buttonWriteChanges.Click += new System.EventHandler(this.buttonWriteChanges_Click);
      this.FillButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.FillButton.Location = new Point(24, 440);
      this.FillButton.Name = "FillButton";
      this.FillButton.Size = new Size(75, 24);
      this.FillButton.TabIndex = 4;
      this.FillButton.Text = "fülle mit";
      this.FillButton.Click += new System.EventHandler(this.FillButton_Click);
      this.FillTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.FillTextBox.Location = new Point(104, 440);
      this.FillTextBox.Name = "FillTextBox";
      this.FillTextBox.Size = new Size(48, 20);
      this.FillTextBox.TabIndex = 5;
      this.FillTextBox.Text = "00";
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.Location = new Point(160, 440);
      this.label1.Name = "label1";
      this.label1.Size = new Size(32, 16);
      this.label1.TabIndex = 6;
      this.label1.Text = "von:";
      this.textBoxVon.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxVon.Location = new Point(192, 440);
      this.textBoxVon.Name = "textBoxVon";
      this.textBoxVon.Size = new Size(48, 20);
      this.textBoxVon.TabIndex = 5;
      this.textBoxVon.Text = "";
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.Location = new Point(256, 440);
      this.label2.Name = "label2";
      this.label2.Size = new Size(24, 16);
      this.label2.TabIndex = 6;
      this.label2.Text = "bis:";
      this.textBoxBis.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxBis.Location = new Point(280, 440);
      this.textBoxBis.Name = "textBoxBis";
      this.textBoxBis.Size = new Size(48, 20);
      this.textBoxBis.TabIndex = 5;
      this.textBoxBis.Text = "";
      this.zennerCoroprateDesign1.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(872, 469);
      this.zennerCoroprateDesign1.TabIndex = 7;
      this.buttonWriteAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonWriteAll.Location = new Point(592, 440);
      this.buttonWriteAll.Name = "buttonWriteAll";
      this.buttonWriteAll.Size = new Size(96, 24);
      this.buttonWriteAll.TabIndex = 3;
      this.buttonWriteAll.Text = "WriteAll";
      this.buttonWriteAll.Click += new System.EventHandler(this.buttonWriteAll_Click);
      this.buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSave.Location = new Point(488, 440);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new Size(96, 24);
      this.buttonSave.TabIndex = 3;
      this.buttonSave.Text = "Save to file";
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      this.buttonRead.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRead.Location = new Point(384, 440);
      this.buttonRead.Name = "buttonRead";
      this.buttonRead.Size = new Size(96, 24);
      this.buttonRead.TabIndex = 3;
      this.buttonRead.Text = "Read from file";
      this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(872, 469);
      this.ControlBox = false;
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.FillTextBox);
      this.Controls.Add((Control) this.textBoxVon);
      this.Controls.Add((Control) this.textBoxBis);
      this.Controls.Add((Control) this.FillButton);
      this.Controls.Add((Control) this.buttonWriteChanges);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.dataGridMemoryData);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.buttonWriteAll);
      this.Controls.Add((Control) this.buttonSave);
      this.Controls.Add((Control) this.buttonRead);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Name = nameof (MemoryDump);
      this.Text = "Memory Dump";
      this.dataGridMemoryData.EndInit();
      this.ResumeLayout(false);
    }

    private void InitGridFormat()
    {
      this.MyData = new DataTable("RAM-Layout");
      this.DataArray = new ArrayList();
      this.MyData.Columns.Add("Address ", typeof (string));
      this.MyData.Columns[0].ReadOnly = true;
      this.MyData.Columns.Add("0x00 ", typeof (string));
      this.MyData.Columns.Add("0x01 ", typeof (string));
      this.MyData.Columns.Add("0x02 ", typeof (string));
      this.MyData.Columns.Add("0x03 ", typeof (string));
      this.MyData.Columns.Add("0x04 ", typeof (string));
      this.MyData.Columns.Add("0x05 ", typeof (string));
      this.MyData.Columns.Add("0x06 ", typeof (string));
      this.MyData.Columns.Add("0x07 ", typeof (string));
      this.MyData.Columns.Add("0x08 ", typeof (string));
      this.MyData.Columns.Add("0x09 ", typeof (string));
      this.MyData.Columns.Add("0x0a ", typeof (string));
      this.MyData.Columns.Add("0x0b ", typeof (string));
      this.MyData.Columns.Add("0x0c ", typeof (string));
      this.MyData.Columns.Add("0x0d ", typeof (string));
      this.MyData.Columns.Add("0x0e ", typeof (string));
      this.MyData.Columns.Add("0x0f ", typeof (string));
    }

    public void SetMemory(int StartAddress, int EndAddress, int[] data, string RangeName)
    {
      this.MyData.Clear();
      this.DataArray.Clear();
      int num1 = 0;
      int num2 = StartAddress / 16 * 16;
      int index1 = 0;
      int[] RowData = new int[16];
      while (num2 <= EndAddress)
      {
        for (int index2 = 0; index2 < 16; ++index2)
        {
          if (num2 < StartAddress || num2 > EndAddress)
          {
            RowData[index2] = -1;
          }
          else
          {
            RowData[index2] = data[index1];
            ++index1;
          }
          this.DataArray.Add((object) new MemoryDump.DataInfo()
          {
            Address = num2,
            data = RowData[index2],
            row = num1,
            column = (index2 + 1)
          });
          ++num2;
        }
        this.AddMemoryRow(num2 - 16, RowData);
        ++num1;
      }
      this.dataGridMemoryData.CaptionText = RangeName;
      this.dataGridMemoryData.DataSource = (object) this.MyData;
    }

    private void AddMemoryRow(int Address, int[] RowData)
    {
      DataRow row = this.MyData.NewRow();
      row[0] = (object) Address.ToString("x4");
      for (int index = 0; index < 16; ++index)
        row[index + 1] = RowData[index] >= 0 ? (object) RowData[index].ToString("x2") : (object) "";
      this.MyData.Rows.Add(row);
    }

    public void GetChangedData(ref ArrayList data)
    {
      for (int index = 0; index < this.DataArray.Count; ++index)
      {
        int data1 = ((MemoryDump.DataInfo) this.DataArray[index]).data;
        string str = data1 != -1 ? data1.ToString("x2") : "";
        string s = this.MyData.Rows[((MemoryDump.DataInfo) this.DataArray[index]).row][((MemoryDump.DataInfo) this.DataArray[index]).column].ToString();
        if (s.Length != 0 && (this.WriteAll || s != str))
        {
          int num = int.Parse(s, NumberStyles.HexNumber);
          ((MemoryDump.DataInfo) this.DataArray[index]).dataNew = num;
          data.Add((object) new MemoryDump.ChangedDataInfo()
          {
            Address = ((MemoryDump.DataInfo) this.DataArray[index]).Address,
            NewData = num
          });
        }
      }
    }

    private void buttonOk_Click(object sender, EventArgs e) => this.Hide();

    private void buttonWriteChanges_Click(object sender, EventArgs e)
    {
      this.Hide();
      this.WriteChanges = true;
    }

    private void buttonWriteAll_Click(object sender, EventArgs e)
    {
      this.Hide();
      this.WriteChanges = true;
      this.WriteAll = true;
    }

    private void FillButton_Click(object sender, EventArgs e)
    {
      int num1;
      int num2;
      int num3;
      try
      {
        num1 = int.Parse(this.FillTextBox.Text, NumberStyles.AllowHexSpecifier);
        num2 = int.Parse(this.textBoxVon.Text, NumberStyles.AllowHexSpecifier);
        num3 = int.Parse(this.textBoxBis.Text, NumberStyles.AllowHexSpecifier);
      }
      catch
      {
        int num4 = (int) MessageBox.Show("format error");
        return;
      }
      if (num1 < 0 || num1 > (int) byte.MaxValue)
      {
        int num5 = (int) MessageBox.Show("value out of range");
      }
      else
      {
        string str = num1.ToString("x2");
        foreach (MemoryDump.DataInfo data in this.DataArray)
        {
          if (data.Address <= num3 && data.Address >= num2)
            this.MyData.Rows[data.row][data.column] = (object) str;
        }
      }
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      if (!this.SelectFilename(false))
        return;
      StreamWriter streamWriter = new StreamWriter(this.FileName);
      StringBuilder stringBuilder = new StringBuilder(100);
      bool flag1 = true;
      bool flag2 = true;
      int num = 0;
      foreach (MemoryDump.DataInfo data in this.DataArray)
      {
        if (!flag2 && (data.Address & 2147483632) != num)
        {
          streamWriter.WriteLine(stringBuilder.ToString());
          flag1 = true;
        }
        flag2 = false;
        if (flag1)
        {
          stringBuilder.Length = 0;
          num = data.Address & 2147483632;
          stringBuilder.Append(num.ToString("x08"));
          stringBuilder.Append(": ");
          flag1 = false;
        }
        if ((data.Address & 3) == 0 && (data.Address & 15) != 0)
          stringBuilder.Append(" | ");
        else
          stringBuilder.Append(" ");
        if (data.data >= 0)
          stringBuilder.Append(data.data.ToString("x02"));
        else
          stringBuilder.Append("..");
      }
      if (!flag2)
        streamWriter.WriteLine(stringBuilder.ToString());
      streamWriter.Close();
    }

    private void buttonRead_Click(object sender, EventArgs e)
    {
      if (!this.SelectFilename(true))
        return;
      int[] data = new int[65536];
      int num1 = 0;
      try
      {
        using (StreamReader streamReader = new StreamReader(this.FileName))
        {
          int StartAddress = -1;
          int num2 = -1;
          int num3 = -1;
          try
          {
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              int startIndex = 11;
              int num4 = int.Parse(str.Substring(0, 8), NumberStyles.AllowHexSpecifier);
              if (num3 == -1)
                num3 = num4;
              else if (num3 != num4)
              {
                int num5 = (int) MessageBox.Show("Illegal line address!");
                return;
              }
              for (int index = 0; index < 16; ++index)
              {
                string s = str.Substring(startIndex, 2);
                if (s == "..")
                {
                  if (StartAddress != -1 && num2 == -1)
                    num2 = num3 - 1;
                }
                else
                {
                  int num6 = int.Parse(s, NumberStyles.AllowHexSpecifier);
                  data[num1++] = num6;
                  if (StartAddress == -1)
                    StartAddress = num3;
                }
                ++num3;
                startIndex += 3;
                if (startIndex == 23 || startIndex == 37 | startIndex == 51)
                  startIndex += 2;
              }
            }
          }
          catch (Exception ex)
          {
            int num7 = (int) MessageBox.Show("Illegal format\r\n" + ex.ToString());
          }
          if (num2 == -1)
          {
            int num8 = num3 - 1;
          }
          this.SetMemory(StartAddress, StartAddress + num1 - 1, data, "From file");
        }
      }
      catch (Exception ex)
      {
        int num9 = (int) MessageBox.Show("File open error\r\n" + ex.ToString());
      }
    }

    internal bool SelectFilename(bool read)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = Path.GetFullPath(Application.ExecutablePath);
      openFileDialog.Filter = "Dump files (*.txt)|*.txt| All files (*.*)|*.*";
      openFileDialog.FilterIndex = 1;
      openFileDialog.RestoreDirectory = true;
      if (read)
      {
        openFileDialog.Title = "Read memory dump from file";
        openFileDialog.CheckFileExists = true;
      }
      else
      {
        openFileDialog.Title = "Save memory dunp to file";
        openFileDialog.CheckFileExists = false;
      }
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return false;
      this.FileName = openFileDialog.FileName;
      return true;
    }

    private struct DataInfo
    {
      public int Address;
      public int data;
      public int dataNew;
      public int row;
      public int column;
    }

    public struct ChangedDataInfo
    {
      public int Address;
      public int NewData;
    }
  }
}
