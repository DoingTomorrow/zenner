// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Synchronization.HandleConflicts.ResolveConflict
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using Microsoft.Synchronization.Data;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace MSS.Business.Modules.Synchronization.HandleConflicts
{
  public class ResolveConflict : Form
  {
    private DbApplyChangeFailedEventArgs dbApplyChangeFailedEventArgs;
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private DataGridView grdViewLocal;
    private DataGridView grdViewRemote;
    private Panel pnlConflictInfo;
    private Label label3;
    private Label label2;
    private Label label1;
    private Panel panel2;
    private RadioButton rbRetryApplyingRow;
    private RadioButton rbRetryWithForceWrite;
    private RadioButton rbContinue;
    private RadioButton rbRetryNextSync;
    private Button btnResolveConflict;
    private TextBox txtConflictInfo;

    public ResolveConflict(DbApplyChangeFailedEventArgs e, string conflictInfo)
    {
      this.InitializeComponent();
      this.dbApplyChangeFailedEventArgs = e;
      this.txtConflictInfo.Text = conflictInfo;
      this.grdViewLocal.DataSource = (object) this.dbApplyChangeFailedEventArgs.Conflict.LocalChange;
      this.grdViewRemote.DataSource = (object) this.dbApplyChangeFailedEventArgs.Conflict.RemoteChange;
    }

    private void btnResolveConflict_Click(object sender, EventArgs e)
    {
      if (this.rbContinue.Checked)
        this.dbApplyChangeFailedEventArgs.Action = ApplyAction.Continue;
      else if (this.rbRetryWithForceWrite.Checked)
        this.dbApplyChangeFailedEventArgs.Action = ApplyAction.RetryWithForceWrite;
      else if (this.rbRetryApplyingRow.Checked)
        this.dbApplyChangeFailedEventArgs.Action = ApplyAction.RetryApplyingRow;
      else if (this.rbRetryNextSync.Checked)
        this.dbApplyChangeFailedEventArgs.Action = ApplyAction.RetryNextSync;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.label3 = new Label();
      this.label2 = new Label();
      this.grdViewRemote = new DataGridView();
      this.grdViewLocal = new DataGridView();
      this.pnlConflictInfo = new Panel();
      this.txtConflictInfo = new TextBox();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.btnResolveConflict = new Button();
      this.rbRetryNextSync = new RadioButton();
      this.rbRetryApplyingRow = new RadioButton();
      this.rbRetryWithForceWrite = new RadioButton();
      this.rbContinue = new RadioButton();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.grdViewRemote).BeginInit();
      ((ISupportInitialize) this.grdViewLocal).BeginInit();
      this.pnlConflictInfo.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.grdViewRemote);
      this.panel1.Controls.Add((Control) this.grdViewLocal);
      this.panel1.Location = new Point(12, 93);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(843, 192);
      this.panel1.TabIndex = 0;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(3, 95);
      this.label3.Name = "label3";
      this.label3.Size = new Size(148, 20);
      this.label3.TabIndex = 4;
      this.label3.Text = "Remote Changes";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(-1, 2);
      this.label2.Name = "label2";
      this.label2.Size = new Size(128, 20);
      this.label2.TabIndex = 3;
      this.label2.Text = "Local Changes";
      this.grdViewRemote.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdViewRemote.Location = new Point(3, 118);
      this.grdViewRemote.Name = "grdViewRemote";
      this.grdViewRemote.Size = new Size(835, 65);
      this.grdViewRemote.TabIndex = 1;
      this.grdViewLocal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdViewLocal.Location = new Point(3, 25);
      this.grdViewLocal.Name = "grdViewLocal";
      this.grdViewLocal.Size = new Size(835, 65);
      this.grdViewLocal.TabIndex = 0;
      this.pnlConflictInfo.BorderStyle = BorderStyle.FixedSingle;
      this.pnlConflictInfo.Controls.Add((Control) this.txtConflictInfo);
      this.pnlConflictInfo.Location = new Point(12, 25);
      this.pnlConflictInfo.Name = "pnlConflictInfo";
      this.pnlConflictInfo.Size = new Size(843, 62);
      this.pnlConflictInfo.TabIndex = 1;
      this.txtConflictInfo.Location = new Point(3, 3);
      this.txtConflictInfo.Multiline = true;
      this.txtConflictInfo.Name = "txtConflictInfo";
      this.txtConflictInfo.ReadOnly = true;
      this.txtConflictInfo.Size = new Size(833, 54);
      this.txtConflictInfo.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(167, 20);
      this.label1.TabIndex = 2;
      this.label1.Text = "Conflict Information";
      this.panel2.BorderStyle = BorderStyle.FixedSingle;
      this.panel2.Controls.Add((Control) this.btnResolveConflict);
      this.panel2.Controls.Add((Control) this.rbRetryNextSync);
      this.panel2.Controls.Add((Control) this.rbRetryApplyingRow);
      this.panel2.Controls.Add((Control) this.rbRetryWithForceWrite);
      this.panel2.Controls.Add((Control) this.rbContinue);
      this.panel2.Location = new Point(12, 295);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(312, 102);
      this.panel2.TabIndex = 3;
      this.btnResolveConflict.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnResolveConflict.Location = new Point(150, 3);
      this.btnResolveConflict.Name = "btnResolveConflict";
      this.btnResolveConflict.Size = new Size(147, 94);
      this.btnResolveConflict.TabIndex = 4;
      this.btnResolveConflict.Text = "Resolve Conflict";
      this.btnResolveConflict.UseVisualStyleBackColor = true;
      this.btnResolveConflict.Click += new EventHandler(this.btnResolveConflict_Click);
      this.rbRetryNextSync.AutoSize = true;
      this.rbRetryNextSync.Location = new Point(3, 69);
      this.rbRetryNextSync.Name = "rbRetryNextSync";
      this.rbRetryNextSync.Size = new Size(102, 17);
      this.rbRetryNextSync.TabIndex = 3;
      this.rbRetryNextSync.Text = "Retry Nex tSync";
      this.rbRetryNextSync.UseVisualStyleBackColor = true;
      this.rbRetryApplyingRow.AutoSize = true;
      this.rbRetryApplyingRow.Location = new Point(3, 46);
      this.rbRetryApplyingRow.Name = "rbRetryApplyingRow";
      this.rbRetryApplyingRow.Size = new Size(118, 17);
      this.rbRetryApplyingRow.TabIndex = 2;
      this.rbRetryApplyingRow.Text = "Retry Applying Row";
      this.rbRetryApplyingRow.UseVisualStyleBackColor = true;
      this.rbRetryWithForceWrite.AutoSize = true;
      this.rbRetryWithForceWrite.Location = new Point(3, 26);
      this.rbRetryWithForceWrite.Name = "rbRetryWithForceWrite";
      this.rbRetryWithForceWrite.Size = new Size(85, 17);
      this.rbRetryWithForceWrite.TabIndex = 1;
      this.rbRetryWithForceWrite.Text = "Keep remote";
      this.rbRetryWithForceWrite.UseVisualStyleBackColor = true;
      this.rbContinue.AutoSize = true;
      this.rbContinue.Checked = true;
      this.rbContinue.Location = new Point(3, 3);
      this.rbContinue.Name = "rbContinue";
      this.rbContinue.Size = new Size(75, 17);
      this.rbContinue.TabIndex = 0;
      this.rbContinue.TabStop = true;
      this.rbContinue.Text = "Keep mine";
      this.rbContinue.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(860, 403);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pnlConflictInfo);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (ResolveConflict);
      this.Text = nameof (ResolveConflict);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((ISupportInitialize) this.grdViewRemote).EndInit();
      ((ISupportInitialize) this.grdViewLocal).EndInit();
      this.pnlConflictInfo.ResumeLayout(false);
      this.pnlConflictInfo.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
