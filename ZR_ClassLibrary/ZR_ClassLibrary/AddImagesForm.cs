// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.AddImagesForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class AddImagesForm : Form
  {
    private int? currentImageID;
    private IContainer components = (IContainer) null;
    private Button btnOK;
    private Panel panel1;
    private Label label2;
    private Label label3;
    private Button btnNew;
    private Button btnRemove;
    private Button btnSave;
    private TextBox txtDescriptionEN;
    private Label lblDescriptionEN;
    private TextBox txtDescriptionDE;
    private Label lblDescriptionDE;
    private Button btnAddImage;
    private PictureBox picture;
    private DataGridView tableImages;
    private DataGridViewTextBoxColumn colReadoutDeviceSettings;
    private ErrorProvider error;

    public AddImagesForm() => this.InitializeComponent();

    public static void Show() => AddImagesForm.Show((Form) null);

    public static void Show(Form owner)
    {
      using (AddImagesForm addImagesForm = new AddImagesForm())
      {
        if (owner != null)
          addImagesForm.Owner = owner;
        int num = (int) addImagesForm.ShowDialog();
      }
    }

    private void AddImagesForm_Load(object sender, EventArgs e) => this.LoadImages();

    private void tableImages_SelectionChanged(object sender, EventArgs e)
    {
      if (this.tableImages.SelectedRows.Count != 1)
      {
        this.txtDescriptionDE.Text = string.Empty;
        this.txtDescriptionEN.Text = string.Empty;
        this.picture.Image = (Image) null;
        this.currentImageID = new int?();
      }
      else
      {
        if (!(this.tableImages.SelectedRows[0].Tag is GMMImage tag))
          return;
        this.currentImageID = new int?(tag.ImageID);
        this.picture.Image = tag.ImageSmall;
      }
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      this.tableImages.ClearSelection();
      this.error.Clear();
      this.txtDescriptionDE.Text = string.Empty;
      this.txtDescriptionEN.Text = string.Empty;
      this.picture.Image = (Image) null;
      this.currentImageID = new int?();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
    }

    private void btnAddImage_Click(object sender, EventArgs e)
    {
      try
      {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
          if (openFileDialog.ShowDialog() != DialogResult.OK)
            return;
          this.picture.Image = (Image) new Bitmap(openFileDialog.FileName);
          this.txtDescriptionDE.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
          this.txtDescriptionEN.Text = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, this.Name, MessageBoxButtons.OK);
      }
    }

    private void SelectImage(int? idToSelect)
    {
      this.tableImages.ClearSelection();
      if (!idToSelect.HasValue)
        return;
      foreach (DataGridViewRow row in (IEnumerable) this.tableImages.Rows)
      {
        if ((row.Tag as GMMImage).ImageID == idToSelect.Value)
        {
          row.Selected = true;
          break;
        }
      }
    }

    private void LoadImages()
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
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddImagesForm));
      this.btnOK = new Button();
      this.panel1 = new Panel();
      this.label2 = new Label();
      this.label3 = new Label();
      this.btnNew = new Button();
      this.btnRemove = new Button();
      this.btnSave = new Button();
      this.txtDescriptionEN = new TextBox();
      this.lblDescriptionEN = new Label();
      this.txtDescriptionDE = new TextBox();
      this.lblDescriptionDE = new Label();
      this.btnAddImage = new Button();
      this.picture = new PictureBox();
      this.tableImages = new DataGridView();
      this.colReadoutDeviceSettings = new DataGridViewTextBoxColumn();
      this.error = new ErrorProvider(this.components);
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.picture).BeginInit();
      ((ISupportInitialize) this.tableImages).BeginInit();
      ((ISupportInitialize) this.error).BeginInit();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.Cancel;
      this.btnOK.Image = (Image) componentResourceManager.GetObject("btnOK.Image");
      this.btnOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOK.ImeMode = ImeMode.NoControl;
      this.btnOK.Location = new Point(654, 451);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 27);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.panel1.BackColor = Color.White;
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(739, 64);
      this.panel1.TabIndex = 35;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f);
      this.label2.ImeMode = ImeMode.NoControl;
      this.label2.Location = new Point(40, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(182, 15);
      this.label2.TabIndex = 1;
      this.label2.Text = "Tool to add images to database.";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold);
      this.label3.ImeMode = ImeMode.NoControl;
      this.label3.Location = new Point(23, 13);
      this.label3.Name = "label3";
      this.label3.Size = new Size(59, 16);
      this.label3.TabIndex = 0;
      this.label3.Text = "Images";
      this.btnNew.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnNew.Image = (Image) componentResourceManager.GetObject("btnNew.Image");
      this.btnNew.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnNew.ImeMode = ImeMode.NoControl;
      this.btnNew.Location = new Point(12, 451);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(76, 27);
      this.btnNew.TabIndex = 3;
      this.btnNew.Text = "New";
      this.btnNew.TextAlign = ContentAlignment.MiddleRight;
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      this.btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnRemove.Image = (Image) componentResourceManager.GetObject("btnRemove.Image");
      this.btnRemove.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnRemove.ImeMode = ImeMode.NoControl;
      this.btnRemove.Location = new Point(210, 451);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(76, 27);
      this.btnRemove.TabIndex = 5;
      this.btnRemove.Text = "Remove";
      this.btnRemove.TextAlign = ContentAlignment.MiddleRight;
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnSave.Image = (Image) componentResourceManager.GetObject("btnSave.Image");
      this.btnSave.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSave.ImeMode = ImeMode.NoControl;
      this.btnSave.Location = new Point(94, 451);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(76, 27);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "Save";
      this.btnSave.TextAlign = ContentAlignment.MiddleRight;
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      this.txtDescriptionEN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescriptionEN.Location = new Point(9, 411);
      this.txtDescriptionEN.MaxLength = (int) byte.MaxValue;
      this.txtDescriptionEN.Multiline = true;
      this.txtDescriptionEN.Name = "txtDescriptionEN";
      this.txtDescriptionEN.Size = new Size(721, 21);
      this.txtDescriptionEN.TabIndex = 2;
      this.lblDescriptionEN.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDescriptionEN.AutoSize = true;
      this.lblDescriptionEN.Location = new Point(9, 395);
      this.lblDescriptionEN.Name = "lblDescriptionEN";
      this.lblDescriptionEN.Size = new Size(78, 13);
      this.lblDescriptionEN.TabIndex = 43;
      this.lblDescriptionEN.Text = "Description EN";
      this.txtDescriptionDE.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescriptionDE.Location = new Point(9, 371);
      this.txtDescriptionDE.MaxLength = (int) byte.MaxValue;
      this.txtDescriptionDE.Multiline = true;
      this.txtDescriptionDE.Name = "txtDescriptionDE";
      this.txtDescriptionDE.Size = new Size(721, 21);
      this.txtDescriptionDE.TabIndex = 1;
      this.lblDescriptionDE.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblDescriptionDE.AutoSize = true;
      this.lblDescriptionDE.Location = new Point(9, 355);
      this.lblDescriptionDE.Name = "lblDescriptionDE";
      this.lblDescriptionDE.Size = new Size(78, 13);
      this.lblDescriptionDE.TabIndex = 45;
      this.lblDescriptionDE.Text = "Description DE";
      this.btnAddImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAddImage.Location = new Point(688, 325);
      this.btnAddImage.Name = "btnAddImage";
      this.btnAddImage.Size = new Size(38, 23);
      this.btnAddImage.TabIndex = 0;
      this.btnAddImage.Text = "...";
      this.btnAddImage.UseVisualStyleBackColor = true;
      this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
      this.picture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.picture.BackColor = Color.White;
      this.picture.BorderStyle = BorderStyle.FixedSingle;
      this.picture.Location = new Point(460, 74);
      this.picture.Name = "picture";
      this.picture.Size = new Size(270, 278);
      this.picture.TabIndex = 50;
      this.picture.TabStop = false;
      this.tableImages.AllowUserToAddRows = false;
      this.tableImages.AllowUserToDeleteRows = false;
      this.tableImages.AllowUserToResizeRows = false;
      this.tableImages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.tableImages.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.tableImages.BackgroundColor = Color.White;
      this.tableImages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.tableImages.ColumnHeadersVisible = false;
      this.tableImages.Columns.AddRange((DataGridViewColumn) this.colReadoutDeviceSettings);
      this.tableImages.Location = new Point(9, 74);
      this.tableImages.MultiSelect = false;
      this.tableImages.Name = "tableImages";
      this.tableImages.ReadOnly = true;
      this.tableImages.RowHeadersVisible = false;
      this.tableImages.ScrollBars = ScrollBars.Vertical;
      this.tableImages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.tableImages.Size = new Size(445, 278);
      this.tableImages.TabIndex = 51;
      this.tableImages.SelectionChanged += new System.EventHandler(this.tableImages_SelectionChanged);
      this.colReadoutDeviceSettings.HeaderText = "ReadoutDeviceSettings";
      this.colReadoutDeviceSettings.Name = "colReadoutDeviceSettings";
      this.colReadoutDeviceSettings.ReadOnly = true;
      this.error.ContainerControl = (ContainerControl) this;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(739, 490);
      this.Controls.Add((Control) this.btnAddImage);
      this.Controls.Add((Control) this.tableImages);
      this.Controls.Add((Control) this.picture);
      this.Controls.Add((Control) this.txtDescriptionDE);
      this.Controls.Add((Control) this.lblDescriptionDE);
      this.Controls.Add((Control) this.txtDescriptionEN);
      this.Controls.Add((Control) this.lblDescriptionEN);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnNew);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnOK);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (AddImagesForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Images";
      this.Load += new System.EventHandler(this.AddImagesForm_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.picture).EndInit();
      ((ISupportInitialize) this.tableImages).EndInit();
      ((ISupportInitialize) this.error).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
