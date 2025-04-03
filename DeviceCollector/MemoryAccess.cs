// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MemoryAccess
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MemoryAccess : Form
  {
    private Color ReadBackColor = Color.LightGreen;
    private Color NewBackColor = Color.Yellow;
    private Color ChangedBackColor = Color.LightPink;
    private Color EditBackColor = Color.LightSkyBlue;
    private DeviceCollectorFunctions MyFunctions;
    private SortedList<int, byte> ReadData;
    private SortedList<int, byte> NewReceivedData;
    private SortedList<int, byte> WriteData;
    private bool ChangeActive = true;
    private MemoryLocation TheLocation;
    private int StartAddress;
    private int ByteSize;
    private string EditCellValue;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private Panel panel1;
    private DataGridView dataGridViewMemory;
    private ComboBox comboBoxLocation;
    private Label label1;
    private TextBox textBoxStartAddress;
    private Label label2;
    private TextBox textBoxNumberOfBytes;
    private Label labelLocation;
    private Button buttonEraseFlash;
    private Button buttonWriteChanges;
    private Button buttonReadMemory;
    private Button buttonClearAll;
    private DataGridViewTextBoxColumn Address;
    private DataGridViewTextBoxColumn Col00;
    private DataGridViewTextBoxColumn Col01;
    private DataGridViewTextBoxColumn Col02;
    private DataGridViewTextBoxColumn Col03;
    private DataGridViewTextBoxColumn Col04;
    private DataGridViewTextBoxColumn Col05;
    private DataGridViewTextBoxColumn Col06;
    private DataGridViewTextBoxColumn Col07;
    private DataGridViewTextBoxColumn Col08;
    private DataGridViewTextBoxColumn Col09;
    private DataGridViewTextBoxColumn Col0a;
    private DataGridViewTextBoxColumn Col0b;
    private DataGridViewTextBoxColumn Col0c;
    private DataGridViewTextBoxColumn Col0d;
    private DataGridViewTextBoxColumn Col0e;
    private DataGridViewTextBoxColumn Col0f;

    public MemoryAccess(DeviceCollectorFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      this.comboBoxLocation.DataSource = (object) Enum.GetNames(typeof (MemoryLocation));
      this.ReadData = new SortedList<int, byte>();
      this.NewReceivedData = new SortedList<int, byte>();
      this.WriteData = new SortedList<int, byte>();
    }

    private void MemoryAccess_Load(object sender, EventArgs e)
    {
      this.comboBoxLocation.SelectedIndex = 1;
      this.ChangeActive = false;
    }

    private void MemoryAccess_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = true;
      this.Hide();
    }

    private void buttonClearAll_Click(object sender, EventArgs e)
    {
      this.ChangeActive = true;
      this.dataGridViewMemory.Rows.Clear();
      this.ReadData.Clear();
      this.NewReceivedData.Clear();
      this.WriteData.Clear();
      this.ChangeActive = false;
    }

    private void buttonReadMemory_Click(object sender, EventArgs e)
    {
      if (!this.LoadSetupValues())
        return;
      this.WriteData.Clear();
      ByteField MemoryData;
      if (!this.MyFunctions.ReadMemory(this.TheLocation, this.StartAddress, this.ByteSize, out MemoryData))
      {
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
      {
        this.AddReadDataFromList(this.NewReceivedData);
        int startAddress = this.StartAddress;
        for (int index = 0; index < MemoryData.Count; ++index)
        {
          this.NewReceivedData.Add(startAddress, MemoryData.Data[index]);
          ++startAddress;
        }
        this.UpdateGrid();
      }
    }

    private void buttonEraseFlash_Click(object sender, EventArgs e)
    {
      if (!this.LoadSetupValues())
        return;
      if (!this.MyFunctions.EraseFlash(this.StartAddress, this.ByteSize))
      {
        ZR_ClassLibMessages.AddErrorDescription("Error on erase flash");
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
      {
        this.AddReadDataFromList(this.NewReceivedData);
        this.AddReadDataFromList(this.WriteData);
        int num = this.StartAddress + this.ByteSize;
        for (int startAddress = this.StartAddress; startAddress < num; ++startAddress)
          this.WriteData.Add(startAddress, byte.MaxValue);
        this.AddReadDataFromList(this.WriteData);
        this.UpdateGrid();
      }
    }

    private void buttonWriteChanges_Click(object sender, EventArgs e)
    {
      if (this.WriteData.Count == 0)
        return;
      int key = this.WriteData.Keys[0];
      int num = key;
      ByteField data = new ByteField(this.WriteData.Count);
      int index = 0;
      while (true)
      {
        if (index >= this.WriteData.Count || this.WriteData.Keys[index] > num + 1)
        {
          if (this.MyFunctions.WriteMemory(this.TheLocation, key, data))
          {
            if (index < this.WriteData.Count)
            {
              key = this.WriteData.Keys[index];
              num = key;
              data.Count = 0;
              data.Add(this.WriteData.Values[index]);
            }
            else
              goto label_10;
          }
          else
            break;
        }
        else
        {
          num = this.WriteData.Keys[index];
          data.Add(this.WriteData.Values[index]);
        }
        ++index;
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
      return;
label_10:
      this.AddReadDataFromList(this.NewReceivedData);
      this.AddReadDataFromList(this.WriteData);
      this.UpdateGrid();
    }

    private void UpdateGrid()
    {
      this.dataGridViewMemory.Rows.Clear();
      this.ChangeActive = true;
      int num1 = 0;
      if (this.ReadData.Count > 0)
        num1 = this.ReadData.Keys[this.ReadData.Count - 1] | 15;
      if (this.NewReceivedData.Count > 0 && num1 < this.NewReceivedData.Keys[this.NewReceivedData.Count - 1])
        num1 = this.NewReceivedData.Keys[this.NewReceivedData.Count - 1];
      int num2 = num1 | 15;
      int num3 = int.MaxValue;
      if (this.ReadData.Count > 0)
        num3 = this.ReadData.Keys[0];
      if (this.NewReceivedData.Count > 0 && num3 > this.NewReceivedData.Keys[0])
        num3 = this.NewReceivedData.Keys[0];
      int num4 = num3 & -16;
      DataGridViewRow dataGridViewRow = (DataGridViewRow) null;
      bool flag1 = true;
      bool flag2 = false;
      for (int key = num4; key <= num2; ++key)
      {
        if ((key & 15) == 0)
        {
          if (flag1)
          {
            flag2 = false;
            flag1 = false;
            dataGridViewRow = this.dataGridViewMemory.Rows[this.dataGridViewMemory.Rows.Add()];
          }
          else if (!flag2)
          {
            flag2 = true;
            dataGridViewRow = this.dataGridViewMemory.Rows[this.dataGridViewMemory.Rows.Add()];
          }
          dataGridViewRow.Cells[0].Value = (object) key.ToString("x04");
          dataGridViewRow.Cells[0].ReadOnly = true;
        }
        int index1 = (key & 15) + 1;
        int index2 = this.ReadData.IndexOfKey(key);
        int index3 = this.NewReceivedData.IndexOfKey(key);
        if (index3 >= 0)
        {
          flag1 = true;
          if (index2 >= 0)
          {
            if ((int) this.NewReceivedData.Values[index3] == (int) this.ReadData.Values[index2])
            {
              dataGridViewRow.Cells[index1].Value = (object) this.NewReceivedData.Values[index3].ToString("x02");
              dataGridViewRow.Cells[index1].Style.BackColor = this.ReadBackColor;
            }
            else
            {
              dataGridViewRow.Cells[index1].Value = (object) (this.NewReceivedData.Values[index3].ToString("x02") + " (" + this.ReadData.Values[index2].ToString("x02") + ")");
              dataGridViewRow.Cells[index1].Style.BackColor = this.ChangedBackColor;
            }
          }
          else
          {
            dataGridViewRow.Cells[index1].Value = (object) this.NewReceivedData.Values[index3].ToString("x02");
            dataGridViewRow.Cells[index1].Style.BackColor = this.NewBackColor;
          }
        }
        else if (index2 >= 0)
        {
          flag1 = true;
          dataGridViewRow.Cells[index1].Value = (object) this.ReadData.Values[index2].ToString("x02");
        }
        else
          dataGridViewRow.Cells[index1].Value = (object) "--";
      }
      this.ChangeActive = false;
    }

    private void dataGridViewMemory_CellEnter(object sender, DataGridViewCellEventArgs e)
    {
      if (this.ChangeActive)
        return;
      if (this.dataGridViewMemory[e.ColumnIndex, e.RowIndex].Value == null)
        this.EditCellValue = string.Empty;
      else
        this.EditCellValue = this.dataGridViewMemory[e.ColumnIndex, e.RowIndex].Value.ToString();
    }

    private void dataGridViewMemory_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      if (this.dataGridViewMemory[e.ColumnIndex, e.RowIndex].Value == null || e.ColumnIndex == 0)
        return;
      int result1;
      if (!int.TryParse(this.dataGridViewMemory[0, e.RowIndex].Value != null ? this.dataGridViewMemory[0, e.RowIndex].Value.ToString() : string.Empty, NumberStyles.HexNumber, (IFormatProvider) null, out result1))
      {
        this.dataGridViewMemory[0, e.RowIndex].Value = (object) "err";
        this.dataGridViewMemory[e.ColumnIndex, e.RowIndex].Value = (object) "err";
      }
      else
      {
        string s = this.dataGridViewMemory[e.ColumnIndex, e.RowIndex].Value.ToString().Trim();
        if (!(s != this.EditCellValue))
          return;
        this.EditCellValue = s;
        byte result2;
        if (!byte.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result2))
        {
          this.dataGridViewMemory[e.ColumnIndex, e.RowIndex].Value = (object) "err";
        }
        else
        {
          int key = result1 + e.ColumnIndex - 1;
          int index = this.WriteData.IndexOfKey(key);
          if (index >= 0)
            this.WriteData.RemoveAt(index);
          this.WriteData.Add(key, result2);
          this.dataGridViewMemory[e.ColumnIndex, e.RowIndex].Style.BackColor = this.EditBackColor;
        }
      }
    }

    private bool LoadSetupValues()
    {
      ZR_ClassLibMessages.ClearErrors();
      this.TheLocation = (MemoryLocation) Enum.Parse(typeof (MemoryLocation), this.comboBoxLocation.SelectedItem.ToString());
      this.StartAddress = this.GetNumber(this.textBoxStartAddress.Text);
      if (this.StartAddress < 0)
      {
        this.textBoxStartAddress.Text = "0";
        int num = (int) GMM_MessageBox.ShowMessage("ReadMemory", "Illegal address");
        return false;
      }
      this.ByteSize = this.GetNumber(this.textBoxNumberOfBytes.Text);
      if (this.ByteSize < 0)
      {
        this.textBoxNumberOfBytes.Text = "0";
        int num = (int) GMM_MessageBox.ShowMessage("ReadMemory", "Illegal number of bytes.");
        return false;
      }
      return this.ByteSize >= 1;
    }

    private int GetNumber(string Text)
    {
      int result = 0;
      NumberStyles style = NumberStyles.Integer;
      Text = Text.Trim();
      if (Text.StartsWith("0x"))
      {
        Text = Text.Substring(2);
        style = NumberStyles.HexNumber;
      }
      return !int.TryParse(Text, style, (IFormatProvider) null, out result) ? -1 : result;
    }

    private void AddReadDataFromList(SortedList<int, byte> NewData)
    {
      for (int index1 = 0; index1 < NewData.Count; ++index1)
      {
        int index2 = this.ReadData.IndexOfKey(NewData.Keys[index1]);
        if (index2 >= 0)
        {
          if ((int) this.ReadData.Values[index2] != (int) NewData.Values[index1])
          {
            this.ReadData.RemoveAt(index2);
            this.ReadData.Add(NewData.Keys[index1], NewData.Values[index1]);
          }
        }
        else
          this.ReadData.Add(NewData.Keys[index1], NewData.Values[index1]);
      }
      NewData.Clear();
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
      this.panel1 = new Panel();
      this.buttonEraseFlash = new Button();
      this.buttonWriteChanges = new Button();
      this.buttonClearAll = new Button();
      this.buttonReadMemory = new Button();
      this.comboBoxLocation = new ComboBox();
      this.label1 = new Label();
      this.textBoxStartAddress = new TextBox();
      this.label2 = new Label();
      this.textBoxNumberOfBytes = new TextBox();
      this.labelLocation = new Label();
      this.dataGridViewMemory = new DataGridView();
      this.Address = new DataGridViewTextBoxColumn();
      this.Col00 = new DataGridViewTextBoxColumn();
      this.Col01 = new DataGridViewTextBoxColumn();
      this.Col02 = new DataGridViewTextBoxColumn();
      this.Col03 = new DataGridViewTextBoxColumn();
      this.Col04 = new DataGridViewTextBoxColumn();
      this.Col05 = new DataGridViewTextBoxColumn();
      this.Col06 = new DataGridViewTextBoxColumn();
      this.Col07 = new DataGridViewTextBoxColumn();
      this.Col08 = new DataGridViewTextBoxColumn();
      this.Col09 = new DataGridViewTextBoxColumn();
      this.Col0a = new DataGridViewTextBoxColumn();
      this.Col0b = new DataGridViewTextBoxColumn();
      this.Col0c = new DataGridViewTextBoxColumn();
      this.Col0d = new DataGridViewTextBoxColumn();
      this.Col0e = new DataGridViewTextBoxColumn();
      this.Col0f = new DataGridViewTextBoxColumn();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewMemory).BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(961, 477);
      this.zennerCoroprateDesign2.TabIndex = 15;
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.buttonEraseFlash);
      this.panel1.Controls.Add((Control) this.buttonWriteChanges);
      this.panel1.Controls.Add((Control) this.buttonClearAll);
      this.panel1.Controls.Add((Control) this.buttonReadMemory);
      this.panel1.Controls.Add((Control) this.comboBoxLocation);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.textBoxStartAddress);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.textBoxNumberOfBytes);
      this.panel1.Controls.Add((Control) this.labelLocation);
      this.panel1.Controls.Add((Control) this.dataGridViewMemory);
      this.panel1.Location = new Point(2, 41);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(958, 435);
      this.panel1.TabIndex = 16;
      this.buttonEraseFlash.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonEraseFlash.BackColor = SystemColors.Control;
      this.buttonEraseFlash.ImeMode = ImeMode.NoControl;
      this.buttonEraseFlash.Location = new Point(779, 370);
      this.buttonEraseFlash.Name = "buttonEraseFlash";
      this.buttonEraseFlash.Size = new Size(168, 24);
      this.buttonEraseFlash.TabIndex = 27;
      this.buttonEraseFlash.Text = "Erase Flash";
      this.buttonEraseFlash.UseVisualStyleBackColor = false;
      this.buttonEraseFlash.Click += new System.EventHandler(this.buttonEraseFlash_Click);
      this.buttonWriteChanges.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonWriteChanges.ImeMode = ImeMode.NoControl;
      this.buttonWriteChanges.Location = new Point(779, 400);
      this.buttonWriteChanges.Name = "buttonWriteChanges";
      this.buttonWriteChanges.Size = new Size(168, 24);
      this.buttonWriteChanges.TabIndex = 28;
      this.buttonWriteChanges.Text = "Write Changes";
      this.buttonWriteChanges.Click += new System.EventHandler(this.buttonWriteChanges_Click);
      this.buttonClearAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonClearAll.ImeMode = ImeMode.NoControl;
      this.buttonClearAll.Location = new Point(779, 310);
      this.buttonClearAll.Name = "buttonClearAll";
      this.buttonClearAll.Size = new Size(168, 24);
      this.buttonClearAll.TabIndex = 26;
      this.buttonClearAll.Text = "Clear All";
      this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
      this.buttonReadMemory.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonReadMemory.ImeMode = ImeMode.NoControl;
      this.buttonReadMemory.Location = new Point(779, 340);
      this.buttonReadMemory.Name = "buttonReadMemory";
      this.buttonReadMemory.Size = new Size(168, 24);
      this.buttonReadMemory.TabIndex = 26;
      this.buttonReadMemory.Text = "Read Memory";
      this.buttonReadMemory.Click += new System.EventHandler(this.buttonReadMemory_Click);
      this.comboBoxLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.comboBoxLocation.FormattingEnabled = true;
      this.comboBoxLocation.Location = new Point(826, 65);
      this.comboBoxLocation.Name = "comboBoxLocation";
      this.comboBoxLocation.Size = new Size(121, 21);
      this.comboBoxLocation.TabIndex = 25;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label1.ImeMode = ImeMode.NoControl;
      this.label1.Location = new Point(731, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(88, 16);
      this.label1.TabIndex = 24;
      this.label1.Text = "StartAddress";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.textBoxStartAddress.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxStartAddress.Location = new Point(827, 14);
      this.textBoxStartAddress.Name = "textBoxStartAddress";
      this.textBoxStartAddress.Size = new Size(120, 20);
      this.textBoxStartAddress.TabIndex = 20;
      this.textBoxStartAddress.Text = "0x200";
      this.textBoxStartAddress.WordWrap = false;
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label2.ImeMode = ImeMode.NoControl;
      this.label2.Location = new Point(731, 40);
      this.label2.Name = "label2";
      this.label2.Size = new Size(88, 16);
      this.label2.TabIndex = 23;
      this.label2.Text = "NumberOfBytes";
      this.label2.TextAlign = ContentAlignment.MiddleRight;
      this.textBoxNumberOfBytes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxNumberOfBytes.Location = new Point(827, 40);
      this.textBoxNumberOfBytes.Name = "textBoxNumberOfBytes";
      this.textBoxNumberOfBytes.Size = new Size(120, 20);
      this.textBoxNumberOfBytes.TabIndex = 21;
      this.textBoxNumberOfBytes.Text = "100";
      this.textBoxNumberOfBytes.WordWrap = false;
      this.labelLocation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.labelLocation.ImeMode = ImeMode.NoControl;
      this.labelLocation.Location = new Point(731, 66);
      this.labelLocation.Name = "labelLocation";
      this.labelLocation.Size = new Size(88, 16);
      this.labelLocation.TabIndex = 22;
      this.labelLocation.Text = "Location";
      this.labelLocation.TextAlign = ContentAlignment.MiddleRight;
      this.dataGridViewMemory.AllowUserToDeleteRows = false;
      this.dataGridViewMemory.AllowUserToResizeColumns = false;
      this.dataGridViewMemory.AllowUserToResizeRows = false;
      this.dataGridViewMemory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewMemory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMemory.Columns.AddRange((DataGridViewColumn) this.Address, (DataGridViewColumn) this.Col00, (DataGridViewColumn) this.Col01, (DataGridViewColumn) this.Col02, (DataGridViewColumn) this.Col03, (DataGridViewColumn) this.Col04, (DataGridViewColumn) this.Col05, (DataGridViewColumn) this.Col06, (DataGridViewColumn) this.Col07, (DataGridViewColumn) this.Col08, (DataGridViewColumn) this.Col09, (DataGridViewColumn) this.Col0a, (DataGridViewColumn) this.Col0b, (DataGridViewColumn) this.Col0c, (DataGridViewColumn) this.Col0d, (DataGridViewColumn) this.Col0e, (DataGridViewColumn) this.Col0f);
      this.dataGridViewMemory.Location = new Point(4, 4);
      this.dataGridViewMemory.Name = "dataGridViewMemory";
      this.dataGridViewMemory.Size = new Size(704, 428);
      this.dataGridViewMemory.TabIndex = 0;
      this.dataGridViewMemory.CellEndEdit += new DataGridViewCellEventHandler(this.dataGridViewMemory_CellEndEdit);
      this.dataGridViewMemory.CellEnter += new DataGridViewCellEventHandler(this.dataGridViewMemory_CellEnter);
      this.Address.HeaderText = "Address";
      this.Address.Name = "Address";
      this.Address.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col00.HeaderText = "00";
      this.Col00.Name = "Col00";
      this.Col00.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col00.Width = 30;
      this.Col01.HeaderText = "01";
      this.Col01.Name = "Col01";
      this.Col01.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col01.Width = 30;
      this.Col02.HeaderText = "02";
      this.Col02.Name = "Col02";
      this.Col02.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col02.Width = 30;
      this.Col03.HeaderText = "03";
      this.Col03.Name = "Col03";
      this.Col03.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col03.Width = 30;
      this.Col04.HeaderText = "04";
      this.Col04.Name = "Col04";
      this.Col04.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col04.Width = 30;
      this.Col05.HeaderText = "05";
      this.Col05.Name = "Col05";
      this.Col05.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col05.Width = 30;
      this.Col06.HeaderText = "06";
      this.Col06.Name = "Col06";
      this.Col06.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col06.Width = 30;
      this.Col07.HeaderText = "07";
      this.Col07.Name = "Col07";
      this.Col07.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col07.Width = 30;
      this.Col08.HeaderText = "08";
      this.Col08.Name = "Col08";
      this.Col08.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col08.Width = 30;
      this.Col09.HeaderText = "09";
      this.Col09.Name = "Col09";
      this.Col09.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col09.Width = 30;
      this.Col0a.HeaderText = "0a";
      this.Col0a.Name = "Col0a";
      this.Col0a.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col0a.Width = 30;
      this.Col0b.HeaderText = "0b";
      this.Col0b.Name = "Col0b";
      this.Col0b.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col0b.Width = 30;
      this.Col0c.HeaderText = "0c";
      this.Col0c.Name = "Col0c";
      this.Col0c.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col0c.Width = 30;
      this.Col0d.HeaderText = "0d";
      this.Col0d.Name = "Col0d";
      this.Col0d.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col0d.Width = 30;
      this.Col0e.HeaderText = "0e";
      this.Col0e.Name = "Col0e";
      this.Col0e.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col0e.Width = 30;
      this.Col0f.HeaderText = "0f";
      this.Col0f.Name = "Col0f";
      this.Col0f.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Col0f.Width = 30;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(961, 477);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (MemoryAccess);
      this.Text = nameof (MemoryAccess);
      this.Load += new System.EventHandler(this.MemoryAccess_Load);
      this.FormClosing += new FormClosingEventHandler(this.MemoryAccess_FormClosing);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.dataGridViewMemory).EndInit();
      this.ResumeLayout(false);
    }
  }
}
