// Decompiled with JetBrains decompiler
// Type: PDC_Handler.OverwriteEditor
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace PDC_Handler
{
  public class OverwriteEditor : Form
  {
    private PDC_HandlerFunctions MyFunctions;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button btnCancel;
    private Button btnOverwrite;
    private CheckedListBox listParts;
    private Label label1;
    private CheckBox cboxSelectDeselectAll;

    internal OverwriteEditor() => this.InitializeComponent();

    private void OverwriteEditor_Load(object sender, EventArgs e)
    {
      foreach (object obj in new List<string>((IEnumerable<string>) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (OverwritePart))))
        this.listParts.Items.Add(obj, false);
    }

    internal static void ShowDialog(Form owner, PDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (OverwriteEditor overwriteEditor = new OverwriteEditor())
      {
        overwriteEditor.MyFunctions = MyFunctions;
        int num = (int) overwriteEditor.ShowDialog((IWin32Window) owner);
      }
    }

    private void btnOverwrite_Click(object sender, EventArgs e)
    {
      OverwritePart checkedParts = this.GetCheckedParts();
      try
      {
        if (this.MyFunctions.Overwrite(checkedParts))
        {
          this.Close();
        }
        else
        {
          int num = (int) MessageBox.Show("Failed to overwrite selected parts!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Failed to overwrite selected parts! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void cboxSelectDeselectAll_CheckedChanged(object sender, EventArgs e)
    {
      for (int index = 0; index < this.listParts.Items.Count; ++index)
        this.listParts.SetItemChecked(index, this.cboxSelectDeselectAll.Checked);
    }

    private OverwritePart GetCheckedParts()
    {
      OverwritePart checkedParts = (OverwritePart) 0;
      foreach (object checkedItem in this.listParts.CheckedItems)
      {
        OverwritePart overwritePart = (OverwritePart) Enum.Parse(typeof (OverwritePart), checkedItem.ToString(), true);
        checkedParts |= overwritePart;
      }
      return checkedParts;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OverwriteEditor));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.btnCancel = new Button();
      this.btnOverwrite = new Button();
      this.listParts = new CheckedListBox();
      this.label1 = new Label();
      this.cboxSelectDeselectAll = new CheckBox();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(295, 36);
      this.zennerCoroprateDesign1.TabIndex = 2;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(166, 215);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(104, 29);
      this.btnCancel.TabIndex = 21;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOverwrite.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOverwrite.Image = (Image) componentResourceManager.GetObject("btnOverwrite.Image");
      this.btnOverwrite.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnOverwrite.ImeMode = ImeMode.NoControl;
      this.btnOverwrite.Location = new Point(39, 215);
      this.btnOverwrite.Name = "btnOverwrite";
      this.btnOverwrite.Size = new Size(104, 29);
      this.btnOverwrite.TabIndex = 20;
      this.btnOverwrite.Text = "Overwrite";
      this.btnOverwrite.Click += new System.EventHandler(this.btnOverwrite_Click);
      this.listParts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.listParts.CheckOnClick = true;
      this.listParts.FormattingEnabled = true;
      this.listParts.Location = new Point(12, 62);
      this.listParts.Name = "listParts";
      this.listParts.Size = new Size(271, 124);
      this.listParts.TabIndex = 22;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 43);
      this.label1.Name = "label1";
      this.label1.Size = new Size(121, 13);
      this.label1.TabIndex = 23;
      this.label1.Text = "Select parts to overwrite";
      this.cboxSelectDeselectAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cboxSelectDeselectAll.AutoSize = true;
      this.cboxSelectDeselectAll.Location = new Point(12, 190);
      this.cboxSelectDeselectAll.Name = "cboxSelectDeselectAll";
      this.cboxSelectDeselectAll.Size = new Size(120, 17);
      this.cboxSelectDeselectAll.TabIndex = 24;
      this.cboxSelectDeselectAll.Text = "Select / deselect all";
      this.cboxSelectDeselectAll.UseVisualStyleBackColor = true;
      this.cboxSelectDeselectAll.CheckedChanged += new System.EventHandler(this.cboxSelectDeselectAll_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(295, 256);
      this.Controls.Add((Control) this.cboxSelectDeselectAll);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.listParts);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOverwrite);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (OverwriteEditor);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select overwrite items";
      this.Load += new System.EventHandler(this.OverwriteEditor_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
