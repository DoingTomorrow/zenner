// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ValueIdentForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ValueIdentForm : Form
  {
    private bool isReadyValueIdentBoxes = false;
    private static ValueIdentForm instance;
    private IContainer components = (IContainer) null;
    private Label lblValueIdent;
    private Label label2;
    private GroupBox groupBox2;
    private ComboBox cboxPhysicalQuantity;
    private Label label21;
    private Label label16;
    private ComboBox cboxCreation;
    private ComboBox cboxMeterType;
    private Label label17;
    private ComboBox cboxStorageInterval;
    private Label label20;
    private Label label18;
    private ComboBox cboxCalculationStart;
    private ComboBox cboxCalculation;
    private Label label19;
    private Button btnOK;

    public ValueIdentForm()
    {
      this.InitializeComponent();
      this.InitializeComboBoxes();
    }

    public static void Show(long valueIdent)
    {
      if (ValueIdentForm.instance == null)
        ValueIdentForm.instance = new ValueIdentForm();
      ValueIdentForm.instance.lblValueIdent.Text = valueIdent.ToString();
      ValueIdentForm.instance.ChooseValueIdentComboBoxes(valueIdent);
      int num = (int) ValueIdentForm.instance.ShowDialog();
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();

    private void InitializeComboBoxes()
    {
      this.cboxPhysicalQuantity.DataSource = (object) new BindingSource()
      {
        DataSource = (object) ValueIdent.GetTranslatedStringListForValueIdPart(typeof (ValueIdent.ValueIdPart_PhysicalQuantity))
      };
      this.cboxCalculation.DataSource = (object) new BindingSource()
      {
        DataSource = (object) ValueIdent.GetTranslatedStringListForValueIdPart(typeof (ValueIdent.ValueIdPart_Calculation))
      };
      this.cboxCalculationStart.DataSource = (object) new BindingSource()
      {
        DataSource = (object) ValueIdent.GetTranslatedStringListForValueIdPart(typeof (ValueIdent.ValueIdPart_CalculationStart))
      };
      this.cboxCreation.DataSource = (object) new BindingSource()
      {
        DataSource = (object) ValueIdent.GetTranslatedStringListForValueIdPart(typeof (ValueIdent.ValueIdPart_Creation))
      };
      this.cboxMeterType.DataSource = (object) new BindingSource()
      {
        DataSource = (object) ValueIdent.GetTranslatedStringListForValueIdPart(typeof (ValueIdent.ValueIdPart_MeterType))
      };
      this.cboxStorageInterval.DataSource = (object) new BindingSource()
      {
        DataSource = (object) ValueIdent.GetTranslatedStringListForValueIdPart(typeof (ValueIdent.ValueIdPart_StorageInterval))
      };
      this.isReadyValueIdentBoxes = true;
    }

    private long CalculateValueIdent()
    {
      if (this.cboxPhysicalQuantity.SelectedItem == null || this.cboxMeterType.SelectedItem == null || this.cboxCalculation.SelectedItem == null || this.cboxCalculationStart.SelectedItem == null || this.cboxStorageInterval.SelectedItem == null || this.cboxCreation.SelectedItem == null)
        return 0;
      long key1 = ((KeyValuePair<long, string>) this.cboxPhysicalQuantity.SelectedItem).Key;
      KeyValuePair<long, string> selectedItem = (KeyValuePair<long, string>) this.cboxMeterType.SelectedItem;
      long key2 = selectedItem.Key;
      selectedItem = (KeyValuePair<long, string>) this.cboxCalculation.SelectedItem;
      long key3 = selectedItem.Key;
      selectedItem = (KeyValuePair<long, string>) this.cboxCalculationStart.SelectedItem;
      long key4 = selectedItem.Key;
      selectedItem = (KeyValuePair<long, string>) this.cboxStorageInterval.SelectedItem;
      long key5 = selectedItem.Key;
      selectedItem = (KeyValuePair<long, string>) this.cboxCreation.SelectedItem;
      long key6 = selectedItem.Key;
      // ISSUE: variable of a boxed type
      __Boxed<ValueIdent.ValueIdPart_Index> index = (Enum) ValueIdent.ValueIdPart_Index.Any;
      return ValueIdent.GetValueIdForValueEnum((ValueIdent.ValueIdPart_PhysicalQuantity) key1, (ValueIdent.ValueIdPart_MeterType) key2, (ValueIdent.ValueIdPart_Calculation) key3, (ValueIdent.ValueIdPart_CalculationStart) key4, (ValueIdent.ValueIdPart_StorageInterval) key5, (ValueIdent.ValueIdPart_Creation) key6, (object) index);
    }

    private void ChooseValueIdentComboBoxes(long valueToShow)
    {
      if (ValueIdent.IsValid(valueToShow))
      {
        this.cboxPhysicalQuantity.SelectedValue = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_PhysicalQuantity>(valueToShow);
        this.cboxMeterType.SelectedValue = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_MeterType>(valueToShow);
        this.cboxCalculation.SelectedValue = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Calculation>(valueToShow);
        this.cboxCalculationStart.SelectedValue = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_CalculationStart>(valueToShow);
        this.cboxStorageInterval.SelectedValue = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_StorageInterval>(valueToShow);
        this.cboxCreation.SelectedValue = (object) (long) ValueIdent.ValueIdPart_Get<ValueIdent.ValueIdPart_Creation>(valueToShow);
      }
      else
      {
        int num = (int) MessageBox.Show("Illegel ValueIdent number");
      }
    }

    private void cboxMeterType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.isReadyValueIdentBoxes)
        return;
      this.lblValueIdent.Text = this.CalculateValueIdent().ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblValueIdent = new Label();
      this.label2 = new Label();
      this.groupBox2 = new GroupBox();
      this.cboxPhysicalQuantity = new ComboBox();
      this.label21 = new Label();
      this.label16 = new Label();
      this.cboxCreation = new ComboBox();
      this.cboxMeterType = new ComboBox();
      this.label17 = new Label();
      this.cboxStorageInterval = new ComboBox();
      this.label20 = new Label();
      this.label18 = new Label();
      this.cboxCalculationStart = new ComboBox();
      this.cboxCalculation = new ComboBox();
      this.label19 = new Label();
      this.btnOK = new Button();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.lblValueIdent.AutoSize = true;
      this.lblValueIdent.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblValueIdent.Location = new Point(146, 21);
      this.lblValueIdent.Name = "lblValueIdent";
      this.lblValueIdent.Size = new Size(116, 25);
      this.lblValueIdent.TabIndex = 0;
      this.lblValueIdent.Text = "00000000";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(12, 21);
      this.label2.Name = "label2";
      this.label2.Size = new Size(131, 25);
      this.label2.TabIndex = 1;
      this.label2.Text = "ValueIdent:";
      this.groupBox2.Controls.Add((Control) this.cboxPhysicalQuantity);
      this.groupBox2.Controls.Add((Control) this.label21);
      this.groupBox2.Controls.Add((Control) this.label16);
      this.groupBox2.Controls.Add((Control) this.cboxCreation);
      this.groupBox2.Controls.Add((Control) this.cboxMeterType);
      this.groupBox2.Controls.Add((Control) this.label17);
      this.groupBox2.Controls.Add((Control) this.cboxStorageInterval);
      this.groupBox2.Controls.Add((Control) this.label20);
      this.groupBox2.Controls.Add((Control) this.label18);
      this.groupBox2.Controls.Add((Control) this.cboxCalculationStart);
      this.groupBox2.Controls.Add((Control) this.cboxCalculation);
      this.groupBox2.Controls.Add((Control) this.label19);
      this.groupBox2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.groupBox2.Location = new Point(17, 58);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(270, 177);
      this.groupBox2.TabIndex = 8;
      this.groupBox2.TabStop = false;
      this.cboxPhysicalQuantity.DisplayMember = "Value";
      this.cboxPhysicalQuantity.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxPhysicalQuantity.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboxPhysicalQuantity.FormattingEnabled = true;
      this.cboxPhysicalQuantity.Location = new Point(107, 42);
      this.cboxPhysicalQuantity.Name = "cboxPhysicalQuantity";
      this.cboxPhysicalQuantity.Size = new Size(141, 21);
      this.cboxPhysicalQuantity.TabIndex = 8;
      this.cboxPhysicalQuantity.ValueMember = "Key";
      this.cboxPhysicalQuantity.SelectedIndexChanged += new System.EventHandler(this.cboxMeterType_SelectedIndexChanged);
      this.label21.AutoSize = true;
      this.label21.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label21.Location = new Point(7, 45);
      this.label21.Name = "label21";
      this.label21.Size = new Size(89, 13);
      this.label21.TabIndex = 20;
      this.label21.Text = "Physical quantity:";
      this.label16.AutoSize = true;
      this.label16.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label16.Location = new Point(47, 151);
      this.label16.Name = "label16";
      this.label16.Size = new Size(49, 13);
      this.label16.TabIndex = 30;
      this.label16.Text = "Creation:";
      this.cboxCreation.DisplayMember = "Value";
      this.cboxCreation.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxCreation.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboxCreation.FormattingEnabled = true;
      this.cboxCreation.Location = new Point(107, 148);
      this.cboxCreation.Name = "cboxCreation";
      this.cboxCreation.Size = new Size(141, 21);
      this.cboxCreation.TabIndex = 13;
      this.cboxCreation.ValueMember = "Key";
      this.cboxCreation.SelectedIndexChanged += new System.EventHandler(this.cboxMeterType_SelectedIndexChanged);
      this.cboxMeterType.DisplayMember = "Value";
      this.cboxMeterType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxMeterType.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboxMeterType.FormattingEnabled = true;
      this.cboxMeterType.Location = new Point(107, 16);
      this.cboxMeterType.Name = "cboxMeterType";
      this.cboxMeterType.Size = new Size(141, 21);
      this.cboxMeterType.TabIndex = 9;
      this.cboxMeterType.ValueMember = "Key";
      this.cboxMeterType.SelectedIndexChanged += new System.EventHandler(this.cboxMeterType_SelectedIndexChanged);
      this.label17.AutoSize = true;
      this.label17.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label17.Location = new Point(12, 125);
      this.label17.Name = "label17";
      this.label17.Size = new Size(84, 13);
      this.label17.TabIndex = 28;
      this.label17.Text = "Storage interval:";
      this.cboxStorageInterval.DisplayMember = "Value";
      this.cboxStorageInterval.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxStorageInterval.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboxStorageInterval.FormattingEnabled = true;
      this.cboxStorageInterval.Location = new Point(107, 121);
      this.cboxStorageInterval.Name = "cboxStorageInterval";
      this.cboxStorageInterval.Size = new Size(141, 21);
      this.cboxStorageInterval.TabIndex = 12;
      this.cboxStorageInterval.ValueMember = "Key";
      this.cboxStorageInterval.SelectedIndexChanged += new System.EventHandler(this.cboxMeterType_SelectedIndexChanged);
      this.label20.AutoSize = true;
      this.label20.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label20.Location = new Point(36, 20);
      this.label20.Name = "label20";
      this.label20.Size = new Size(60, 13);
      this.label20.TabIndex = 22;
      this.label20.Text = "Meter type:";
      this.label18.AutoSize = true;
      this.label18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label18.Location = new Point(11, 98);
      this.label18.Name = "label18";
      this.label18.Size = new Size(85, 13);
      this.label18.TabIndex = 26;
      this.label18.Text = "Calculation start:";
      this.cboxCalculationStart.DisplayMember = "Value";
      this.cboxCalculationStart.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxCalculationStart.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboxCalculationStart.FormattingEnabled = true;
      this.cboxCalculationStart.Location = new Point(107, 94);
      this.cboxCalculationStart.Name = "cboxCalculationStart";
      this.cboxCalculationStart.Size = new Size(141, 21);
      this.cboxCalculationStart.TabIndex = 11;
      this.cboxCalculationStart.ValueMember = "Key";
      this.cboxCalculationStart.SelectedIndexChanged += new System.EventHandler(this.cboxMeterType_SelectedIndexChanged);
      this.cboxCalculation.DisplayMember = "Value";
      this.cboxCalculation.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxCalculation.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboxCalculation.FormattingEnabled = true;
      this.cboxCalculation.Location = new Point(107, 68);
      this.cboxCalculation.Name = "cboxCalculation";
      this.cboxCalculation.Size = new Size(141, 21);
      this.cboxCalculation.TabIndex = 10;
      this.cboxCalculation.ValueMember = "Key";
      this.cboxCalculation.SelectedIndexChanged += new System.EventHandler(this.cboxMeterType_SelectedIndexChanged);
      this.label19.AutoSize = true;
      this.label19.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label19.Location = new Point(34, 71);
      this.label19.Name = "label19";
      this.label19.Size = new Size(62, 13);
      this.label19.TabIndex = 24;
      this.label19.Text = "Calculation:";
      this.btnOK.Location = new Point(114, 246);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 9;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(303, 281);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.lblValueIdent);
      this.Name = nameof (ValueIdentForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ValueIdent";
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
