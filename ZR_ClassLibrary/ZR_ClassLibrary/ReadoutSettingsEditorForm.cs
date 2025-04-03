// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ReadoutSettingsEditorForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ReadoutSettingsEditorForm : Form
  {
    private IContainer components = (IContainer) null;
    private Button btnUpdateDeviceType;
    private Button btnRemoveDeviceType;
    private Button btnAddDeviceType;
    private DataGridView tableReadoutType;
    private Panel panel1;
    private Label label2;
    private Label label3;
    private DataGridView tableReadoutSettings;
    private FlowLayoutPanel imageList;
    private GroupBox gboxImages;
    private GroupBox gboxReadoutTypes;
    private GroupBox gboxDeviceTypes;
    private Button btnSelectreadingTypes;
    private DataGridViewTextBoxColumn colReadoutDeviceType;
    private DataGridViewTextBoxColumn ReadoutSettings;
    private Button btnSelectImages;

    public ReadoutSettingsEditorForm() => this.InitializeComponent();

    public static void Show() => ReadoutSettingsEditorForm.Show((Form) null);

    public static void Show(Form owner)
    {
      using (ReadoutSettingsEditorForm settingsEditorForm = new ReadoutSettingsEditorForm())
      {
        if (owner != null)
          settingsEditorForm.Owner = owner;
        int num = (int) settingsEditorForm.ShowDialog();
      }
    }

    private void ReadoutSettingsEditorForm_Load(object sender, EventArgs e)
    {
      this.LoadReadoutType();
    }

    private void tableReadoutType_SelectionChanged(object sender, EventArgs e)
    {
      this.LoadReadoutSettings();
    }

    private void tableReadoutSettings_SelectionChanged(object sender, EventArgs e)
    {
      this.LoadImages();
    }

    private void btnAddDeviceType_Click(object sender, EventArgs e)
    {
      string str = AddReadoutTypeForm.Show((Form) this);
      if (string.IsNullOrEmpty(str))
        return;
      MeterDatabase.AddReadoutType(str);
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.LoadReadoutType();
      this.SelectDeviceType(str);
    }

    private void btnRemoveDeviceType_Click(object sender, EventArgs e)
    {
      ReadoutType selectedDeviceType = this.GetSelectedDeviceType();
      if (selectedDeviceType == null || DialogResult.Yes != MessageBox.Show((IWin32Window) this, this.GetTranslatedLanguageText("MeterInstaller", "ConfirmReally"), this.GetTranslatedLanguageText("MeterReader", "Delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
        return;
      MeterDatabase.DeleteReadoutType(selectedDeviceType.ReadoutDeviceTypeID);
      ZR_ClassLibMessages.ShowAndClearErrors();
      this.LoadReadoutType();
    }

    private string GetTranslatedLanguageText(string GmmModule, string TextKey)
    {
      string str = GmmModule + TextKey;
      return Ot.Gtt(Tg.Common, str, str);
    }

    private void btnUpdateDeviceType_Click(object sender, EventArgs e)
    {
      ReadoutType selectedDeviceType = this.GetSelectedDeviceType();
      if (selectedDeviceType == null)
        return;
      string newReadoutDeviceType = AddReadoutTypeForm.Show((Form) this, selectedDeviceType.ReadoutDeviceType);
      if (string.IsNullOrEmpty(newReadoutDeviceType))
        return;
      MeterDatabase.UpdateReadoutType(selectedDeviceType, newReadoutDeviceType);
      this.LoadReadoutType();
      this.SelectDeviceType(selectedDeviceType);
    }

    private void btnSelectreadingTypes_Click(object sender, EventArgs e)
    {
      ReadoutType selectedDeviceType = this.GetSelectedDeviceType();
      if (selectedDeviceType == null)
        return;
      SelectReadoutSettingsForm.Show((Form) this, selectedDeviceType);
      this.LoadReadoutSettings();
      this.SelectDeviceType(selectedDeviceType);
    }

    private void btnSelectImages_Click(object sender, EventArgs e)
    {
      ReadoutType selectedReadoutSettings = this.GetSelectedReadoutSettings();
      if (selectedReadoutSettings == null)
        return;
      SelectImagesForm.Show((Form) this, selectedReadoutSettings);
      this.LoadReadoutSettings();
      this.SelectDeviceType(selectedReadoutSettings);
      this.LoadImages();
    }

    private void LoadReadoutType()
    {
      this.tableReadoutType.Rows.Clear();
      List<ReadoutType> readoutTypeList = MeterDatabase.LoadReadoutType();
      ZR_ClassLibMessages.ShowAndClearErrors();
      if (readoutTypeList != null)
      {
        SortedList sortedList = new SortedList();
        foreach (ReadoutType readoutType in readoutTypeList)
        {
          if (!sortedList.ContainsKey((object) readoutType.ReadoutDeviceType))
          {
            sortedList.Add((object) readoutType.ReadoutDeviceType, (object) null);
            this.tableReadoutType.Rows[this.tableReadoutType.Rows.Add(new object[1]
            {
              (object) readoutType.ReadoutDeviceType
            })].Tag = (object) readoutType;
          }
        }
      }
      this.tableReadoutType.ClearSelection();
      if (this.tableReadoutType.Rows.Count <= 0)
        return;
      this.tableReadoutType.Rows[0].Selected = true;
    }

    private ReadoutType GetSelectedDeviceType()
    {
      return this.tableReadoutType.SelectedRows != null && this.tableReadoutType.SelectedRows.Count == 1 ? this.tableReadoutType.SelectedRows[0].Tag as ReadoutType : (ReadoutType) null;
    }

    private ReadoutType GetSelectedReadoutSettings()
    {
      return this.tableReadoutSettings.SelectedRows != null && this.tableReadoutSettings.SelectedRows.Count == 1 ? this.tableReadoutSettings.SelectedRows[0].Tag as ReadoutType : (ReadoutType) null;
    }

    private void SelectDeviceType(string newDeviceType)
    {
      if (string.IsNullOrEmpty(newDeviceType))
        return;
      foreach (DataGridViewRow row in (IEnumerable) this.tableReadoutType.Rows)
      {
        if (row.Cells[this.colReadoutDeviceType.Name].Value.ToString() == newDeviceType)
          row.Selected = true;
      }
    }

    private void SelectDeviceType(ReadoutType readoutType)
    {
      if (readoutType == null)
        return;
      this.tableReadoutSettings.ClearSelection();
      foreach (DataGridViewRow row in (IEnumerable) this.tableReadoutSettings.Rows)
      {
        if (((ReadoutType) row.Tag).ReadoutSettingsID == readoutType.ReadoutSettingsID)
        {
          row.Selected = true;
          break;
        }
      }
    }

    private void LoadReadoutSettings()
    {
      ReadoutType selectedDeviceType = this.GetSelectedDeviceType();
      if (selectedDeviceType == null)
        return;
      this.tableReadoutSettings.Rows.Clear();
      List<ReadoutType> readoutTypeList = MeterDatabase.LoadReadoutType(new int?(selectedDeviceType.ReadoutDeviceTypeID));
      ZR_ClassLibMessages.ShowAndClearErrors();
      if (readoutTypeList != null)
      {
        foreach (ReadoutType readoutType in readoutTypeList)
        {
          if (readoutType.ReadoutSettingsID != 0)
          {
            int index = this.tableReadoutSettings.Rows.Add();
            this.tableReadoutSettings.Rows[index].Cells[0].Value = (object) readoutType.ReadoutSettings;
            this.tableReadoutSettings.Rows[index].Tag = (object) readoutType;
          }
        }
      }
      this.tableReadoutSettings.ClearSelection();
      if (this.tableReadoutSettings.Rows.Count <= 0)
        return;
      this.tableReadoutSettings.Rows[0].Selected = true;
    }

    private void LoadImages()
    {
    }

    private void AddImage(GMMImage image)
    {
      PictureBox pictureBox = new PictureBox();
      pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
      pictureBox.Visible = true;
      pictureBox.Image = image.ImageSmall;
      pictureBox.MouseDown += new MouseEventHandler(this.pbox_MouseDown);
      pictureBox.DragOver += new DragEventHandler(this.pbox_DragOver);
      pictureBox.DragDrop += new DragEventHandler(this.pbox_DragDrop);
      pictureBox.AllowDrop = true;
      pictureBox.Tag = (object) image;
      this.imageList.Controls.Add((Control) pictureBox);
    }

    private void pbox_DragDrop(object sender, DragEventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Control control in (ArrangedElementCollection) this.imageList.Controls)
      {
        GMMImage tag = control.Tag as GMMImage;
        if (stringBuilder.Length > 0)
          stringBuilder.Append(",");
        stringBuilder.Append(tag.ImageID);
      }
      ReadoutType selectedReadoutSettings = this.GetSelectedReadoutSettings();
      if (selectedReadoutSettings == null || selectedReadoutSettings.ImageIdList == stringBuilder.ToString())
        return;
      MeterDatabase.UpdateReadoutType(selectedReadoutSettings.ReadoutDeviceTypeID, selectedReadoutSettings.ReadoutSettingsID, stringBuilder.ToString());
      this.LoadReadoutSettings();
      this.SelectDeviceType(selectedReadoutSettings);
    }

    private void pbox_DragOver(object sender, DragEventArgs e)
    {
      this.OnDragOver(e);
      if (e.Data.GetData(typeof (PictureBox)) == null)
        return;
      FlowLayoutPanel parent = (FlowLayoutPanel) (sender as PictureBox).Parent;
      int childIndex = parent.Controls.GetChildIndex((Control) (sender as PictureBox));
      PictureBox data = (PictureBox) e.Data.GetData(typeof (PictureBox));
      parent.Controls.SetChildIndex((Control) data, childIndex);
      e.Effect = e.AllowedEffect;
    }

    private void pbox_MouseDown(object sender, MouseEventArgs e)
    {
      this.OnMouseDown(e);
      int num = (int) this.DoDragDrop(sender, DragDropEffects.All);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReadoutSettingsEditorForm));
      this.btnUpdateDeviceType = new Button();
      this.btnRemoveDeviceType = new Button();
      this.btnAddDeviceType = new Button();
      this.tableReadoutType = new DataGridView();
      this.colReadoutDeviceType = new DataGridViewTextBoxColumn();
      this.panel1 = new Panel();
      this.label2 = new Label();
      this.label3 = new Label();
      this.tableReadoutSettings = new DataGridView();
      this.ReadoutSettings = new DataGridViewTextBoxColumn();
      this.imageList = new FlowLayoutPanel();
      this.gboxImages = new GroupBox();
      this.btnSelectImages = new Button();
      this.gboxReadoutTypes = new GroupBox();
      this.btnSelectreadingTypes = new Button();
      this.gboxDeviceTypes = new GroupBox();
      ((ISupportInitialize) this.tableReadoutType).BeginInit();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.tableReadoutSettings).BeginInit();
      this.gboxImages.SuspendLayout();
      this.gboxReadoutTypes.SuspendLayout();
      this.gboxDeviceTypes.SuspendLayout();
      this.SuspendLayout();
      this.btnUpdateDeviceType.Image = (Image) componentResourceManager.GetObject("btnUpdateDeviceType.Image");
      this.btnUpdateDeviceType.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnUpdateDeviceType.ImeMode = ImeMode.NoControl;
      this.btnUpdateDeviceType.Location = new Point(89, 19);
      this.btnUpdateDeviceType.Name = "btnUpdateDeviceType";
      this.btnUpdateDeviceType.Size = new Size(76, 27);
      this.btnUpdateDeviceType.TabIndex = 27;
      this.btnUpdateDeviceType.Text = "Update...";
      this.btnUpdateDeviceType.TextAlign = ContentAlignment.MiddleRight;
      this.btnUpdateDeviceType.UseVisualStyleBackColor = true;
      this.btnUpdateDeviceType.Click += new System.EventHandler(this.btnUpdateDeviceType_Click);
      this.btnRemoveDeviceType.Image = (Image) componentResourceManager.GetObject("btnRemoveDeviceType.Image");
      this.btnRemoveDeviceType.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnRemoveDeviceType.ImeMode = ImeMode.NoControl;
      this.btnRemoveDeviceType.Location = new Point(173, 19);
      this.btnRemoveDeviceType.Name = "btnRemoveDeviceType";
      this.btnRemoveDeviceType.Size = new Size(76, 27);
      this.btnRemoveDeviceType.TabIndex = 26;
      this.btnRemoveDeviceType.Text = "Remove";
      this.btnRemoveDeviceType.TextAlign = ContentAlignment.MiddleRight;
      this.btnRemoveDeviceType.UseVisualStyleBackColor = true;
      this.btnRemoveDeviceType.Click += new System.EventHandler(this.btnRemoveDeviceType_Click);
      this.btnAddDeviceType.Image = (Image) componentResourceManager.GetObject("btnAddDeviceType.Image");
      this.btnAddDeviceType.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnAddDeviceType.ImeMode = ImeMode.NoControl;
      this.btnAddDeviceType.Location = new Point(6, 19);
      this.btnAddDeviceType.Name = "btnAddDeviceType";
      this.btnAddDeviceType.Size = new Size(76, 27);
      this.btnAddDeviceType.TabIndex = 25;
      this.btnAddDeviceType.Text = "Add...";
      this.btnAddDeviceType.TextAlign = ContentAlignment.MiddleRight;
      this.btnAddDeviceType.UseVisualStyleBackColor = true;
      this.btnAddDeviceType.Click += new System.EventHandler(this.btnAddDeviceType_Click);
      this.tableReadoutType.AllowUserToAddRows = false;
      this.tableReadoutType.AllowUserToDeleteRows = false;
      this.tableReadoutType.AllowUserToResizeRows = false;
      this.tableReadoutType.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableReadoutType.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.tableReadoutType.BackgroundColor = Color.White;
      this.tableReadoutType.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.tableReadoutType.ColumnHeadersVisible = false;
      this.tableReadoutType.Columns.AddRange((DataGridViewColumn) this.colReadoutDeviceType);
      this.tableReadoutType.Location = new Point(6, 52);
      this.tableReadoutType.MultiSelect = false;
      this.tableReadoutType.Name = "tableReadoutType";
      this.tableReadoutType.ReadOnly = true;
      this.tableReadoutType.RowHeadersVisible = false;
      this.tableReadoutType.ScrollBars = ScrollBars.Vertical;
      this.tableReadoutType.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.tableReadoutType.Size = new Size(243, 428);
      this.tableReadoutType.TabIndex = 23;
      this.tableReadoutType.SelectionChanged += new System.EventHandler(this.tableReadoutType_SelectionChanged);
      this.colReadoutDeviceType.HeaderText = "ReadoutDeviceType";
      this.colReadoutDeviceType.Name = "colReadoutDeviceType";
      this.colReadoutDeviceType.ReadOnly = true;
      this.panel1.BackColor = Color.White;
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(784, 64);
      this.panel1.TabIndex = 28;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f);
      this.label2.ImeMode = ImeMode.NoControl;
      this.label2.Location = new Point(40, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(178, 15);
      this.label2.TabIndex = 1;
      this.label2.Text = "Tool to create read-out settings.";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold);
      this.label3.ImeMode = ImeMode.NoControl;
      this.label3.Location = new Point(23, 13);
      this.label3.Name = "label3";
      this.label3.Size = new Size(177, 16);
      this.label3.TabIndex = 0;
      this.label3.Text = "Read-out Settings Editor";
      this.tableReadoutSettings.AllowUserToAddRows = false;
      this.tableReadoutSettings.AllowUserToDeleteRows = false;
      this.tableReadoutSettings.AllowUserToResizeRows = false;
      this.tableReadoutSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableReadoutSettings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.tableReadoutSettings.BackgroundColor = Color.White;
      this.tableReadoutSettings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.tableReadoutSettings.ColumnHeadersVisible = false;
      this.tableReadoutSettings.Columns.AddRange((DataGridViewColumn) this.ReadoutSettings);
      this.tableReadoutSettings.Location = new Point(6, 52);
      this.tableReadoutSettings.MultiSelect = false;
      this.tableReadoutSettings.Name = "tableReadoutSettings";
      this.tableReadoutSettings.ReadOnly = true;
      this.tableReadoutSettings.RowHeadersVisible = false;
      this.tableReadoutSettings.ScrollBars = ScrollBars.Vertical;
      this.tableReadoutSettings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.tableReadoutSettings.Size = new Size(498, 114);
      this.tableReadoutSettings.TabIndex = 24;
      this.tableReadoutSettings.SelectionChanged += new System.EventHandler(this.tableReadoutSettings_SelectionChanged);
      this.ReadoutSettings.HeaderText = "colReadoutSettings";
      this.ReadoutSettings.Name = "ReadoutSettings";
      this.ReadoutSettings.ReadOnly = true;
      this.imageList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.imageList.BackColor = Color.White;
      this.imageList.BorderStyle = BorderStyle.FixedSingle;
      this.imageList.Location = new Point(9, 52);
      this.imageList.Name = "imageList";
      this.imageList.Size = new Size(498, 250);
      this.imageList.TabIndex = 29;
      this.gboxImages.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gboxImages.Controls.Add((Control) this.btnSelectImages);
      this.gboxImages.Controls.Add((Control) this.imageList);
      this.gboxImages.Location = new Point(267, 248);
      this.gboxImages.Name = "gboxImages";
      this.gboxImages.Size = new Size(513, 310);
      this.gboxImages.TabIndex = 30;
      this.gboxImages.TabStop = false;
      this.gboxImages.Text = "Images";
      this.btnSelectImages.Image = (Image) componentResourceManager.GetObject("btnSelectImages.Image");
      this.btnSelectImages.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSelectImages.ImeMode = ImeMode.NoControl;
      this.btnSelectImages.Location = new Point(6, 19);
      this.btnSelectImages.Name = "btnSelectImages";
      this.btnSelectImages.Size = new Size(88, 27);
      this.btnSelectImages.TabIndex = 35;
      this.btnSelectImages.Text = "Select...";
      this.btnSelectImages.TextAlign = ContentAlignment.MiddleRight;
      this.btnSelectImages.UseVisualStyleBackColor = true;
      this.btnSelectImages.Click += new System.EventHandler(this.btnSelectImages_Click);
      this.gboxReadoutTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gboxReadoutTypes.Controls.Add((Control) this.btnSelectreadingTypes);
      this.gboxReadoutTypes.Controls.Add((Control) this.tableReadoutSettings);
      this.gboxReadoutTypes.Location = new Point(267, 70);
      this.gboxReadoutTypes.Name = "gboxReadoutTypes";
      this.gboxReadoutTypes.Size = new Size(510, 172);
      this.gboxReadoutTypes.TabIndex = 32;
      this.gboxReadoutTypes.TabStop = false;
      this.gboxReadoutTypes.Text = "Read-out Types";
      this.btnSelectreadingTypes.Image = (Image) componentResourceManager.GetObject("btnSelectreadingTypes.Image");
      this.btnSelectreadingTypes.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSelectreadingTypes.ImeMode = ImeMode.NoControl;
      this.btnSelectreadingTypes.Location = new Point(7, 19);
      this.btnSelectreadingTypes.Name = "btnSelectreadingTypes";
      this.btnSelectreadingTypes.Size = new Size(88, 27);
      this.btnSelectreadingTypes.TabIndex = 34;
      this.btnSelectreadingTypes.Text = "Select...";
      this.btnSelectreadingTypes.TextAlign = ContentAlignment.MiddleRight;
      this.btnSelectreadingTypes.UseVisualStyleBackColor = true;
      this.btnSelectreadingTypes.Click += new System.EventHandler(this.btnSelectreadingTypes_Click);
      this.gboxDeviceTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.gboxDeviceTypes.Controls.Add((Control) this.tableReadoutType);
      this.gboxDeviceTypes.Controls.Add((Control) this.btnAddDeviceType);
      this.gboxDeviceTypes.Controls.Add((Control) this.btnUpdateDeviceType);
      this.gboxDeviceTypes.Controls.Add((Control) this.btnRemoveDeviceType);
      this.gboxDeviceTypes.Location = new Point(6, 70);
      this.gboxDeviceTypes.Name = "gboxDeviceTypes";
      this.gboxDeviceTypes.Size = new Size((int) byte.MaxValue, 488);
      this.gboxDeviceTypes.TabIndex = 34;
      this.gboxDeviceTypes.TabStop = false;
      this.gboxDeviceTypes.Text = "Device Types";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 562);
      this.Controls.Add((Control) this.gboxDeviceTypes);
      this.Controls.Add((Control) this.gboxReadoutTypes);
      this.Controls.Add((Control) this.gboxImages);
      this.Controls.Add((Control) this.panel1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (ReadoutSettingsEditorForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Readout Settings Editor";
      this.Load += new System.EventHandler(this.ReadoutSettingsEditorForm_Load);
      ((ISupportInitialize) this.tableReadoutType).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.tableReadoutSettings).EndInit();
      this.gboxImages.ResumeLayout(false);
      this.gboxReadoutTypes.ResumeLayout(false);
      this.gboxDeviceTypes.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
