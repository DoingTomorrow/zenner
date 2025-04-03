// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.SelectImagesForm
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class SelectImagesForm : Form
  {
    private ReadoutType readoutType;
    private List<int> selectedImageIDs;
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private Label label2;
    private Label lblTitle;
    private Button btnOK;
    private Button btnGoToImagesForm;
    private CheckedListBox listImages;
    private PictureBox picture;

    public SelectImagesForm()
    {
      this.InitializeComponent();
      this.selectedImageIDs = new List<int>();
    }

    private void SelectImagesForm_Load(object sender, EventArgs e) => this.LoadImages();

    private void listImages_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listImages.SelectedItem == null)
      {
        this.picture.Image = (Image) null;
      }
      else
      {
        if (!(this.listImages.SelectedItem is GMMImage selectedItem))
          return;
        this.picture.Image = selectedItem.ImageSmall;
      }
    }

    public static void Show(ReadoutType readoutType)
    {
      SelectImagesForm.Show((Form) null, readoutType);
    }

    public static void Show(Form owner, ReadoutType readoutType)
    {
      if (readoutType == null)
        throw new ArgumentNullException(nameof (readoutType));
      using (SelectImagesForm selectImagesForm = new SelectImagesForm())
      {
        if (owner != null)
          selectImagesForm.Owner = owner;
        selectImagesForm.lblTitle.Text = readoutType.ToString();
        selectImagesForm.readoutType = readoutType;
        int num = (int) selectImagesForm.ShowDialog();
      }
    }

    private void btnGoToImagesForm_Click(object sender, EventArgs e)
    {
      AddImagesForm.Show((Form) this);
      this.LoadImages();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (GMMImage checkedItem in this.listImages.CheckedItems)
      {
        if (stringBuilder.Length != 0)
          stringBuilder.Append(",");
        stringBuilder.Append(checkedItem.ImageID);
      }
      MeterDatabase.UpdateReadoutType(this.readoutType, new ReadoutType()
      {
        ReadoutSettingsID = this.readoutType.ReadoutSettingsID,
        ReadoutDeviceTypeID = this.readoutType.ReadoutDeviceTypeID,
        ImageIdList = stringBuilder.ToString()
      });
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void LoadImages()
    {
      this.listImages.Items.Clear();
      this.selectedImageIDs.Clear();
      if (string.IsNullOrEmpty(this.readoutType.ImageIdList))
        return;
      string[] strArray = this.readoutType.ImageIdList.Split(',');
      if (strArray != null)
      {
        foreach (string str in strArray)
        {
          if (!string.IsNullOrEmpty(str))
            this.selectedImageIDs.Add(Convert.ToInt32(str));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelectImagesForm));
      this.panel1 = new Panel();
      this.label2 = new Label();
      this.lblTitle = new Label();
      this.btnOK = new Button();
      this.btnGoToImagesForm = new Button();
      this.listImages = new CheckedListBox();
      this.picture = new PictureBox();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.picture).BeginInit();
      this.SuspendLayout();
      this.panel1.BackColor = Color.White;
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.lblTitle);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(784, 64);
      this.panel1.TabIndex = 34;
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 9f);
      this.label2.ImeMode = ImeMode.NoControl;
      this.label2.Location = new Point(40, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(237, 15);
      this.label2.TabIndex = 1;
      this.label2.Text = "Please select images of read-out settings. ";
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.ImeMode = ImeMode.NoControl;
      this.lblTitle.Location = new Point(23, 8);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(275, 24);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "{Images of read-out settings}";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.Cancel;
      this.btnOK.Image = (Image) componentResourceManager.GetObject("btnOK.Image");
      this.btnOK.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOK.ImeMode = ImeMode.NoControl;
      this.btnOK.Location = new Point(696, 531);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 23);
      this.btnOK.TabIndex = 36;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      this.btnGoToImagesForm.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnGoToImagesForm.Location = new Point(497, 531);
      this.btnGoToImagesForm.Name = "btnGoToImagesForm";
      this.btnGoToImagesForm.Size = new Size(50, 23);
      this.btnGoToImagesForm.TabIndex = 35;
      this.btnGoToImagesForm.Text = "...";
      this.btnGoToImagesForm.UseVisualStyleBackColor = true;
      this.btnGoToImagesForm.Click += new System.EventHandler(this.btnGoToImagesForm_Click);
      this.listImages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.listImages.FormattingEnabled = true;
      this.listImages.Location = new Point(7, 70);
      this.listImages.Name = "listImages";
      this.listImages.Size = new Size(484, 484);
      this.listImages.TabIndex = 37;
      this.listImages.SelectedIndexChanged += new System.EventHandler(this.listImages_SelectedIndexChanged);
      this.picture.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.picture.BackColor = Color.White;
      this.picture.BorderStyle = BorderStyle.FixedSingle;
      this.picture.Location = new Point(497, 71);
      this.picture.Name = "picture";
      this.picture.Size = new Size(280, 278);
      this.picture.TabIndex = 51;
      this.picture.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 562);
      this.Controls.Add((Control) this.picture);
      this.Controls.Add((Control) this.listImages);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnGoToImagesForm);
      this.Controls.Add((Control) this.panel1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (SelectImagesForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Images";
      this.Load += new System.EventHandler(this.SelectImagesForm_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.picture).EndInit();
      this.ResumeLayout(false);
    }
  }
}
